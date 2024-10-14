async function register() {
    console.log("register worked !")
    await connection.invoke("UpdateContactsForOtherUsers");
}

register();