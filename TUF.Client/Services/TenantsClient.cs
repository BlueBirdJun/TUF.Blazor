using Microsoft.VisualBasic;

namespace TUF.Client.Services
{
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.15.10.0 (NJsonSchema v10.6.10.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial interface ITenantsClient
    {

    }
    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.15.10.0 (NJsonSchema v10.6.10.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class TenantsClient :ITenantsClient
    {
        private System.Net.Http.HttpClient _httpClient;
        private System.Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;
        public TenantsClient(System.Net.Http.HttpClient httpClient)
        {
            _httpClient = httpClient;
            _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);
        }
        private Newtonsoft.Json.JsonSerializerSettings CreateSerializerSettings()
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            UpdateJsonSerializerSettings(settings);
            return settings;
        }
        protected Newtonsoft.Json.JsonSerializerSettings JsonSerializerSettings { get { return _settings.Value; } }
        partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings);
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url);
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder);
        partial void ProcessResponse(System.Net.Http.HttpClient client, System.Net.Http.HttpResponseMessage response);

        /// <summary>
        /// Get a list of all tenants.
        /// </summary>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        //public virtual System.Threading.Tasks.Task<System.Collections.Generic.ICollection<string>> GetListAsync()
        //{
        //    return GetListAsync(System.Threading.CancellationToken.None);
        //}

        //public virtual async System.Threading.Tasks.Task<System.Collections.Generic.ICollection<string>> GetListAsync(System.Threading.CancellationToken cancellationToken)
        //{
        //    var urlBuilder_ = new System.Text.StringBuilder();
        //    urlBuilder_.Append("api/tenants");
        //    var client_ = _httpClient;
        //    var disposeClient_ = false;
        //    try
        //    {
        //        using (var request_ = new System.Net.Http.HttpRequestMessage())
        //        { 

        //            request_.Method = new System.Net.Http.HttpMethod("GET");
        //            request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
        //            PrepareRequest(client_, request_, urlBuilder_);

        //            var url_ = urlBuilder_.ToString();
        //            request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
        //            PrepareRequest(client_, request_, url_);

        //            var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        //            var disposeResponse_ = true;
        //            try
        //            {
        //                var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
        //                if (response_.Content != null && response_.Content.Headers != null)
        //                {
        //                    foreach (var item_ in response_.Content.Headers)
        //                        headers_[item_.Key] = item_.Value;
        //                }
        //                ProcessResponse(client_, response_);
        //                var status_ = (int)response_.StatusCode;
        //                if (status_ == 200)
        //                {
        //                    var objectResponse_ = await ReadObjectResponseAsync<System.Collections.Generic.ICollection<TenantDto>>(response_, headers_, cancellationToken).ConfigureAwait(false);
        //                    if (objectResponse_.Object == null)
        //                    {
        //                        throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
        //                    }
        //                    return objectResponse_.Object;
        //                }
        //                else
        //                if (status_ == 400)
        //                {
        //                    var objectResponse_ = await ReadObjectResponseAsync<HttpValidationProblemDetails>(response_, headers_, cancellationToken).ConfigureAwait(false);
        //                    if (objectResponse_.Object == null)
        //                    {
        //                        throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
        //                    }
        //                    throw new ApiException<HttpValidationProblemDetails>("A server side error occurred.", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
        //                }
        //                else
        //                {
        //                    var objectResponse_ = await ReadObjectResponseAsync<ErrorResult>(response_, headers_, cancellationToken).ConfigureAwait(false);
        //                    if (objectResponse_.Object == null)
        //                    {
        //                        throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
        //                    }
        //                    throw new ApiException<ErrorResult>("A server side error occurred.", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
        //                }
        //            }
        //            finally
        //            {
        //                if (disposeResponse_)
        //                    response_.Dispose();
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        if (disposeClient_)
        //            client_.Dispose();
        //    }
        //}
    }
}
