using Microsoft.AspNetCore.Mvc;
using OneclickSolutions.Api.Authorization;
using OneClickSolutions.Application.Common;
using OneClickSolutions.Application.Identity;
using OneClickSolutions.Application.Identity.Models;
using OneClickSolutions.Infrastructure.Web.API;

namespace OneclickSolutions.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : EntityController<IUserService, long, UserReadModel, UserModel>
    {
        private readonly ILookupService _lookupService;

        public UsersController(IUserService service, ILookupService lookupService) : base(service)
        {
            _lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
        }

        protected override string CreatePermissionName => PermissionNames.Users_Create;
        protected override string EditPermissionName => PermissionNames.Users_Edit;
        protected override string ViewPermissionName => PermissionNames.Users_View;
        protected override string DeletePermissionName => PermissionNames.Users_Delete;

        [HttpGet("[action]")]
        // [PermissionAuthorize(PermissionNames.Users_Create, PermissionNames.Users_Edit)]
        public async Task<IActionResult> RoleList()
        {
            var result = await _lookupService.FetchRolesAsync();
            return Ok(result);
        }
    }
}