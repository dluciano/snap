using System;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Snap.Server
{
    /// <summary>
    /// @view https://github.com/IdentityServer/IdentityServer4/issues/2349
    /// </summary>
    internal class CustomTokenRetriever
    {
        internal const string TokenItemsKey = "idsrv4:tokenvalidation:token";
        // custom token key change it to the one you use for sending the access_token to the server
        // during websocket handshake
        internal const string SignalRTokenKey = "access_token";

        private static Func<HttpRequest, string> AuthHeaderTokenRetriever { get; set; }
        private static Func<HttpRequest, string> QueryStringTokenRetriever { get; set; }

        static CustomTokenRetriever()
        {
            AuthHeaderTokenRetriever = TokenRetrieval.FromAuthorizationHeader();
            QueryStringTokenRetriever = TokenRetrieval.FromQueryString();
        }

        public static string FromHeaderAndQueryString(HttpRequest request)
        {
            var token = AuthHeaderTokenRetriever(request);
            if (!string.IsNullOrWhiteSpace(token))
                return token;

            token = QueryStringTokenRetriever(request);

            if (!string.IsNullOrWhiteSpace(token))
                return token;

            token = request.HttpContext.Items[TokenItemsKey] as string;

            if (!string.IsNullOrWhiteSpace(token))
                return token;

            if (request.Query.TryGetValue(SignalRTokenKey, out StringValues extract))
            {
                token = extract.ToString();
            }

            return token;
        }
    }
}
