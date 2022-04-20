using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OneclickSolutions.Api.Authorization;
using OneClickSolutions.Application.Configuration;
using OneClickSolutions.Domain.Identity;
using OneClickSolutions.Infrastructure.Collections;
using OneClickSolutions.Infrastructure.Cryptography;
using OneClickSolutions.Infrastructure.Data;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;

namespace OneclickSolutions.Api.Application
{
    public class DbSetup : IDbSetup
    {
        private readonly IDbContext _dbContext;
        private readonly IOptionsSnapshot<ProjectOptions> _settings;
        private readonly IUserPasswordHashAlgorithm _password;
        private readonly ILogger<DbSetup> _logger;

        public DbSetup(IDbContext dbContext,
            IOptionsSnapshot<ProjectOptions> settings,
            IUserPasswordHashAlgorithm password,
            ILogger<DbSetup> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _password = password ?? throw new ArgumentNullException(nameof(password));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Seed()
        {
            SeedIdentity();
        }

        private void SeedIdentity()
        {
            var role = _dbContext.Set<Role>()
                .Include(r => r.Permissions)
                .FirstOrDefault(r => r.Name == RoleNames.Administrators);
            if (role == null)
            {
                role = new Role
                {
                    Name = RoleNames.Administrators,
                    NormalizedName = RoleNames.Administrators.ToUpperInvariant(),
                    Description = "Removing default system role cause problems!",
                };
                _dbContext.Set<Role>().Add(role);
            }
            else
            {
                _logger.LogInformation($"{nameof(Seed)}: `{RoleNames.Administrators}` role already exists.");
            }

            var rolePermissionNames = role.Permissions.Select(a => a.Name).ToList();
            var allPermissionNames = PermissionNames.NameList;

            var newPermissions = allPermissionNames.Except(rolePermissionNames)
                .Select(permissionName => new RolePermission { Name = permissionName }).ToList();
            role.Permissions.AddRange(newPermissions);

            _logger.LogInformation(
                $"{nameof(Seed)}: newPermissions: {string.Join("\n", newPermissions.Select(a => a.Name))}");

            var admin = _settings.Value.UserSeed;

            var user = _dbContext.Set<User>()
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.NormalizedUserName == admin.UserName.ToUpperInvariant());
            if (user == null)
            {
                user = new User
                {
                    UserName = admin.UserName,
                    NormalizedUserName = admin.UserName.ToUpperInvariant(),
                    DisplayName = admin.DisplayName,
                    NormalizedDisplayName = admin.DisplayName, //.NormalizePersianTitle(),
                    IsActive = true,
                    PasswordHash = _password.HashPassword(admin.Password),
                    SecurityToken = Guid.NewGuid().ToString("N")
                };

                _dbContext.Set<User>().Add(user);
            }
            else
            {
                _logger.LogInformation($"{nameof(Seed)}: admin user already exists.");
            }

            if (user.Roles.All(ur => ur.RoleId != role.Id))
            {
                _dbContext.Set<UserRole>().Add(new UserRole { Role = role, User = user });
            }
            else
            {
                _logger.LogInformation($"{nameof(Seed)}: admin user already is assigned to `{RoleNames.Administrators}` role.");
            }

            user.Permissions.Clear();

            _dbContext.SaveChanges();
        }
    }
}