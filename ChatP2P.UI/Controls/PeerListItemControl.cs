using System;
using System.Drawing;
using System.Windows.Forms;
using ChatP2P.Core.Models;

namespace ChatP2P.UI.Controls
{
    public class PeerListItemControl : UserControl
    {
        private Panel _statusDot = null!;
        private Label _lblName = null!;
        private Label _lblAddress = null!;
        private Label _lblStatusText = null!;

        public Peer PeerData { get; private set; } = null!;

        // Khi người dùng click vào dòng Peer này để chọn chat
        public event EventHandler<Peer>? PeerSelected;

        public PeerListItemControl()
        {
            InitializeComponentManual();
        }

        private void InitializeComponentManual()
        {
            this.Size = new Size(240, 55);
            this.Margin = new Padding(0, 2, 0, 2);
            this.BackColor = Color.White;
            this.Cursor = Cursors.Hand;

            // 1. Status Online (Xanh) / Offline (Xám)
            _statusDot = new Panel
            {
                Size = new Size(12, 12),
                Location = new Point(12, 22),
                BackColor = Color.Gray
            };

            // 2. Nhãn Tên hiển thị của Peer
            _lblName = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30),
                Location = new Point(32, 8)
            };

            // 3. Nhãn Địa chỉ IP:Port
            _lblAddress = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(32, 30)
            };

            // 4. Nhãn chữ Trạng thái (Online/Offline)
            _lblStatusText = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(170, 30)
            };

            this.Controls.Add(_statusDot);
            this.Controls.Add(_lblName);
            this.Controls.Add(_lblAddress);
            this.Controls.Add(_lblStatusText);

            // Đăng ký sự kiện Click cho tất cả các thành phần con để bấm vào đâu cũng chọn được Peer
            this.Click += OnItemClicked;
            _statusDot.Click += OnItemClicked;
            _lblName.Click += OnItemClicked;
            _lblAddress.Click += OnItemClicked;
            _lblStatusText.Click += OnItemClicked;

            // Hiệu ứng di chuột (Hover effect)
            this.MouseEnter += (s, e) => this.BackColor = Color.FromArgb(235, 243, 250);
            this.MouseLeave += (s, e) => this.BackColor = Color.White;
        }

        /// <summary>
        /// Nạp dữ liệu Peer vào Control dòng danh sách
        /// </summary>
        public void SetData(Peer peer)
        {
            PeerData = peer;
            _lblName.Text = peer.Name;
            _lblAddress.Text = $"{peer.IpAddress}:{peer.Port}";

            if (peer.IsOnline)
            {
                _statusDot.BackColor = Color.FromArgb(46, 204, 113); // Màu xanh lá  Online
                _lblStatusText.Text = "Online";
                _lblStatusText.ForeColor = Color.Green;
            }
            else
            {
                _statusDot.BackColor = Color.LightGray; // Màu xám Offline
                _lblStatusText.Text = "Offline";
                _lblStatusText.ForeColor = Color.Gray;
            }
        }

        private void OnItemClicked(object? sender, EventArgs e)
        {
            if (PeerData != null)
            {
                PeerSelected?.Invoke(this, PeerData);
            }
        }
    }
}
