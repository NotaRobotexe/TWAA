// ==UserScript==
// @name         TribalWars Auto Attacker
// @author       NOTaRobot
// @grant       GM_xmlhttpRequest
// @include        https://*
// ==/UserScript==

window.onload = function() {
    var tribalWarsAdres = "https://skp2.divoke-kmene.sk/game.php?village=3729&screen=place";
    var tribalWarsAdresConfrim = tribalWarsAdres+"&try=confirm";
    var currentLocation = window.location;

    if(currentLocation == tribalWarsAdres)
    {
        GM_xmlhttpRequest({
            method: 'GET',
            url:    'http://localhost:60024/',
            onload: function(response) {
                var message = response.responseText;
                var messagesplit = message.split("*");
                var backupstring = "";
                var size = messagesplit.length;
                var type=messagesplit[0];
                var amount=messagesplit[1];
                var target=messagesplit[2];

                var untilCoo = 0;
                var whileExit = 1;
                while(whileExit == 1){
                    if(messagesplit[untilCoo].indexOf("|")  == -1 ){
                        $("#"+messagesplit[untilCoo]).val(messagesplit[untilCoo+1]);
                        untilCoo+=2;
                    }
                    else{
                        document.getElementsByClassName("target-input-field target-input-autocomplete ui-autocomplete-input")[0].setAttribute("value", messagesplit[untilCoo]);
                        whileExit = 2;
                    }
                }

                for(var a =untilCoo+1; a<size;a++ ){
                    if(a==size-1){
                        backupstring +=messagesplit[a];
                    }
                    else{
                        backupstring +=messagesplit[a]+"*";
                    }
                }

                GM_xmlhttpRequest ( {
                    method:     "POST",
                    url:        "http://localhost:60024/",
                    data:       backupstring,
                    headers:    {"Content-Type": "application/x-www-form-urlencoded"},
                    onload:     function (response) {
                    }
                } );
                var elementA = document.getElementById("target_attack");
                if (elementA){
                    elementA.click();
                }
            }
        });
    }

    else if(currentLocation == tribalWarsAdresConfrim){
        GM_xmlhttpRequest({
            method: 'GET',
            url:    'http://localhost:60024/',
            onload: function(response) {
                var confirm = response.responseText;
                if(confirm == "ok"){
                    var elementC = document.getElementById("troop_confirm_go");
                    if (elementC){
                        elementC.click();
                    }
                }
            }
        });
    }
};
