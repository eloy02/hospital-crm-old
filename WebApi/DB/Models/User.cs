using Core.Types.Enumerations;
using System;

namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public Guid UserGuid { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PatronymicName { get; set; }
        public EAccessGroup AccessGroup { get; set; }
    }
}