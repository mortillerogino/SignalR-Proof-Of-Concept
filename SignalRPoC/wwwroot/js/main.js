"use strict";

function addElement(status, message) {
    var li = document.createElement("li");
    var newMessage = "";
    if (status === "Step") {
        li.setAttribute('class', 'list-group-item')
    }
    else if (status === "Info") {
        li.setAttribute('class', 'list-group-item list-group-item-warning')
    }
    else if (status === "Warning") {
        li.setAttribute('class', 'list-group-item list-group-item-danger')
    }
    else {
        li.setAttribute('class', 'list-group-item list-group-item-success')
    }
    li.textContent = message;
    var list = document.getElementById("messagesList");
    list.insertBefore(li, list.firstChild);
}

var connection = new signalR.HubConnectionBuilder().withUrl("/timeUpdateHub").build();
var fiveBtn = document.getElementById("fiveBtn");
var threeBtn = document.getElementById("threeBtn");
var oneBtn = document.getElementById("oneBtn");
var stopBtn = document.getElementById("stopBtn");
var isStarted = true;

connection.on("TimeUpdate", function (status, message) {
    addElement(status, message);
});

connection.start().then(addElement("Success", "Connection Successful with Update Interval at 5 seconds")).catch(function (err) {
    return console.error(err.toString());
});

fiveBtn.addEventListener("click", function (event) {
    connection.invoke("ChangeTime", 5).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

threeBtn.addEventListener("click", function (event) {
    connection.invoke("ChangeTime", 3).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

oneBtn.addEventListener("click", function (event) {
    connection.invoke("ChangeTime", 1).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

stopBtn.addEventListener("click", function (event) {
    if (isStarted) {
        connection.invoke("StopTime").catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
        stopBtn.textContent = "START"
        stopBtn.setAttribute('class', 'btn btn-lg btn-success btn-block')
        isStarted = false;
    }
    else {
        connection.invoke("StartTime").catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
        stopBtn.textContent = "STOP"
        stopBtn.setAttribute('class', 'btn btn-lg btn-danger btn-block')
        isStarted = true;
    }
    
});




