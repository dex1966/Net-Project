using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using ChatP2P.Core.Models;

namespace ChatP2P.UI.Controls
{
    public class PeerListItemControl : UserControl
    {
        private Peer? _peer;
        private string _lastMessage = string.Empty;
        private int _unreadCount = 0;
        private bool _isSelected = false;
        private bool _isHovered = false;

        public Peer? PeerData => _peer;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    Invalidate();
                }
            }
        }

        public event EventHandler<Peer>? PeerSelected;

        public PeerListItemControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            Height = 64;
            Margin = new Padding(0, 1, 0, 1);
            Cursor = Cursors.Hand;
        }

        public void SetData(Peer peer, string? lastMessage = null, int unreadCount = 0)
        {
            _peer = peer ?? throw new ArgumentNullException(nameof(peer));
            _lastMessage = lastMessage ?? $"{peer.IpAddress}:{peer.Port}";
            _unreadCount = unreadCount;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (_peer != null)
            {
                PeerSelected?.Invoke(this, _peer);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // 1. Khung nền & Hover/Selection state
            Color bgColor = Color.White;
            if (_isSelected)
            {
                bgColor = Color.FromArgb(230, 240, 255); // Xanh dương nhạt khi chọn
            }
            else if (_isHovered)
            {
                bgColor = Color.FromArgb(245, 247, 250); // Xám cực nhạt khi rê chuột
            }

            using (SolidBrush bgBrush = new SolidBrush(bgColor))
            {
                g.FillRectangle(bgBrush, ClientRectangle);
            }

            // Đường viền ngăn cách ở dưới
            using (Pen borderPen = new Pen(Color.FromArgb(238, 238, 238), 1))
            {
                g.DrawLine(borderPen, 68, Height - 1, Width - 12, Height - 1);
            }

            if (_peer == null) return;

            // 2. Vẽ Avatar tròn (Size 44x44, lề trái 12)
            int avatarSize = 44;
            int avatarX = 12;
            int avatarY = (Height - avatarSize) / 2;
            Rectangle avatarRect = new Rectangle(avatarX, avatarY, avatarSize, avatarSize);

            bool drawnAvatar = false;
            if (!string.IsNullOrEmpty(_peer.AvatarPath) && File.Exists(_peer.AvatarPath))
            {
                try
                {
                    using Image originalImg = Image.FromFile(_peer.AvatarPath);
                    using GraphicsPath path = new GraphicsPath();
                    path.AddEllipse(avatarRect);
                    g.SetClip(path);
                    g.DrawImage(originalImg, avatarRect);
                    g.ResetClip();
                    drawnAvatar = true;
                }
                catch
                {
                    drawnAvatar = false;
                }
            }

            if (!drawnAvatar)
            {
                // Gradient avatar tròn dựa trên tên peer
                Color c1 = GetAvatarColor(_peer.Name, 0);
                Color c2 = GetAvatarColor(_peer.Name, 1);
                using (LinearGradientBrush avatarBrush = new LinearGradientBrush(avatarRect, c1, c2, LinearGradientMode.ForwardDiagonal))
                {
                    g.FillEllipse(avatarBrush, avatarRect);
                }

                // Viết chữ cái đầu tên
                string initial = !string.IsNullOrWhiteSpace(_peer.Name) ? _peer.Name[0].ToString().ToUpper() : "?";
                using (Font initialFont = new Font("Segoe UI", 14F, FontStyle.Bold))
                using (SolidBrush textBrush = new SolidBrush(Color.White))
                {
                    SizeF textSize = g.MeasureString(initial, initialFont);
                    float tx = avatarX + (avatarSize - textSize.Width) / 2f;
                    float ty = avatarY + (avatarSize - textSize.Height) / 2f;
                    g.DrawString(initial, initialFont, textBrush, tx, ty);
                }
            }

            // 3. Vẽ Chấm Trạng Thái Online/Offline
            int statusDotSize = 12;
            int statusX = avatarX + avatarSize - statusDotSize;
            int statusY = avatarY + avatarSize - statusDotSize;
            Rectangle statusRect = new Rectangle(statusX, statusY, statusDotSize, statusDotSize);

            // Viền trắng quanh chấm
            using (SolidBrush borderBrush = new SolidBrush(bgColor))
            {
                g.FillEllipse(borderBrush, statusX - 1, statusY - 1, statusDotSize + 2, statusDotSize + 2);
            }

            Color statusColor = _peer.IsOnline ? Color.FromArgb(46, 204, 113) : Color.FromArgb(189, 195, 199);
            using (SolidBrush statusBrush = new SolidBrush(statusColor))
            {
                g.FillEllipse(statusBrush, statusRect);
            }

            // 4. Vẽ Tên và Thông tin tin nhắn / IP
            int textX = avatarX + avatarSize + 12;
            int textWidth = Width - textX - 60; // Dành chỗ cho thời gian / badge unread

            // Tên Peer
            using (Font nameFont = new Font("Segoe UI", 9.75F, _unreadCount > 0 ? FontStyle.Bold : FontStyle.Regular))
            using (SolidBrush nameBrush = new SolidBrush(Color.FromArgb(33, 37, 41)))
            {
                g.DrawString(_peer.Name, nameFont, nameBrush, textX, 12, new StringFormat
                {
                    Trimming = StringTrimming.EllipsisCharacter,
                    FormatFlags = StringFormatFlags.NoWrap
                });
            }

            // Tin nhắn cuối / IP:Port
            using (Font msgFont = new Font("Segoe UI", 8.5F, _unreadCount > 0 ? FontStyle.Bold : FontStyle.Regular))
            using (SolidBrush msgBrush = new SolidBrush(_unreadCount > 0 ? Color.FromArgb(20, 20, 20) : Color.FromArgb(108, 117, 125)))
            {
                g.DrawString(_lastMessage, msgFont, msgBrush, new RectangleF(textX, 34, textWidth, 20), new StringFormat
                {
                    Trimming = StringTrimming.EllipsisCharacter,
                    FormatFlags = StringFormatFlags.NoWrap
                });
            }

            // 5. Vẽ Badge Unread Count (nếu có)
            if (_unreadCount > 0)
            {
                string badgeStr = _unreadCount > 99 ? "99+" : _unreadCount.ToString();
                using (Font badgeFont = new Font("Segoe UI", 8F, FontStyle.Bold))
                {
                    SizeF badgeSize = g.MeasureString(badgeStr, badgeFont);
                    int badgeWidth = Math.Max(20, (int)badgeSize.Width + 8);
                    int badgeHeight = 18;
                    int badgeX = Width - badgeWidth - 14;
                    int badgeY = (Height - badgeHeight) / 2;

                    Rectangle badgeRect = new Rectangle(badgeX, badgeY, badgeWidth, badgeHeight);
                    using (GraphicsPath path = GetRoundedRectanglePath(badgeRect, badgeHeight / 2))
                    using (SolidBrush badgeBrush = new SolidBrush(Color.FromArgb(0, 122, 255)))
                    {
                        g.FillPath(badgeBrush, path);
                    }

                    using (SolidBrush textBrush = new SolidBrush(Color.White))
                    {
                        float tx = badgeX + (badgeWidth - badgeSize.Width) / 2f;
                        float ty = badgeY + (badgeHeight - badgeSize.Height) / 2f;
                        g.DrawString(badgeStr, badgeFont, textBrush, tx, ty);
                    }
                }
            }
            else
            {
                // Hiển thị thời gian lần cuối thấy (LastSeen)
                string timeStr = _peer.IsOnline ? "Online" : _peer.LastSeen.ToString("HH:mm");
                using (Font timeFont = new Font("Segoe UI", 8F, FontStyle.Regular))
                using (SolidBrush timeBrush = new SolidBrush(_peer.IsOnline ? Color.FromArgb(46, 204, 113) : Color.FromArgb(149, 165, 166)))
                {
                    SizeF timeSize = g.MeasureString(timeStr, timeFont);
                    g.DrawString(timeStr, timeFont, timeBrush, Width - timeSize.Width - 14, 14);
                }
            }
        }

        private Color GetAvatarColor(string name, int offset)
        {
            int hash = Math.Abs((name ?? "").GetHashCode());
            Color[] colors1 = new[]
            {
                Color.FromArgb(52, 152, 219), Color.FromArgb(155, 89, 182),
                Color.FromArgb(46, 204, 113), Color.FromArgb(230, 126, 34),
                Color.FromArgb(231, 76, 60),  Color.FromArgb(26, 188, 156)
            };
            Color[] colors2 = new[]
            {
                Color.FromArgb(41, 128, 185), Color.FromArgb(142, 68, 173),
                Color.FromArgb(39, 174, 96),  Color.FromArgb(211, 84, 0),
                Color.FromArgb(192, 57, 43),  Color.FromArgb(22, 160, 133)
            };

            int index = hash % colors1.Length;
            return offset == 0 ? colors1[index] : colors2[index];
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            Rectangle arc = new Rectangle(rect.Location, new Size(diameter, diameter));

            path.AddArc(arc, 180, 90);
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
