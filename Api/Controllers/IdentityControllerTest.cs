// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Api.Policy;
using Api.Service;
using System.Threading.Tasks;
using Api.Contracts;
using Api.DTO;

namespace Api.Controllers
{
    [AllowAnonymous]
    [Route("identityT/")]
    public class IdentityControllerTest : ControllerBase
    {
        private ChangePolicyFactory _changePolicyFactory;
        private IUserService<ChatUserDto, string> _userService;
        public IdentityControllerTest(ChangePolicyFactory changePolicyFactory, IUserService<ChatUserDto, string> userService)
        {
            _changePolicyFactory = changePolicyFactory;
            _userService = userService;
        }

        [AllowAnonymous]
        [Route("t")]
        [HttpGet]
        public async Task<IActionResult> test(string id)
        {
            var user =  await _userService.GetAsync(id);
            return Ok(user);
        }

        [Route("claim/not")]
        [HttpPost]
        public IActionResult NotClaim()
        {
            return Ok("not have any claims");
        }

        [Route("claim/email")]
        [HttpPost]
        [Authorize(Policy = "ApiClame")]
        public IActionResult EmailClaim()
        {
            return Ok("Email claim");
        }


        [Route("claim/role")]
        [HttpPost]
        [Authorize(Roles ="adm")]
        public IActionResult RoleClaim()
        {
            return Ok("role claim");
        }

        [Route("claim/spec")]
        [HttpPost]
        [Authorize(Policy = "ApiSpacialClaim")]
        public IActionResult SpecClaim()
        {
            return Ok("SPEc claim");
        }


        [Route("claim/change")]
        [HttpPost]
        [Authorize]
        public IActionResult AddClaim(ChangePolici polici)
        {
          //  var t = ControllerContext.ActionDescriptor.EndpointMetadata;

            var i = polici.PoliciName;
            return Ok($"Changed - {i}");
        }


        [Route("claim/changeTest")]
        [HttpPost]
        [Authorize(Policy = "change")]
        public IActionResult TestChange()
        {

            return Ok($"Changed ENTER!!!!");
        }



        public class ChangePolici
        {
            public string PoliciName { get; set; }
            public string Claime { get; set; }
        }
    }


}