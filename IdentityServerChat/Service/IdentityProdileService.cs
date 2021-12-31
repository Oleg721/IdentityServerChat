using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EntityServerTests.Service
{
    public class IdentityProfileService : IProfileService
    {

        //private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityProfileService( UserManager<ApplicationUser> userManager)
        {
           // _claimsFactory = claimsFactory;
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                throw new ArgumentException("");
            }
            var rolesClaim = (await _userManager.GetRolesAsync(user))
                .Select(e => new Claim(ClaimTypes.Role, e));
            var claims = await _userManager.GetClaimsAsync(user) as List<Claim>;
            claims.AddRange(rolesClaim);
            

            //Add more claims like this
            //claims.Add(new System.Security.Claims.Claim("MyProfileID", user.Id));

            context.IssuedClaims = claims;
    
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }

        //public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        //{
        //    var sub = context.Subject.GetSubjectId();
        //    var user = await _userManager.FindByIdAsync(sub);
        //    if (user == null)
        //    {
        //        throw new ArgumentException("");
        //    }

        //    var principal = await _claimsFactory.CreateAsync(user);
        //    var claims = principal.Claims.ToList();

        //    //Add more claims like this
        //    //claims.Add(new System.Security.Claims.Claim("MyProfileID", user.Id));

        //    context.IssuedClaims = claims;
        //}

        //public async Task IsActiveAsync(IsActiveContext context)
        //{
        //    var sub = context.Subject.GetSubjectId();
        //    var user = await _userManager.FindByIdAsync(sub);
        //    context.IsActive = user != null;
        //}
    }

}
