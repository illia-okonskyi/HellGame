using Newtonsoft.Json;

namespace HellGame.Api.ViewModels.ApiPayload.Locale
{
    [JsonObject]
    public class SetLocale
    {
        public string Locale { get; set; }
    }
}
