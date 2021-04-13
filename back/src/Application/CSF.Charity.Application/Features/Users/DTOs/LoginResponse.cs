namespace CSF.Charity.Application.Features.Users.DTOs
{
    public class LoginResponse
    {
        public LoginResponse()
        {
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string AssociationId { get; set; }
    }

}
