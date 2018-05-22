namespace Auth.Models.DTOs
{
    public class LoginDTO
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public RoleType Role { get; set; }
    }
}