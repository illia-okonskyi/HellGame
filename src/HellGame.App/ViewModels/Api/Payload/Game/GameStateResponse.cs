using Newtonsoft.Json;
using System.Collections.Generic;

namespace HellGame.App.ViewModels.Api.Payload.Game
{
    [JsonObject]
    public class GameStateResponse
    {
        public string TextAssetKey { get; set; }
        public string ImageAssetKey { get; set; }
        public List<Transition> Transitions { get; set; }
    }
}
