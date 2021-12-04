window.scheduledNotifications = {
    isSupported: () => {
        return 'showTrigger' in Notification.prototype && 'serviceWorker' in navigator;
    }
}

if ('showTrigger' in Notification.prototype && 'serviceWorker' in navigator) {
    window.scheduledNotifications.createScheduledNotification = async (notificationTrigger) => {
        if (!window.scheduledNotifications.ensureNotificationPermissions()) {
            throw 'You need to grant notifications permission before you can schedule a notification.';
        }
        const registration = await navigator.serviceWorker.getRegistration();
        var typeOfTriggerTime = typeof (notificationTrigger.triggerTime);
        var timestamp;
        console.log("Creating notifcation from: ", notificationTrigger);
        if (typeOfTriggerTime === 'object' && 'toLocaleString' in notificationTrigger.triggerTime) {
            timestamp = notificationTrigger.triggerTime; // Already in Date type
        } else if (typeOfTriggerTime === 'string' && notificationTrigger.timestamp) {
            timestamp = new Date(notificationTrigger.timestamp);
        }

        if (timestamp) {
            var trigger = new TimestampTrigger(timestamp);
            var options = {
                tag: notificationTrigger.tag,
                body: notificationTrigger.body,
                showTrigger: trigger
            };
            if (notificationTrigger.icon && notificationTrigger['icon'] !== '') {
                options.icon = notificationTrigger.icon;
            }
            if (notificationTrigger.data) {
                options.data = notificationTrigger.data;
            }
            registration.showNotification(notificationTrigger.title, options);
        }
    };
    window.scheduledNotifications.cancelScheduledNotification = async (tag) => {
        return await navigator.serviceWorker.getRegistration()
            .then(registration => registration.getNotifications({ tag: tag, includeTriggered: true }))
            .then(notifications => notifications.forEach(notification => notification.close()));
    };
    window.scheduledNotifications.getScheduledNotifications = async (tag) => {
        const registration = await navigator.serviceWorker.getRegistration();
        var notifications = (await registration.getNotifications({
            tag: tag,
            includeTriggered: true,
        }));
        var jsNotifications = notifications.map(notification => ({
            title: notification.title,
            body: notification.body,
            timestamp: notification.showTrigger.timestamp,
            data: notification.data,
            tag: notification.tag,
            icon: notification.icon,
            badge: notification.badge,
            image: notification.image
        }));
        return jsNotifications;
    };
    window.scheduledNotifications.ensureNotificationPermissions = async () => {
        let { state } = await navigator.permissions.query({ name: 'notifications' });
        if (state === 'prompt') {
            await Notification.requestPermission();
        }
        state = (await navigator.permissions.query({ name: 'notifications' })).state;
        if (state !== 'granted') {
            return false;
        }
        return true;
    };
    navigator.serviceWorker.addEventListener('message', event => {
        if (event.data && event.data.returnUrl) {
            window.location.pathname = event.data.returnUrl;
        }
    });
}