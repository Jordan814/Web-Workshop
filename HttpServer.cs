using System.Net;
using System.Net.Sockets;
using System.Text;
using WebServer.Server.Http;
using WebServer.Server.Rauting;

namespace WebServer.Server;

public class HttpServer
{
    private readonly IPAddress ipAddress;

    private readonly int port;

    private readonly TcpListener listener;

    private readonly RoutingTable routingTable;
    
    public HttpServer(string ipAddress, int port, Action<IRoutingTable> routingTableConfiguration)
    {
         this.ipAddress = IPAddress.Parse(ipAddress); 

         this.port = port;

         listener = new TcpListener(this.ipAddress, this.port);

         this.routingTable = new RoutingTable();
         routingTableConfiguration(this.routingTable);
    }

    public HttpServer(int port,Action<IRoutingTable> routingTable) : this("127.0.0.1", port, routingTable )
    {
        
    }

    public HttpServer(Action<IRoutingTable> routingTable) : this(5000, routingTable)
    {
        
    }

    public async Task Start()
    {
        this.listener.Start();

        while (true)
        {
            var connection = await this.listener.AcceptTcpClientAsync();

            var networkStream = connection.GetStream(); // Stream of bites 

            var requestText = await this.ReadRequest(networkStream, connection);

            Console.WriteLine(requestText);

            var request = HttpRequest.Parse(requestText); 
            
            Console.WriteLine($"Received request:\n{request} characters");

            var response = this.routingTable.ExecuteRequest(request);
            
            await WriteResponse(networkStream, response);

            
            connection.Close();
        }
    }

    private async Task<string> ReadRequest(NetworkStream networkStream, TcpClient connection)
    {
        var buffer = new byte[1024];
        var requestBuilder = new StringBuilder();
    
        while (true)
        {
            var bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead == 0) break; // If there are no more data stop
            requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

            if (requestBuilder.Length > 10 * 1024) // Defence from bigger requests
            {
                connection.Close();
                return string.Empty;
            }

            // Stop if we reached the end
            if (requestBuilder.ToString().Contains("\r\n\r\n")) break;
        }

        return requestBuilder.ToString();
    }

    private async Task WriteResponse(NetworkStream networkStream, HttpResponse response)
    {

        var responseBytes = Encoding.UTF8.GetBytes(response.ToString());
        await networkStream.WriteAsync(responseBytes);
    }
}