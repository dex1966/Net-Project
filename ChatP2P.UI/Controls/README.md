# Tài liệu Hướng dẫn & Giải thích Mã nguồn: UI Controls (ChatP2P.UI)

Tài liệu này giải thích chi tiết cấu trúc, nguyên lý thiết kế và mã nguồn của thư mục `ChatP2P.UI/Controls`, phục vụ cho việc báo cáo (Doc/Slide) và hiểu mã nguồn.

---

## 1. Tổng quan Thư mục Controls

Thư mục `ChatP2P.UI/Controls` chứa các **Custom UserControls** (Thành phần giao diện tự định nghĩa). Thay vì viết toàn bộ giao diện phức tạp vào một tệp Form chính, ứng dụng chia nhỏ giao diện thành các linh kiện tái sử dụng:

1. **[MessageBubbleControl.cs](file:///d:/Lap%20Trinh%20Mang/Net-Project/ChatP2P.UI/Controls/MessageBubbleControl.cs)**: Bong bóng hiển thị từng dòng tin nhắn trong ô chat.
2. **[PeerListItemControl.cs](file:///d:/Lap%20Trinh%20Mang/Net-Project/ChatP2P.UI/Controls/PeerListItemControl.cs)**: Dòng hiển thị thông tin từng bạn bè (Peer) trong danh sách Sidebar bên trái.

---

## 2. Nguyên lý Thiết kế Kiến trúc (Design Patterns)

### 🟢 1. Thiết kế Code-behind 1 Tệp duy nhất (Standalone Single-File)
*   **Vấn đề của WinForms chuẩn:** Thường tự sinh ra tệp `.Designer.cs` rườm rà, dễ gây lỗi hỏng giao diện khi copy/paste hoặc sửa code thủ công.
*   **Giải pháp:** Cả 2 Control được khởi tạo hoàn toàn bằng code trong hàm `InitializeComponentManual()`. Không phụ thuộc vào tệp `.Designer.cs`.

### 🟢 2. Luồng Nạp dữ liệu Một chiều (One-Way Data Binding)
Mỗi Control chỉ có duy nhất **1 hàm nhận dữ liệu `SetData(...)`**:
*   Truyền Object dữ liệu (`ChatMessage` hoặc `Peer`) vào hàm `SetData()`.
*   Control tự động đọc thuộc tính, tự tính kích thước khung, gán màu sắc và căn chỉnh vị trí.

### 🟢 3. Kiến trúc Bắt sự kiện (Event-Driven Architecture)
*   `PeerListItemControl` đăng ký sự kiện `Click` trên tất cả các label con.
*   Khi người dùng nhấp chuột vào dòng đó, Control sẽ phát ra sự kiện `PeerSelected` truyền Object `Peer` về cho `MainForm` xử lý mở đoạn chat.

---

## 3. Phân tích Chi tiết từng Control

### 💬 Control 1: Bong bóng Chat (`MessageBubbleControl.cs`)

#### ✦ Cấu trúc các thành phần UI:
*   `_bubblePanel` (`Panel`): Khung bo nền của tin nhắn.
*   `_lblContent` (`Label`): Nhãn chứa nội dung tin nhắn. Được thiết lập `MaximumSize = new Size(350, 0)` để khi tin nhắn dài quá 350px sẽ **tự động xuống dòng** mà không bị tràn màn hình.
*   `_lblTime` (`Label`): Nhãn nhỏ hiển thị thời gian gửi (định dạng `HH:mm`). Tự động tính toán nằm bên dưới `_lblContent`.

#### ✦ Logic xử lý trong hàm `SetData(ChatMessage message, bool isMyMessage)`:
```csharp
if (isMyMessage)
{
    // Tin nhắn của TÔI -> Nền Xanh nhạt (RGB: 220, 248, 198), Căn lề PHẢI (DockStyle.Right)
}
else
{
    // Tin nhắn NGƯỜI KHÁC -> Nền XÁM nhạt (RGB: 240, 240, 240), Căn lề TRÁI (DockStyle.Left)
}
```

---

### 👤 Control 2: Dòng Bạn bè Sidebar (`PeerListItemControl.cs`)

#### ✦ Cấu trúc các thành phần UI:
*   `_statusDot` (`Panel`): Chấm tròn nhỏ thể hiện trạng thái (Xanh lá `RGB 46, 204, 113` = Online; Xám = Offline).
*   `_lblName` (`Label`): Tên hiển thị của Peer (In đậm).
*   `_lblAddress` (`Label`): Địa chỉ mạng dưới dạng `IP:Port`.
*   `_lblStatusText` (`Label`): Chữ nhỏ báo trạng thái ("Online" hoặc "Offline").

#### ✦ Hiệu ứng Giao diện & Sự kiện:
*   **Hover Effect (Rê chuột)**: Đăng ký sự kiện `MouseEnter` (đổi nền sang xanh nhạt `RGB 235, 243, 250`) và `MouseLeave` (trả về màu trắng).
*   **Sự kiện chọn Chat (`PeerSelected`)**: Phát sự kiện khi người dùng click vào dòng để `MainForm` biết và tải lịch sử chat.

---

## 4. Cách sử dụng trong Màn hình chính (`MainForm.cs`)

Khi nạp tin nhắn hoặc danh sách bạn bè vào `MainForm.cs`, mã nguồn chỉ cần gọi đơn giản như sau:

```csharp
// 1. Thêm 1 bạn bè vào Sidebar:
var peerItem = new PeerListItemControl();
peerItem.SetData(peerObj);
peerItem.PeerSelected += (sender, selectedPeer) => {
    // Mở khung chat với selectedPeer
};
sidebarFlowLayoutPanel.Controls.Add(peerItem);

// 2. Thêm 1 tin nhắn vào Khung Chat:
var bubble = new MessageBubbleControl();
bubble.SetData(messageObj, isMyMessage: messageObj.SenderId == myId);
chatFlowLayoutPanel.Controls.Add(bubble);
```
