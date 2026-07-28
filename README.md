# Net-Project
# Chat P2P (C# / WinForms)

Ứng dụng chat ngang hàng (peer-to-peer) viết bằng C# + WinForms, cho phép chat 1-1 và chat nhóm giữa nhiều máy trong LAN mà không cần server trung tâm. Đồ án môn Lập trình mạng.

## Tính năng

- Đăng nhập/đăng ký tài khoản (lưu cục bộ trên từng máy, mật khẩu được hash)
- Kết nối trực tiếp tới peer khác bằng địa chỉ IP:port
- Chat 1-1 (private message) - dữ liệu đi thẳng giữa 2 máy, không qua trung gian
- Tạo nhóm chat, mời thành viên, gửi tin nhắn nhóm
- Xem danh sách peer đang kết nối và danh sách nhóm đã tham gia
- Lưu lịch sử chat, xem lại theo từng trang (nút "Tải thêm lịch sử")
- **Reply tin nhắn**: trả lời một tin nhắn cụ thể, hiển thị kèm nội dung tin nhắn gốc được reply ngay phía trên tin nhắn trả lời
- **Forward tin nhắn**: chuyển tiếp một tin nhắn sang peer khác hoặc cuộc trò chuyện/nhóm khác
- **Avatar**: hiển thị ảnh đại diện của người dùng trong danh sách peer và trong khung tin nhắn
- **Emoji**: chọn emoji từ bảng chọn (emoji picker) và hiển thị emoji trực tiếp trong nội dung tin nhắn
- Toàn bộ thao tác qua giao diện WinForms, không cần dòng lệnh

## Vì sao gọi là P2P?

Mô hình truyền thống (client-server) có 1 máy chủ đứng giữa: mọi tin nhắn đều đi qua server rồi mới tới người nhận, và server là nơi duy nhất lưu dữ liệu.

Ở đây thì khác: mỗi máy chạy cùng một chương trình, và đóng đồng thời 2 vai trò:

| Vai trò | Việc làm |
|---|---|
| Server (thụ động) | Mở 1 cổng (`TcpListener`), lắng nghe chờ người khác kết nối vào |
| Client (chủ động) | Tự kết nối sang máy khác (`TcpClient`) khi người dùng nhập IP:port |

Khi 2 máy đã bắt tay xong, kết nối giữa chúng là ngang hàng - không phân biệt "ai phục vụ ai" nữa, cả 2 đều có thể gửi và nhận bất cứ lúc nào.

```
   Peer A                              Peer B
┌───────────┐      kết nối trực tiếp  ┌───────────┐
│ TcpListener│◀────────────────────────│ TcpClient │
│ TcpClient │────────────────────────▶│TcpListener│
└───────────┘                         └───────────┘
     │                                      │
  SQLite riêng                         SQLite riêng
 (chatp2p_local.db)                  (chatp2p_local.db)
```

Không có máy nào là "trung tâm" - nếu 1 máy tắt, các máy còn lại vẫn chat được với nhau bình thường (miễn là chúng đã kết nối trực tiếp).

## Cấu trúc project

```
ChatP2P/
├── ChatP2P.sln
├── ChatP2P.UI/                     # Project WinForms - nơi người dùng thao tác
│   ├── Forms/
│   │   ├── LoginForm.cs             # Màn hình đăng ký/đăng nhập + nhập port lắng nghe
│   │   ├── MainForm.cs              # Sidebar (peer, nhóm) + khung chat + nút "Tải thêm lịch sử"
│   │   ├── EmojiPickerForm.cs       # Bảng chọn emoji (control/popup)
│   │   └── ForwardMessageForm.cs    # Chọn peer/nhóm đích khi forward tin nhắn
│   ├── Controls/
│   │   ├── MessageBubbleControl.cs  # UserControl vẽ 1 bong bóng chat (avatar, reply-preview, emoji)
│   │   └── PeerListItemControl.cs   # UserControl hiển thị avatar + tên peer trong sidebar
│   └── Program.cs
│
├── ChatP2P.Core/                   # Class library - lớp lõi (không phụ thuộc UI)
│   ├── Network/
│   │   └── PeerNode.cs              # + StartListening()  -> vai trò server (lắng nghe)
│   │                                #   + ConnectToPeer()  -> vai trò client (kết nối ra)
│   │                                #   + SendToPeer() / SendToMany()
│   │                                #   + Đóng gói/mở gói JSON qua TCP (length-prefixed)
│   ├── Models/
│   │   ├── ChatMessage.cs           # Id, Type, Content, ReplyToId, ForwardedFromId, ...
│   │   ├── Peer.cs
│   │   └── GroupChat.cs
│   └── Services/
│       └── AvatarService.cs         # Lưu/đọc file avatar cục bộ (Base64 hoặc đường dẫn ảnh)
│
├── ChatP2P.Data/                   # Truy cập dữ liệu SQLite cục bộ
│   ├── AppDbContext.cs              # Dùng Microsoft.Data.Sqlite hoặc EF Core Sqlite
│   └── Repositories/
│       ├── AccountRepository.cs
│       ├── PeerRepository.cs
│       ├── GroupRepository.cs
│       └── MessageRepository.cs     # Query phân trang lịch sử chat
│
├── ChatP2P.Data/chatp2p_local.db   # File database SQLite - tự sinh khi chạy lần đầu
│                                    #   (không commit lên git, mỗi máy có file riêng)
│
└── README.md                        # Tài liệu này
```

Không có project `Server` riêng như mô hình client-server, vì mỗi máy chạy chung 1 ứng dụng WinForms duy nhất - bản thân nó vừa là client vừa là server nhờ `PeerNode`.

## Giao thức tin nhắn

Dữ liệu trao đổi giữa 2 peer là JSON, có 4 byte header ghi độ dài gói tin (giúp TCP tách đúng từng message, tránh bị dính gói) - tương tự bản Python, chỉ khác là serialize/deserialize bằng `System.Text.Json`.

| Loại (`Type`) | Khi nào gửi | Nội dung |
|---|---|---|
| `hello` | Ngay khi 2 peer vừa kết nối | Tên hiển thị + avatar (Base64/URL) của mình |
| `private_message` | Chat 1-1 | `Content`, `ReplyToId` (nếu có), `ForwardedFromId` (nếu có) |
| `group_message` | Chat nhóm | `GroupId`, `GroupName`, `Content`, `ReplyToId`, `ForwardedFromId` |
| `group_invite` | Khi ai đó tạo nhóm mới có bạn trong đó | `GroupId`, `GroupName`, `Members` |
| `avatar_update` | Khi người dùng đổi avatar | Avatar mới (Base64), gửi lại cho các peer đã kết nối |

`ReplyToId` chứa `Id` của tin nhắn được trả lời; khi hiển thị, ứng dụng tự tra trong lịch sử cục bộ để vẽ khung xem trước nội dung gốc phía trên tin nhắn reply. `ForwardedFromId` (kèm `ForwardedFromSender`) dùng để hiển thị nhãn "Đã chuyển tiếp" trên bong bóng chat.

Ví dụ 1 gói tin nhắn nhóm có reply và emoji trong nội dung:

```json
{
  "type": "group_message",
  "group_id": "3f2a-...",
  "group_name": "Nhom_LTM",
  "content": "Đồng ý 👍 để mai họp nhé 😄",
  "reply_to_id": "a1b2-..."
}
```

## Lưu trữ dữ liệu

Không có database dùng chung - mỗi máy có 1 file SQLite riêng tên `chatp2p_local.db`, tự tạo bảng khi chạy lần đầu (không cần chạy file `.sql` thủ công).

Các bảng chính:

- `account` - tài khoản đăng nhập trên máy đó (`username`, `password_hash`, `salt`, `avatar_path`)
- `known_peers` - IP/port của các peer đã từng kết nối
- `groups_local` - danh sách nhóm (khóa chính là UUID/`Guid`, không phải số tự tăng)
- `group_members` - ai thuộc nhóm nào
- `messages` - toàn bộ tin nhắn (cả private và group), có thêm cột `reply_to_id`, `forwarded_from_id` và cột `sent_at` để phân trang
- `avatars` - lưu avatar của các peer đã biết (để hiển thị offline mà không cần peer đó đang online)

Vì sao `group_id` là UUID? Vì không có server nào cấp số thứ tự chung cho tất cả máy. Người tạo nhóm tự sinh 1 `Guid`, gửi kèm trong `group_invite` cho các thành viên - nhờ vậy mọi máy đều lưu đúng cùng 1 mã nhóm, dù mỗi máy có database độc lập.

Lịch sử chat được truy vấn phân trang bằng `LIMIT ... OFFSET ...`, mỗi lần tải thêm 20 tin nhắn cũ hơn.

## Cài đặt & chạy

Yêu cầu: .NET 8 SDK (hoặc mới hơn), Windows (WinForms chỉ chạy trên Windows).

Mỗi máy chạy:

```
dotnet build
dotnet run --project ChatP2P.UI
```

Hoặc mở `ChatP2P.sln` bằng Visual Studio, chọn `ChatP2P.UI` làm Startup Project rồi nhấn F5.

Các bước:

1. Đăng ký tài khoản (chỉ lưu trên máy đó), có thể chọn ảnh avatar → đăng nhập
2. Nhập port muốn lắng nghe (ví dụ `6000`)
3. Lấy IP LAN của máy muốn kết nối tới bằng lệnh `ipconfig`, tìm dòng IPv4 Address
4. Nhập `IP:port` đó vào ô "Kết nối" trên giao diện
5. Sau khi kết nối, chọn peer trong danh sách để chat 1-1, hoặc chọn nhiều peer rồi bấm "Tạo nhóm"
6. Nhấn giữ/right-click 1 tin nhắn để chọn **Reply** hoặc **Forward**
7. Bấm biểu tượng emoji cạnh ô nhập để mở bảng chọn emoji

Muốn thử trên 1 máy: chạy 2 instance của `ChatP2P.UI.exe`, đăng nhập 2 tài khoản khác nhau, mỗi cửa sổ dùng port khác nhau (VD `6000` và `6001`), rồi kết nối `127.0.0.1:6001`.

## Lưu ý kỹ thuật quan trọng

**Bind `0.0.0.0`, không phải `127.0.0.1`**

```csharp
_listener = new TcpListener(IPAddress.Any, listenPort); // IPAddress.Any = 0.0.0.0
_listener.Start();
```

`127.0.0.1` chỉ nhận kết nối từ chính máy đó. Muốn máy khác trong LAN kết nối vào được, phải bind `IPAddress.Any` (mọi network interface).

**Mỗi peer vừa mở cổng vừa gọi ra ngoài**

Khác với client-server (chỉ server cần mở port), ở đây máy nào cũng phải mở 1 port lắng nghe. Nếu 2 máy không cùng LAN (khác NAT/router) thì cần thêm bước port forwarding - đây là giới hạn tất yếu của mô hình P2P thuần.

**Cập nhật UI từ thread khác phải dùng `Invoke`**

Vì `PeerNode` đọc dữ liệu mạng trên thread nền (background thread/`async`), mọi thao tác cập nhật control WinForms (thêm dòng chat, cập nhật danh sách peer...) phải gọi qua `this.Invoke(...)` hoặc `BeginInvoke(...)` để tránh lỗi cross-thread.

**Nhóm hoạt động theo kiểu "gửi lần lượt" (flood), không phải broadcast qua server**

Khi gửi tin nhắn nhóm, máy gửi tự lặp qua từng thành viên đang kết nối và gửi trực tiếp tới từng người. Nếu 1 thành viên đang offline lúc đó, họ sẽ không nhận được tin nhắn này (không có server nào lưu hộ để gửi lại sau).

## Hướng phát triển thêm

- **Tự động khám phá peer (auto-discovery)**: Hiện phải nhập tay IP:port. Có thể thêm `UdpClient` broadcast: mỗi peer định kỳ gửi gói tin "tôi đang online tại IP:port này" ra toàn mạng LAN, các peer khác lắng nghe và tự hiển thị vào danh sách "Peer khả dụng" mà không cần gõ tay.
- **Gửi lại tin nhắn khi peer offline (offline messaging)**: Vì không có server lưu hộ, tin nhắn gửi lúc người nhận offline sẽ mất. Có thể khắc phục bằng cách: máy gửi tự lưu các tin chưa gửi thành công vào 1 hàng đợi cục bộ (`pending_messages`), rồi tự động gửi lại khi phát hiện peer đó kết nối lại.
- **Mã hóa đầu-cuối (end-to-end encryption)**: Hiện dữ liệu truyền dạng JSON thuần, không mã hóa. Có thể thêm bước trao đổi khóa bằng ECDH (`System.Security.Cryptography.ECDiffieHellman`) ngay sau `hello`, rồi mã hóa nội dung bằng AES trước khi gửi.
- **Truyền file / hình ảnh dạng đính kèm**: Hiện avatar và emoji đã đi qua kênh riêng; có thể mở rộng thêm `type: "file_transfer"`, gửi file theo từng chunk kèm metadata (tên file, kích thước, checksum) để client ghép lại.
- **Xác nhận đã đọc / đang gõ (read receipt, typing indicator)**: Thêm 2 loại message mới: `typing` (gửi khi người dùng đang gõ, không lưu vào lịch sử) và `read_receipt` (gửi khi mở khung chat, đánh dấu tin nhắn cũ là "đã xem").
- **Danh sách "Peer đã biết" (`known_peers`) hiện chưa dùng để kết nối nhanh**: Bảng này đã có sẵn trong `PeerRepository` nhưng UI chưa có nút "Kết nối lại" từ danh sách này - có thể bổ sung để không phải gõ lại IP:port mỗi lần mở app.
- **NAT traversal cho peer khác mạng**: Hiện chỉ chạy tốt trong cùng LAN. Muốn 2 máy ở 2 mạng khác nhau (khác NAT) kết nối được mà không cần port forwarding thủ công, có thể tìm hiểu kỹ thuật STUN/TURN hoặc dùng 1 rendezvous server nhẹ chỉ để 2 peer "làm quen" nhau rồi vẫn chat trực tiếp (không phá vỡ tinh thần P2P, server đó không route tin nhắn).
