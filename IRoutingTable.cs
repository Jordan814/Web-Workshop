using WebServer.Server.Http;
using HttpMethod = WebServer.Server.Http.HttpMethod;

namespace WebServer.Server.Rauting;

public interface IRoutingTable
{
    IRoutingTable Map( HttpMethod method, string path, HttpResponse response);
    
    IRoutingTable Map(HttpMethod method ,string path, Func<HttpRequest, HttpResponse> responseFunction);

    IRoutingTable MapGet(string path, HttpResponse response);

    IRoutingTable MapGet(string path, Func<HttpRequest, HttpResponse> responseFunction);
    

    IRoutingTable MapPost(string path, HttpResponse response);
    
    IRoutingTable MapPost(string path, Func<HttpRequest, HttpResponse> responseFunction);
    


}