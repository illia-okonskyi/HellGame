using Newtonsoft.Json;

namespace HellGame.App.ViewModels.Api.Payload.Game
{
    [JsonObject]
    public class StartGameRequest
    {
        public string UserName { get; set; }
    }
}
