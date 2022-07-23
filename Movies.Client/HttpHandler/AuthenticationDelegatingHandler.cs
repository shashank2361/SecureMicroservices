using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.Client.HttpHandler
{
    public class AuthenticationDelegatingHandler :DelegatingHandler
    {
        // intercepts all http requests  -> gets token and before sending request to the protected API

        //private readonly IHttpClientFactory _httpClientFactory;
        //private readonly ClientCredentialsTokenRequest _tokenRequest;


        //public AuthenticationDelegatingHandler(IHttpClientFactory httpClientFactory , ClientCredentialsTokenRequest clientCredentialsTokenRequest)
        //{

        //    _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

        //    _tokenRequest = clientCredentialsTokenRequest ?? throw new ArgumentNullException(nameof(clientCredentialsTokenRequest));
        //}
        // commented above because I I am using HTTPCOntextAccreessor to get token

        public IHttpContextAccessor _httpContextAccessor { get; }

        public AuthenticationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            //var httpClient = _httpClientFactory.CreateClient("IDPClient");
            //var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(_tokenRequest);

            //if (tokenResponse.IsError)
            //{
            //    throw new HttpRequestException("Something went wrong while requesting the access token");
            //}
            //request.SetBearerToken(tokenResponse.AccessToken);
            
            
            // Commented to Add Hybrid Flow

            // HybridFlow
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.SetBearerToken(accessToken);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
