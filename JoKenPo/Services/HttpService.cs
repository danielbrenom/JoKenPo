using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JoKenPo.Domain.Interfaces;

namespace JoKenPo.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _client;

        public HttpService(IHttpClientFactory httpClientFactory, IConfigurationManager configurationManager)
        {
            _client = httpClientFactory.CreateClient();
            _client.Timeout = System.TimeSpan.FromSeconds(50);
            _baseUrl = configurationManager.GetConfigKey("BaseUrl");
            Headers = new Dictionary<string, string>();
            Query = new Dictionary<string, string>();
            FormBody = new Dictionary<string, string>();
        }

        private readonly string _baseUrl;
        private string Path { get; set; }
        private HttpMethod Method { get; set; }
        private Dictionary<string, string> Headers { get; }
        private Dictionary<string, string> Query { get; }
        private Dictionary<string, string> FormBody { get; }

        private object Body { get; set; }

        public async Task<T> ExecuteAsync<T>()
        {
                var request = MakeRequest();
                var cancelationToken = new CancellationTokenSource();
                var response = await _client.SendAsync(request, cancelationToken.Token);
                ClearParameters();
                var responseBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode && !response.StatusCode.Equals(HttpStatusCode.OK))
                    throw new HttpRequestException(responseBody, null);
                var resp = default(T);
                if (typeof(T) != typeof(string) && response.Content.Headers.ContentType != null &&
                    response.Content.Headers.ContentType.MediaType.Contains("application/json"))
                    resp = (T) JsonConvert.DeserializeObject(responseBody, typeof(T),
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
                if (responseBody.StartsWith("\"") && response.Content.Headers.ContentType != null &&
                    response.Content.Headers.ContentType.MediaType.Contains("application/json"))
                    resp = (T) JsonConvert.DeserializeObject(responseBody, typeof(T),
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
                // if (resp is null && typeof(T) == typeof(string))
                //     resp = responseBody.getAs<T>();
                return resp;
        }

        public IHttpService AddHeader(string name, object value)
        {
            AddParameter("Header", name, value);
            return this;
        }

        public IHttpService AddBody(string name, object value)
        {
            AddParameter("Body", name, value);
            return this;
        }

        public IHttpService AddQuery(string name, object value)
        {
            AddParameter("Query", name, value);
            return this;
        }

        public IHttpService AddPath(string name, object value)
        {
            throw new System.NotImplementedException();
        }

        public IHttpService Get(string url)
        {
            Method = HttpMethod.Get;
            Path = url;
            return this;
        }

        public IHttpService Post(string url)
        {
            Method = HttpMethod.Post;
            Path = url;
            return this;
        }

        public IHttpService Delete(string url)
        {
            Method = HttpMethod.Delete;
            Path = url;
            return this;
        }

        public IHttpService AddParameter(string type, string name, object value)
        {
            var adaptedValue =
                value is decimal ? value.ToString().Replace(".", "").Replace(",", ".") : value.ToString();
            switch (type)
            {
                case "Body":
                    Body = value;
                    break;
                case "FormBody":
                    FormBody.Add(name, adaptedValue);
                    break;
                case "Header":
                    Headers.Add(name, adaptedValue);
                    break;
                case "Query":
                    Query.Add(name, adaptedValue);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(type), type);
            }
            return this;
        }

        private HttpRequestMessage MakeRequest()
        {
            var request = new HttpRequestMessage();
            if (Method is null)
                throw new System.Exception("Method not set");
            request.Method = Method;
            if (Body != null || FormBody != null && FormBody.Count > 0)
                request.Content = Body is null
                    ? new FormUrlEncodedContent(FormBody)
                    : (HttpContent) new StringContent(Body.ToString(), Encoding.UTF8, "application/json");
            foreach (var (key, value) in Headers)
            {
                request.Headers.Add(key, value);
            }
            var queryString = GenerateQueryString(Query);
            var uri = System.Uri.EscapeUriString($"{_baseUrl}{Path}?{queryString}");
            request.RequestUri = new System.Uri(uri);
            return request;
        }

        private static string GenerateQueryString(Dictionary<string, string> query)
        {
            var queryString = new StringBuilder();
            var qtd = 0;
            foreach (var (key, value) in query)
            {
                queryString.Append(key).Append("=").Append(value);
                if (++qtd < query.Count)
                    queryString.Append("&");
            }
            return queryString.ToString();
        }

        private void ClearParameters()
        {
            Body = null;
            Query.Clear();
            Headers.Clear();
            FormBody.Clear();
        }
    }
}