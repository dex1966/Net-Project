using System;
using System.Drawing;
using System.Windows.Forms;
using ChatP2P.Core.Models;
using ChatP2P.UI.Controls;
using ChatP2P.UI.Form;

namespace ChatP2P.UI
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        private TabControl _tabControl = null!;
        private EmojiPickerForm? _emojiPicker;

        public Form1()
        {
            SetupTestInterface();
        }

        private void SetupTestInterface()
        {
            Text = "ChatP2P UI Test Suite - (MessageBubble, PeerListItem, EmojiPicker)";
            Size = new Size(600, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);

            _tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.75F, FontStyle.Regular),
                Padding = new Point(16, 8)
            };

            // Tab 1: Test PeerListItemControl (1 Dòng Peer)
            TabPage tabPeers = new TabPage("👤 Danh Sách Peer (PeerListItem)");
            tabPeers.Controls.Add(CreatePeerListTestPanel());

            // Tab 2: Test EmojiPickerForm (Bảng Emoji)
            TabPage tabEmoji = new TabPage("😀 Bảng Chọn Emoji (EmojiPicker)");
            tabEmoji.Controls.Add(CreateEmojiTestPanel());

            // Tab 3: Test MessageBubbleControl (Bong Bóng Chat)
            TabPage tabBubbles = new TabPage("💬 Bong Bóng Chat (MessageBubble)");
            tabBubbles.Controls.Add(CreateMessageBubblesTestPanel());

            _tabControl.TabPages.Add(tabPeers);
            _tabControl.TabPages.Add(tabEmoji);
            _tabControl.TabPages.Add(tabBubbles);

            Controls.Add(_tabControl);
        }

        // ==========================================
        // 1. TEST UI 1 DÒNG PEER (PeerListItemControl)
        // ==========================================
        private Control CreatePeerListTestPanel()
        {
            Panel container = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            Label titleLabel = new Label
            {
                Text = "Thử nghiệm Control PeerListItemControl (1 Dòng Peer):",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 32,
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            Label statusInfo = new Label
            {
                Text = "Nhấp vào bất kỳ dòng Peer nào bên dưới để chọn (Highlight & Event):",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                Dock = DockStyle.Top,
                Height = 25,
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            FlowLayoutPanel peerListPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            void AdjustPeerWidths()
            {
                peerListPanel.SuspendLayout();
                int usableWidth = peerListPanel.ClientSize.Width - 4;
                if (usableWidth > 100)
                {
                    foreach (Control ctrl in peerListPanel.Controls)
                    {
                        ctrl.Width = usableWidth;
                    }
                }
                peerListPanel.ResumeLayout(true);
            }

            peerListPanel.Resize += (s, e) => AdjustPeerWidths();

            // Sample Data Peers
            var peers = new[]
            {
                new { Peer = new Peer { Name = "Nguyễn Văn Nam", IpAddress = "192.168.1.5", Port = 6000, IsOnline = true }, LastMsg = "Chào bạn, khi nào họp nhóm vậy?", Unread = 3 },
                new { Peer = new Peer { Name = "Trần Thị Mai", IpAddress = "192.168.1.12", Port = 6001, IsOnline = true }, LastMsg = "👍 Đã gửi file báo cáo rồi nhé!", Unread = 0 },
                new { Peer = new Peer { Name = "Lê Hoàng Long", IpAddress = "10.0.0.44", Port = 6002, IsOnline = false, LastSeen = DateTime.Now.AddHours(-2) }, LastMsg = "Hẹn gặp lại bạn sau.", Unread = 0 },
                new { Peer = new Peer { Name = "Đặng Phương Thảo", IpAddress = "192.168.1.88", Port = 6003, IsOnline = true }, LastMsg = "Dự án P2P Chat chạy ngon lắm 😄", Unread = 12 },
                new { Peer = new Peer { Name = "Phạm Quốc Bảo", IpAddress = "172.16.0.15", Port = 6004, IsOnline = false, LastSeen = DateTime.Now.AddDays(-1) }, LastMsg = "Off máy đây bai bai...", Unread = 0 }
            };

            PeerListItemControl? currentlySelected = null;

            foreach (var item in peers)
            {
                var itemControl = new PeerListItemControl();
                itemControl.SetData(item.Peer, item.LastMsg, item.Unread);

                itemControl.PeerSelected += (s, selectedPeer) =>
                {
                    if (currentlySelected != null)
                    {
                        currentlySelected.IsSelected = false;
                    }
                    currentlySelected = itemControl;
                    currentlySelected.IsSelected = true;

                    statusInfo.Text = $"Đã chọn Peer: {selectedPeer.Name} ({selectedPeer.IpAddress}:{selectedPeer.Port}) - Trang thái: {(selectedPeer.IsOnline ? "Online" : "Offline")}";
                    statusInfo.ForeColor = Color.FromArgb(0, 122, 255);
                };

                peerListPanel.Controls.Add(itemControl);
            }

            container.Controls.Add(peerListPanel);
            container.Controls.Add(statusInfo);
            container.Controls.Add(titleLabel);

            container.Layout += (s, e) => AdjustPeerWidths();

            return container;
        }

        // ==========================================
        // 2. TEST UI BẢNG EMOJI (EmojiPickerForm)
        // ==========================================
        private Control CreateEmojiTestPanel()
        {
            Panel container = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24),
                BackColor = Color.White
            };

            Label titleLabel = new Label
            {
                Text = "Thử nghiệm Bảng Chọn Emoji (EmojiPickerForm):",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(24, 20),
                AutoSize = true,
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            Label descLabel = new Label
            {
                Text = "Nhấn nút 'Mở Bảng Emoji' bên dưới để mở cửa sổ chọn emoji.\nEmoji bạn bấm chọn sẽ được chèn trực tiếp vào ô nhập tin nhắn bên dưới.",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                Location = new Point(24, 52),
                Size = new Size(500, 45),
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            TextBox txtInput = new TextBox
            {
                Location = new Point(24, 110),
                Size = new Size(380, 36),
                Font = new Font("Segoe UI Emoji", 12F),
                PlaceholderText = "Nhập tin nhắn hoặc chèn emoji tại đây..."
            };

            Button btnOpenEmoji = new Button
            {
                Text = "😀 Mở Bảng Emoji",
                Location = new Point(415, 108),
                Size = new Size(130, 36),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 122, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnOpenEmoji.FlatAppearance.BorderSize = 0;

            Label previewLabel = new Label
            {
                Text = "Xem trước nội dung đã nhập:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Location = new Point(24, 170),
                AutoSize = true
            };

            Panel bubbleDisplay = new Panel
            {
                Location = new Point(24, 200),
                Size = new Size(520, 120),
                BackColor = Color.FromArgb(240, 244, 248),
                Padding = new Padding(12),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblPreviewText = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Emoji", 13F),
                Text = "Nội dung tin nhắn thử nghiệm sẽ hiển thị tại đây...",
                ForeColor = Color.FromArgb(40, 40, 40)
            };
            bubbleDisplay.Controls.Add(lblPreviewText);

            txtInput.TextChanged += (s, e) =>
            {
                lblPreviewText.Text = string.IsNullOrEmpty(txtInput.Text)
                    ? "Nội dung tin nhắn thử nghiệm sẽ hiển thị tại đây..."
                    : txtInput.Text;
            };

            btnOpenEmoji.Click += (s, e) =>
            {
                if (_emojiPicker == null || _emojiPicker.IsDisposed)
                {
                    _emojiPicker = new EmojiPickerForm();
                    _emojiPicker.EmojiSelected += (emoji) =>
                    {
                        txtInput.SelectionLength = 0;
                        int selectionStart = txtInput.SelectionStart;
                        txtInput.Text = txtInput.Text.Insert(selectionStart, emoji);
                        txtInput.SelectionStart = selectionStart + emoji.Length;
                        txtInput.Focus();
                    };
                }

                Point btnScreenLoc = btnOpenEmoji.PointToScreen(new Point(0, btnOpenEmoji.Height + 4));
                _emojiPicker.ShowAtLocation(btnScreenLoc);
            };

            container.Controls.Add(bubbleDisplay);
            container.Controls.Add(previewLabel);
            container.Controls.Add(btnOpenEmoji);
            container.Controls.Add(txtInput);
            container.Controls.Add(descLabel);
            container.Controls.Add(titleLabel);

            return container;
        }

        // ==========================================
        // 3. TEST UI BONG BÓNG CHAT (MessageBubbleControl)
        // ==========================================
        private Control CreateMessageBubblesTestPanel()
        {
            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(12),
                BackColor = Color.White
            };

            void AdjustWidths()
            {
                panel.SuspendLayout();
                int usableWidth = panel.ClientSize.Width - panel.Padding.Horizontal - 25;
                if (usableWidth > 50)
                {
                    foreach (Control c in panel.Controls)
                    {
                        c.Width = usableWidth;
                    }
                }
                panel.ResumeLayout(true);
            }

            panel.Resize += (s, e) => AdjustWidths();

            // 1. Tin nhắn nhận (ngắn)
            var msg1 = new MessageBubbleControl();
            msg1.SetData(new ChatMessage
            {
                Content = "Chào bạn! Bạn khoẻ không? 😀",
                Timestamp = DateTime.Now.AddMinutes(-10)
            }, isMyMessage: false);

            // 2. Tin nhắn gửi (ngắn)
            var msg2 = new MessageBubbleControl();
            msg2.SetData(new ChatMessage
            {
                Content = "Mình khoẻ, cảm ơn bạn! 👍❤️",
                Timestamp = DateTime.Now.AddMinutes(-8)
            }, isMyMessage: true);

            // 3. Tin nhắn nhận (dài)
            var msg3 = new MessageBubbleControl();
            msg3.SetData(new ChatMessage
            {
                Content = "Đây là một tin nhắn thử nghiệm có nội dung khá dài kèm emoji 😄 🎉 để kiểm tra khả năng xuống dòng tự động của MessageBubbleControl.",
                Timestamp = DateTime.Now.AddMinutes(-5)
            }, isMyMessage: false);

            // 4. Tin nhắn gửi (dài)
            var msg4 = new MessageBubbleControl();
            msg4.SetData(new ChatMessage
            {
                Content = "Rất tuyệt! Tin nhắn hiển thị đẹp mắt, phân biệt rõ màu sắc giữa tin gửi và tin nhận.",
                Timestamp = DateTime.Now
            }, isMyMessage: true);

            // 5. Tin nhắn chuyển tiếp (Forwarded)
            var msg5 = new MessageBubbleControl();
            msg5.SetData(new ChatMessage
            {
                Content = "Thông báo: Ngày mai họp nhóm Lập trình mạng lúc 9h sáng nhé! 🚀",
                ForwardedFromId = "msg-001",
                Timestamp = DateTime.Now
            }, isMyMessage: false);

            panel.Controls.Add(msg1);
            panel.Controls.Add(msg2);
            panel.Controls.Add(msg3);
            panel.Controls.Add(msg4);
            panel.Controls.Add(msg5);

            AdjustWidths();
            return panel;
        }
    }
}
