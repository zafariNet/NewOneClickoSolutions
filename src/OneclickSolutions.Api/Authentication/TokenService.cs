using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OneClickSolutions.Domain.Identity;
using OneClickSolutions.Infrastructure.Cryptography;
using OneClickSolutions.Infrastructure.Dependency;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;
using OneClickSolutions.Infrastructure.Extensions;
using Claim = System.Security.Claims.Claim;

namespace OneclickSolutions.Api.Authentication;
    public interface ITokenService : IScopedDependency
    {
        Task<bool> IsValidTokenAsync(long userId, string token);
        Task<Token> NewTokenAsync(long userId, IEnumerable<Claim> claims);
        Task InvalidateTokensAsync(string userId = null);
    }

public class TokenService : ITokenService
{
    private readonly IDbContext _dbContext;
    private readonly IOptionsSnapshot<TokenOptions> _options;
    private readonly ISystemClock _clock;
    private readonly ISecurityService _security;
    private readonly DbSet<UserToken> _tokens;

    public TokenService(IDbContext dbContext,
        IOptionsSnapshot<TokenOptions> options,
        ISystemClock clock,
        ISecurityService security)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _security = security ?? throw new ArgumentNullException(nameof(security));

        _tokens = _dbContext.Set<UserToken>();
    }

    public async Task<Token> NewTokenAsync(long userId, IEnumerable<Claim> claims)
    {
        var token = Token.New(_options.Value, claims);

        await AddUserTokenAsync(userId, token.Value);

        return token;
    }

    public async Task InvalidateTokensAsync(string userId)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            if (_options.Value.LogoutEverywhereEnabled)
            {
                await DeleteUserTokensAsync(userId.To<long>());
            }
        }

        await DeleteExpiredTokensAsync();

        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsValidTokenAsync(long userId, string token)
    {
        var tokenHash = ComputeHash(token);

        var userToken = await _tokens.AsNoTracking().FirstOrDefaultAsync(
            x => x.TokenHash == tokenHash && x.UserId == userId);

        return userToken?.TokenExpirationDateTime >= _clock.UtcNow;
    }

    private async Task AddUserTokenAsync(UserToken token)
    {
        if (!_options.Value.LoginFromSameUserEnabled)
        {
            await DeleteUserTokensAsync(token.UserId);
        }

        _tokens.Add(token);

        await _dbContext.SaveChangesAsync();
    }

    private async Task AddUserTokenAsync(long userId, string token)
    {
        var now = _clock.UtcNow;
        var userToken = new UserToken
        {
            UserId = userId,
            TokenHash = ComputeHash(token),
            //TODO Currection
            //TokenExpirationDateTime = now.Add(_options.Value.TokenExpiration)
            //  TokenExpirationDateTime = now.AddHours(1);
        };

        await AddUserTokenAsync(userToken);
    }

    private async Task DeleteExpiredTokensAsync()
    {
        var now = _clock.UtcNow;
        await _tokens.Where(x => x.TokenExpirationDateTime < now)
            .ForEachAsync(userToken => { _tokens.Remove(userToken); });
    }


    private async Task DeleteUserTokensAsync(long userId)
    {
        await _tokens.Where(x => x.UserId == userId)
            .ForEachAsync(userToken => { _tokens.Remove(userToken); });
    }

    private string ComputeHash(string input)
    {
        return _security.ComputeSha256Hash(input);
    }
}