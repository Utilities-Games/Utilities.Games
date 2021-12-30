window.utilities_games = {
    localStores: {
        objectStoreDto: function (storeName, parameters) {
            this.name = storeName;
            this.parameters = parameters;

            return this;
        },
        installLocalStore: function (databaseName, version, stores) {
            const db = idb.openDB(databaseName, version, {
                upgrade(db) {
                    for (var i = 0; i < stores.length; i++) {
                        db.createObjectStore(stores[i].name, stores[i].parameters);
                    }
                },
            });

            return {
                get: async (storeName, key) => (await db).transaction(storeName).store.get(key),
                getAll: async (storeName) => (await db).transaction(storeName).store.getAll(),
                getAllKeys: async (storeName) => (await db).transaction(storeName).store.getAllKeys(),
                getFirstFromIndex: async (storeName, indexName, direction) => {
                    const cursor = await (await db).transaction(storeName).store.index(indexName).openCursor(null, direction);
                    return (cursor && cursor.value) || null;
                },
                put: async (storeName, key, value) => (await db).transaction(storeName, 'readwrite').store.put(value, key === null ? undefined : key),
                putAllFromJson: async (storeName, json) => {
                    const store = (await db).transaction(storeName, 'readwrite').store;
                    JSON.parse(json).forEach(item => store.put(item));
                },
                delete: async (storeName, key) => (await db).transaction(storeName, 'readwrite').store.delete(key),
                autocompleteKeys: async (storeName, text, maxResults) => {
                    const results = [];
                    let cursor = await (await db).transaction(storeName).store.openCursor(IDBKeyRange.bound(text, text + '\uffff'));
                    while (cursor && results.length < maxResults) {
                        results.push(cursor.key);
                        cursor = await cursor.continue();
                    }
                    return results;
                }
            };
        },
        exists: function (databaseName) {
            return typeof window[databaseName] !== 'undefined' && window[databaseName] !== null;
        },
        register: function (accessor, installationFunction) {
            if (!window.utilities_games.localStores.registeredStores) {
                window.utilities_games.localStores.registeredStores = [];
            }

            window.utilities_games.localStores.registeredStores.push({
                name: accessor,
                installer: installationFunction
            });
        },
        install: function (accessor) {
            var registeredStore = window.utilities_games.localStores.registeredStores.filter(o => o.name === accessor);
            if (registeredStore.length === 0) throw "Registered store '" + accessor + "' not found.";
            registeredStore = registeredStore[0];

            window[accessor] = registeredStore.installer();
        },
        getStore: function (accessor) {
            if (!window.utilities_games.localStores.exists(accessor)) {
                window.utilities_games.localStores.install(accessor);
            }
            return window[accessor];
        },
        fromStore: {
            get: async (accessor, storeName, key) => await window.utilities_games.localStores.getStore(accessor)?.get(storeName, key),
            getAll: async (accessor, storeName) => await window.utilities_games.localStores.getStore(accessor)?.getAll(storeName),
            getAllKeys: async (accessor, storeName) => await window.utilities_games.localStores.getStore(accessor)?.getAllKeys(storeName),
            put: async (accessor, storeName, key, value) => await window.utilities_games.localStores.getStore(accessor)?.put(storeName, key, value),
            putAllFromJson: async (accessor, storeName, json) => await window.utilities_games.localStores.getStore(accessor)?.putAllFromJson(storeName, json),
            delete: async (accessor, storeName, key) => await window.utilities_games.localStores.getStore(accessor)?.delete(storeName, key),
            autocompleteKeys: async (accessor, storeName, text, maxResults) => await window.utilities_games.localStores.getStore(accessor)?.autocompleteKeys(storeName, text, maxResults)
        }
    }
}