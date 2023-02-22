"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

const msgerForm = get(".msger-inputarea");
const msgerInput = get(".msger-input");
const msgerChat = get(".msger-chat");
const msgetSendButton = get(".msger-send-btn");

msgetSendButton.disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    const imageUrlRaw = get("#receiver-img").style.backgroundImage;
    const imageUrl = imageUrlRaw.substring(5, imageUrlRaw.length - 2);

    appendMessage(user, imageUrl, "left", msg);
});

connection.start()
    .then(function () {
        //Subscribe to the group
        const receiver = get("#receiver").value;

        connection.invoke("Subscribe", receiver).catch(function (err) {
            return console.error(err.toString());
        });

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

    fetch("/api/Message", {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': document.getElementById("RequestVerificationToken")
        },
        body: JSON.stringify({
            text: message,
            receiverUsername: receiver
        })
    })
        .then(() => {
            const imageUrlRaw = get("#your-img").style.backgroundImage;
            const imageUrl = imageUrlRaw.substring(5, imageUrlRaw.length - 2);

            appendMessage("You", imageUrl, "right", message);
            msgerInput.value = "";
        })
        .catch(err => { alert(err.message) })
})



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