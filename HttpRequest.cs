using System.Diagnostics;

namespace WebServer.Server.Http;

public class HttpRequest
{
    private const string newLine = "\r\n";
    
    
    public HttpMethod Method { get; private set; }

    public string Path { get; private set; }

    public Dictionary<string,string> Query { get; private set; }

    public HttpHeaderCollection Headers { get; private set; } = new HttpHeaderCollection();

    public string Body { get; set; }

    public static HttpRequest Parse(string request)
    {
        Console.WriteLine($"Raw request:\n{request}"); 

        if (string.IsNullOrWhiteSpace(request))
        {
            Console.WriteLine("Received an empty HTTP request. Skipping...");
            return null; // Instead of throwing exception just ignore
        }

        
        var lines = request.Trim().Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        
        if (lines.Length == 0 || string.IsNullOrWhiteSpace(lines[0]))
        {
            throw new InvalidOperationException("Invalid HTTP request: Empty request line.");
        }

        var startLine = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);

        if (startLine.Length < 2)
        {
            throw new InvalidOperationException($"Invalid HTTP request: {lines[0]}");
        }


        var method = ParseHttpMethod(startLine[0]);

        var url = startLine[1];

        var (path, query) = ParseUrl(url);

        var headerCollection = ParseHttpHeaders(lines.Skip(1));

        var bodyLines = lines.Skip(headerCollection.Count + 2).ToArray();
        
        var body = string.Join(newLine ,bodyLines);
        
        

        return new HttpRequest()
        {
            Method = method,
            Path = path,
            Query = query,
            Headers = headerCollection,
            Body = body

        };
        
        

    }

   

    private static HttpMethod ParseHttpMethod(string method)
    {
        return method.ToUpper() switch
        {
            "GET" => HttpMethod.Get,
            "POST" => HttpMethod.Post,
            "PUT" => HttpMethod.Put,
            "DELETE" => HttpMethod.Delete,
            _ => throw new System.InvalidOperationException($"Method {method} is not supported")


        };
    }
    private static (string , Dictionary<string,string> ) ParseUrl(string url)
    {
        var urlParts = url.Split('?');

        var path = urlParts[0];

        var query = urlParts.Length > 1 ? ParseQuery(urlParts[1]) : new Dictionary<string, string>();
        
        return (path, query);
    }

    private static Dictionary<string, string> ParseQuery(string queryString)
    {
        return  queryString.Split('&')
                .Select(part => part.Split('=')).Where(part => part.Length == 2)
                .ToDictionary(p => p[0], p => p[1]);


        

    }
    private static HttpHeaderCollection ParseHttpHeaders(IEnumerable<string>  headerLines)
    {
        var headerCollection = new HttpHeaderCollection();

        foreach (var headerLine in headerLines)
        {
            if (headerLine == string.Empty)
            {
                break;
            }
            var indexOfColon = headerLine.IndexOf(":");

            if (indexOfColon < 0 )
            {
                throw new InvalidOperationException("Request is not valid");
            }

            var headerName = headerLine.Substring(0, indexOfColon);
            var headerValue = headerLine.Substring(indexOfColon + 1).Trim();
            
            var header = new HttpHeader(headerName, headerValue);

            headerCollection.Add(headerName, headerValue);

        }

        return headerCollection;
    }
    
    
}