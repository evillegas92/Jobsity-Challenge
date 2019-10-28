"use strict";

var signalRConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

signalRConnection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says: " + msg;

    insertMessage(encodedMsg);

});

signalRConnection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").innerHTML;
    var message = document.getElementById("messageInput").value;
    document.getElementById("messageInput").value = '';

    signalRConnection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function insertMessage(msg)
{
    var li = document.createElement("li");
    li.textContent = msg;
    var ul = document.getElementById("messagesList");
  
    if (ul.getElementsByTagName("li").length >= 50)
    {
        ul.removeChild(ul.getElementsByTagName("li")[0]);
    }
    ul.appendChild(li);
}