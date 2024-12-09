using System.Net;
using System.Net.Sockets;
using System.Text;
using AESCrypto;
Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;
int port = 5000;
AeSCrypto aeSCrypto = new AeSCrypto("0123456789ABCDEF0123456789ABCDEF", "0123456789ABCDEF");
TcpListener server = new TcpListener(IPAddress.Any,port);
server.Start();
Console.WriteLine($"Server started in to port: {port}");
try
{
    while (true)
    {
        Console.WriteLine("Waiting on connection");
        TcpClient client = server.AcceptTcpClient();
        var stream = client.GetStream();
        Console.WriteLine("Connected!");
        var writer = new StreamWriter(stream, System.Text.Encoding.UTF8) { AutoFlush = true };
        var reader = new StreamReader(stream, Encoding.UTF8);
        try
        {
            
            Task.Run(()=>
            {
                while (true)
                {
                    string clientMessage = reader.ReadLine();
                    if (clientMessage.Length > 0)
                    {
                        string decrypted = aeSCrypto.Decrypt(clientMessage);
                        Console.WriteLine("Message: " + decrypted);
                    }
                        
                }
            }
            );
            while (true)
            {
                string message = Console.ReadLine();
                 
                string encrypted = aeSCrypto.Encrypt(message);
                Console.WriteLine(encrypted);
                // Запись сообщения обратно клиенту
                writer.WriteLine(encrypted);
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine("Ошибка при работе с клиентом: " + ex.Message);
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}