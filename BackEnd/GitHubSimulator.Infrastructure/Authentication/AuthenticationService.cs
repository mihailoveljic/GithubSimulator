using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Infrastructure.Authentication
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IJwtProvider jwtProvider, IUserRepository userRepository)
        {
            _jwtProvider = jwtProvider;
            _userRepository = userRepository;
        }

        public async Task<Result<string>> Authenticate(string email, string password)
        {
            var loginUser = await _userRepository.GetByUsername(email);

            loginUser ??= await _userRepository.GetByEmail(Mail.Create(email));

            if (loginUser is null)
            {
                return Result.Failure<string>("User not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, loginUser.AccountCredentials.PasswordHash))
            {
                return Result.Failure<string>("Invalid password");
            }

            return _jwtProvider.Generate(loginUser);
        }
    }
}
