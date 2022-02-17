"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("sendButton").disabled = true;
document.getElementById("messageDiv").style.display = "none";

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    var messageList = document.getElementById("messagesList");
    messageList.insertBefore(li, messageList.firstChild);
    if (messageList.childNodes.length > 50) {
        messageList.removeChild(messageList.lastChild);
    }
    li.textContent = message;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

    if (message.includes("/stock=")) {
        let stock_code = message.split('=')[1].split(' ')[0];
        callStockBot(stock_code);
    }

    var room = document.getElementById("room").value;
    connection.invoke("SendMessage", user, message, room, false).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("joinButton").addEventListener("click", function (event) {

    var room = document.getElementById("room").value;
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message, room, true).then(function () {
        document.getElementById("messageDiv").style.display = "block";
    }).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function callStockBot(stock_code) {
    fetch("/api/v1/stocks/" + stock_code)
        .catch((error) => {
            console.error('Error:', error);
        });
}