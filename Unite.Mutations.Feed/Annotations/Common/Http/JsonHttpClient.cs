﻿using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Unite.Mutations.Feed.Annotations.Common.Http
{
    public class JsonHttpClient: IDisposable
    {
        private readonly HttpClient _httpClient;

        public JsonHttpClient()
        {
            var handler = new HttpClientHandler { UseProxy = false };
            _httpClient = new HttpClient(handler);
        }

        public JsonHttpClient(string baseUrl) : this()
        {
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<T> GetAsync<T>(string url, (string name, string value)[] headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            AddRequestHeaders(request, headers);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var dataJson = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<T>(dataJson);

                return data;
            }
            else
            {
                var message = await response.Content?.ReadAsStringAsync();
                throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase} - {message}");
            }
        }

        public async Task<T> PostAsync<T, TBody>(string url, TBody body, (string name, string value)[] headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var contentJson = JsonSerializer.Serialize(body);
            var content = new StringContent(contentJson, encoding: Encoding.UTF8, "application/json");

            request.Content = content;

            AddRequestHeaders(request, headers);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions()
                {
                    Converters = { new JsonStringEnumMemberConverter() },
                    WriteIndented = true
                };

                var dataJson = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<T>(dataJson, options);

                return data;
            }
            else
            {
                var message = await response.Content?.ReadAsStringAsync();
                throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase} - {message}");
            }
        }


        private void AddRequestHeaders(HttpRequestMessage request, params (string name, string value)[] headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.name, header.value);
                }
            }
        }


        #region IDisposable
        public void Dispose()
        {
            _httpClient.Dispose();
        }
        #endregion
    }
}
