using System.Net;

namespace WebServer.Server.Results;

public class HtmlResponse : ContentResponse
{
    public HtmlResponse(string html) 
        : base(html, "text/html; charset=UTF-8\r\n")
    {
    }

    public HtmlResponse(HttpStatusCode statusCode) 
        : base("", "text/html; charset=UTF-8\r\n")
    {
        this.StatusCode = (Http.HttpStatusCode)statusCode;
    }
}
