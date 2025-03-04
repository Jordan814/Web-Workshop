using System.Net.Mime;
using WebServer.Server;
using WebServer.Server.Http;
using WebServer.Server.Results;

namespace Web_Workshop.Controllers;

public class HomeController : Controller
{
    public HomeController(HttpRequest request) : base(request)
    {
    }

    public HttpResponse LocalRedirect()
    {
        return Redirect("/Cats");
    }
    public HttpResponse Index()
    {
        return Text("Hello from Yordan!");
    }
    
    
}