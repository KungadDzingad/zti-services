using System;
using System.Collections.Generic;

namespace Gateway.Models.Entities
{
    public partial class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpirationTime { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
