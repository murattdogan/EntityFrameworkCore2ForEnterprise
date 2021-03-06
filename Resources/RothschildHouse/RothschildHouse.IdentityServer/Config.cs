﻿using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;

namespace RothschildHouse.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
            => new List<ApiResource>
            {
                new ApiResource("RothschildHouseApi", "RothschildHouse API")
            };

        public static IEnumerable<Client> GetClients()
            => new List<Client>
            {
                new Client
                {
                    ClientId = "rothschildhouseclient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("rothschildhousesecret1".Sha256())
                    },
                    AllowedScopes =
                    {
                        "RothschildHouseApi"
                    }
                },
                new Client
                {
                    ClientId = "rothschildhousecustomerclient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("rothschildhousesecret1".Sha256())
                    },
                    AllowedScopes =
                    {
                        "RothschildHouseApi"
                    },
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Role, "Customer")
                    }
                }
            };
    }
}
