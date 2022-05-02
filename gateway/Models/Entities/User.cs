using System;
using System.Collections.Generic;

namespace Gateway.Models.Entities
{
    public partial class User
    {
        public User()
        {
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
