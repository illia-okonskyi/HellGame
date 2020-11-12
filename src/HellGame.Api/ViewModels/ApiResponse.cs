using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HellGame.Api.ViewModels
{
    [JsonObject]
    public class ApiResponse<TPayload> where TPayload : class
    {
        public bool Success { get; }
        public string Error { get; }
        public TPayload Payload { get; }

        private ApiResponse(bool success, string error, TPayload payload)
        {
            Success = success;
            Error = error;
            Payload = payload;
        }

        public static ApiResponse<TPayload> MakeSuccess()
        {
            return new ApiResponse<TPayload>(true, null, null);
        }

        public static ApiResponse<TPayload> MakeSuccess(TPayload payload)
        {
            return new ApiResponse<TPayload>(true, null, payload);
        }

        public static ApiResponse<TPayload> MakeError(string error)
        {
            return new ApiResponse<TPayload>(false, error, null);
        }

        public static ApiResponse<TPayload> MakeError(Exception ex)
        {
            var errors = new List<string> { ex.Message };
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                errors.Add(ex.Message);
            }

            return new ApiResponse<TPayload>(false, string.Join("; ", errors), null);
        }
    }
}
