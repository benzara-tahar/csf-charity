namespace CSF.Charity.Application.Features.Users.DTOs
{
    public class UserDetailsResponse
    {
        public UserDetailsResponse()
        {
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string AssociationId { get; set; }
    }

}
