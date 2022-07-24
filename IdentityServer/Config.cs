using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients =>
              new Client[]
              {
                   //new Client
                   //{ //API
                   //     ClientId = "movieClient",
                   //     AllowedGrantTypes = GrantTypes.ClientCredentials,
                   //     ClientSecrets =
                   //     {
                   //         new Secret("secret".Sha256())
                   //     },
                   //     AllowedScopes = { "movieAPI" }
                   //},
                   new Client
                   { //MVC project
                       ClientId = "movies_mvc_client",
                       ClientName = "Movies MVC Web App",
                      // AllowedGrantTypes = GrantTypes.Code,   // user ID ad password
                       AllowedGrantTypes = GrantTypes.Hybrid,   // user ID ad password
                       RequirePkce = false,
                       AllowRememberConsent = false,
                       RedirectUris = new List<string>()
                       {
                           "https://localhost:5002/signin-oidc"
                       },
                       PostLogoutRedirectUris = new List<string>()
                       {
                           "https://localhost:5002/signout-callback-oidc"
                       },
                       ClientSecrets = new List<Secret>
                       {
                           new Secret("secret".Sha256())
                       },
                       AllowedScopes = new List<string>
                       {
                           IdentityServerConstants.StandardScopes.OpenId,
                           IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Address,    // added so that we can access more inofrmation like address and email when used logins to openid connect 
                           IdentityServerConstants.StandardScopes.Email,
                           "movieAPI",     // Added For Hybrid Flow
                           "roles"   // adding roles in test users for role base authorization  and here to ristrict access accordning to roles.
                       }
                   }
              };

        public static IEnumerable<ApiScope> ApiScopes =>
           new ApiScope[]
           {
               new ApiScope("movieAPI", "Movie API")
           };

        public static IEnumerable<ApiResource> ApiResources =>
          new ApiResource[]
          {
              //new ApiResource("movieAPI", "Movie API")
          };

        public static IEnumerable<IdentityResource> IdentityResources =>
          new IdentityResource[]
          {
              new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
              new IdentityResources.Address(),  // added so that we can access more inofrmation like address and email when used logins to openid connect
              new IdentityResources.Email(),
              new IdentityResource(             // adding roles in test users and here to ristrict access accordning to roles.
                    "roles",
                    "Your role(s)",
                    new List<string>() { "role" })
          };

        public static List<TestUser> TestUsers =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "Shashank",
                    Password = "Shashank",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.GivenName, "Shashank"),
                        new Claim(JwtClaimTypes.FamilyName, "Mishra")
                    }
                }
            };
    }
}