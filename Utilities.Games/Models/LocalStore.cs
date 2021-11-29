using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Utilities.Games.Models
{
    public abstract class LocalStore<T>
    {
        /// <summary>
        /// Unique key for the subsite this localstore is tied to.
        /// </summary>
        public abstract string SubsiteKey { get; }

        /// <summary>
        /// Optional override of the local store that maintains local edits.
        /// </summary>
        public abstract string STORE_NAME { get; }

        /// <summary>
        /// Optional override of the local store's prefix.
        /// </summary>
        public virtual string PREFIX => $"{SubsiteKey}_localStore";

        private readonly HttpClient httpClient;
        private readonly IJSRuntime js;

        public LocalStore(HttpClient httpClient, IJSRuntime js)
        {
            this.httpClient = httpClient;
            this.js = js;
        }

        public abstract object GetKey(T item);

        public async Task<T> Get(string key)
            => await GetAsync<T>(STORE_NAME, key);

        public async Task<IEnumerable<T>> GetAll()
        {
            IEnumerable<T> items = await GetAllAsync(STORE_NAME);
            return items;
        }
        ValueTask<IEnumerable<T>> GetAllAsync(string storeName)
            => js.InvokeAsync<IEnumerable<T>>($"{PREFIX}.getAll", storeName);

        public virtual ValueTask SaveAsync(T item) => PutAsync(STORE_NAME, null, item);

        ValueTask<U> GetAsync<U>(string storeName, object key)
            => js.InvokeAsync<U>($"{PREFIX}.get", storeName, key);


        ValueTask PutAsync<U>(string storeName, object key, U value)
            => js.InvokeVoidAsync($"{PREFIX}.put", storeName, key, value);

        ValueTask DeleteAsync(string storeName, object key)
            => js.InvokeVoidAsync($"{PREFIX}.delete", storeName, key);
    }
}
