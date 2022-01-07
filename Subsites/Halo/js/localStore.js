window.utilities_games.localStores.register("Halo_localStore", function () {
    const VERSION = 1;
    return window.utilities_games.localStores.installLocalStore(
        'Halo',
        VERSION,
        [
            new window.utilities_games.localStores.objectStoreDto('skulls', { keyPath: 'id' }),
            new window.utilities_games.localStores.objectStoreDto('ranks', { keyPath: 'installment' })
        ]);
});