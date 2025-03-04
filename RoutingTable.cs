using WebServer.Server.Common;
using WebServer.Server.Http;
using WebServer.Server.Results;
using HttpMethod = WebServer.Server.Http.HttpMethod;

namespace WebServer.Server.Rauting;

public class RoutingTable : IRoutingTable
{
    private readonly Dictionary<HttpMethod, Dictionary<string, Func<HttpRequest,HttpResponse>>> routes;

    public RoutingTable()
    {
        this.routes = new()
        {
            [HttpMethod.Get] = new Dictionary<string, Func<HttpRequest,HttpResponse>>(),
            [HttpMethod.Post] =new Dictionary<string, Func<HttpRequest,HttpResponse>>(),
            [HttpMethod.Put] = new Dictionary<string, Func<HttpRequest,HttpResponse>>(),
            [HttpMethod.Delete] = new Dictionary<string, Func<HttpRequest,HttpResponse>>(),

        };
    }

    public IRoutingTable Map( HttpMethod method, string path, HttpResponse response)
    {
        
        Guard.AgainstNull(response, nameof(response));

        return this.Map(method,path, request => response);
    }

    public IRoutingTable Map(HttpMethod method, string path, Func<HttpRequest, HttpResponse> responseFunction)
    {
        Guard.AgainstNull(path, nameof(path));
        Guard.AgainstNull(responseFunction, nameof(responseFunction));

        this.routes[method][path.ToLower()] = responseFunction;

        return this;
    }

    public IRoutingTable MapGet(string path, HttpResponse response)
    {
        return MapGet( path, request => response);
    }

    public IRoutingTable MapGet(string path, Func<HttpRequest, HttpResponse> responseFunction)
    {
         return Map(HttpMethod.Get, path, responseFunction);

    }



    public IRoutingTable MapPost(string path, HttpResponse response)
    {
        return MapPost( path, request => response);
    }

    public IRoutingTable MapPost(string path, Func<HttpRequest, HttpResponse> responseFunction)
    {
        return Map(HttpMethod.Post, path, responseFunction);

    }

    

    public HttpResponse ExecuteRequest(HttpRequest request)
    {
        var requestMethod = request.Method;
        var requestPath = request.Path.ToLower();

        if (!this.routes.ContainsKey(requestMethod) || !this.routes[requestMethod].ContainsKey(requestPath))
        {
            return new NotFoundResponse();
        }
        
        var responseFunction =  this.routes[requestMethod][requestPath];

        return responseFunction(request);
    }
}