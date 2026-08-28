using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ChatP2P.Core.Models;

namespace ChatP2P.Core.Network
{
    // Gói tin trao đổi qua mạng - bọc ChatMessage hoặc các loại thông điệp khác (kết nối, presence...)
    public class NetworkPacket
    {
        public string PacketType { get; set; } = "Message";
        public string SenderName { get; set; } = string.Empty;
        public int SenderPort { get; set; }
        public ChatMessage? Message { get; set; }
    }

    public class PeerNode
    {
        private TcpListener? _listener;
        private CancellationTokenSource? _listenCts;

        // Danh sách kết nối TCP đang mở, key = "ip:port" của peer đối diện
        private readonly Dictionary<string, TcpClient> _connections = new();

        public string LocalName { get; }
        public int LocalPort { get; }

        // Sự kiện bắn ra khi có tin nhắn/gói tin mới đến
        public event Action<NetworkPacket, string>? PacketReceived;
        public event Action<string>? PeerDisconnected; 

        public PeerNode(string localName, int localPort)
        {
            LocalName = localName;
            LocalPort = localPort;
        }

        // VAI TRÒ SERVER: Nhận kết nối đến
        public void StartListening()
        {
            _listenCts = new CancellationTokenSource();
            _listener = new TcpListener(IPAddress.Any, LocalPort);
            _listener.Start();

            _ = AcceptLoopAsync(_listenCts.Token);
        }

        public void StopListening()
        {
            _listenCts?.Cancel();
            _listener?.Stop();
        }

        private async Task AcceptLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var client = await _listener!.AcceptTcpClientAsync(token);
                    var remoteIp = ((IPEndPoint)client.Client.RemoteEndPoint!).Address.ToString();
                    var key = $"{remoteIp}:{LocalPort}";
                    _connections[key] = client;

                    _ = ReceiveLoopAsync(client, key, token);
                }
            }
            catch (OperationCanceledException) { /* dừng bình thường khi StopListening */ }
            catch (Exception ex)
            {
                Console.WriteLine($"[PeerNode] AcceptLoop error: {ex.Message}");
            }
        }

        // VAI TRÒ CLIENT: chủ động kết nối ra peer khác
        public async Task<bool> ConnectToPeer(string ip, int port)
        {
            try
            {
                var client = new TcpClient();
                await client.ConnectAsync(ip, port);

                var key = $"{ip}:{port}";
                _connections[key] = client;

                _ = ReceiveLoopAsync(client, key, CancellationToken.None);

                // Gửi gói Hello để bên kia biết mình là ai
                var hello = new NetworkPacket
                {
                    PacketType = "Hello",
                    SenderName = LocalName,
                    SenderPort = LocalPort
                };
                await SendPacketAsync(client, hello);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PeerNode] ConnectToPeer error: {ex.Message}");
                return false;
            }
        }

        // GỬI DỮ LIỆU
        public async Task SendToPeer(string peerKey, ChatMessage message)
        {
            if (!_connections.TryGetValue(peerKey, out var client))
            {
                Console.WriteLine($"[PeerNode] Không tìm thấy kết nối tới {peerKey}");
                return;
            }

            var packet = new NetworkPacket
            {
                PacketType = "Message",
                SenderName = LocalName,
                SenderPort = LocalPort,
                Message = message
            };

            await SendPacketAsync(client, packet);
        }

        public async Task SendToMany(IEnumerable<string> peerKeys, ChatMessage message)
        {
            foreach (var key in peerKeys)
                await SendToPeer(key, message);
        }

        private async Task SendPacketAsync(TcpClient client, NetworkPacket packet)
        {
            var json = JsonSerializer.Serialize(packet);
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            var lengthPrefix = BitConverter.GetBytes(jsonBytes.Length);

            var stream = client.GetStream();
            await stream.WriteAsync(lengthPrefix);
            await stream.WriteAsync(jsonBytes);
        }

        // NHẬN DỮ LIỆU 
        private async Task ReceiveLoopAsync(TcpClient client, string peerKey, CancellationToken token)
        {
            var stream = client.GetStream();
            var lengthBuffer = new byte[4];

            try
            {
                while (!token.IsCancellationRequested && client.Connected)
                {
                    // Đọc 4 byte length-prefix trước
                    int read = await ReadExactAsync(stream, lengthBuffer, 4, token);
                    if (read == 0) break;

                    int messageLength = BitConverter.ToInt32(lengthBuffer, 0);
                    var messageBuffer = new byte[messageLength];
                    await ReadExactAsync(stream, messageBuffer, messageLength, token);

                    var json = Encoding.UTF8.GetString(messageBuffer);
                    var packet = JsonSerializer.Deserialize<NetworkPacket>(json);

                    if (packet != null)
                    {
                        var remoteIp = ((IPEndPoint)client.Client.RemoteEndPoint!).Address.ToString();
                        PacketReceived?.Invoke(packet, remoteIp);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PeerNode] ReceiveLoop error ({peerKey}): {ex.Message}");
            }
            finally
            {
                _connections.Remove(peerKey);
                client.Close();
                PeerDisconnected?.Invoke(peerKey);
            }
        }

        // Đọc đúng "count" byte từ stream (TCP có thể trả về ít hơn mỗi lần đọc)
        private static async Task<int> ReadExactAsync(NetworkStream stream, byte[] buffer, int count, CancellationToken token)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                int read = await stream.ReadAsync(buffer.AsMemory(totalRead, count - totalRead), token);
                if (read == 0) return 0; 
                totalRead += read;
            }
            return totalRead;
        }

        public void Disconnect(string peerKey)
        {
            if (_connections.TryGetValue(peerKey, out var client))
            {
                client.Close();
                _connections.Remove(peerKey);
            }
        }

        public void DisconnectAll()
        {
            foreach (var client in _connections.Values)
                client.Close();
            _connections.Clear();
            StopListening();
        }
    }
}