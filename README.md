# Net-Project
# Mini Messenger - Chat 1-1 & Chat Nhóm (Python Socket + Tkinter)

Ứng dụng chat client-server viết bằng Python, hỗ trợ nhắn tin riêng (private message) và nhắn tin theo nhóm (group message) cùng lúc, lấy cảm hứng từ Messenger của Meta. Đồ án môn **Lập trình mạng**.

## Tính năng

- Đăng nhập bằng username, hiển thị danh sách người dùng đang online real-time
- Chat 1-1 (private message) giữa 2 người dùng
- Tạo nhóm chat, mời thành viên, gửi tin nhắn nhóm (broadcast tới các thành viên)
- Giao diện quản lý nhiều cuộc trò chuyện song song (chuyển đổi giữa các tab người/nhóm)
- Thông báo hệ thống (user online/offline, tham gia nhóm...)
- Mã hóa tin nhắn (TLS/SSL socket)
- Xác thực đăng nhập (username/password, hash password)
- Phân trang lịch sử chat (load thêm tin cũ khi scroll lên)

## Kiến trúc

```
Server (trung tâm)
 ├── Quản lý danh sách client đang online (username <-> socket)
 ├── Quản lý danh sách group (group_name <-> danh sách thành viên)
 └── Xử lý routing tin nhắn theo loại (private / group / system)

Client (Tkinter UI)
 ├── Đăng nhập với username
 ├── Danh sách người online (chat 1-1)
 ├── Danh sách group đã tham gia / tạo mới
 └── Khung chat hiển thị theo người/nhóm đang chọn
```

## Giao thức tin nhắn (Message Protocol)

Giao tiếp giữa client và server dùng JSON qua TCP socket.

**Client → Server**
```json
{"type": "private", "to": "username_B", "content": "hello"}
{"type": "group", "group": "nhom_hoc", "content": "hi all"}
{"type": "create_group", "group": "nhom_hoc", "members": ["A", "B", "C"]}
{"type": "join_group", "group": "nhom_hoc"}
```

**Server → Client**
```json
{"type": "private", "from": "username_A", "content": "hello"}
{"type": "group", "group": "nhom_hoc", "from": "username_A", "content": "hi all"}
{"type": "online_list", "users": ["A", "B", "C"]}
{"type": "system", "content": "B đã tham gia nhóm nhom_hoc"}
```

## Lưu trữ dữ liệu (Database)

Dùng **SQL Server** để lưu trữ dữ liệu bền vững (persistent) - server ghi mọi tin nhắn, user, group vào DB, khi restart server hoặc client đăng nhập lại vẫn load được lịch sử cũ. Kết nối từ Python qua thư viện `pyodbc`.

**Schema đầy đủ** (cũng là nội dung file `db_setup.sql`, chạy trong SSMS để tạo DB):

```sql
CREATE DATABASE ChatDB;
GO

USE ChatDB;
GO

-- Người dùng (có password hash + salt để xác thực đăng nhập)
CREATE TABLE users (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(50) UNIQUE NOT NULL,
    password_hash NVARCHAR(200) NOT NULL,
    salt NVARCHAR(100) NOT NULL,
    created_at DATETIME DEFAULT GETDATE()
);
GO

-- Nhóm chat
CREATE TABLE groups (
    id INT IDENTITY(1,1) PRIMARY KEY,
    group_name NVARCHAR(100) UNIQUE NOT NULL,
    created_by NVARCHAR(50) NOT NULL,
    created_at DATETIME DEFAULT GETDATE()
);
GO

-- Thành viên nhóm (quan hệ nhiều-nhiều giữa users và groups)
CREATE TABLE group_members (
    group_id INT NOT NULL,
    username NVARCHAR(50) NOT NULL,
    joined_at DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (group_id, username),
    FOREIGN KEY (group_id) REFERENCES groups(id)
);
GO

-- Tin nhắn (dùng chung cho cả private và group)
CREATE TABLE messages (
    id INT IDENTITY(1,1) PRIMARY KEY,
    sender NVARCHAR(50) NOT NULL,
    receiver NVARCHAR(50) NULL,     -- username nếu là private message
    group_id INT NULL,              -- id nhóm nếu là group message
    content NVARCHAR(MAX) NOT NULL,
    sent_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (group_id) REFERENCES groups(id)
);
GO

-- Index hỗ trợ query lịch sử chat + phân trang nhanh hơn
CREATE INDEX idx_messages_private ON messages (sender, receiver, sent_at);
CREATE INDEX idx_messages_group ON messages (group_id, sent_at);
GO
```

**Connection string dùng trong `server.py`** (Windows Authentication, server name `Nhom8`):
```python
DB_CONN_STR = (
    "DRIVER={ODBC Driver 17 for SQL Server};"
    "SERVER=Nhom8;"
    "DATABASE=ChatDB;"
    "Trusted_Connection=yes;"
)
```

**Luồng hoạt động:**
- Mỗi khi có tin nhắn gửi đi (private hoặc group), server ghi 1 dòng vào bảng `messages` trước khi forward tới người nhận
- Khi client đăng nhập, server query lịch sử chat liên quan (private: `WHERE sender=? OR receiver=?`; group: `WHERE group_id=?`) để trả về, giúp client hiển thị lại hội thoại cũ
- Bảng `groups` + `group_members` giúp server biết nhóm nào tồn tại và ai thuộc nhóm nào, kể cả sau khi restart

**Lưu ý khi code:**
- Server xử lý nhiều client đồng thời (multi-threading) → nên dùng **connection pool** (mỗi thread lấy 1 connection riêng) thay vì dùng chung 1 connection, tránh lỗi tranh chấp
- Đây là điểm liên hệ trực tiếp tới kiến thức môn Database (transaction, Dirty Read, concurrent access) bạn đang học song song

## Chat được giữa nhiều máy khác nhau (multi-device, không chỉ localhost)

Để nhiều máy thật (không chỉ trên cùng 1 máy) chat được với nhau, cần lưu ý:

**1. Server phải bind địa chỉ `0.0.0.0` thay vì `127.0.0.1`**
```python
server_socket.bind(("0.0.0.0", 12345))  # lắng nghe mọi network interface
```
`127.0.0.1` (localhost) chỉ cho phép kết nối từ chính máy đó, không cho máy khác trong mạng kết nối vào.

**2. Client kết nối tới địa chỉ IP thật của máy chạy server**
- Nếu cùng mạng LAN/Wifi (ví dụ phòng lab, ký túc xá dùng chung wifi): dùng IP nội bộ của máy server, xem bằng `ipconfig` (Windows) → tìm IPv4 Address (dạng `192.168.x.x`)
```python
client_socket.connect(("192.168.1.15", 12345))
```
- Nếu khác mạng (server ở nhà, client ở trường): cần **port forwarding** trên router (mở port 12345 trỏ vào máy server) và dùng IP public của mạng đó, hoặc dùng dịch vụ tunnel (ví dụ `ngrok`) để demo nhanh không cần cấu hình router

**3. Firewall**
- Windows Firewall mặc định chặn port lạ → cần mở port (Windows Defender Firewall → Inbound Rules → New Rule → cho phép port 12345 TCP)

**4. SQL Server cũng cần cho phép remote connection nếu server chat và SQL Server đặt khác máy**
- Bật TCP/IP trong SQL Server Configuration Manager
- Mở port 1433 trên firewall máy chứa SQL Server
- Dùng SQL Server Authentication (username/password) thay vì chỉ Windows Authentication, vì client có thể không cùng domain

**Gợi ý demo/báo cáo:** chạy thử trong LAN (cùng wifi lớp/lab) là đơn giản và ổn định nhất để bảo vệ đồ án, không cần đụng tới port forwarding hay ngrok.

**5. Chỉ máy server cần cài SQL Server, client thì không**
- SQL Server chỉ cần cài **trên máy chạy `server.py`**
- Các máy client khác (máy B, C, D...) chỉ cần Python + Tkinter, không cần cài gì thêm về DB — client chỉ gửi/nhận qua socket, còn server mới là bên trực tiếp đọc/ghi dữ liệu

```
Máy A (Server)                    Máy B, C, D... (Client)
 ├── server.py                     ├── client.py (chỉ cần Python + Tkinter)
 ├── SQL Server (ChatDB)           └── kết nối socket tới IP máy A
 └── Ghi/đọc messages, users, groups
```

- Việc chat qua nhiều máy LAN và việc lưu dữ liệu là **hai chuyện độc lập** — dù chat từ bao nhiêu máy, mọi tin nhắn vẫn đi qua server và được ghi vào SQL Server như bình thường, tắt/mở lại server vẫn còn nguyên lịch sử

## Công nghệ sử dụng

- Python 3
- `socket` (built-in) - giao tiếp TCP
- `threading` (built-in) - xử lý đa client, nhận tin không block UI
- `tkinter` (built-in) - giao diện người dùng
- `json` (built-in) - định dạng message
- `pyodbc` - kết nối và thao tác với SQL Server

## Cài đặt & Chạy

**Yêu cầu:**
- Python 3
- SQL Server đã cài, server name `Nhom8`, dùng Windows Authentication
- OpenSSL (có sẵn trong Git Bash) hoặc Python package `cryptography` để tạo certificate
- Cài các thư viện:
```bash
pip install pyodbc
```
(Windows cần cài thêm "ODBC Driver 17 for SQL Server" nếu chưa có, tải từ Microsoft)

**1. Tạo database và các bảng**
- Mở SSMS, kết nối vào server `Nhom8`, chạy script SQL ở mục "Lưu trữ dữ liệu (Database)" phía trên (hoặc copy ra file `.sql` riêng để chạy)

**2. Tạo self-signed certificate cho SSL/TLS**

Server cần 1 cặp certificate + private key để bọc socket bằng SSL. Vì đây là đồ án/demo nội bộ (LAN), dùng self-signed certificate là đủ, không cần mua CA-signed certificate.

Cách tạo (cần OpenSSL - có sẵn trong Git Bash / WSL / Linux / macOS), chạy trong thư mục chứa `server.py`:
```bash
openssl req -x509 -newkey rsa:2048 -keyout key.pem -out cert.pem -days 365 -nodes -subj "/CN=Nhom8"
```
Giải thích:
- `-x509`: tạo self-signed certificate
- `-newkey rsa:2048`: tạo key RSA 2048-bit
- `-keyout key.pem` / `-out cert.pem`: file private key / certificate tạo ra
- `-days 365`: hạn dùng 1 năm
- `-nodes`: không mã hóa private key bằng password (để server tự load, không cần nhập password mỗi lần chạy)
- `-subj "/CN=Nhom8"`: đặt Common Name là `Nhom8` (tên server), khỏi cần trả lời các câu hỏi tương tác (Country, Organization...)

Sau khi chạy xong sẽ có `cert.pem` (certificate) và `key.pem` (private key, giữ bí mật). Copy cả 2 file vào thư mục chứa `server.py`; copy riêng `cert.pem` sang thư mục chứa `client.py` (client cần để verify).

**Nếu máy không có OpenSSL (Windows không có sẵn):** cài Git for Windows (có kèm Git Bash + OpenSSL), mở Git Bash rồi chạy lệnh trên. Hoặc dùng Python (`pip install cryptography`) chạy script sau để tự sinh cert mà không cần OpenSSL:
```python
from cryptography import x509
from cryptography.hazmat.primitives import hashes, serialization
from cryptography.hazmat.primitives.asymmetric import rsa
from cryptography.x509.oid import NameOID
import datetime

key = rsa.generate_private_key(public_exponent=65537, key_size=2048)
subject = issuer = x509.Name([x509.NameAttribute(NameOID.COMMON_NAME, "Nhom8")])
cert = (
    x509.CertificateBuilder()
    .subject_name(subject).issuer_name(issuer)
    .public_key(key.public_key())
    .serial_number(x509.random_serial_number())
    .not_valid_before(datetime.datetime.utcnow())
    .not_valid_after(datetime.datetime.utcnow() + datetime.timedelta(days=365))
    .sign(key, hashes.SHA256())
)

with open("key.pem", "wb") as f:
    f.write(key.private_bytes(
        encoding=serialization.Encoding.PEM,
        format=serialization.PrivateFormat.TraditionalOpenSSL,
        encryption_algorithm=serialization.NoEncryption(),
    ))
with open("cert.pem", "wb") as f:
    f.write(cert.public_bytes(serialization.Encoding.PEM))

print("Đã tạo cert.pem và key.pem")
```

**3. Chạy server** (trên máy đóng vai trò server)
```bash
python server.py
```

**4. Chạy client** (trên máy server hoặc máy khác cùng mạng LAN)
```bash
python client.py
```
- Lần đầu: bấm **Đăng ký** để tạo tài khoản (password sẽ được hash trước khi lưu DB)
- Sau đó: **Đăng nhập** để vào chat
- Khi chạy client từ máy khác, sửa `SERVER_IP` trong `client.py` thành IP thật của máy server (xem mục "Chat được giữa nhiều máy")

## Cấu trúc thư mục

```
├── server.py           # Server: SSL socket, xác thực, chat 1-1 + nhóm, lưu SQL Server
├── client.py            # Client UI (Tkinter) + kết nối SSL
├── requirements.txt       # Danh sách thư viện cần cài
├── cert.pem / key.pem     # Certificate + private key (tự tạo, không commit lên git)
└── README.md              # Tài liệu đầy đủ: kiến trúc, schema DB, hướng dẫn cert, cài đặt & chạy
```

> **Lưu ý bảo mật khi push lên git:** không commit `key.pem` (private key) lên git thật, dù đây chỉ là đồ án demo. Thêm `*.pem` vào `.gitignore` cho an toàn.

## Hướng phát triển thêm
- [ ] Trạng thái "đang nhập..." (typing indicator)
- [ ] Gửi file/hình ảnh
