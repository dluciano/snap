using System.Collections.Generic;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Snap.Server
{
    internal class IdentityConfiguration
    {
        public ICollection<OAuthOptions> OAuthProvidersOptions { get; } = new List<OAuthOptions>();
        public ICollection<TwitterOptions> TwitterProvidersOptions { get; } = new List<TwitterOptions>();
        public ICollection<GoogleOptions> GoogleProvidersOptions { get; } = new List<GoogleOptions>();
        public ICollection<MicrosoftAccountOptions> MicrosoftProvidersOptions { get; } = new List<MicrosoftAccountOptions>();
        public ICollection<FacebookOptions> FacebookProvidersOptions { get; } = new List<FacebookOptions>();
        public ICollection<IdentityServerAuthenticationOptions> IdentityServerProvidersOptions { get; } = new List<IdentityServerAuthenticationOptions>();

        public void ConfigureServices(AuthenticationBuilder builder)
        {
            foreach (var provider in OAuthProvidersOptions)
            {
                builder
                    .AddOAuth(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        options.ClientId = provider.ClientId;
                        options.ClientId = provider.ClientSecret;
                        options.Validate();
                    });
            }
            foreach (var provider in TwitterProvidersOptions)
            {
                builder.AddTwitter(options =>
                {
                    options.ConsumerKey = provider.ConsumerKey;
                    options.ConsumerSecret = provider.ConsumerSecret;
                    options.Validate();
                });
            }
            foreach (var provider in MicrosoftProvidersOptions)
            {
                builder.AddMicrosoftAccount(options =>
                {
                    options.ClientId = provider.ClientId;
                    options.ClientSecret = provider.ClientSecret;
                    options.Validate();
                });
            }
            foreach (var provider in FacebookProvidersOptions)
            {
                builder.AddFacebook(options =>
                {
                    options.ClientId = provider.ClientId;
                    options.ClientSecret = provider.ClientSecret;
                    options.Validate();
                });
            }
            foreach (var provider in GoogleProvidersOptions)
            {
                builder.AddGoogle(options =>
                {
                    options.ClientId = provider.ClientId;
                    options.ClientSecret = provider.ClientSecret;
                    options.Validate();
                });
            }
            foreach (var provider in IdentityServerProvidersOptions)
            {
                builder.AddIdentityServerAuthentication(options =>
                {
                    options.Authority = provider.Authority;
                    options.TokenRetriever = CustomTokenRetriever.FromHeaderAndQueryString;
                    options.Validate();
                });
            }
        }
    }
}