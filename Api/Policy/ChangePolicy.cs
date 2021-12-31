using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Policy
{
    public class ChangePolicyFactory 
    {
        public static AuthorizationPolicy Policy;
        private IAuthorizationRequirement _requirement;
        public ChangePolicyFactory()
        {
            Console.WriteLine("Stand UP!!!");
            _requirement = new ClaimsAuthorizationRequirement("myOwnClaim", new List<string>() { "someClaim", "fds"}) { };
            Policy = GetPolicy();
        } 

        private AuthorizationPolicy GetPolicy()
        {
            Console.WriteLine("ITS WORK!!!");
            return new AuthorizationPolicy(new List<IAuthorizationRequirement>() {_requirement },
                new List<string>() {})
            {
                
            };
        }

    }
}
