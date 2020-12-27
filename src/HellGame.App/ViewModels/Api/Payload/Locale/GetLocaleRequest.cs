using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellGame.App.ViewModels.Api.Payload.Locale
{
    [JsonObject]
    public class GetLocaleRequest
    {
        public string SessionId { get; set; }
    }
}
