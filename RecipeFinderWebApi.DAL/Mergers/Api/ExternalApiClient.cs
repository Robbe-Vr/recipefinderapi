using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.DAL.Mergers.Api
{
    internal static class ExternalApiClient
    {
        private static readonly HttpClient _client = new HttpClient();

        public static async Task<string> SendRequest(string endpoint, Param[] parameters = null)
        {
            try
            {
                HttpResponseMessage res = await _client.GetAsync(endpoint + (parameters != null ? "?" + String.Join('&', parameters.Select(x => x.Key + "=" + x.Value)) : ""));
                res.EnsureSuccessStatusCode();

                string resBody = await res.Content.ReadAsStringAsync();

                return resBody;
            }
            catch (HttpRequestException e)
            {
                Trace.WriteLine("External Api request Failed! Reason: " + e.Message);
                return "";
            }
        }

        internal class Param
        {
            public Param(string key, string value)
            {
                Key = key ?? "unknown";
                Value = value ?? "false";
            }

            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
