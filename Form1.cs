using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace TribalWars_AutoAttacker
{
    public partial class Form1 : Form
    {
        HttpListener listener;
        private string adress, coordinates = "", target = "";
        private int activeTable = 1;
        public string list1, list2, list3, list4, list5;

        public Form1()
        {
            InitializeComponent();
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:60024/");
            listener.Start();

            createFol();
            createPath();
            load();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adress = textBox1.Text;
            if (adress != "place here your address")
            {                                
                buildCommand();
                int count = target.Split('|').Length - 1;
                
                adress = adress.Replace("overview", "place");

                try
                {
                    Process.Start("chrome.exe", adress);
                }
                catch
                {
                    Process.Start(adress);
                }
                
                postdata();
                for (int i = 0; i < count; i++)
                {                   
                    string data = readdata();
                    target = data;
                    confirm();
                    postdata();
                }

                save();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ListViewItem ls = new ListViewItem(textBox14.Text);
            ls.SubItems.Add(t1.Text); ls.SubItems.Add(t2.Text); ls.SubItems.Add(t3.Text); ls.SubItems.Add(t4.Text); ls.SubItems.Add(t5.Text); ls.SubItems.Add(t6.Text); ls.SubItems.Add(t7.Text); ls.SubItems.Add(t8.Text); ls.SubItems.Add(t9.Text); ls.SubItems.Add(t10.Text); ls.SubItems.Add(t11.Text); ls.SubItems.Add(t12.Text);
            listView1.Items.Add(ls);
            textBox14.Text = "";
            t1.Text = "0"; t2.Text = "0"; t3.Text = "0"; t4.Text = "0"; t5.Text = "0"; t6.Text = "0"; t7.Text = "0"; t8.Text = "0"; t9.Text = "0"; t10.Text = "0"; t11.Text = "0"; t12.Text = "0";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Selected)
                {
                    listView1.Items[i].Remove();
                    i--;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            activeTable = 1;
            load();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            save();
        }        

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            activeTable = 2;
            load();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            activeTable = 3;
            load();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            activeTable = 4;
            load();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            activeTable = 5;
            load();
        }

        void buildCommand()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                for (int a = 0; a < 13; a++)
                {
                    if (listView1.Items[i].SubItems[a].Text != "0")
                    {
                        if (a == 1)
                        {
                            target += "unit_input_light*" + listView1.Items[i].SubItems[a].Text + "*";
                        }

                        else if (a == 2)
                        {
                            target += "unit_input_marcher*" + listView1.Items[i].SubItems[a].Text + "*";
                        }

                        else if (a == 3)
                        {
                            target += "unit_input_heavy*" + listView1.Items[i].SubItems[a].Text + "*";
                        }

                        else if (a == 4)
                        {
                            target += "unit_input_spy*" + listView1.Items[i].SubItems[a].Text + "*";
                        }

                        else if (a == 5)
                        {
                            target += "unit_input_catapult*" + listView1.Items[i].SubItems[a].Text + "*";
                        }

                        else if (a == 6)
                        {
                            target += "unit_input_ram*" + listView1.Items[i].SubItems[a].Text + "*";
                        }

                        else if (a == 7)
                        {
                            target += "unit_input_axe*" + listView1.Items[i].SubItems[a].Text + "*";
                        }

                        else if (a == 8)
                        {
                            target += "unit_input_archer*" + listView1.Items[i].SubItems[a].Text + "*";
                        }

                        else if (a == 9)
                        {
                            target += "unit_input_spear*" + listView1.Items[i].SubItems[a].Text + "*";
                        }

                        else if (a == 10)
                        {
                            target += "unit_input_sword*" + listView1.Items[i].SubItems[a].Text + "*";
                        }

                        else if (a == 11)
                        {
                            target += "unit_input_knight*" + listView1.Items[i].SubItems[a].Text + "*";
                        }

                        else if (a == 12)
                        {
                            target += "unit_input_snob*" + listView1.Items[i].SubItems[a].Text + "*";
                        }

                        else if (a == 0) // coordination  ->last
                        {
                            if (i == listView1.Items.Count - 1)
                                coordinates = listView1.Items[i].SubItems[a].Text;
                            else
                                coordinates = listView1.Items[i].SubItems[a].Text + "*";
                        }
                    }
                }

                target += coordinates;
            }
            target = System.Text.RegularExpressions.Regex.Replace(target, @"\t|\n|\r", "");
            
        }

        private void postdata()
        {
            var context = listener.GetContext();
            HttpListenerResponse response = context.Response;
            response.ContentType = "text/html";

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(target);
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;

            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

        private string readdata()
        {
            var context = listener.GetContext();
            var request = context.Request;
            string text;
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                text = reader.ReadToEnd();
            }

            return text;
        }

        private void confirm()
        {
            var context = listener.GetContext();
            HttpListenerResponse response = context.Response;
            response.ContentType = "text/html";

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes("ok");
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;

            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

        private void save()
        {
            string data = "";
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                for (int a = 0; a < 13; a++)
                {
                    data += listView1.Items[i].SubItems[a].Text + "/";
                }
            }
            if (data.Length != 0)
            {
                data = data.Substring(0, data.Length - 1);
            }

            string path = "";

            if (activeTable == 1)
                path = list1;
            else if (activeTable == 2)
                path = list2;
            else if (activeTable == 3)
                path = list3;
            else if (activeTable == 4)
                path = list4;
            else if (activeTable == 5)
                path = list5;

            data = System.Text.RegularExpressions.Regex.Replace(data, @"\t|\n|\r", "");

            using (StreamWriter sw = new StreamWriter(path))
            {

                sw.WriteLine(data);
            }
        }

        private void load()
        {
            string path = "";

            if (activeTable == 1)
                path = list1;
            else if (activeTable == 2)
                path = list2;
            else if (activeTable == 3)
                path = list3;
            else if (activeTable == 4)
                path = list4;
            else if (activeTable == 5)
                path = list5;

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string loadData = sr.ReadToEnd();

                    if (loadData != "")
                    {
                        string[] part = loadData.Split('/');

                        for (int i = 0; i < part.Length - 1;)
                        {
                            ListViewItem ls = new ListViewItem(part[i]);
                            ls.SubItems.Add(part[i + 1]); ls.SubItems.Add(part[i + 2]); ls.SubItems.Add(part[i + 3]); ls.SubItems.Add(part[i + 4]); ls.SubItems.Add(part[i + 5]); ls.SubItems.Add(part[i + 6]); ls.SubItems.Add(part[i + 7]); ls.SubItems.Add(part[i + 8]); ls.SubItems.Add(part[i + 9]); ls.SubItems.Add(part[i + 10]); ls.SubItems.Add(part[i + 11]); ls.SubItems.Add(part[i + 12]);
                            listView1.Items.Add(ls);
                            i += 13;
                        }
                    }
                }    
            }
            catch (IOException e)
            {
                MessageBox.Show(e.ToString());
            }
        }        

        private void createFol()
        {
            list1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "list1.txt");
            list2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "list2.txt");
            list3 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "list3.txt");
            list4 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "list4.txt");
            list5 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "list5.txt");
        }

        private void createPath()
        {
            if (!File.Exists(list1))
            {
                using (StreamWriter sw = new StreamWriter(list1))
                {

                    sw.WriteLine("");
                }

            }            

            if (!File.Exists(list2))
            {
                using (StreamWriter sw = new StreamWriter(list2))
                {

                    sw.WriteLine("");
                }

            }           

            if (!File.Exists(list3))
            {
                using (StreamWriter sw = new StreamWriter(list3))
                {

                    sw.WriteLine("");
                }

            }
            
            if (!File.Exists(list4))
            {
                using (StreamWriter sw = new StreamWriter(list4))
                {

                    sw.WriteLine("");
                }

            }           

            if (!File.Exists(list5))
            {
                using (StreamWriter sw = new StreamWriter(list5))
                {

                    sw.WriteLine("");
                }

            }            
        }
    }
}
