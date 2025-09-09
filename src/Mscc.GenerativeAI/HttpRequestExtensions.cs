#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Net.Http;
#endif
using System.Net.Http.Headers;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpRequestExtensions
    {
        private const string TimeoutKey = "RequestTimeout";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="timeout"></param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        internal static void SetTimeout(this HttpRequestMessage request,
            TimeSpan? timeout)
        {
#if NET472_OR_GREATER || NETSTANDARD2_0
            if (request is null) throw new ArgumentNullException(nameof(request));
            request.Properties[TimeoutKey] = timeout;
#else
            ArgumentNullException.ThrowIfNull(request);
            request.Options.Set(new HttpRequestOptionsKey<TimeSpan?>(TimeoutKey), timeout);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <returns></returns>
        internal static TimeSpan? GetTimeout(this HttpRequestMessage request)
        {
#if NET472_OR_GREATER || NETSTANDARD2_0
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (request.Properties.TryGetValue(TimeoutKey, out var value)
                && value is TimeSpan timeout)
                return timeout;
#else
            ArgumentNullException.ThrowIfNull(request);
            if (request.Options.TryGetValue(new HttpRequestOptionsKey<TimeSpan?>(TimeoutKey), out var value)
                && value is TimeSpan timeout)
                return timeout;
#endif
            
            return null;
        }

        /// <summary>
        /// Specify API key in HTTP header
        /// </summary>
        /// <seealso href="https://cloud.google.com/docs/authentication/api-keys-use#using-with-rest">Using an API key with REST</seealso>
        /// <param name="request"><see cref="HttpRequestMessage"/> to send to the API.</param>
        /// <param name="apiKey">The API key to use for the request.</param>
        internal static void AddApiKeyHeader(this HttpRequestMessage request, 
            string? apiKey)
        {
            if (!string.IsNullOrEmpty(apiKey))
            {
                if (request.Headers.Contains("x-goog-api-key"))
                {
                    request.Headers.Remove("x-goog-api-key");
                }
                request.Headers.Add("x-goog-api-key", apiKey);
            }
        }
        
        internal static void AddAccessTokenHeader(this HttpRequestMessage request, 
            string? accessToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                if (request.Headers.Contains("Authorization"))
                {
                    request.Headers.Remove("Authorization");
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        internal static void AddProjectIdHeader(this HttpRequestMessage request, 
            string? projectId)
        {
            if (!string.IsNullOrEmpty(projectId))
            {
                if (request.Headers.Contains("x-goog-user-project"))
                {
                    request.Headers.Remove("x-goog-user-project");
                }

                request.Headers.Add("x-goog-user-project", projectId);
            }
        }

        internal static void AddRequestHeaders(this HttpRequestMessage request,
            HttpRequestHeaders? headers)
        {
            if (headers != null)
            {
                foreach (KeyValuePair<string, IEnumerable<string>> header in headers)
                {
                    if (request.Headers.Contains(header.Key))
                    {
                        request.Headers.Remove(header.Key);
                    }

                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }
    }
}