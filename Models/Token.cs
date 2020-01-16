using Newtonsoft.Json;

namespace ExportApiDemo.Models {
    public class Token {
        [JsonProperty("access_token")]
        public string AuthToken { get; set; }
    }
}