var CACHE_NAME = "v0.1.1";

// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development more difficult (changes would not
// be reflected on the first load after each change).
self.addEventListener('fetch', (event) => {
    event.respondWith(
        caches.match(event.request)
            .then(function (response) {
                if (response) {
                    return response;
                }
                return fetch(event.request);
            })
    );
});

self.addEventListener('install', (event) => {
    //self.skipWaiting();
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => cache.addAll([
                './favicon.ico',
                './icon-48.png',
                './icon-96.png',
                './icon-512.png',
                './icon-512_mono.png',
                './css/app.css',
                './css/bootstrap/bootstrap.min.css',
                './js/idb.js',
                './js/localStore.js'
            ]))
    );
});

self.addEventListener('message', function (event) {
    if (event.data.action === 'skipWaiting') {
        self.skipWaiting();
    }
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