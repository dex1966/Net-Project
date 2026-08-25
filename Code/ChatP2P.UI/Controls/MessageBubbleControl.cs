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

        private bool _isMyMessage;

        public MessageBubbleControl()
        {
            InitializeComponentManual();
        }

        private void InitializeComponentManual()
        {
            // UserControl bên ngoài
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Margin = new Padding(6, 4, 6, 4);
            Padding = new Padding(0);
            BackColor = Color.Transparent;

            // Bong bóng chat
            _bubblePanel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(12, 8, 12, 7),
                Margin = new Padding(0),
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Nội dung tin nhắn
            _lblContent = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(350, 0),
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.FromArgb(30, 30, 30),
                Location = new Point(12, 8),
                Text = ""
            };

            // Thời gian
            _lblTime = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Regular),
                ForeColor = Color.Gray,
                Text = ""
            };

            _bubblePanel.Controls.Add(_lblContent);
            _bubblePanel.Controls.Add(_lblTime);

            Controls.Add(_bubblePanel);
        }

        /// <summary>
        /// Gán dữ liệu tin nhắn cho bong bóng chat.
        /// </summary>
        /// <param name="message">Tin nhắn cần hiển thị.</param>
        /// <param name="isMyMessage">
        /// true: tin nhắn của tôi.
        /// false: tin nhắn nhận từ peer.
        /// </param>
        public void SetData(ChatMessage message, bool isMyMessage)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            _isMyMessage = isMyMessage;

            _lblContent.Text = message.Content ?? string.Empty;
            _lblTime.Text = message.Timestamp.ToString("HH:mm");

            ApplyMessageStyle();

            // Phải tính lại sau khi content thay đổi
            PerformLayout();
            UpdateBubbleLayout();
        }

        private void ApplyMessageStyle()
        {
            if (_isMyMessage)
            {
                // Tin nhắn mình gửi
                _bubblePanel.BackColor = Color.FromArgb(220, 248, 198);
                _lblContent.ForeColor = Color.FromArgb(25, 25, 25);
                _lblTime.ForeColor = Color.FromArgb(90, 110, 90);
            }
            else
            {
                // Tin nhắn nhận
                _bubblePanel.BackColor = Color.FromArgb(242, 242, 242);
                _lblContent.ForeColor = Color.FromArgb(30, 30, 30);
                _lblTime.ForeColor = Color.Gray;
            }
        }

        private void UpdateBubbleLayout()
        {
            // Nội dung
            _lblContent.Location = new Point(12, 8);

            // Thời gian nằm dưới nội dung
            _lblTime.Location = new Point(
                12,
                _lblContent.Bottom + 4
            );

            // Tính kích thước panel thủ công để tránh AutoSize bị lỗi layout
            int bubbleWidth = Math.Max(
                _lblContent.Width,
                _lblTime.Width
            ) + 24;

            int bubbleHeight =
                _lblTime.Bottom + 7;

            _bubblePanel.Size = new Size(
                bubbleWidth,
                bubbleHeight
            );

            Size = _bubblePanel.Size;
        }

        private void InitializeComponent()
        {

        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            if (_lblContent != null &&
                _lblTime != null &&
                _bubblePanel != null)
            {
                UpdateBubbleLayout();
            }
        }
    }
}