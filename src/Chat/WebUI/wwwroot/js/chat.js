"use strict"

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build()

document.getElementById("sendButton").disabled = true
document.getElementById("messageDiv").style.display = "none"

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false
}).catch(function (err) {
    return console.error(err.toString())
})

connection.on("ReceiveMessage", function (user, message) {
    showMessages(message)
})

connection.on("History", function (message) {
    for (var i = 0; i < message.length; i++) {
        showMessages(message[i])
    }
})

document.getElementById("sendButton").addEventListener("click", function (event) {
    var room = document.getElementById("room").value
    var user = document.getElementById("userInput").value
    var message = document.getElementById("messageInput").value

    if (message == "") {
        alert("Please fill a message.")
        return
    }

    sendMessage(user, message, room, false)

    event.preventDefault()
})

document.getElementById("joinButton").addEventListener("click", function (event) {
    var room = document.getElementById("room").value
    var user = document.getElementById("userInput").value
    var message = document.getElementById("messageInput").value

    if (room == "") {     
        alert("Please select a room.")
        return
    }

    document.getElementById("messagesList").innerHTML = ""
    sendMessage(user, message, room, true)
    document.getElementById("messageDiv").style.display = "block"

    event.preventDefault()
})

function showMessages(message) {
    var li = document.createElement("li")
    var messageList = document.getElementById("messagesList")
    messageList.insertBefore(li, messageList.firstChild)
    if (messageList.childNodes.length > 50) {
        messageList.removeChild(messageList.lastChild)
    }
    li.textContent = message
}

function sendMessage(user, message, room, isJoin) {
    connection.invoke("SendMessage", user, message, room, isJoin).then(function () {
        document.getElementById("messageInput").value = ""
    }).catch(function (err) {
        return console.error(err.toString())
    })
}