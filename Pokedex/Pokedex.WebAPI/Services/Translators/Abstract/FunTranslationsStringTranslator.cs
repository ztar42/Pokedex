using Pokedex.WebAPI.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pokedex.WebAPI.Translators
{
    /// <summary>
    /// Abstract string translator who uses the API from https://funtranslations.com/
    /// </summary>
    public abstract class FunTranslationsStringTranslator : ITranslator<string>
    {
        private readonly HttpClient client;
        public FunTranslationsStringTranslator(HttpClient client)
        {
            this.client = client;
        }
        protected virtual string GetTranslation(JsonElement root) => root.GetProperty("contents").GetProperty("translated").GetString();
        protected async Task<JsonDocument> GetJsonDocumentAsync(string url)
        {
            var uri = new Uri(client.BaseAddress + url);
            using (var httpResponse = await client.GetAsync(uri))
            {
                httpResponse.EnsureSuccessStatusCode();
                using (var content = await httpResponse.Content.ReadAsStreamAsync())
                {
                    return await JsonDocument.ParseAsync(content);
                }
            }
        }
        public virtual async Task<string> TranslateAsync(string target)
        {
            var json = await GetJsonDocumentAsync(target);
            return GetTranslation(json.RootElement);
        }
    }  
}
