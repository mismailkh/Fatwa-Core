using FATWA_WEB.Data;
using FATWA_WEB.Extensions;
using Radzen;
using System.Net.Http.Headers;
using System.Text;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services
{
    public abstract class GenericODataService<TEntity> : GenericODataService<TEntity, int>
        where TEntity : class
    {
        protected GenericODataService(string _entitySetName, LoginState _loginState) : base(_entitySetName, _loginState)
        {
        }
    }

    public abstract class GenericODataService<TEntity, TKey> : IGenericODataService<TEntity, TKey>, IDisposable
        where TEntity : class
    {
        protected readonly Uri baseUri;
        protected readonly string entitySetName;
        protected readonly HttpClient httpClient;
        private readonly HttpClientHandler httpClientHandler; // TODO: This should not be used in production.. its just to bypass certificate validation for localhost..
        private bool isDisposed;
        private readonly LoginState loginState;


        public GenericODataService(string _entitySetName, LoginState _loginState)
        {
            httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            httpClient = new HttpClient(httpClientHandler);

            baseUri = new Uri("https://localhost:7250/api/v1/");
            this.entitySetName = _entitySetName;
            this.loginState = _loginState;
        }

        public virtual async Task<ApiResponse<TEntity>> FindAsync(
            string filter = default,
            int? top = default,
            int? skip = default,
            string orderby = default,
            string expand = default,
            string select = default,
            bool? count = default)
        {
            var uri = new Uri(baseUri, entitySetName);
            //uri = uri.GetODataUri(filter: filter, top: top, skip: skip, orderby: orderby, expand: expand, select: select, count: count); 
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginState.Token);
            var response = await httpClient.SendAsync(httpRequestMessage);
            var result = await response.ReadAsync<ApiResponse<TEntity>>();
            return result;
        }

        public virtual async Task<TEntity> FindOneAsync(TKey key)
        {
            try
            {
                var uri = new Uri(baseUri, $"{entitySetName}/{key}");
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginState.Token);
                var response = await httpClient.SendAsync(httpRequestMessage);
                var result = await response.ReadAsync<TEntity>();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            var uri = new Uri(baseUri, entitySetName);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(ODataJsonSerializer.Serialize(entity), Encoding.UTF8, "application/json"),
            };
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginState.Token);

            var response = await httpClient.SendAsync(httpRequestMessage);
            return await response.ReadAsync<TEntity>();
        }

        public virtual async Task<TEntity> UpdateAsync(TKey key, TEntity entity)
        {
            var uri = new Uri(baseUri, $"{entitySetName}");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, uri)
            {
                Content = new StringContent(ODataJsonSerializer.Serialize(entity), Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginState.Token);

            var response = await httpClient.SendAsync(httpRequestMessage);
            return await response.ReadAsync<TEntity>();
        }

        public virtual async Task<HttpResponseMessage> DeleteAsync(TKey key)
        {
            var uri = new Uri(baseUri, $"{entitySetName}/{key}");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginState.Token);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        #region IDisposable Members

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    httpClient.DisposeIfNotNull();
                    httpClientHandler.DisposeIfNotNull();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                isDisposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~GenericODataService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        #endregion IDisposable Members
    }
}
