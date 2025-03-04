using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Unicode;
using Web_Workshop.Controllers;
using WebServer.Server;
using WebServer.Server.Controllers;
using WebServer.Server.Results;

public class Program
{
    //http://localhost:5000
    public static async Task Main() => await new HttpServer(
        routes => routes
            .MapGet<HomeController>("/", c => c.Index())
            .MapGet<HomeController>("/Cats", c => c.LocalRedirect())
            .MapGet<AnimalsController>("/Cats", c => c.Cats())
            .MapGet<AnimalsController>("/Dogs", c => c.Dogs())).Start();



}