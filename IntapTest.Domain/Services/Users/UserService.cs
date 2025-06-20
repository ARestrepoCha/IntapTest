using FluentResults;
using IntapTest.Data.Entities;
using IntapTest.Shared.AppConfigurations.Sections;
using IntapTest.Shared.Dtos.Requests;
using IntapTest.Shared.Dtos.Responses;
using IntapTest.Shared.Enums;
using IntapTest.Shared.Errors;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace IntapTest.Domain.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly JWTConfiguration _jwtConfiguration;

        public UserService(UserManager<User> userManager,
            IOptions<JWTConfiguration> jwtConfiguration)
        {
            _userManager = userManager;
            _jwtConfiguration = jwtConfiguration.Value;
        }

        public async Task<Result<bool>> CreateUser(CreateUserRequestDto createUserRequest)
        {
            var existingUser = await _userManager.FindByEmailAsync(createUserRequest.Email);

            if (existingUser != null)
                return Result.Fail(new InvalidInputError("Ya existe un usuario registrado con ese email."));

            var entity = createUserRequest.Adapt<User>();
            entity.UserName = createUserRequest.Email;
            entity.SecurityStamp = Guid.NewGuid().ToString();
            entity.PasswordHash = _userManager.PasswordHasher.HashPassword(existingUser, createUserRequest.Password);
            entity.CreatedOn = DateTime.UtcNow;
            entity.LastModifiedOn = DateTime.UtcNow;
            entity.IsActive = true;
            entity.IsDeleted = false;

            var registerUser = await _userManager.CreateAsync(entity, createUserRequest.Password);

            var registerRole = await _userManager.AddToRoleAsync(entity, nameof(RoleEnum.Employee));
            if (registerRole == null || !registerRole.Succeeded)
                return Result.Fail(new InvalidInputError("Error assigning role."));

            return Result.Ok(true);
        }

        public async Task<Result<LoginResponseDto>> Login(LoginUserRequestDto loginUser)
        {
            var user = await _userManager.FindByEmailAsync(loginUser.UserName);
            if (user == null || !user.IsActive)
                return Result.Fail(new InvalidInputError("Usuario o contraseña incorrectos"));

            if (!await _userManager.CheckPasswordAsync(user, loginUser.Password))
                return Result.Fail(new InvalidInputError("Usuario o contraseña incorrectos"));

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Any())
                return Result.Fail(new ResourceNotFoundError($"Role no he encontrado, username: {loginUser.UserName}"));
            return Result.Ok(await GenerateTokens(user, roles));
        }

        private async Task<LoginResponseDto> GenerateTokens(User user, IList<string> roles)
        {
            var secretKey = DeriveKeyBytes(_jwtConfiguration.TokenSecretKey);
            var expiryTimeSpan = Convert.ToDouble(_jwtConfiguration.ExpiryInMinutes);
            var secretRefreshKey = DeriveKeyBytes(_jwtConfiguration.AccessRefreshToken);
            var expiryRefreshTimeSpan = Convert.ToDouble(_jwtConfiguration.AccessRefreshTokenExpirationMinutes);
            var roleName = roles[0];

            var claims = new ClaimsIdentity(new List<Claim>
            {
                new Claim("userName", $"{user.UserName}"),
                new Claim("role", roleName),
                new Claim("id", $"{user.Id}"),
                new Claim("FullName", $"{user.FullName}")
            });

            var accessToken = GenerateToken(
                secretKey,
                null,
                null,
                expiryTimeSpan,
                claims);

            await _userManager.SetAuthenticationTokenAsync(user, "IntapTest", "MainToken", accessToken.Token);

            var refreshToken = GenerateToken(
                secretRefreshKey,
                 null,
                 null,
                 expiryRefreshTimeSpan,
                 claims);

            await _userManager.SetAuthenticationTokenAsync(user, "IntapTest", "RefreshToken", refreshToken.Token);

            var loginResponseDto = new LoginResponseDto
            {
                ExpirationTime = $"{accessToken.ExpirationTime}",
                TokenValue = accessToken.Token,
                ExpirationRefreshTime = $"{refreshToken.ExpirationTime}",
                RefreshTokenValue = refreshToken.Token
            };

            return loginResponseDto;
        }

        private static byte[] DeriveKeyBytes(string secret)
        {
            byte[] keyBytes = Convert.FromBase64String(secret);
            int desiredLength = 256 / 8;

            using Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(keyBytes, new byte[] { 0x01, 0x02, 0x03, 0x04 }, 10000);
            // Deriva bytes adicionales hasta alcanzar la longitud deseada
            while (deriveBytes.GetBytes(desiredLength).Length < desiredLength) { }

            return deriveBytes.GetBytes(desiredLength);
        }

        private static AccessTokenResponseDto GenerateToken(byte[] secretKey,
            string issuer,
            string audience,
            double expirationTime,
            ClaimsIdentity claims)
        {
            var credentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = issuer,
                Audience = audience,
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(expirationTime),
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                SigningCredentials = credentials
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            var jwToken = jwtHandler.CreateJwtSecurityToken(tokenDescriptor);
            var jwtToken = jwtHandler.WriteToken(jwToken);

            return new AccessTokenResponseDto
            {
                Token = jwtToken,
                ExpirationTime = DateTime.Now.AddMinutes(expirationTime)
            };
        }
    }
}
