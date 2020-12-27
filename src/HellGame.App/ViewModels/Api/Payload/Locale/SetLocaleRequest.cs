using Newtonsoft.Json;

namespace HellGame.App.ViewModels.Api.Payload.Locale
{
    [JsonObject]
    public class SetLocaleRequest
    {
        public string SessionId { get; set; }
        public string Locale { get; set; }
    }
}
