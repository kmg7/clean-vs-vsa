namespace Api.Requests;

public class CreateUserRequest
{
    public string Username { get; set; }
    public string EmailAddress { get; set; }
    public string ClerkId { get; set; }
}
