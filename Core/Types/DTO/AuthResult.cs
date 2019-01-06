using Core.Types.Enumerations;
using System;

namespace Core.Types.DTO
{
    public class AuthResult
    {
        public Guid SessionGuid { get; set; }
        public EAccessGroup? UserAccess { get; set; }
    }
}