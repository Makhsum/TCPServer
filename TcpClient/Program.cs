using AESCrypto;
using System.IO;
using System.Net.Sockets;
Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;
string serverAddress = "7.tcp.eu.ngrok.io";
int port = 18304;
AeSCrypto aeSCrypto = new AeSCrypto("0123456789ABCDEF0123456789ABCDEF", "0123456789ABCDEF");
try
{
	TcpClient client = new TcpClient(serverAddress, port);
    Console.WriteLine("Connected");
    

    using (NetworkStream stream = client.GetStream())
    using (StreamReader reader = new StreamReader(stream))
	{
        var writer = new StreamWriter(stream, System.Text.Encoding.UTF8) { AutoFlush = true };
       

        Task.Run(() =>
        {
            while (true)
            {
                string serverMessage = reader.ReadLine();
                if (serverMessage.Length > 0)
                {
                    string decrypted = aeSCrypto.Decrypt(serverMessage);
                    Console.WriteLine($"Message: {decrypted}");
                }
            }
        });
        while (true)
        {
            string message = Console.ReadLine();
            string encrypted = aeSCrypto.Encrypt(message);
            writer.WriteLine(encrypted);

        }

    }
}
catch (Exception ex)
{

    Console.WriteLine(ex.Message);
}
Console.ReadLine();