if ('showTrigger' in Notification.prototype && 'serviceWorker' in navigator) {
    window.createScheduledNotification = async (notificationTrigger) => {
        if (!window.ensureNotificationPermissions()) {
            throw 'You need to grant notifications permission before you can schedule a notification.';
        }

        const registration = await navigator.serviceWorker.getRegistration();
        var options = {
            tag: notificationTrigger.tag,
            body: notificationTrigger.body,
            showTrigger: new TimestampTrigger(notificationTrigger.triggerTime)
        };
        if (notificationTrigger.icon && notificationTrigger['icon'] !== '') {
            options.icon = notificationTrigger.icon;
        }
        if (notificationTrigger.data) {
            options.data = notificationTrigger.data;
        }
        registration.showNotification(notificationTrigger.title, options);
    };
    window.cancelScheduledNotification = async (tag) => {
        const registration = await navigator.serviceWorker.getRegistration();
        const notifications = await registration.getNotifications({
            tag: tag,
            includeTriggered: true,
        });
        notifications.forEach((notification) => notification.close());
    };
    window.ensureNotificationPermissions = async () => {
        let { state } = await navigator.permissions.query({ name: 'notifications' });
        if (state === 'prompt') {
            await Notification.requestPermission();
        }
        state = (await navigator.permissions.query({ name: 'notifications' })).state;
        if (state !== 'granted') {
            return false;
        }
        return true;
    }
    navigator.serviceWorker.addEventListener('message', event => {
        if (event.data && event.data.returnUrl) {
            window.location.pathname = event.data.returnUrl;
        }
    });
}