using OneClickSolutions.Infrastructure.IO;
using Microsoft.AspNetCore.Hosting;

namespace OneClickSolutions.Infrastructure.Web.IO
{
    internal class EnvironmentPath : IEnvironmentPath
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EnvironmentPath(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        public string MapPath(string path)
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, path);
            return filePath;
        }
    }
}