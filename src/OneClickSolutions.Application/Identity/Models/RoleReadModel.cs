using OneClickSolutions.Infrastructure.Application;

namespace OneClickSolutions.Application.Identity.Models;
public class RoleReadModel : ReadModel<long>
{
    public string Name { get; set; }
    public string Description { get; set; }
}