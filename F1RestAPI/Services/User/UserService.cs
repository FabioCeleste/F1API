using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ErrorOr;
using F1RestAPI.Contracts.Users;
using F1RestAPI.Data;
using F1RestAPI.Models;
using F1RestAPI.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace F1RestAPI.Services.Users;

public class UserService : IUserService
{
    private readonly DataContext _dataContext;
    private readonly JwtSettings _jwtSettings;

    public UserService(DataContext dataContext, IOptions<JwtSettings> jwtSettings)
    {
        _dataContext = dataContext;
        _jwtSettings = jwtSettings.Value;
    }

    private string _createPasswordHash(string password)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        return passwordHash;
    }

    private bool _verifyPasswordHash(string password, string storedHash)
    {
        bool isMatch = BCrypt.Net.BCrypt.Verify(password, storedHash);
        return isMatch;
    }

    private string _createToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);


        List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "User"),
            };

        var symmetricKey = new SymmetricSecurityKey(key);

        var creds = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public async Task<bool> CreateUser(CreateUserRequest createUserRequest)
    {
        try
        {
            var passwordHash = _createPasswordHash(createUserRequest.password);

            if (passwordHash == null)
            {
                return false;
            }

            var user = new User(0, createUserRequest.username, passwordHash);

            _dataContext.Users.Add(user);

            await _dataContext.SaveChangesAsync();

            return true;
        }
        catch (System.Exception)
        {

            return false;
        }

    }

    public async Task<ErrorOr<string>> Login(CreateUserRequest loginRequest)
    {
        try
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.username);


            if (user == null || !_verifyPasswordHash(loginRequest.password, user.PasswordHash))
            {
                return Error.NotFound("User not found");
            }

            var token = _createToken(user);

            return token;
        }
        catch (Exception ex)
        {
            return Error.Failure($"An error occurred: {ex.Message}");
        }
    }
}