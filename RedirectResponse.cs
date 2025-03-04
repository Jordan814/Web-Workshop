using System.Net;
using HttpStatusCode = WebServer.Server.Http.HttpStatusCode;

namespace WebServer.Server.Results;

public class RedirectResponse : HtmlResponse
{
    public RedirectResponse(string location) : base((System.Net.HttpStatusCode)HttpStatusCode.Found)
    {
        this.Headers.Add("Location",location);
    }
}