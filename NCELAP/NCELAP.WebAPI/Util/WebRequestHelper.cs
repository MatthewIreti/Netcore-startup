using AspNetCore.Http.Extensions;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Util
{
    public class WebRequestHelper
    {
        public static async Task<string> PostRequest<T>(string url, T model, string token)
        {
            string response="";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    var responseMessage = await client.PostAsJsonAsync(url, model);
                    var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                    response = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }
            return response;
        }
        public static async Task<string> GetRequest(string url, string token)
        {
            string jsonResponse = "";
            try
            {
                var webRequest = WebRequest.Create(url);
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 120000;
                    if (!string.IsNullOrEmpty(token))
                    {
                        webRequest.Headers.Add("Authorization", "Bearer " + token);
                    }
                    WebResponse response = await webRequest.GetResponseAsync();
                    Stream dataStream = response.GetResponseStream();

                    StreamReader reader = new StreamReader(dataStream);

                    jsonResponse = reader.ReadToEnd();
                    response.Dispose();
                    dataStream.Close();
                    dataStream.Dispose();
                    reader.Close();
                    reader.Dispose();
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
                throw ex;
            }

            return jsonResponse;
        }

    }
}
