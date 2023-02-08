using Daniel.Common;
using Daniel.Common.Models;
using Knus.Common.Helpers;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Knus.Common.Services
{
    public class ApiProvider<T> : IApiProvider<T>
    {

        #region field
        private readonly TimeSpan _timeout;
        private HttpClient _httpClient;
        private HttpClientHandler _httpClientHandler;
        //private readonly string _baseUrl;
        private const string ClientUserAgent = "tuf-api-client-v1";
        private const string MediaTypeJson = "application/json";
        #endregion

        #region public field       
        public ApiMetaData Apimeta { get; set; }
        public string BaseAddress {get;set;}
        public string SendValue { get; set; }
        public string ParameterValue { get; set; }
        public string JwtKey { get; set; } = "";
        public bool Debug { get; set; } = false;

        //public string QueryPath { get; set; }
        //public HttpMethods HttpMethodValue { get; set; }
        #endregion

        public ApiProvider()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _timeout = new TimeSpan(0, 0, 20);
        }
        private Uri GetUrl()
        {
            string strurl = "";
            if (!this.Apimeta.UrlPath.IsNullOrEmpty())
                strurl += this.ParameterValue;
            Uri sendurl = new Uri(strurl);
            return sendurl;
        }

         

        private static string ConvertToJsonString(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        private void EnsureHttpClientCreated()
        {
            if (_httpClient == null)
                CreateHttpClient();
        }
        private void CreateHttpClient()
        {
            _httpClientHandler = new HttpClientHandler();
            //{
            //    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            //};
            _httpClient = new HttpClient(_httpClientHandler, false)
            {
                Timeout = _timeout
            };
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(ClientUserAgent);

            _httpClient.BaseAddress = new Uri(BaseAddress);

            //if (!string.IsNullOrWhiteSpace(""))
            //{
                
            //}
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeJson));
            //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "Bearer " + JwtKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {JwtKey}");
            //string ddd = CrytoHelper.Encrypt(DateTime.Now.ToString("dd:yyyyMM knuscube"));
            //_httpClient.DefaultRequestHeaders.Add("seqkey", ddd);

        }

        private HttpRequestMessage GetRequestMessage()
        {
            string content = string.Empty;
            if (!this.SendValue.IsNullOrEmpty())
                content = SendValue;
            //Uri sendurl = new Uri(""+  this.QueryPath);
            Uri sendurl = new Uri(BaseAddress.EndsWith("/") ? BaseAddress : BaseAddress + this.Apimeta.UrlPath);

            var uriBuilder = new UriBuilder(sendurl);
            Uri finalUrl = uriBuilder.Uri;
            var request = new HttpRequestMessage()
            {
                RequestUri = finalUrl,
                Method = new HttpMethod(this.Apimeta.httpmethod.ToString())
            };

            if (content.Length > 0)
            {
                request.Content = new StringContent(content,
                                    Encoding.UTF8,
                                    "application/json");
            }

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"Bearer {JwtKey}");
            request.Headers.Add("Authorization", $"Bearer {JwtKey}");
            //Authorization
            return request;
        }

        // add by eastone
        public async Task<ApiBaseEntityModel<T>> AsyncCallDataByteArray()
        {
            ApiBaseEntityModel<T> _data = new ApiBaseEntityModel<T>();
            try
            {
                EnsureHttpClientCreated();

                switch (this.Apimeta.httpmethod)
                {
                    case HttpMethods.GET:
                        using (var response = await _httpClient.GetAsync(GetUrl()))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var rt = await response.Content.ReadAsByteArrayAsync();
                                _data.Success = true;
                                _data.OutValueByteArray = rt;
                                _data.Header_filename = response.Content.Headers?.ContentDisposition?.FileName;
                            }
                            else
                            {
                                _data.Success = false;
                                _data.Message = $"{response.StatusCode}/{response.ReasonPhrase}";

                            }
                        }
                        break;
                    case HttpMethods.POST:

                        using (var response = await _httpClient.SendAsync(GetRequestMessage()))
                        {
                            _data.HttpCode = response.StatusCode.ToString();
                            if (response.IsSuccessStatusCode)
                            {
                                var rt = await response.Content.ReadAsByteArrayAsync();
                                _data.Success = true;
                                _data.OutValueByteArray = rt;
                                _data.Header_filename = response.Content.Headers?.ContentDisposition?.FileName;
                            }
                            else
                            {
                                _data.Success = false;
                                //_data.Message = await response.Content.ReadAsStringAsync();
                                _data.ErrorMessage = await response.Content.ReadAsStringAsync();
                                _data.Message = $"{response.RequestMessage.RequestUri}|{response.StatusCode}|{response.ReasonPhrase}";
                            }
                        }
                        break;
                }
            }
            catch (WebException wxc)
            {
                _data.HasError = true;
                _data.Success = false;
                _data.Message = wxc.Message;
            }
            catch (Exception exc)
            {
                _data.HasError = true;
                _data.Success = false;
                if (exc.Message == "The operation was canceled.")
                    _data.Message = "Time Out";
                else
                    _data.Message = exc.Message;
            }

            finally
            {
            }
            return _data;
        }

        public async Task<ApiBaseEntityModel<T>> AsyncCallData()
        {
            ApiBaseEntityModel<T> _data = new ApiBaseEntityModel<T>();
            try
            {
                //_baseUrl = PathValue;
                EnsureHttpClientCreated();
                switch (this.Apimeta.httpmethod)
                {
                    case HttpMethods.GET:
                        using (var response = await _httpClient.GetAsync(GetUrl()))
                        {
                            _data.StatusCode = response.StatusCode;
                            _data.HttpCode = response.StatusCode.ToString();
                            if (response.IsSuccessStatusCode)
                            {
                                var rt = await response.Content.ReadAsStringAsync();
                                _data.Success = true;

                                if (this.Debug)
                                    _data.DebugMessage = rt;
                                _data.OutValue = JsonConvert.DeserializeObject<T>(rt);
                            }
                            else
                            {
                                _data.Success = false;
                                _data.ErrorMessage = await response.Content.ReadAsStringAsync();
                                _data.Message = $"{response.StatusCode}/{response.ReasonPhrase}";

                            }
                        }
                        break;
                    case HttpMethods.POST:
                     
                        using (var response = await _httpClient.SendAsync(GetRequestMessage()))
                        {
                            _data.StatusCode = response.StatusCode;
                            _data.HttpCode = response.StatusCode.ToString();
                            if (response.IsSuccessStatusCode)
                            {
                                _data.Success = true;
                                var rt = await response.Content.ReadAsStringAsync();
                                _data.OutValue = JsonConvert.DeserializeObject<T>(rt);
                            }
                            else
                            {
                                _data.Success = false;
                                //_data.Message = await response.Content.ReadAsStringAsync();
                                _data.ErrorMessage = await response.Content.ReadAsStringAsync();
                                _data.Message = $"{response.RequestMessage.RequestUri}|{response.StatusCode}|{response.ReasonPhrase}";
                            }
                        }
                        break;
                    case HttpMethods.PUT:
                        using (var requestContent = new StringContent(ConvertToJsonString(SendValue), Encoding.UTF8, MediaTypeJson))
                        {

                            using (var response = await _httpClient.PutAsync(GetUrl(), requestContent))
                            {
                                _data.StatusCode = response.StatusCode;
                                _data.HttpCode = response.StatusCode.ToString();
                                if (response.IsSuccessStatusCode)
                                {
                                    _data.Success = true;
                                    var rt = await response.Content.ReadAsStringAsync();
                                }
                                else
                                {
                                    _data.Success = false;
                                    _data.Message = $"{response.StatusCode}/{response.ReasonPhrase}";
                                }
                            }
                        }
                        break;
                    case HttpMethods.DELETE:
                        using (var response = await _httpClient.DeleteAsync(this.Apimeta.UrlPath))
                        {
                            _data.StatusCode = response.StatusCode;
                            _data.HttpCode = response.StatusCode.ToString();
                            if (response.IsSuccessStatusCode)
                            {
                                var rt = await response.Content.ReadAsStringAsync();
                            }
                            else
                            {
                                _data.Message = $"{response.StatusCode}/{response.ReasonPhrase}";
                            }
                        }
                        break;
                    case HttpMethods.DEBUG:
                        using (var response = await _httpClient.GetAsync(GetUrl()))
                        {
                            _data.StatusCode = response.StatusCode;
                            _data.HttpCode = response.StatusCode.ToString();

                            try
                            {
                                var rt = await response.Content.ReadAsStringAsync();
                                _data.Success = true;
                                _data.Message = rt;
                            }
                            catch (Exception exc)
                            {
                                _data.Message = exc.Message;
                            }
                        }
                        break;
                }
            }
            catch (WebException wxc)
            {
                _data.HasError = true;
                _data.Success = false;
                _data.Message = wxc.Message;
            } 
            catch (Exception exc)
            {
                _data.HasError = true;
                _data.Success = false;
                if (exc.Message == "The operation was canceled.")
                    _data.Message = "Time Out";
                else
                    _data.Message = exc.Message;
            }

            finally
            {            
            }
            return _data;
        }


    }

    public enum HttpMethods
    {
        POST, GET, PUT, DELETE, PATCH, DEBUG
    }

    public class ApiBaseEntityModel<T>
    {
        public bool HasError { get; set; }
        public bool HasAlert { get; set; }
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string HttpCode { get; set; }
        public string Message { get; set; }
        public T OutValue { get; set; }
        public byte[] OutValueByteArray { get; set; }
        public string Header_filename { get; set; }
        public string SendValue { get; set; }

        public bool Debug { get; set; }
        public string DebugMessage { get; set; }
        public string ErrorMessage { get; set; }
    }
}
