window.utilities_games.localStores.register("LOTR_RiseToWar_localStore", function () {
    const VERSION = 1;
    return window.utilities_games.localStores.installLocalStore(
        'LOTR_RiseToWar',
        VERSION,
        [
            new window.utilities_games.localStores.objectStoreDto('userServers', { keyPath: 'serverNumber' }),
            new window.utilities_games.localStores.objectStoreDto('userCommanders', { keyPath: 'id' })
        ]);
});