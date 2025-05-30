namespace MamaFit.BusinessObjects.DTO.AuthDto
{
    public class GoogleJwtPayload
    {
        public string iss { get; set; }
        public string azp { get; set; }
        public string aud { get; set; }
        public string sub { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; }
        public string at_hash { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public long iat { get; set; } // Unix timestamp (issued at)
        public long exp { get; set; } // Unix timestamp (expiration)
    }
}
