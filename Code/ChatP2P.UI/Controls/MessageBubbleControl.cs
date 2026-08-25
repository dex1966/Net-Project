using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ChatP2P.Core.Models;

namespace ChatP2P.UI.Controls
{
    public class MessageBubbleControl : UserControl
    {
        private Panel _bubblePanel = null!;
        private Label _lblForwarded = null!;
        private Label _lblContent = null!;
        private Label _lblTime = null!;

        private bool _isMyMessage;
        private bool _isUpdatingLayout;

        public MessageBubbleControl()
        {
            InitializeComponentManual();
        }

        private void InitializeComponentManual()
        {
            // UserControl bên ngoài
            AutoSize = false;
            Margin = new Padding(0, 4, 0, 4);
            Padding = new Padding(0);
            BackColor = Color.Transparent;

            // Bong bóng chat
            _bubblePanel = new Panel
            {
                AutoSize = false,
                Padding = new Padding(12, 8, 12, 7),
                Margin = new Padding(0),
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Nhãn "Đã chuyển tiếp"
            _lblForwarded = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Text = "↩ Đã chuyển tiếp",
                Visible = false
            };

            // Nội dung tin nhắn
            _lblContent = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(320, 0),
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

            _bubblePanel.Controls.Add(_lblForwarded);
            _bubblePanel.Controls.Add(_lblContent);
            _bubblePanel.Controls.Add(_lblTime);

            Controls.Add(_bubblePanel);
        }

        /// <summary>
        /// Gán dữ liệu tin nhắn cho bong bóng chat.
        /// </summary>
        /// <param name="message">Tin nhắn cần hiển thị.</param>
        /// <param name="isMyMessage">
        /// true: tin nhắn của tôi (căn phải).
        /// false: tin nhắn nhận từ peer (căn trái).
        /// </param>
        public void SetData(ChatMessage message, bool isMyMessage)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            _isMyMessage = isMyMessage;

            // Kiểm tra tin nhắn chuyển tiếp
            bool isForwarded = !string.IsNullOrEmpty(message.ForwardedFromId);
            _lblForwarded.Visible = isForwarded;

            _lblContent.Text = message.Content ?? string.Empty;
            _lblTime.Text = message.Timestamp.ToString("HH:mm");

            ApplyMessageStyle();

            // Phải tính lại sau khi content thay đổi
            UpdateBubbleLayout();
        }

        private void ApplyMessageStyle()
        {
            if (_isMyMessage)
            {
                // Tin nhắn mình gửi (Xanh lục nhạt)
                _bubblePanel.BackColor = Color.FromArgb(220, 248, 198);
                _lblContent.ForeColor = Color.FromArgb(25, 25, 25);
                _lblTime.ForeColor = Color.FromArgb(90, 110, 90);
                _lblForwarded.ForeColor = Color.FromArgb(80, 110, 80);
            }
            else
            {
                // Tin nhắn nhận (Xám nhạt)
                _bubblePanel.BackColor = Color.FromArgb(242, 242, 242);
                _lblContent.ForeColor = Color.FromArgb(30, 30, 30);
                _lblTime.ForeColor = Color.Gray;
                _lblForwarded.ForeColor = Color.Gray;
            }
        }

        public void UpdateBubbleLayout()
        {
            if (_isUpdatingLayout || _lblContent == null || _lblTime == null || _bubblePanel == null)
                return;

            _isUpdatingLayout = true;
            try
            {
                // Tính chiều rộng tối đa cho phần chữ
                int parentWidth = Parent?.ClientSize.Width ?? Width;
                int maxTextWidth = 320;
                if (parentWidth > 100)
                {
                    maxTextWidth = Math.Min(350, (int)(parentWidth * 0.70));
                }
                _lblContent.MaximumSize = new Size(Math.Max(120, maxTextWidth), 0);

                int currentTop = 8;
                if (_lblForwarded != null && _lblForwarded.Visible)
                {
                    _lblForwarded.Location = new Point(12, currentTop);
                    currentTop = _lblForwarded.Bottom + 2;
                }

                // Nội dung tin nhắn
                _lblContent.Location = new Point(12, currentTop);

                // Thời gian nằm dưới nội dung
                _lblTime.Location = new Point(
                    12,
                    _lblContent.Bottom + 4
                );

                // Tính kích thước panel bong bóng
                int textAndForwardedWidth = _lblForwarded != null && _lblForwarded.Visible
                    ? Math.Max(_lblContent.Width, _lblForwarded.Width)
                    : _lblContent.Width;

                int bubbleWidth = Math.Max(textAndForwardedWidth, _lblTime.Width) + 24;
                int bubbleHeight = _lblTime.Bottom + 7;

                _bubblePanel.Size = new Size(bubbleWidth, bubbleHeight);

                // Bo tròn góc cho bong bóng chat
                ApplyPanelRoundedRegion();

                // Nếu có parent container, đảm bảo UserControl chiếm đủ chiều rộng để căn lề
                if (Parent != null && Parent.ClientSize.Width > 0)
                {
                    int targetWidth = Parent.ClientSize.Width - Parent.Padding.Horizontal - 20;
                    if (targetWidth > bubbleWidth)
                    {
                        Width = targetWidth;
                    }
                    else
                    {
                        Width = bubbleWidth + 12;
                    }
                }
                else if (Width < bubbleWidth + 12)
                {
                    Width = bubbleWidth + 12;
                }

                // Căn lề Trái / Phải cho bong bóng chat
                if (_isMyMessage)
                {
                    // Tin nhắn gửi -> Căn PHẢI
                    _bubblePanel.Location = new Point(Math.Max(0, Width - _bubblePanel.Width - 6), 0);
                }
                else
                {
                    // Tin nhắn nhận -> Căn TRÁI
                    _bubblePanel.Location = new Point(6, 0);
                }

                Height = _bubblePanel.Height + 6;
            }
            finally
            {
                _isUpdatingLayout = false;
            }
        }

        private void ApplyPanelRoundedRegion()
        {
            if (_bubblePanel != null && _bubblePanel.Width > 0 && _bubblePanel.Height > 0)
            {
                using (GraphicsPath path = GetRoundedRectanglePath(new Rectangle(0, 0, _bubblePanel.Width, _bubblePanel.Height), 14))
                {
                    _bubblePanel.Region = new Region(path);
                }
            }
        }

        private static GraphicsPath GetRoundedRectanglePath(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // Top-left arc
            path.AddArc(arc, 180, 90);

            // Top-right arc
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom-right arc
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom-left arc
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void InitializeComponent()
        {

        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            UpdateBubbleLayout();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateBubbleLayout();
        }
    }
}