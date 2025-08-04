using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

class TcpClientApp
{
    static void Main()
    {
        const string serverIp = "127.0.0.1";
        const int serverPort = 5000;

        using var client = new TcpClient(serverIp, serverPort);
        using var stream = client.GetStream();

        Console.WriteLine("Connected to server.");

        while (true)
        {
            int processType = PromptProcessType();
            string jsonMessage = processType switch
            {
                0 => BuildGenerateCodeMessage(),
                1 => BuildUseCodeMessage(),
                _ => null
            };

            if (string.IsNullOrEmpty(jsonMessage))
                continue;

            SendMessage(stream, jsonMessage);
            string response = ReceiveResponse(stream);
            Console.WriteLine("Response: " + response);
            Console.WriteLine();
        }
    }

    static int PromptProcessType()
    {
        while (true)
        {
            Console.Write("Enter Process Type [0 - Generate Code / 1 - Use Code]: ");
            string input = Console.ReadLine();
            if (input == "0" || input == "1")
                return int.Parse(input);
            Console.WriteLine("Invalid input. Please enter 0 or 1.");
        }
    }

    static string BuildGenerateCodeMessage()
    {
        ushort count = PromptUShort("Code Count: ");
        byte length = PromptByte("Code Length [7-8]: ");

        var message = new
        {
            Type = 0,
            Params = new
            {
                Count = count,
                Length = length
            }
        };
        return JsonSerializer.Serialize(message);
    }

    static string BuildUseCodeMessage()
    {
        Console.Write("Code: ");
        string code = Console.ReadLine();

        var message = new
        {
            Type = 1,
            Params = new
            {
                Code = code
            }
        };
        return JsonSerializer.Serialize(message);
    }

    static ushort PromptUShort(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (ushort.TryParse(Console.ReadLine(), out ushort value))
                return value;
            Console.WriteLine("Invalid number. Please enter a valid unsigned short value.");
        }
    }

    static byte PromptByte(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (byte.TryParse(Console.ReadLine(), out byte value) && value >= 7 && value <= 8)
                return value;
            Console.WriteLine("Invalid length. Please enter 7 or 8.");
        }
    }

    static void SendMessage(NetworkStream stream, string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    static string ReceiveResponse(NetworkStream stream)
    {
        byte[] responseBuffer = new byte[1024];
        int byteCount = stream.Read(responseBuffer, 0, responseBuffer.Length);
        return Encoding.UTF8.GetString(responseBuffer, 0, byteCount);
    }
}