// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Api.Policy;

namespace Api.Controllers
{
    [Route("identityT/")]
    public class IdentityControllerTest : ControllerBase
    {
        ChangePolicyFactory _changePolicyFactory;
        public IdentityControllerTest(ChangePolicyFactory changePolicyFactory)
        {
            _changePolicyFactory = changePolicyFactory;
        }

        [AllowAnonymous]
        [Route("t")]
        [HttpGet]
        public IActionResult test()
        {
            return Ok("TEST GET");
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