import "https://www.gstatic.com/firebasejs/9.1.1/firebase-app-compat.js";
import "https://www.gstatic.com/firebasejs/9.1.1/firebase-messaging-compat.js";

firebase.initializeApp({
    apiKey: "AIzaSyBkRNXnaENjbBJm2MNkgT8MUCu9SKsiCq4",

    authDomain: "flutter-asp-notifications.firebaseapp.com",

    projectId: "flutter-asp-notifications",

    storageBucket: "flutter-asp-notifications.appspot.com",

    messagingSenderId: "813339707057",

    appId: "1:813339707057:web:2b1310a0e5c4d2b25e8490",

    measurementId: "G-P3KYB99Q56"

});

const messaging = firebase.messaging();

messaging.getToken({ vapidKey: 'BKqr3PDQDiIXmFl9wzbPeRE-ZoSgv0z5-zsMldvqeWUk2lx-l14VntjNwU9a-NEra4DY7MET0Sd5oxWOjHnFRpQ' }).then((currentToken) => {
    if (currentToken) {
        // Send the token to your server and update the UI if necessary
        // ...
        console.log(currentToken);
        document.getElementById("fcmToken").value = currentToken;

    } else {
        // Show permission request UI
        console.log('No registration token available. Request permission to generate one.');
        // ...
    }
}).catch((err) => {
    console.log('An error occurred while retrieving token. ', err);
    // ...
});
