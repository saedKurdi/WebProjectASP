// updating layout of views :
function updateLayoutView() {
    $.ajax({
        url: "https://localhost:7220/Account/GetCurrentUser",
        method: "GET",
        contentType: "application/json",
        success: async function (response) {
            const currentUser = response.user;
            $("#layoutFriendRequestCount").text(currentUser.friendRequests.filter(fr => fr.receiverId == currentUser.id).length);
            $("#layoutUnreadMessagesCount").text(currentUser.chats.flatMap(c => c.messages).filter(m => !m.isRead && m.receiverId == currentUser.id).length);
            $("#layoutUnreadNotificationsCount").text(currentUser.receivedNotifications.length);
            $("#layoutProfileImageUrl").attr('src', `/images/${currentUser.imageUrl}`);
            $("#layoutUserUsername").text(currentUser.userName);
            $("#layoutUserIsOnline").attr('class', currentUser.isOnline ? "status-online" : "status-offline");
            $("#layoutUserUsername2").text(currentUser.userName);
            $("#layoutUserEmail").text(currentUser.email);

            // binding friend requests to layout :
            const sentFriendRequests = await getSentFriendRequests();
            console.log(sentFriendRequests);
            let contentt = "";
            for (let i = 0; i < sentFriendRequests.length; i++) {
                contentt += `
                <div class="item d-flex align-items-center">
                                                <div class="figure">
                                                    <a href="#"><img src="/images/${sentFriendRequests[i].sender.imageUrl}" class="rounded-circle" alt="image"></a>
                                                </div>

                                                <div class="content d-flex justify-content-between align-items-center">
                                                    <div class="text">
                                                        <h4><a href="#">${sentFriendRequests[i].sender.userName}</a></h4>
                                                        <span>${sentFriendRequests[i].sender.friends.length} Friends</span>
                                                    </div>
                                                    <div class="btn-box d-flex align-items-center">
                                                        <button onclick="rejectFriendRequest('${sentFriendRequests[i].senderId}')" class="delete-btn d-inline-block me-2" data-bs-toggle="tooltip" data-bs-placement="top" title="Delete" type="button"><i class="ri-close-line"></i></button>

                                                        <button onclick="acceptFriendRequest('${sentFriendRequests[i].senderId}')" class="confirm-btn d-inline-block" data-bs-toggle="tooltip" data-bs-placement="top" title="Confirm" type="button"><i class="ri-check-line"></i></button>
                                                    </div>
                                                </div>
                </div>
                `;
            }
            contentt += `
            <div class="view-all-requests-btn">
               <a href="/Home/Friends" class="default-btn">View All Requests</a>
            </div>`;
            $("#layoutFriendRequests").html(contentt);


            // binding messages to layout : 
            $.ajax({
                url: "https://localhost:7220/api/Chat/GetChatsBySenderOrReceiver",
                method: "GET",
                contentType: "application/json",
                data: {
                    id: currentUser.id
                },
                success: function (response) {
                    const chats = response.chats;
                    let msgContent = "";
                    for (let i = 0; i < chats.length; i++) {
                        if (chats[i].messages.length === 0) continue;
                        const sender = chats[i].user1Id === currentUser.id ? chats[i].user2 : chats[i].user1;
                        const item = `
                        <div class="item d-flex justify-content-between align-items-center">
                            <div class="figure">
                                <a href="#"><img src="/images/${sender.imageUrl}" class="rounded-circle" alt="image"></a>
                            </div>
                            <div class="text">
                                <h4><a href="#">${sender.userName} </a></h4>
                                <span>${chats[i].messages[chats[i].messages.length - 1].messageText}</span>
                            </div>
                        </div>`;
                        msgContent += item;
                    }
                    msgContent += `  
                    <div class="view-all-messages-btn">
                        <a href="/Home/Messages" class="default-btn">View All Messages</a>
                    </div>`;
                    $("#layoutMessages").html(msgContent);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error('Error:', textStatus, errorThrown);
                }
            });

            // binding notifications to layout : 
            const notifications = currentUser.receivedNotifications;
            let content = "";
            for (let i = 0; i < notifications.length; i++) {
                const item = `
                <div class="item d-flex justify-content-between align-items-center">
                    <div class="figure">
                        <a href="#">
                            <img src="/images/${notifications[i].sender.imageUrl}" class="rounded-circle" alt="image" />
                        </a>
                    </div>
                    <div class="text">
                        <h4><a href="#">${notifications[i].sender.userName}</a></h4>
                        <span>${notifications[i].notificationText}</span>
                        <span class="main-color">${notifications[i].sentAt}</span>
                    </div>
                </div>`;
                content += item;
            }
            content +=` 
            <div class="view-all-notifications-btn">
                <a href="/Home/Notifications" class="default-btn">View All Notifications</a>
            </div>`
            $("#layoutNotifications").html(content);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Failed to get current user .");
            console.error('Error:', textStatus, errorThrown);
        }
    });
}

// updating index view of home controller :
async function updateIndexView() {
    const allRequests = await getAllFriendRequests();
    await $.ajax({
        url: "https://localhost:7220/Account/GetCurrentUser",
        method: "GET",
        contentType: "application/json",
        success: function (response) {
            const currentUser = response.user;
            $("#indexUserImagePath").attr('src', '/images/' + currentUser.imageUrl);
            $("#indexUserUsername").text(currentUser.userName);
            $("#indexLikeCount").text(currentUser.posts.flatMap(p => p.likes).length);
            $("#indexFollowingCount").text(allRequests.filter(fr => fr.senderId === currentUser.id).length);
            $("#indexFollowersCount").text(allRequests.filter(fr => fr.receiverId === currentUser.id).length);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Failed to get current user .");
            console.error('Error:', textStatus, errorThrown);
        }
    });
}

// updating chats of user : 
function updateChats() {
    $.ajax({
        url: "https://localhost:7220/Account/GetAllUsers",
        method: "GET",
        data: {
            key: "",
        },
        contentType: "application/json",
        success: function (response) {
            let allUsers = response.allUsers;
            $.ajax({
                url: "https://localhost:7220/Account/GetCurrentUser",
                method: "GET",
                contentType: "application/json",
                success: function (response) {
                    let content = "";
                    for (let i = 0; i < allUsers.length; i++) {
                        const currentUser = response.user;
                        let item = `
                        <div class="chat-box" onclick="updateSpecialChat('${allUsers[i].id}')">
                            <div style="width:120px;height:120px;" class="image">
                                <a href="#">
                                    <img src="/images/${allUsers[i].imageUrl}" class="rounded-circle" alt="image" />
                                </a>
                                <span class=${allUsers[i].isOnline ? "status-online" : "status-offline"}></span>
                            </div>
                            <h3>
                                <a href="#">${allUsers[i].userName}</a>
                            </h3>
                        </div>`;
                        content += item;
                    }
                    $("#chat-boxes").html(content);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert("Failed to get current user .");
                    console.error('Error:', textStatus, errorThrown);
                }
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Failed to send requst to the /Account/GetAllUsers from updateChats function .");
            console.error('Error:', textStatus, errorThrown);
        }
    });
}

// updating special chat of user :
function updateSpecialChat(senderId) {
    $.ajax({
        url: "https://localhost:7220/Account/GetUserById",
        method: "GET",
        contentType: "application/json",
        data: {
            id: senderId,
        },
        success: function (response) {
            const user = response.user;
            $("#senderImage").attr('src', `/images/${user.imageUrl}`);
            $("#senderUsername").text(user.userName);
            setMessagesReaden(senderId);
            updateMessages(senderId);
        },
        error: function () {
            console.error("When tried to get user by id failed !");
        }
    });   
}

// setting messages readen who sent to this user :
function setMessagesReaden(senderId) {
    $.ajax({
        url: "https://localhost:7220/Account/GetCurrentUser",
        method: "GET",
        contentType: "application/json",
        success: function (response) {
            const currentUser = response.user;
            $.ajax({
                url: "https://localhost:7220/api/Message/SetMessagesReaden",
                method: "GET",
                contentType: "application/json",
                data: {
                    receiverId: currentUser.id,
                    senderId: senderId,
                },
                success: function () {
                    console.log("messages readen succesfully .");
                    updateChats();
                    updateLayoutView();
                },
                error: function () {
                    console.log("messages can not be readen there was error .");
                }
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Failed to get current user .");
            console.error('Error:', textStatus, errorThrown);
        }
    });
}

// updating chat of user with the user which we have id : 
function updateMessages(receiverId) {
    $.ajax({
        url: "https://localhost:7220/Account/GetCurrentUser",
        method: "GET",
        contentType: "application/json",
        success: function (response) {
            const currentUser = response.user;
            $.ajax({
                url: "https://localhost:7220/api/Chat/GetChatMessages",
                contentType: "application/json",
                method: "GET",
                data: {
                    user1Id: currentUser.id,
                    user2Id: receiverId,
                },
                success: async function (response) {
                    const messages = response.messages;
                    let content = "";
                    for (let i = 0; i < messages.length; i++) {
                        const item = `
                            <div class='${messages[i].senderId === currentUser.id ? "chat chat-left" : "chat"}' >
                                <div class="chat-avatar">
                                    <a class="d-inline-block">
                                        <img src="/images/${messages[i].sender.imageUrl}" width="50" height="50" class="rounded-circle" alt="image">
                                    </a>
                                </div>
                                <div class="chat-body">
                                    <div class="chat-message">
                                        <p>${messages[i].messageText}</p>
                                        <span class="time d-block">${messages[i].sentAt}</span>
                                    </div>
                                </div>
                            </div>`;
                        content += item;
                    }
                    $("#chat-content").html(content);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert("Failed to get chat with user ids");
                    console.error('Error:', textStatus, errorThrown);
                }
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Failed to get current user .");
            console.error('Error:', textStatus, errorThrown);
        }
    });
}

// deleting chat : 
function deleteChat() {
    $.ajax({
        url: "https://localhost:7220/Account/GetUserByUsername",
        contentType: "application/json",
        method: "GET",
        data: {
            username: $("#senderUsername").text(),
        },
        success: function (response) {
            const receiverId = response.user.id;
            $.ajax({
                url: "https://localhost:7220/Account/GetCurrentUser",
                contentType: "application/json",
                method: "GET",
                success: function (response) {
                    const senderId = response.user.id;
                    $.ajax({
                        url: "https://localhost:7220/api/Chat/ClearChatMessages",
                        contentType: "application/json",
                        method: "GET",
                        data: {
                            user1Id: senderId,
                            user2Id: receiverId,
                        },
                        success: async function (response) {
                            console.log("chat deleted with success !");
                            updateMessages(receiverId);
                            await connection.invoke("UpdateUserMessagesForReceiver", receiverId, senderId);
                        },
                        error: function () {
                            console.error("error while deleting chat ");
                        }
                    });
                },
                error: function () {
                    console.error("ERROR When tried to get current user ");
                }
            });
        },
        error: function () {
            console.error("Error while tried to get user by username");
        }
    });
}

function deleteNotificationsOfCurrentUser() {
    $.ajax({
        url: "https://localhost:7220/Account/GetCurrentUser",
        method: "GET",
        contentType: "application/json",
        success: function (response) {
            const currentUser = response.user;
            $.ajax({
                url: "https://localhost:7220/api/Notification/RemoveAllNotificationsOfUser",
                method: "GET",
                contentType: "application/json",
                success: function () {
                    console.log("messages have been deleted with success !");
                    updateNotifications();
                },
                error: function () {
                    console.log("error while deleting the notifications of current user ");
                }
            })
        },
        error: function () {
            console.log("Error while getting current user .");
        }
    });
}

function deleteNotification(notificationId) {
    $.ajax({
        url: "https://localhost:7220/api/Notification/RemoveNotification",
        method: "GET",
        contentType: "application/json",
        data: {
            notificationId: notificationId,
        },
        success: function () {
            console.log("notification has been deleted succesfully !");
            updateNotifications();
        },
        error: function () {
            console.log("Error while removing notification .");
        }
    });
}

// sending message : 
function sendMessage(e) {
    e.preventDefault();
    $.ajax({
        url: "https://localhost:7220/Account/GetUserByUsername",
        contentType: "application/json",
        method: "GET",
        data: {
            username: $("#senderUsername").text(),
        },
        success: function (response) {
            const receiverId = response.user.id;
            $.ajax({
                url: "https://localhost:7220/Account/GetCurrentUser",
                contentType: "application/json",
                method: "GET",
                success: function (response) {
                    const senderId = response.user.id;
                    $.ajax({
                        url: "https://localhost:7220/api/Chat/GetChatBySenderReceiverId",
                        contentType: "application/json",
                        method: "GET",
                        data: {
                            user1Id: senderId,
                            user2Id: receiverId,
                        },
                        success: function (response) {
                            const chat = response.chat;
                            $.ajax({
                                url: "https://localhost:7220/api/Message/AddMessage",
                                contentType: "application/json",
                                method: "GET",
                                data: {
                                    chatId: chat.id,
                                    senderId: senderId,
                                    receiverId: receiverId,
                                    messageText: $("#messageInput").val(),
                                },
                                success: async function () {
                                    console.log("message was succesfully sent !");
                                    $("#messageInput").val(""),
                                    updateMessages(receiverId);
                                    updateChats();
                                    updateLayoutView();
                                    sendNotification(senderId, receiverId, " sent a message . ");
                                    await connection.invoke("UpdateUserMessagesForReceiver", receiverId, senderId);
                                }
                                , error: function () {
                                    console.error("while sending message there was error !");
                                }
                            });
                        },
                        error: function () {
                            console.error("error while getting chat ");
                        }
                    });
                },
                error: function () {
                    console.error("ERROR When tried to get current user ");
                }
            });
        },
        error: function () {
            console.error("Error while tried to get user by username");
        }
    });

}

// adding notification :
function sendNotification(senderId,receiverId,notificationText) {
    $.ajax({
        url: "https://localhost:7220/api/Notification/AddNotification",
        contentType: "application/json",
        method: "GET",
        data: {
            senderId: senderId,
            receiverId: receiverId,
            notificationText: notificationText,
        },
        success: async function () {
            console.log("Notification was succesfully added .");
            await connection.invoke("UpdateNotificationsForReceiver", receiverId);
        },
        error: function () {
            console.log("Error while adding notification .");
        }
    });
}

// updating notifications view of home controller :
function updateNotifications() {
    $.ajax({
        url: "https://localhost:7220/Account/GetCurrentUser",
        method: "GET",
        contentType: "application/json",
        success: function (response) {
            const currentUser = response.user;
            const notifications = currentUser.receivedNotifications;
            let content = "";
            for (let i = 0; i < notifications.length; i++) {
                const item = `
                <div class="item d-flex justify-content-between align-items-center">
                    <div class="figure">
                        <a href="#">
                            <img src="/images/${notifications[i].sender.imageUrl}" class="rounded-circle" alt="image"/>
                        </a>
                    </div>
                    <div class="text">
                        <h4><a href="my-profile.html">${notifications[i].sender.userName}</a></h4>
                        <span>${notifications[i].notificationText}</span>
                        <span class="main-color">${notifications[i].sentAt}</span>
                    </div>
                    <div class="icon">
                        <a onclick="deleteNotification(${notifications[i].id})" href="#"><i class="flaticon-x-mark"></i></a>
                    </div>
                </div>`;
                content += item;
            }
            $("#allNotificationsBody").html(content);
            updateLayoutView();
        },
        error: function () {
            console.log("Error while getting current user .");
        }
    });
}

// updating friends view of home controller : 

updateLayoutView();

updateIndexView();

updateChats();

updateNotifications();

