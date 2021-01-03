using Newtonsoft.Json;

namespace HellGame.App.ViewModels.Api.Payload.Locale
{
    [JsonObject]
    public class SetLocaleRequest
    {
        public string Locale { get; set; }
    }
}
