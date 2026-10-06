using FUNewsManagement.Models;
using FUNewsManagement.Models.DTOs;
using FUNewsManagement.Repositories.Interfaces;
using FUNewsManagement.Services.Interfaces;
using FUNewsManagement.Services.Utilities;

namespace FUNewsManagement.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ISystemAccountRepository _accountRepository;

        public AuthService(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public LoginResponse Authenticate(LoginRequest request, string adminEmail, string adminPassword,
            string jwtSecret, string jwtIssuer, string jwtAudience)
        {
            string role = null;
            string name = null;
            short? accountId = null;

            if (request.Email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) &&
                request.Password == adminPassword)
            {
                role = "Admin";
                name = "Administrator";
                accountId = null;
            }
            else
            {
                var account = _accountRepository.GetByEmailAndPassword(request.Email, request.Password);
                if (account == null)
                {
                    return null;
                }
                accountId = account.AccountID;
                name = account.AccountName;
                role = account.AccountRole == 1 ? "Staff" : account.AccountRole == 2 ? "Lecturer" : "Staff";
            }

            var token = JwtTokenGenerator.Instance.GenerateToken(
                request.Email, role, name, accountId, jwtSecret, jwtIssuer, jwtAudience);

            return new LoginResponse
            {
                Token = token,
                Email = request.Email,
                Role = role,
                Name = name,
                AccountID = accountId
            };
        }
    }
}
