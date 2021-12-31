// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServerAspNetIdentity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // machine to machine client
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                },

                //new Client
                //{
                //    ClientId = "tes",
                //    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, //основной сценарий входа
                //    RequireClientSecret = false, //Client Secret в браузере не понадобится, выключаем
                //    AllowedScopes = { 
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile},//для получения инфы о пользователе по /connect/userinfo
                //    AllowOfflineAccess = true //включает рефреш-токен
                //},
                
                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                   // AllowOfflineAccess = true,
                   
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                   
                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },


                   AlwaysSendClientClaims = false,
                   AlwaysIncludeUserClaimsInIdToken = false,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                },
                new Client
                {
                    ClientId = "mvc2",
                   // ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    //RefreshTokenUsage = TokenUsage.OneTimeOnly,
                 //   RequireConsent = false,
                   // AllowOfflineAccess = true,
                    RequireClientSecret = false,
                  RequirePkce = true,
                   AllowAccessTokensViaBrowser = true,
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:4200/sign-up" },
                   
                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:4200" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        //IdentityServerConstants.StandardScopes.Profile,
                    }
                },

                new Client
                {
                    ClientId = "myTestClient",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    //RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    //AllowOfflineAccess = true,
                    RequirePkce =false,
                    RequireClientSecret = false,
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:4200" },
                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:4200" },

                    //FrontChannelLogoutUri = "http://localhost:4200",

                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                }
            };
    }
}