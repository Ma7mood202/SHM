importScripts('https://www.gstatic.com/firebasejs/9.7.0/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/9.7.0/firebase-messaging-compat.js');


firebase.initializeApp({
    apiKey: "AIzaSyBkRNXnaENjbBJm2MNkgT8MUCu9SKsiCq4",
    authDomain: "flutter-asp-notifications.firebaseapp.com",
    projectId: "flutter-asp-notifications",
    storageBucket: "flutter-asp-notifications.appspot.com",
    messagingSenderId: "813339707057",
    appId: "1:813339707057:web:2b1310a0e5c4d2b25e8490",
    messagingSenderId: '813339707057',
});
const messaging = firebase.messaging();

messaging.onBackgroundMessage((payload) => {
    console.log('[firebase-messaging-sw.js] Received background message ', payload);
    // Customize notification here
    const notificationTitle = payload.data["title"];
    const notificationOptions = {
        body: payload.data["body"],
        badge: '~\Medicio2\assets\img\smart4.png',
        icon: '~\Medicio2\assets\img\smart4.png',
        dir: 'rtl',
    };

    self.registration.showNotification(notificationTitle,
        notificationOptions);
});
