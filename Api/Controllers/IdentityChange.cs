// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Api.DAL;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("/change")]
    [Authorize]
    [Authorize(Roles = "adm")]
    public class IdentityChange : ControllerBase
    {
        ChangeClaimsRepository _changeClaimsRepository;
        public IdentityChange(ChangeClaimsRepository changeClaimsRepository)
        {
            _changeClaimsRepository = changeClaimsRepository;
        }
        public IActionResult Get()
        {
            return Ok(_changeClaimsRepository.GetClaims());
        }

        [HttpPost]
        public IActionResult Add(string type, string value)
        {
            if (type == null || value == null)
            {
                return BadRequest("Empty content");
            }
            _changeClaimsRepository.AddClaim(new Claim(type, value));
            return Ok("added");
        }

        [HttpDelete]
        public IActionResult Delite(string type, string value)
        {
            _changeClaimsRepository.Delete(new Claim(type, value));
            return Ok("deleted");
        }

    }
}