using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Api.DAL
{
    public class ChangeClaimsRepository
    {

        private List<Claim> _claimes;

        public ChangeClaimsRepository()
        {
            _claimes = new List<Claim>();
        }

       public bool AddClaim(Claim claim)
        {
            _claimes.Add(claim);

            return true;
        }
        public bool AddClaim(IEnumerable<Claim> claim)
        {
            _claimes.AddRange(claim);
            return true;
        }

        public List<Claim> GetClaims()
        {
            return _claimes;
        }

        public bool Delete(Claim claim)
        {
  
            _claimes.Remove(claim);
            return true;
        } 

    }
}
