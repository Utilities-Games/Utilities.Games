// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development more difficult (changes would not
// be reflected on the first load after each change).
self.addEventListener('fetch', () => { });

self.addEventListener('install', (event) => {
    self.skipWaiting();
});

self.addEventListener('activate', event => {
    event.waitUntil(self.clients.claim().then(() => {
        // See https://developer.mozilla.org/en-US/docs/Web/API/Clients/matchAll
        return self.clients.matchAll({ type: 'window' });
    }));
});

self.onnotificationclick = function (event) {
    event.notification.close();
    var eventData = event.notification.data;

    event.waitUntil(self.clients.matchAll({
            includeUncontrolled: true,
            type: "window"
        }).then(function (clientList) {
            for (var i = 0; i < clientList.length; i++) {
                var client = clientList[i];
                if ('focus' in client) {
                    if (eventData && eventData.returnUrl) {
                        return client.postMessage(eventData);
                    }
                    return client.focus();
                }
            }
            if (clients.openWindow) {
                var url = '/';
                if (eventData && eventData.returnUrl) {
                    url = eventData.returnUrl;
                }
                return clients.openWindow(url);
            }
            return null;
        })
    );
}