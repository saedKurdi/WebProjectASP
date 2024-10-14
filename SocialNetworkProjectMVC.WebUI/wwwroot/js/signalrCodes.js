// create a connection to the SignalR hub :
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/zustHub")  // this should match the route you mapped in Program.cs
    .build();

// start the connection :
connection.start()
    .then(() => {
        console.log("SignalR connection established");
    })
    .catch(err => console.error("SignalR connection failed: ", err));

// subscribe to the UpdateContacts event :
connection.on("UpdateContacts", async() => {
    await updateMyProfileView();
    updateIndexView();
    sendRequestAndUpdateContacts("");
    updateLayoutView();
    updateChats();
});

// subscribing to update messages for users :
connection.on("UpdateAllMessages", (senderId) => {
    updateMessages(senderId);
    updateLayoutView();
});

// updating notifications for receiver user : 
connection.on("UpdateNotificationsForReceiver", () => {
    updateNotifications();
});

// updating friends and friend requests for users : 
connection.on("UpdateFriendRequestsAndFriends", async() => {
    updateLayoutView();
    updateIndexView();
    await updateSettingView();
    await updateAllAboutFriends();
});