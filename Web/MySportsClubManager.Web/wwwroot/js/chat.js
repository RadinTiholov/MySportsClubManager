"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

const msgerForm = get(".msger-inputarea");
const msgerInput = get(".msger-input");
const msgerChat = get(".msger-chat");
const msgetSendButton = get(".msger-send-btn");

msgetSendButton.disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    appendMessage(user, "https://cdn-icons-png.flaticon.com/512/17/17004.png", "left", msg);
});

connection.start().then(function () {
    msgetSendButton.disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

msgerForm.addEventListener("submit", event => {
    event.preventDefault();

    const receiver = get("#receiver").value;
    const message = msgerInput.value;
    if (!message) return;

    connection.invoke("SendMessageToGroup", receiver, message).catch(function (err) {
        return console.error(err.toString());
    });

    appendMessage("Gosho", "https://cdn-icons-png.flaticon.com/512/17/17004.png", "right", message);
    msgerInput.value = "";
});

function appendMessage(name, img, side, text) {
    const msgHTML = `
    <div class="msg ${side}-msg">
      <div class="msg-img" style="background-image: url(${img})"></div>

      <div class="msg-bubble">
        <div class="msg-info">
          <div class="msg-info-name">${name}</div>
        </div>

        <div class="msg-text">${text}</div>
      </div>
    </div>`;

    msgerChat.insertAdjacentHTML("beforeend", msgHTML);
    msgerChat.scrollTop += 500;
}


// Utils
function get(selector, root = document) {
    return root.querySelector(selector);
}


//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var receiver = document.getElementById("receiverInput").value;
//    var message = document.getElementById("messageInput").value;

//    if (receiver != "") {

//        connection.invoke("SendMessageToGroup", receiver, message).catch(function (err) {
//            return console.error(err.toString());
//        });
//    }
//    else {
//        connection.invoke("SendMessage", message).catch(function (err) {
//            return console.error(err.toString());
//        });
//    }


//    event.preventDefault();
//});