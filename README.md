# Net-Project
# Chat P2P

Ứng dụng chat ngang hàng (peer-to-peer) bằng Python Socket + Tkinter, cho phép chat 1-1 và chat nhóm giữa nhiều máy trong LAN **mà không cần server trung tâm**. Đồ án môn Lập trình mạng.

---

## Tính năng

- Đăng nhập/đăng ký tài khoản (lưu cục bộ trên từng máy, mật khẩu được hash)
- Kết nối trực tiếp tới peer khác bằng địa chỉ IP:port
- Chat 1-1 (private message) - dữ liệu đi thẳng giữa 2 máy, không qua trung gian
- Tạo nhóm chat, mời thành viên, gửi tin nhắn nhóm
- Xem danh sách peer đang kết nối và danh sách nhóm đã tham gia
- Lưu lịch sử chat, xem lại theo từng trang (nút "Tải thêm lịch sử")
- Toàn bộ thao tác qua giao diện Tkinter, không cần dòng lệnh

---

## Vì sao gọi là P2P?

Mô hình truyền thống (client-server) có 1 máy chủ đứng giữa: mọi tin nhắn đều đi qua server rồi mới tới người nhận, và server là nơi duy nhất lưu dữ liệu.

Ở đây thì khác: **mỗi máy chạy cùng một chương trình**, và đóng đồng thời 2 vai trò:

| Vai trò | Việc làm |
|---|---|
| Server (thụ động) | Mở 1 cổng, lắng nghe chờ người khác kết nối vào |
| Client (chủ động) | Tự kết nối sang máy khác khi người dùng nhập IP:port |

Khi 2 máy đã bắt tay xong, kết nối giữa chúng là ngang hàng - không phân biệt "ai phục vụ ai" nữa, cả 2 đều có thể gửi và nhận bất cứ lúc nào.

```
   Peer A                              Peer B
┌───────────┐      kết nối trực tiếp  ┌───────────┐
│  Listener │◀────────────────────────│ Connector │
│  Connector│────────────────────────▶│  Listener │
└───────────┘                         └───────────┘
     │                                      │
  SQLite riêng                         SQLite riêng
 (chatp2p_local.db)                  (chatp2p_local.db)
```

Không có máy nào là "trung tâm" - nếu 1 máy tắt, các máy còn lại vẫn chat được với nhau bình thường (miễn là chúng đã kết nối trực tiếp).

---

## Cấu trúc project

```
chat_p2p/
├── app.py                  # Giao diện Tkinter - nơi người dùng thao tác
│                            #   + màn hình đăng ký/đăng nhập + nhập port lắng nghe
│                            #   + sidebar: kết nối peer, danh sách peer, danh sách nhóm
│                            #   + khung chat + nút "Tải thêm lịch sử"
│
├── network.py               # Lớp mạng - class PeerNode
│                            #   + start_listening()   -> vai trò server (lắng nghe)
│                            #   + connect_to_peer()   -> vai trò client (kết nối ra)
│                            #   + send_to_peer() / send_to_many()
│                            #   + đóng gói/mở gói JSON qua TCP (length-prefixed)
│
├── db.py                    # Lưu trữ SQLite cục bộ (tự tạo bảng khi chạy lần đầu)
│                            #   + account, known_peers, groups_local,
│                            #     group_members, messages
│
├── chatp2p_local.db         # File database SQLite - tự sinh ra khi chạy app.py
│                            #   (không commit lên git, mỗi máy có file riêng)
│
└── README.md                 # Tài liệu này
```

**Không có file server.py riêng** như mô hình client-server, vì mỗi máy chạy chung 1 chương trình `app.py` duy nhất - bản thân nó vừa là client vừa là server nhờ `network.py`.

---

## Giao thức tin nhắn

Dữ liệu trao đổi giữa 2 peer là JSON, có 4 byte header ghi độ dài gói tin (giúp TCP tách đúng từng message, tránh bị dính gói).

| Loại (`type`) | Khi nào gửi | Nội dung |
|---|---|---|
| `hello` | Ngay khi 2 peer vừa kết nối | Trao đổi tên hiển thị của nhau |
| `private_message` | Chat 1-1 | `content` |
| `group_message` | Chat nhóm | `group_id`, `group_name`, `content` |
| `group_invite` | Khi ai đó tạo nhóm mới có bạn trong đó | `group_id`, `group_name`, `members` |

Ví dụ 1 gói tin nhắn nhóm:
```json
{"type": "group_message", "group_id": "3f2a-...", "group_name": "Nhom_LTM", "content": "chào cả nhóm"}
```

---

## Lưu trữ dữ liệu

Không có database dùng chung - **mỗi máy có 1 file SQLite riêng** tên `chatp2p_local.db`, tự tạo bảng khi chạy lần đầu (không cần chạy file `.sql` thủ công).

Các bảng chính:

- `account` - tài khoản đăng nhập trên máy đó (username, password_hash, salt)
- `known_peers` - IP/port của các peer đã từng kết nối
- `groups_local` - danh sách nhóm (khóa chính là UUID, không phải số tự tăng)
- `group_members` - ai thuộc nhóm nào
- `messages` - toàn bộ tin nhắn (cả private và group), có cột `sent_at` để phân trang

**Vì sao group_id là UUID?** Vì không có server nào cấp số thứ tự chung cho tất cả máy. Người tạo nhóm tự sinh 1 UUID, gửi kèm trong `group_invite` cho các thành viên - nhờ vậy mọi máy đều lưu đúng cùng 1 mã nhóm, dù mỗi máy có database độc lập.

Lịch sử chat được truy vấn phân trang bằng `LIMIT ... OFFSET ...`, mỗi lần tải thêm 20 tin nhắn cũ hơn.

---

## Cài đặt & chạy

Yêu cầu: Python 3.9+ (không cần cài thêm thư viện ngoài - `sqlite3` và `tkinter` có sẵn).

Mỗi máy chạy cùng 1 lệnh:
```powershell
python app.py
```

Các bước:
1. Đăng ký tài khoản (chỉ lưu trên máy đó) → đăng nhập
2. Nhập port muốn lắng nghe (ví dụ `6000`)
3. Lấy IP LAN của máy muốn kết nối tới bằng lệnh `ipconfig`, tìm dòng `IPv4 Address`
4. Nhập IP:port đó vào ô "Kết nối" trên giao diện
5. Sau khi kết nối, chọn peer trong danh sách để chat 1-1, hoặc chọn nhiều peer rồi bấm "Tạo nhóm"

Muốn thử trên 1 máy: mở 2 cửa sổ `app.py`, đăng nhập 2 tài khoản khác nhau, mỗi cửa sổ dùng port khác nhau (VD 6000 và 6001), rồi kết nối `127.0.0.1:6001`.

---

## Lưu ý kỹ thuật quan trọng

**Bind 0.0.0.0, không phải 127.0.0.1**
```python
self._server_sock.bind(("0.0.0.0", self.listen_port))
```
`127.0.0.1` chỉ nhận kết nối từ chính máy đó. Muốn máy khác trong LAN kết nối vào được, phải bind `0.0.0.0` (mọi network interface).

**Mỗi peer vừa mở cổng vừa gọi ra ngoài**
Khác với client-server (chỉ server cần mở port), ở đây máy nào cũng phải mở 1 port lắng nghe. Nếu 2 máy không cùng LAN (khác NAT/router) thì cần thêm bước port forwarding - đây là giới hạn tất yếu của mô hình P2P thuần.

**Nhóm hoạt động theo kiểu "gửi lần lượt" (flood), không phải broadcast qua server**
Khi gửi tin nhắn nhóm, máy gửi tự lặp qua từng thành viên đang kết nối và gửi trực tiếp tới từng người. Nếu 1 thành viên đang offline lúc đó, họ sẽ không nhận được tin nhắn này (không có server nào lưu hộ để gửi lại sau).

---

## Hướng phát triển thêm

**Tự động khám phá peer (auto-discovery)**
Hiện phải nhập tay IP:port. Có thể thêm UDP broadcast: mỗi peer định kỳ gửi gói tin "tôi đang online tại IP:port này" ra toàn mạng LAN, các peer khác lắng nghe và tự hiển thị vào danh sách "Peer khả dụng" mà không cần gõ tay.

**Gửi lại tin nhắn khi peer offline (offline messaging)**
Vì không có server lưu hộ, tin nhắn gửi lúc người nhận offline sẽ mất. Có thể khắc phục bằng cách: máy gửi tự lưu các tin chưa gửi thành công vào 1 hàng đợi cục bộ (`pending_messages`), rồi tự động gửi lại khi phát hiện peer đó kết nối lại.

**Mã hóa đầu-cuối (end-to-end encryption)**
Hiện dữ liệu truyền dạng JSON thuần, không mã hóa. Có thể thêm bước trao đổi khóa bằng Diffie-Hellman ngay sau `hello`, rồi mã hóa nội dung bằng AES trước khi gửi - đảm bảo dù ai chặn được gói tin cũng không đọc được nội dung.

**Truyền file / hình ảnh**
Giao thức hiện chỉ hỗ trợ text (`content` là string). Có thể mở rộng thêm `type: "file_transfer"`, gửi file theo từng chunk kèm metadata (tên file, kích thước, checksum) để client ghép lại.

**Xác nhận đã đọc / đang gõ (read receipt, typing indicator)**
Thêm 2 loại message mới: `typing` (gửi khi người dùng đang gõ, không lưu vào lịch sử) và `read_receipt` (gửi khi mở khung chat, đánh dấu tin nhắn cũ là "đã xem").

**Danh sách "Peer đã biết" (known_peers) hiện chưa dùng để kết nối nhanh**
Bảng này đã có sẵn trong `db.py` nhưng GUI chưa có nút "Kết nối lại" từ danh sách này - có thể bổ sung để không phải gõ lại IP:port mỗi lần mở app.

**NAT traversal cho peer khác mạng**
Hiện chỉ chạy tốt trong cùng LAN. Muốn 2 máy ở 2 mạng khác nhau (khác NAT) kết nối được mà không cần port forwarding thủ công, có thể tìm hiểu kỹ thuật STUN/TURN hoặc dùng 1 rendezvous server nhẹ chỉ để 2 peer "làm quen" nhau rồi vẫn chat trực tiếp (không phá vỡ tinh thần P2P, server đó không route tin nhắn).
