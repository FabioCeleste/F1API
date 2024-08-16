namespace F1RestAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }

    public User() { }
    public User(int id, string username, string passwordHash)
    {
        Id = Id;
        Username = username;
        PasswordHash = passwordHash;
    }
}
