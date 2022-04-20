using OneClickSolutions.Infrastructure.Application;

namespace OneClickSolutions.Application.Identity.Models;
public class RoleModel : MasterModel<long>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<PermissionModel> Permissions { get; set; } = new HashSet<PermissionModel>();
}