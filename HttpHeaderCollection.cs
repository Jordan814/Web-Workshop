using System.Collections;

namespace WebServer.Server.Http;

public class HttpHeaderCollection : IEnumerable<HttpHeader>
{
    private readonly Dictionary<string, HttpHeader> headers;

    public HttpHeaderCollection()
    {
        this.headers = new Dictionary<string, HttpHeader>();
    }

    public void Add(string name, string value)
    {
        var header = new HttpHeader(name, value);
        this.headers.Add(name, header);
    }

    public int Count => this.headers.Count;

    public IEnumerator<HttpHeader> GetEnumerator()
    {
        return this.headers.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}