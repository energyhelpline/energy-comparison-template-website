using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BareboneUi.Common
{
    public class ConvertJsonBody
    {
        private readonly HttpResponseMessage _response;

        private ConvertJsonBody(HttpResponseMessage response)
        {
            _response = response;
        }

        public static ConvertJsonBody From(HttpResponseMessage stream)
        {
            return new ConvertJsonBody(stream);
        }

        public async Task<T> To<T>()
        {
            var serializer = new JsonSerializer();

            using (var stream = await _response.Content.ReadAsStreamAsync())
            using (var sr = new StreamReader(stream))
            return Deserialize<T>(sr, serializer);
        }

        private static T Deserialize<T>(StreamReader sr, JsonSerializer serializer)
        {
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }

        public async Task<string> ToStringAsync()
        {
            using (var stream = await _response.Content.ReadAsStreamAsync())
            using (var sr = new StreamReader(stream))
            return sr.ReadToEnd();
        }
    }
}