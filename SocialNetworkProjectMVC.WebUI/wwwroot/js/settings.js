
// adding event listener to search friends : 
document.addEventListener('DOMContentLoaded', () => {
    const searchFriendRequest = document.getElementById("searchFriends");
    searchFriendRequest.addEventListener('input', (e) => {
        updateFriends(e.target.value);
    });
});

// closing account of current user and sending post reqeust by ajax :
const accountUrl = "https://localhost:7220/Account/";
function closeAccount(e) {
    e.preventDefault(); // prevent the form from submitting traditionally

    const email = document.getElementById("closeAccEmail").value;
    const password = document.getElementById("closeAccPassword").value;

    $.ajax({
        url: accountUrl + "CloseAccount",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            email: email,
            password: password
        }),
        success: async function (response) {
            // updating contacts for all users to see the change : 
            await connection.invoke("UpdateContactsForOtherUsers");
            window.location.href = "/";
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Failed to close the account.");
            console.error('Error:', textStatus, errorThrown);
        }
    });
}

// changing password and sending post request to controller : 
function changePassword(e) {
    e.preventDefault(); // prevent the form from submitting traditionally

    const newPass = $("#changePassNewPass").val();
    const oldPass = $("#changePassOldPass").val();

    $.ajax({
        url: accountUrl + "ChangePassword",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            newPassword: newPass,
            oldPassword: oldPass,
        }),
        success: function () {
            alert("password has been changed .");
            location.href("/Account/Login");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("changing password has failed .");
            console.error('Error:', textStatus, errorThrown);
        }
    });
}

// editing the user and sending post request to controller :
function editUser(e) {
    e.preventDefault(); // prevent the form from submitting traditionally
    const username = $("#editUserUsername").val();
    const email = $("#editUserEmail").val();
    $.ajax({
        url: accountUrl + "EditUser",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            username,
            email,
        }),
        success: async function () {
            // updating contacts for all users to see the change : 
            await connection.invoke("UpdateContactsForAllUsers");
            // updating user self infos asyncronously : 
            updateLayoutView();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("editing user has been failed .");
            console.error('Error:', textStatus, errorThrown);
        }
    });
}

// updating my-profile view of settings controller : 
async function updateMyProfileView() {
    const allRequests = await getAllFriendRequests();
    const friends = await getFriends("");
    await $.ajax({
        url: "https://localhost:7220/Account/GetCurrentUser",
        method: "GET",
        contentType: "application/json",
        success: function (response) {
            const user = response.user;
            $("#myProfileBgImage").attr("src", `/images/${user.backgroundImageUrl}`);
            $("#myProfileProfileImage").attr("src", `/images/${user.imageUrl}`);
            $("#myProfileUserUsername").text(user.userName);
            $("#myProfileUserEmail").text(user.email);
            const likeCountOfAllPosts = user.posts.flatMap(p => p.likes).length;
            const followingsCount = allRequests.filter(fr => fr.senderId === user.id).length;
            const followersCount = allRequests.filter(fr => fr.receiverId === user.id).length;
            const photosCount = user.posts.filter(p => p.postType === 0).length;
            $("#myProfileLikeCountOfAllPosts").text(likeCountOfAllPosts);
            $("#myProfileFollowingsCount").text(followingsCount);
            $("#myProfileFollowersCount").text(followersCount);
            $("#myProfilePhotosCount").text(photosCount);
            $("#friendCount").text(friends.filter(f=>f.ownId === user.id || f.yourFriendId === user.id).length);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Failed to get current user .");
            console.error('Error:', textStatus, errorThrown);
        }
    });
}

// updating setting view of settings controller : 
function updateSettingView() {
    $.ajax({
        url: "https://localhost:7220/Account/GetCurrentUser",
        method: "GET",
        contentType: "application/json",
        success: function (response) {
            const user = response.user;
            $("#editUserUsername").val(user.userName);
            $("#editUserEmail").val(user.email);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Failed to get current user .");
            console.error('Error:', textStatus, errorThrown);
        }
    });
}


updateSettingView();

updateMyProfileView();

updateAllAboutFriends();