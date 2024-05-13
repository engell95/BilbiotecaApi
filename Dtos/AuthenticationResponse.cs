using System;

namespace BibliotecaApi.Dtos
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
