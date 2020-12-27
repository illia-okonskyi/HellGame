using Newtonsoft.Json;

namespace HellGame.App.ViewModels.Api.Payload.Locale
{
    [JsonObject]
    public class GetLocaleResponse
    {
        public string Locale { get; set; }
    }
}
