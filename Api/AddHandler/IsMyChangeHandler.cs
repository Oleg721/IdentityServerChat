using Api.DAL;
using Api.Requirements;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.AddHandler
{
    public class IsMyChangeHandler : AuthorizationHandler<ChangeRequirement>
    {
        ChangeClaimsRepository _changeClaimsRepository;
        public IsMyChangeHandler(ChangeClaimsRepository changeClaimsRepository)
        {
            _changeClaimsRepository = changeClaimsRepository;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ChangeRequirement requirement)
        {
            var userClaims = context.User.Claims;
            var requireClames = _changeClaimsRepository.GetClaims();

            if (requireClames.Count() == 0)
            {
                return Task.FromResult(0);
            };

            foreach (var claim in requireClames)
            {
                var res = userClaims.Any(e => 
                    e.Type == claim.Type && e.Value == claim.Value);
                if(!res)
                {
                    return Task.FromResult(0);
                };
            }

            context.Succeed(requirement);

            return Task.FromResult(0);
        }
    }
}
