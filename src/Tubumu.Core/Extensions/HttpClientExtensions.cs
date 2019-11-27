﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tubumu.Core.Extensions
{
    public static class HttpClientExtensions
    {
        public async static Task<T> GetObjectAsync<T>(this HttpClient client, Uri requestUri)
        {
            var json = await client.GetStringAsync(requestUri);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static Task<T> GetObjectAsync<T>(this HttpClient client, string requestUri)
        {
            return client.GetObjectAsync<T>(new Uri(requestUri));
        }

        public async static Task<T> PostObjectAsync<T>(this HttpClient client, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            var message = await client.PostAsync(requestUri, content, cancellationToken);
            message.EnsureSuccessStatusCode();
            var json = await message.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static Task<T> PostObjectAsync<T>(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return client.PostObjectAsync<T>(new Uri(requestUri), content, default(CancellationToken));
        }

        public static Task<T> PostObjectAsync<T>(this HttpClient client, Uri requestUri, HttpContent content)
        {
            return client.PostObjectAsync<T>(requestUri, content, default(CancellationToken));
        }

        public static Task<T> PostObjectAsync<T>(this HttpClient client, string requestUri, HttpContent content)
        {
            return client.PostObjectAsync<T>(new Uri(requestUri), content);
        }

    }
}
