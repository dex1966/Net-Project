using ChatP2P.Core.Network;
using ChatP2P.Core.Models;

Console.WriteLine("=== ChatP2P Test Console ===");
Console.Write("Nhập tên của bạn: ");
var myName = Console.ReadLine() ?? "Unknown";

Console.Write("Nhập port để lắng nghe (VD: 5000 hoặc 5001): ");
var myPort = int.Parse(Console.ReadLine() ?? "5000");

var node = new PeerNode(myName, myPort);

// Đăng ký sự kiện: khi có tin nhắn đến thì in ra màn hình
node.PacketReceived += (packet, fromIp) =>
{
    if (packet.PacketType == "Hello")
    {
        Console.WriteLine($"\n[Hệ thống] {packet.SenderName} vừa kết nối từ {fromIp}:{packet.SenderPort}");
    }
    else if (packet.PacketType == "Message" && packet.Message != null)
    {
        Console.WriteLine($"\n[{packet.SenderName}]: {packet.Message.Content}");
    }
};

node.PeerDisconnected += (key) =>
{
    Console.WriteLine($"\n[Hệ thống] {key} đã ngắt kết nối");
};

// Bắt đầu lắng nghe
node.StartListening();
Console.WriteLine($"Đang lắng nghe ở port {myPort}...\n");

Console.WriteLine("Gõ lệnh:");
Console.WriteLine("  connect <ip> <port>   -> kết nối tới peer khác");
Console.WriteLine("  send <ip:port> <text> -> gửi tin nhắn");
Console.WriteLine("  exit                  -> thoát\n");

while (true)
{
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) continue;

    var parts = input.Split(' ', 3);

    if (parts[0] == "exit")
    {
        node.DisconnectAll();
        break;
    }
    else if (parts[0] == "connect" && parts.Length == 3)
    {
        var ip = parts[1];
        var port = int.Parse(parts[2]);
        var success = await node.ConnectToPeer(ip, port);
        Console.WriteLine(success ? "Kết nối thành công!" : "Kết nối thất bại.");
    }
    else if (parts[0] == "send" && parts.Length == 3)
    {
        var peerKey = parts[1]; // dạng "ip:port"
        var text = parts[2];

        var message = new ChatMessage
        {
            SenderId = myName,
            Content = text
        };

        await node.SendToPeer(peerKey, message);
        Console.WriteLine("Đã gửi.");
    }
    else
    {
        Console.WriteLine("Lệnh không hợp lệ.");
    }
}
Console.WriteLine("Hello, World!");
