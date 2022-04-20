using Microsoft.AspNetCore.Mvc;
using OneclickSolutions.Api.Authorization;
using OneClickSolutions.Application.Identity;
using OneClickSolutions.Application.Identity.Models;
using OneClickSolutions.Infrastructure.Web.API;

namespace OneclickSolutions.Api.Controllers;
[Route("api/[controller]")]
public class RolesController : EntityController<IRoleService, long, RoleReadModel, RoleModel>
{
    public RolesController(IRoleService service) : base(service)
    {
    }

    protected override string CreatePermissionName => PermissionNames.Roles_Create;
    protected override string EditPermissionName => PermissionNames.Roles_Edit;
    protected override string ViewPermissionName => PermissionNames.Roles_View;
    protected override string DeletePermissionName => PermissionNames.Roles_Delete;
}