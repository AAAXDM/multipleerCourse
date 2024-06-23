﻿using System.ComponentModel.DataAnnotations;

namespace Srver
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public ushort Score { get; set; }
        public bool IsOnline { get; set; } 
    }
}
