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

var count=0;

messaging.onMessage((payload) => {
    console.log('Message received. ', payload);

    const notificationTitle = payload.data["title"];
    const notificationbody = payload.data["body"];

    var notification = '<a class="dropdown-item d-flex align-items-center" href=""><div class="mr-3"></div><div style="color:black"><div class="small text-gray-500" style="font-weight: bold; font-size: 50px; font-size: 18px; text-indent: 0; ">' + notificationTitle + '</div>' + notificationbody + '</div></a>';
    var n = document.getElementById("notifications");

    n.insertAdjacentHTML('beforeend', notification);
    var c = parseInt(document.getElementById('counter').innnerHTML);
    c++;
    document.getElementById('counter').innnerHTML = c.toString();

});

