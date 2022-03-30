using TesteApi.Common.Exceptions;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TesteApi.Services.Helpers
{
    public class HttpHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpHelper(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public AsyncRetryPolicy Polly(string msgErroPolly, int retryNumber = 10, int retryIntervalTime = 5)
        {
            var requestPolicy = Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(retryNumber, retryAttempt => TimeSpan.FromSeconds(retryIntervalTime), (exception, timeSpan, retryCount, context) =>
                {
                    Trace.TraceError($@"{msgErroPolly}
                        of {context.PolicyKey}
                        retry count {retryCount}, 
                        due to: {exception}.");

                    Trace.Indent();
                    Trace.Flush();

                    if (retryCount == retryNumber)
                    {
                        throw new ServiceException($"Foram realizadas {retryCount} tentativas. Erro: {exception.Message}");
                    }
                });

            return requestPolicy;
        }

        public async Task<string> GetAsync(string uriString, string msgErroPolly, IList<KeyValuePair<string, string>> headers, double timeout = 180, int retryNumber = 5, int retryInterval = 2)
        {
            return await GetAsync(uriString, msgErroPolly, false, headers, timeout, retryNumber, retryInterval);
        }

        public async Task<string> GetAsync(string uriString, string msgErroPolly, double timeout = 180, int retryNumber = 3, int retryInterval = 2)
        {
            return await GetAsync(uriString, msgErroPolly, true, null, timeout, retryNumber, retryInterval);
        }

        private async Task<string> GetAsync(string uriString, string msgErroPolly, bool commonAuthorization, IList<KeyValuePair<string, string>> headers, double timeout, int retryNumber = 5, int retryInterval = 2)
        {
            try
            {
                var resultRequestPolicy = await Polly(msgErroPolly, retryNumber, retryInterval).ExecuteAndCaptureAsync(async () =>
                {
                    using var client = _httpClientFactory.CreateClient();
                    if (timeout > 0)
                    {
                        client.Timeout = TimeSpan.FromSeconds(timeout);
                    }

                    if (commonAuthorization)
                    {
                        AddCommonAuthorization(client);
                    }
                    else if (headers.Count > 0)
                    {
                        AddCustomHeaders(client, headers);
                    }

                    var responseResult = await client.GetAsync($"{uriString}");
                    return await ReadResultResponse(msgErroPolly, responseResult);
                });

                if (resultRequestPolicy.FinalException != null)
                {
                    throw new ServiceException(resultRequestPolicy.FinalException.Message);
                }

                return resultRequestPolicy.Result;
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message, ex);
            }
        }

        public async Task<string> PostAsync<TResult>(string uriString, string msgErroPolly, object dataParams, int retryNumber = 5, int retryInterval = 2)
        {
            try
            {
                var resultRequestPolicy = await Polly(msgErroPolly, retryNumber, retryInterval).ExecuteAndCaptureAsync(async () =>
                {
                    using var client = _httpClientFactory.CreateClient();

                    var contentRequest = new StringContent(JsonConvert.SerializeObject(dataParams), Encoding.UTF8, "application/json");
                    var responseResult = await client.PostAsync($"{uriString}", contentRequest);
                    return await ReadResultResponse(msgErroPolly, responseResult);
                });

                if (resultRequestPolicy.FinalException != null)
                {
                    throw new ServiceException(resultRequestPolicy.FinalException.Message);
                }

                return resultRequestPolicy.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> ReadResultResponse(string msgErroPolly, HttpResponseMessage responseResult)
        {
            try
            {
                var content = await responseResult.Content.ReadAsStringAsync();

                if (responseResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (content != null)
                    {
                        return content;
                    }
                    else
                    {
                        throw new HttpRequestException($"MessageApi Api - Erro ao recuperar {msgErroPolly}");
                    }
                }
                else
                {
                    throw new HttpRequestException(content);
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message, ex);
            }
        }

        private void AddCommonAuthorization(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
            client.DefaultRequestHeaders.Add("Access-Control-Allow-Methods", "*");
            client.DefaultRequestHeaders.Add("Access-Control-Allow-Headers", "*");
            client.DefaultRequestHeaders.Add("Access-Control-Allow-Credentials", "*");
        }

        private static void AddCustomHeaders(HttpClient client, IList<KeyValuePair<string, string>> headers)
        {
            if (headers != null && headers.Count > 0)
            {
                headers.ToList().ForEach(header =>
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                });
            }
        }
    }
}
