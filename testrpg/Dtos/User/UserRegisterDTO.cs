﻿using Azure.Identity;

namespace testrpg.Dtos.User
{
    public class UserRegisterDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
