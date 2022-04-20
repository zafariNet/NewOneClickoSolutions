using OneClickSolutions.Infrastructure.GuardToolkit;

namespace OneClickSolutions.Infrastructure.Web.Results
{
    public class JavaScriptRedirectResult : JavaScriptResult
    {
        private const string RedirectScriptFormat = "window.location = '{0}';";

        public JavaScriptRedirectResult(string redirectUrl)
        {
            Guard.ArgumentNotEmpty(redirectUrl, nameof(redirectUrl));

            Script = string.Format(RedirectScriptFormat, redirectUrl);
        }
    }
}