using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SendInBlue.Models;
using SendInBlue.Serialization;

namespace SendInBlue
{
    public interface ISendInBlueClient
    {
        Task<ApiResponse> UpsertUserAsync(UserModel userModel);

        Task<ApiResponse> SendTrackEventAsync(string email, string eventName);

        Task<ApiResponse> SendTrackEventAsync<T1, T2>(string email, string eventName, T1 properties, T2 eventData);
    }

    public class SendInBlueClient : ISendInBlueClient
    {
        private readonly string _apiKey;

        private readonly HttpClient _httpClient;

        public SendInBlueClient(string apiKey, string trackingIdKey)
        {
            _apiKey = apiKey;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add(SendInBlueConstants.ApiKey, _apiKey);
            _httpClient.DefaultRequestHeaders.Add(SendInBlueConstants.MaKey, trackingIdKey);
        }

        public async Task<ApiResponse> UpsertUserAsync(UserModel userModel)
        {
            return await UploadStringTaskAsync(SendInBlueConstants.ApiKey, _apiKey, $"{SendInBlueConstants.ApiEndpoints.UserEndpointAddress}/createdituser", userModel);
        }

        public async Task<ApiResponse> SendTrackEventAsync(string email, string eventName)
        {
            return await PostAsync($"{SendInBlueConstants.ApiEndpoints.AutomationEndpointAddress}/trackEvent", new Dictionary<string, object>
            {
                { "event", eventName },
                { "email", email }
            });
        }

        public async Task<ApiResponse> SendTrackEventAsync<T1, T2>(string email, string eventName, T1 properties, T2 eventData)
        {
            return await PostAsync($"{SendInBlueConstants.ApiEndpoints.AutomationEndpointAddress}/trackEvent", new Dictionary<string, object>
            {
                { "event", eventName },
                { "email", email },
                { "properties", properties },
                { "eventdata", new Dictionary<string, object>
                {
                    { "id", Guid.NewGuid().ToString() },
                    { "data", eventData }
                } }
            });
        }

        private static async Task<ApiResponse> UploadStringTaskAsync<T>(string keyName, string key, string url, T model)
        {
            using (var httpClient = new WebClient())
            {
                var retval = new ApiResponse();
                httpClient.Headers.Add(keyName, key);

                try
                {
                    var serialized = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                    {
                        ContractResolver = new LowercaseContractResolver()
                    });
                    var response = await httpClient.UploadStringTaskAsync(url, serialized);

                    retval = JsonConvert.DeserializeObject<ApiResponse>(response);
                }
                catch (WebException webException)
                {
                    retval.Code = HttpStatusCode.BadRequest.ToString();
                    retval.Message = webException.Message;
                }

                return retval;
            }
        }

        private async Task<ApiResponse> PostAsync<T>(string url, T model)
        {
            var returnVal = new ApiResponse();
            var data = JsonConvert.SerializeObject(model);

            using (var httpContent = new StringContent(data, Encoding.UTF8, SendInBlueConstants.MediaTypes.ApplicationJson))
            {
                try
                {
                    var response = await _httpClient.PostAsync(url, httpContent);

                    returnVal.Code = response.StatusCode.ToString();
                    returnVal.Message = response.ReasonPhrase;
                }
                catch (HttpRequestException httpRequestException)
                {
                    returnVal.Code = HttpStatusCode.BadRequest.ToString();
                    returnVal.Message = httpRequestException.Message;
                }
            }

            return returnVal;
        }
    }
}
