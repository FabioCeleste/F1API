namespace F1RestAPI.Contracts.Users;
public record CreateUserRequest(
    string username,
    string password
   );
