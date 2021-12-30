window.utilities_games.localStores.register("TheLegendOfZelda_localStore", function () {
    const VERSION = 1;
    return window.utilities_games.localStores.installLocalStore(
        'TheLegendOfZelda',
        VERSION,
        [
            new window.utilities_games.localStores.objectStoreDto('userIngredients', { keyPath: 'name' })
        ]);
});