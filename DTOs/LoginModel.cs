﻿namespace authentication_jwt_dotnet.DTOs
{
    public class LoginModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}