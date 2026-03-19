const CACHE_NAME = 'gymhub-v1';
const OFFLINE_URL = '/offline.html';

// Files to cache for offline
const PRECACHE_URLS = [
    '/',
    '/css/bootstrap/bootstrap.min.css',
    '/css/site.css',
    '/manifest.json'
];

// Install: cache essential files
self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_NAME).then(cache => {
            return cache.addAll(PRECACHE_URLS);
        })
    );
    self.skipWaiting();
});

// Activate: clean old caches
self.addEventListener('activate', event => {
    event.waitUntil(
        caches.keys().then(keys => {
            return Promise.all(
                keys.filter(key => key !== CACHE_NAME).map(key => caches.delete(key))
            );
        })
    );
    self.clients.claim();
});

// Fetch: network first, fallback to cache
self.addEventListener('fetch', event => {
    // Skip non-GET requests and SignalR/Blazor connections
    if (event.request.method !== 'GET') return;
    if (event.request.url.includes('_blazor')) return;
    if (event.request.url.includes('/api/')) return;

    event.respondWith(
        fetch(event.request)
            .then(response => {
                // Cache successful responses
                if (response.ok && response.type === 'basic') {
                    const clone = response.clone();
                    caches.open(CACHE_NAME).then(cache => {
                        cache.put(event.request, clone);
                    });
                }
                return response;
            })
            .catch(() => {
                // Fallback to cache
                return caches.match(event.request);
            })
    );
});
