using System;
using System.Drawing;
using System.Windows.Forms;
using ChatP2P.Core.Models;

namespace ChatP2P.UI.Controls
{
    public class MessageBubbleControl : UserControl
    {
        private Panel _bubblePanel = null!;
        private Label _lblContent = null!;
        private Label _lblTime = null!;

        public MessageBubbleControl()
        {
            InitializeComponentManual();
        }

        private void InitializeComponentManual()
        {
            // Cấu hình chung cho Control
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Margin = new Padding(5);
            this.Padding = new Padding(5);
            this.BackColor = Color.Transparent;

            // 1. Khung chứa Bong bóng (Panel)
            _bubblePanel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(10),
                Margin = new Padding(0)
            };

            // 2. Label hiển thị nội dung tin nhắn
            _lblContent = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(350, 0), // Giới hạn độ rộng tối đa 350px (tự xuống dòng nếu tin dài)
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(10, 8)
            };

            // 3. Label hiển thị thời gian
            _lblTime = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Italic),
                ForeColor = Color.DimGray
            };

            _bubblePanel.Controls.Add(_lblContent);
            _bubblePanel.Controls.Add(_lblTime);

            this.Controls.Add(_bubblePanel);
        }

        /// <summary>
        /// Hàm nạp dữ liệu tin nhắn vào Bong bóng chat
        /// </summary>
        /// <param name="message">Đối tượng tin nhắn ChatMessage</param>
        /// <param name="isMyMessage">true nếu là tin nhắn của tôi gửi đi, false nếu người khác gửi tới</param>
        public void SetData(ChatMessage message, bool isMyMessage)
        {
            _lblContent.Text = message.Content;
            _lblTime.Text = message.Timestamp.ToString("HH:mm");

            // Tự động tính toán vị trí nhãn Thời gian nằm bên dưới Nội dung tin nhắn
            _lblTime.Location = new Point(10, _lblContent.Bottom + 3);

            if (isMyMessage)
            {
                // TIN NHẮN GỬI ĐI (Của tôi) -> Nền Xanh nhạt (Kiểu Zalo/WhatsApp), Căn bên PHẢI
                _bubblePanel.BackColor = Color.FromArgb(220, 248, 198);
                this.Dock = DockStyle.Right;
            }
            else
            {
                // TIN NHẮN NHẬN ĐẾN (Của bạn) -> Nền XÁM nhạt, Căn bên TRÁI
                _bubblePanel.BackColor = Color.FromArgb(240, 240, 240);
                this.Dock = DockStyle.Left;
            }
        }
    }
}
