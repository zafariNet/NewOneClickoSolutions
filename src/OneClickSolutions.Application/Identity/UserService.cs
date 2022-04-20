
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OneClickSolutions.Application.Identity.Models;
using OneClickSolutions.Domain.Identity;
using OneClickSolutions.Infrastructure.Application;
using OneClickSolutions.Infrastructure.Cryptography;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Application;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Querying;
using OneClickSolutions.Infrastructure.Eventing;
using OneClickSolutions.Infrastructure.Querying;

namespace OneClickSolutions.Application.Identity;
public interface IUserService : IEntityService<long, UserReadModel, UserModel>
{
}

public class UserService : EntityService<User, long, UserReadModel, UserModel>, IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserPasswordHashAlgorithm _password;

    public UserService(
        IDbContext dbContext,
        IEventBus bus,
        IUserPasswordHashAlgorithm password,
        IMapper mapper) : base(dbContext, bus)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _password = password ?? throw new ArgumentNullException(nameof(password));
    }

    protected override IQueryable<User> FindEntityQueryable => base.FindEntityQueryable.Include(u => u.Roles)
        .Include(u => u.Permissions);

    public override Task<IPagedResult<UserReadModel>> FetchPagedListAsync(FilteredPagedRequest request,
        CancellationToken cancellationToken = default)
    {
        return EntitySet.AsNoTracking()
            .Select(u => new UserReadModel
            {
                Id = u.Id,
                IsActive = u.IsActive,
                UserName = u.UserName,
                DisplayName = u.DisplayName,
                LastLoggedInDateTime = u.LastLoggedInDateTime
            }).ToPagedListAsync(request, cancellationToken);
    }

    protected override void MapToEntity(UserModel model, User user)
    {
        _mapper.Map(model, user);

        ResetSecurityToken(user, model);
        ReplayPasswordHash(user, model);
    }

    protected override UserModel MapToModel(User user)
    {
        return _mapper.Map<UserModel>(user);
    }

    private static void ResetSecurityToken(User user, UserModel model)
    {
        if (!model.ShouldResetSecurityToken()) return;

        user.ResetSecurityToken();
    }

    private void ReplayPasswordHash(User user, UserModel model)
    {
        if (!model.ShouldMapPasswordHash()) return;

        user.PasswordHash = _password.HashPassword(model.Password);
    }
}