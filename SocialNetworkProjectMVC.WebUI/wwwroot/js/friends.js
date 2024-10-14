// getting search input from ui and adding event listener that if i was changed the friend requests and people are updated :
const searchFriendRequest = document.getElementById("searchFriendRequests");
searchFriendRequest.addEventListener('input', (e) => {
    const searchValue = e.target.value.trim(); // trim to avoid empty spaces
    if (searchValue !== "") {
        updateFriendRequests(searchValue);
        updatePeopleYouKnow(searchValue);
    } else {
        // Handle empty input case, for example, load all friend requests/people
        updateFriendRequests(""); // No key provided, loads default data
         updatePeopleYouKnow("");
    }
});

// getting friend requests which were sent : 
async function getSentFriendRequests() {
    let sentFriendRequests = null;
    await $.ajax({
       
        url: "https://localhost:7220/api/FriendRequest/GetAllSentFriendRequestsOfCurrentUser",
            contentType: "application/json",
            method: "GET",
        success: function (response) {
            sentFriendRequests = response.sentFriendRequests;
        },
        error: function () {
            console.error("error while getting sent friend requests of current user");
        }
    });
    return sentFriendRequests;
}

// getting friend requests of current user : 
async function getFriendRequests(key) {
    let friendRequests = null;
    await $.ajax({
        url: "https://localhost:7220/api/FriendRequest/GetAllFriendRequestsOfCurrentUser",
        contentType: "application/json",
        method: "GET",
        data:{
            key: key
        },
        success: function (response) {
            friendRequests = response.friendRequests;
        },
        error: function () {
            console.error("error while getting friend requests of current user");
        }
    });
    return friendRequests;
}

// getting all friend requests from db : 
async function getAllFriendRequests() {
    let allFriendRequests = null;
    await $.ajax({
        url: "https://localhost:7220/api/FriendRequest/GetAllFriendRequests",
        contentType: "application/json",
        method: "GET",
        success: function (response) {
            allFriendRequests = response.allFriendRequests;
        },
        error: function () {
            console.error("error while getting all friend requests from db");
        }
    });
    return allFriendRequests;
}

// getting current user's friends :
async function getFriends(key) {
    let friends = null;
    await $.ajax({
        url: "https://localhost:7220/api/Friend/GetAllFriendsOfCurrentUser",
        contentType: "application/json",
        method: "GET",
        data: {
           key : key
        },
        success: function (response) {
            friends = response.allFriends;
        },
        error: function () {
            console.error("error while getting friends of current user");
        }
    });
    return friends;
}

// getting current user with ajax :
async function getCurrentUser() {
    let user = null;
    await $.ajax({
        url: "https://localhost:7220/Account/GetCurrentUser",
        method: "GET",
        contentType: "application/json",
        success: function (response) {
            user = response.user;
        },
        error: function () {
            console.error("error while getting current user .");
        }
    });
    return user;
}

// getting all users from db expect current : 
async function getOtherPeople(key) {
    let users = null;
    await $.ajax({
        url: "https://localhost:7220/api/Friend/GetOtherPeople",
        method: "GET",
        contentType: "application/json",
        data: {
            key : key,
        },
        success: function (response) {
            users = response.otherPeople;
        },
        error: function () {
            console.error("error while getting current user .");
        }
    });
    return users;
}

// updating friend requests list :
async function updateFriendRequests(key) {
    const friendRequests = await getFriendRequests(key);
    const currentUser = await getCurrentUser();
    const allFriendRequests = await getFriendRequests("");
    console.log("Friend Requests", friendRequests);
    let content = "";

    for (let i = 0; i < friendRequests.length; i++) {
        const request = friendRequests[i];
        const otherUser = request.sender.id === currentUser.id ? request.receiver : request.sender;
        let buttons = "";

        // check if the current user has sent the friend request :
        if (request.sender.id === currentUser.id) {
            // current user sent the request, show Cancel Request and Send Message buttons :
            buttons = `
                <div class="add-friend-btn">
                    <button onclick="cancelFriendRequest('${otherUser.id}')">Cancel Request</button>
                </div>
                <div class="send-message-btn">
                    <button onclick="sendMessageToUser('${otherUser.id}')">Send Message</button>
                </div>`;
        }
        // check if the current user has received the friend request :
        else if (request.receiver.id === currentUser.id) {
            // current user received the request, show Accept, Reject, and Send Message buttons :
            buttons = `
                <div class="add-friend-btn">
                    <button onclick="acceptFriendRequest('${otherUser.id}')">Accept</button>
                    <button onclick="rejectFriendRequest('${otherUser.id}')">Reject</button>
                </div>
                <div class="send-message-btn">
                    <button onclick="sendMessageToUser('${otherUser.id}')">Send Message</button>
                </div>`;
        }

        // create card content :
        content += `
        <div class="col-lg-3 col-sm-6">
            <div class="single-friends-card">
                <div class="friends-image">
                    <a href="#">
                        <img src="/images/${otherUser.backgroundImageUrl}.jpg" alt="image">
                    </a>
                    <div class="icon">
                        <a href="#"><i class="flaticon-user"></i></a>
                    </div>
                </div>
                <div class="friends-content">
                    <div class="friends-info d-flex justify-content-between align-items-center">
                        <a href="#">
                            <img src="/images/${otherUser.imageUrl}" alt="image">
                        </a>
                        <div class="text ms-3">
                            <h3><a href="#">${otherUser.userName}</a></h3>
                        </div>
                    </div>
                    <ul class="statistics">
                        <li>
                            <a href="#">
                                <span class="item-number">${otherUser.posts.flatMap(p => p.postLikes).length}</span>
                                <span class="item-text">Likes</span>
                            </a>
                        </li>
                        <li>
                            <a href="#">
                                <span class="item-number">${allFriendRequests.filter(fr => fr.senderId === otherUser.id).length}</span>
                                <span class="item-text">Following</span>
                            </a>
                        </li>
                        <li>
                            <a href="#">
                                <span class="item-number">${allFriendRequests.filter(fr => fr.receiverId === otherUser.id).length}</span>
                                <span class="item-text">Followers</span>
                            </a>
                        </li>
                    </ul>
                    <div class="button-group d-flex justify-content-between align-items-center">
                        ${buttons}
                    </div>
                </div>
            </div>
        </div>`;
    }

    // set the content to your DOM element where the friend requests are displayed :
    document.getElementById("friendRequests").innerHTML = content;
}

// sending friend request to receiver : 
async function sendFriendRequest(receiverId) {
    $.ajax({
        url: "https://localhost:7220/api/FriendRequest/SendFriendRequest",
        contentType: "application/json",
        method: "GET",
        data: {
            receiverId: receiverId
        },
        success: async function () {
            console.log("friend request has been sent .");
            await connection.invoke("UpdateFriendRequestsAndFriendsForUsers");
            await connection.invoke("UpdateContactsForAllUsers");
            const currentUser = await getCurrentUser();
            sendNotification(currentUser.id, receiverId, "send a friend request");
        },
        error: function () {
            console.error("error while sending friend request");
        }
    });
}

// canceling friend request to receiver :
function cancelFriendRequest(receiverId) {
    $.ajax({
        url: "https://localhost:7220/api/FriendRequest/CancelFriendRequest",
        contentType: "application/json",
        method: "GET",
        data: {
            receiverId: receiverId
        },
        success:async function () {
            console.log("friend request canceled .");
            await connection.invoke("UpdateFriendRequestsAndFriendsForUsers");
            await connection.invoke("UpdateContactsForAllUsers");
            const currentUser = await getCurrentUser();
            sendNotification(currentUser.id, receiverId, "canceled a friend request");
        },
        error: function () {
            console.error("error while canceling friend request");
        }
    });
}

// accepting friend request from sender :
function acceptFriendRequest(senderId) {
    $.ajax({
        url: "https://localhost:7220/api/FriendRequest/AcceptFriendRequest",
        contentType: "application/json",
        method: "GET",
        data: {
            senderId: senderId
        },
        success: async function () {
            console.log("friend accepted .");
            await connection.invoke("UpdateFriendRequestsAndFriendsForUsers");
            await connection.invoke("UpdateContactsForAllUsers");
            const currentUser = await getCurrentUser();
            sendNotification(currentUser.id, senderId, "accepted a friend request");
        },
        error: function () {
            console.error("error while accepting friend request");
        }
    });
}

// rejecting friend request from sender :
async function rejectFriendRequest(senderId) {
    await $.ajax({
        url: "https://localhost:7220/api/FriendRequest/RejectFriendRequest",
        contentType: "application/json",
        method: "GET",
        data: {
            senderId: senderId
        },
        success: async function () {
            const currentUser = await getCurrentUser();
            await connection.invoke("UpdateFriendRequestsAndFriendsForUsers");
            await connection.invoke("UpdateContactsForAllUsers");
            sendNotification(currentUser.id, senderId ,"rejected a friend request");
        },
        error: function () {
            console.error("error while rejecting friend request");
        }
    });
}

// sending message to user : 
function sendMessageToUser(userId) {
    window.location.href = "/Home/Messages";
    updateSpecialChat(userId);
}

// updating people u know  :
async function updatePeopleYouKnow(key) {
    const users = await getOtherPeople(key);
    console.log("People you know ",users);
    const allFriendRequests = await getFriendRequests("");
    let content = "";
    for (let i = 0; i < users.length; i++) {
        // create card content :
        content += `
       <div class="col-lg-3 col-sm-6">
    <div class="single-friends-card">
        <div class="friends-image">
            <a href="#">
                <img src="/images/${users[i].backgroundImageUrl}" alt="image">
            </a>
            <div class="icon">
                <a href="#"><i class="flaticon-user"></i></a>
            </div>
        </div>
        <div class="friends-content">
            <div class="friends-info d-flex justify-content-between align-items-center">
                <a href="#">
                    <img src="/images/${users[i].imageUrl}" alt="image">
                </a>
                <div class="text ms-3">
                    <h3><a href="#">${users[i].userName}</a></h3>
                </div>
            </div>
            <ul class="statistics">
                <li>
                    <a href="#">
                        <span class="item-number">${users[i].posts.flatMap(p => p.postLikes).length}</span>
                        <span class="item-text">Likes</span>
                    </a>
                </li>
                <li>
                    <a href="#">
                        <span class="item-number">${allFriendRequests.filter(fr => fr.senderId === users[i].id).length}</span>
                        <span class="item-text">Following</span>
                    </a>
                </li>
                <li>
                    <a href="#">
                        <span class="item-number">${allFriendRequests.filter(fr => fr.receiverId === users[i].id).length}</span>
                        <span class="item-text">Followers</span>
                    </a>
                </li>
            </ul>
            <div class="button-group d-flex justify-content-between align-items-center">
                <div class="add-friend-btn">
                    <button onclick="sendFriendRequest('${users[i].id}')">Send Friend Request</button>
                </div>
                <div class="send-message-btn">
                    <button onclick="sendMessageToUser('${users[i].id}')">Send Message</button>
                </div>
            </div>
        </div>
    </div>
</div>`; 
    }
    $("#peopleYouKnow").html(content);
}

// updating friends :
async function updateFriends(key) {
    console.log("friends updated !");
    const currentUser = await getCurrentUser();
    const friends = await getFriends(key);
    const allFriendRequests = await getAllFriendRequests();
    let content = "";
    for (let i = 0; i < friends.length; i++) {
        const friend = friends[i].ownId === currentUser.id ? friends[i].yourFriend : friends[i].own;
        content += `
<div class="col-lg-3 col-sm-6">
    <div class="single-friends-card">
        <div class="friends-image">
            <a href="#">
                <img src="/images/${friend.backgroundImageUrl}" alt="image">
            </a>
            <div class="icon">
                <a href="#"><i class="flaticon-user"></i></a>
            </div>
        </div>
        <div class="friends-content">
            <div class="friends-info d-flex justify-content-between align-items-center">
                <a href="#">
                    <img src="/images/${friend.imageUrl}" alt="image">
                </a>
                <div class="text ms-3">
                    <h3><a href="#">${friend.userName}</a></h3>
                </div>
            </div>
            <ul class="statistics">
                <li>
                    <a href="#">
                        <span class="item-number">${friend.posts.flatMap(p => p.postLikes).length}</span>
                        <span class="item-text">Likes</span>
                    </a>
                </li>
                <li>
                    <a href="#">
                        <span class="item-number">${allFriendRequests.filter(fr => fr.senderId === friend.id).length}</span>
                        <span class="item-text">Following</span>
                    </a>
                </li>
                <li>
                    <a href="#">
                        <span class="item-number">${allFriendRequests.filter(fr => fr.receiverId === friend.id).length}</span>
                        <span class="item-text">Followers</span>
                    </a>
                </li>
            </ul>
            <div class="button-group d-flex justify-content-between align-items-center">
                <div class="send-message-btn">
                    <button onclick="sendMessageToUser('${friend.id}')">Send Message</button>
                </div>
            </div>
        </div>
    </div>
</div>
        `;
    }
    $("#allFriends").html(content);
}

// removing friend : 
async function removeFriend(friendId) {
    await $.ajax({
        url: "https://localhost:7220/api/Friend/RemoveFriend",
        contentType: "application/json",
        method: "GET",
        data: {
            friendId: friendId
        },
        success: async function () {
            console.log("friend removed .");
            await connection.invoke("UpdateFriendRequestsAndFriendsForUsers");
            await connection.invoke("UpdateContactsForAllUsers");
            sendNotification(currentUser.id, receiverId, "removed you from friends");
        }
    });
}

// updating friends ; requests and people : 
async function updateAllAboutFriends() {
    console.log("all about friends updated .");
    await updateFriends("");
    await updatePeopleYouKnow("");
    await updateFriendRequests("");
    $("#searchFriendRequests").val("");
}

updateAllAboutFriends();