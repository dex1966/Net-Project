using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ChatP2P.UI.Forms
{
    public partial class MainForm : Form
    {
        private string currentChatName = "";
        private bool isConnected = false;

        public MainForm()
        {
            InitializeComponent();

            // Dữ liệu demo để nhìn giao diện
            LoadDemoData();

            // Cho phép Enter gửi tin nhắn
            txtMessage.KeyDown += TxtMessage_KeyDown;

            // Resize lại khung chat
            this.Resize += MainForm_Resize;
        }

        private void LoadDemoData()
        {
            AddPeer("Nguyễn Văn A", "🟢 Online");
            AddPeer("Trần Minh B", "🟢 Online");
            AddPeer("Lê Hoàng C", "🔴 Offline");
            AddPeer("Phạm Gia D", "🟢 Online");

            AddGroup("Nhóm Lập Trình Mạng");
            AddGroup("Nhóm Project");
            AddGroup("Team IT");

            currentChatName = "Nguyễn Văn A";
            lblChatName.Text = currentChatName;
            lblChatStatus.Text = "🟢 Online";

            AddReceivedMessage("Xin chào 👋", "10:20");
            AddReceivedMessage("Bạn đã làm xong giao diện chưa?", "10:21");
            AddSentMessage("Mình đang làm nè 😄", "10:22");
        }

        // =========================
        // PEER
        // =========================

        private void AddPeer(string name, string status)
        {
            Panel item = new Panel();

            item.Width = 260;
            item.Height = 68;
            item.BackColor = Color.White;
            item.Margin = new Padding(0, 2, 0, 2);
            item.Cursor = Cursors.Hand;
            item.Tag = name;

            // Avatar
            Panel avatar = new Panel();
            avatar.Width = 44;
            avatar.Height = 44;
            avatar.Left = 12;
            avatar.Top = 12;
            avatar.BackColor = Color.FromArgb(225, 232, 255);

            Label avatarText = new Label();
            avatarText.Text = name.Length > 0
                ? name.Substring(0, 1).ToUpper()
                : "?";

            avatarText.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            avatarText.ForeColor = Color.FromArgb(70, 90, 180);
            avatarText.AutoSize = false;
            avatarText.TextAlign = ContentAlignment.MiddleCenter;
            avatarText.Dock = DockStyle.Fill;

            avatar.Controls.Add(avatarText);

            // Tên
            Label lblName = new Label();
            lblName.Text = name;
            lblName.Left = 68;
            lblName.Top = 13;
            lblName.Width = 175;
            lblName.Height = 22;
            lblName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblName.ForeColor = Color.FromArgb(35, 35, 35);

            // Status
            Label lblStatus = new Label();
            lblStatus.Text = status;
            lblStatus.Left = 68;
            lblStatus.Top = 36;
            lblStatus.Width = 175;
            lblStatus.Height = 18;
            lblStatus.Font = new Font("Segoe UI", 8.5F);
            lblStatus.ForeColor =
                status.Contains("Online")
                ? Color.FromArgb(50, 170, 100)
                : Color.Gray;

            item.Controls.Add(avatar);
            item.Controls.Add(lblName);
            item.Controls.Add(lblStatus);

            item.Click += PeerItem_Click;
            avatar.Click += (s, e) => PeerItem_Click(item, e);
            avatarText.Click += (s, e) => PeerItem_Click(item, e);
            lblName.Click += (s, e) => PeerItem_Click(item, e);
            lblStatus.Click += (s, e) => PeerItem_Click(item, e);

            flpPeers.Controls.Add(item);
        }

        private void PeerItem_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;

            if (control == null)
                return;

            string name = control.Tag?.ToString();

            if (string.IsNullOrEmpty(name))
            {
                if (control.Parent != null)
                    name = control.Parent.Tag?.ToString();
            }

            if (string.IsNullOrEmpty(name))
                return;

            currentChatName = name;

            lblChatName.Text = name;
            lblChatStatus.Text = "🟢 Online";

            pnlMessages.Controls.Clear();

            AddReceivedMessage("Xin chào 👋", "10:20");
            AddReceivedMessage(
                "Đây là cuộc trò chuyện với " + name,
                "10:21"
            );
        }

        // =========================
        // GROUP
        // =========================

        private void AddGroup(string name)
        {
            Panel item = new Panel();

            item.Width = 260;
            item.Height = 62;
            item.BackColor = Color.White;
            item.Margin = new Padding(0, 2, 0, 2);
            item.Cursor = Cursors.Hand;
            item.Tag = name;

            Panel avatar = new Panel();
            avatar.Width = 42;
            avatar.Height = 42;
            avatar.Left = 12;
            avatar.Top = 10;
            avatar.BackColor = Color.FromArgb(235, 235, 245);

            Label icon = new Label();
            icon.Text = "👥";
            icon.Font = new Font("Segoe UI Emoji", 16F);
            icon.AutoSize = false;
            icon.Dock = DockStyle.Fill;
            icon.TextAlign = ContentAlignment.MiddleCenter;

            avatar.Controls.Add(icon);

            Label lblName = new Label();
            lblName.Text = name;
            lblName.Left = 66;
            lblName.Top = 18;
            lblName.Width = 180;
            lblName.Height = 25;
            lblName.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblName.ForeColor = Color.FromArgb(45, 45, 45);

            item.Controls.Add(avatar);
            item.Controls.Add(lblName);

            item.Click += GroupItem_Click;
            avatar.Click += (s, e) => GroupItem_Click(item, e);
            icon.Click += (s, e) => GroupItem_Click(item, e);
            lblName.Click += (s, e) => GroupItem_Click(item, e);

            flpGroups.Controls.Add(item);
        }

        private void GroupItem_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;

            if (control == null)
                return;

            string name = control.Tag?.ToString();

            if (string.IsNullOrEmpty(name))
            {
                if (control.Parent != null)
                    name = control.Parent.Tag?.ToString();
            }

            if (string.IsNullOrEmpty(name))
                return;

            currentChatName = name;

            lblChatName.Text = name;
            lblChatStatus.Text = "👥 Nhóm";

            pnlMessages.Controls.Clear();

            AddReceivedMessage(
                "Chào mừng bạn đến với " + name + " 👋",
                "09:30"
            );

            AddReceivedMessage(
                "Đây là giao diện chat nhóm.",
                "09:31"
            );
        }

        // =========================
        // MESSAGE
        // =========================

        private void AddReceivedMessage(string message, string time)
        {
            Panel row = CreateMessageRow(false);

            Panel bubble = CreateBubble(
                message,
                time,
                false
            );

            row.Controls.Add(bubble);

            pnlMessages.Controls.Add(row);

            ScrollToBottom();
        }

        private void AddSentMessage(string message, string time)
        {
            Panel row = CreateMessageRow(true);

            Panel bubble = CreateBubble(
                message,
                time,
                true
            );

            row.Controls.Add(bubble);

            pnlMessages.Controls.Add(row);

            ScrollToBottom();
        }

        private Panel CreateMessageRow(bool sent)
        {
            Panel row = new Panel();

            row.Width = pnlMessages.ClientSize.Width - 25;
            row.Height = 65;
            row.Margin = new Padding(5, 4, 5, 4);

            row.BackColor = Color.Transparent;

            return row;
        }

        private Panel CreateBubble(
            string message,
            string time,
            bool sent)
        {
            Panel bubble = new Panel();

            bubble.AutoSize = true;
            bubble.MaximumSize = new Size(500, 0);
            bubble.Padding = new Padding(14, 10, 14, 9);

            if (sent)
            {
                bubble.BackColor = Color.FromArgb(88, 101, 242);
            }
            else
            {
                bubble.BackColor = Color.White;
            }

            Label lblMessage = new Label();

            lblMessage.Text = message;
            lblMessage.AutoSize = true;
            lblMessage.MaximumSize = new Size(450, 0);

            lblMessage.Font = new Font(
                "Segoe UI",
                10F,
                FontStyle.Regular
            );

            lblMessage.ForeColor = sent
                ? Color.White
                : Color.FromArgb(40, 40, 40);

            Label lblTime = new Label();

            lblTime.Text = time;
            lblTime.AutoSize = true;

            lblTime.Font = new Font(
                "Segoe UI",
                7.5F
            );

            lblTime.ForeColor = sent
                ? Color.FromArgb(220, 225, 255)
                : Color.Gray;

            lblTime.Top = lblMessage.Bottom + 5;

            bubble.Controls.Add(lblMessage);
            bubble.Controls.Add(lblTime);

            // Tự resize
            bubble.SizeChanged += (s, e) =>
            {
                lblTime.Left =
                    bubble.ClientSize.Width -
                    lblTime.Width -
                    14;
            };

            return bubble;
        }

        private void ScrollToBottom()
        {
            pnlMessages.VerticalScroll.Value =
                pnlMessages.VerticalScroll.Maximum;

            pnlMessages.PerformLayout();
        }

        // =========================
        // SEND MESSAGE
        // =========================

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendCurrentMessage();
        }

        private void SendCurrentMessage()
        {
            string message = txtMessage.Text.Trim();

            if (string.IsNullOrEmpty(message))
                return;

            if (string.IsNullOrEmpty(currentChatName))
            {
                MessageBox.Show(
                    "Vui lòng chọn một Peer hoặc nhóm để chat.",
                    "ChatP2P",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            string time = DateTime.Now.ToString("HH:mm");

            AddSentMessage(message, time);

            txtMessage.Clear();
            txtMessage.Focus();

            // TODO:
            // Sau này nối PeerNode.SendToPeer()
            // hoặc PeerNode.SendToMany() tại đây.
        }

        private void TxtMessage_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter &&
                !e.Shift)
            {
                e.SuppressKeyPress = true;

                SendCurrentMessage();
            }
        }

        // =========================
        // EMOJI
        // =========================

        private void btnEmoji_Click(object sender, EventArgs e)
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            string[] emojis =
            {
                "😀", "😂", "😍", "🥰",
                "😎", "😭", "😡", "👍",
                "👎", "❤️", "🔥", "🎉",
                "👏", "🙏", "😄", "😆",
                "😉", "😊", "🤔", "😴"
            };

            foreach (string emoji in emojis)
            {
                ToolStripMenuItem item =
                    new ToolStripMenuItem(emoji);

                item.Font =
                    new Font("Segoe UI Emoji", 14F);

                item.Click += (s, ev) =>
                {
                    txtMessage.SelectedText = emoji;
                    txtMessage.Focus();
                };

                menu.Items.Add(item);
            }

            menu.Show(
                btnEmoji,
                new Point(
                    0,
                    -menu.Height
                )
            );
        }

        // =========================
        // CONNECT PEER
        // =========================

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            string port = txtPort.Text.Trim();

            if (string.IsNullOrEmpty(ip) ||
                string.IsNullOrEmpty(port))
            {
                MessageBox.Show(
                    "Vui lòng nhập IP và Port.",
                    "Kết nối Peer",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            if (!int.TryParse(port, out int portNumber))
            {
                MessageBox.Show(
                    "Port phải là số.",
                    "Kết nối Peer",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            if (portNumber < 1 ||
                portNumber > 65535)
            {
                MessageBox.Show(
                    "Port phải nằm trong khoảng 1 - 65535.",
                    "Kết nối Peer",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // UI demo
            isConnected = true;

            lblConnectionStatus.Text =
                "● Đã kết nối";

            lblConnectionStatus.ForeColor =
                Color.FromArgb(45, 175, 100);

            MessageBox.Show(
                $"Đã sẵn sàng kết nối tới {ip}:{portNumber}.",
                "ChatP2P",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            // TODO:
            // Nối với:
            // PeerNode.ConnectToPeer(ip, portNumber)
        }

        // =========================
        // CREATE GROUP
        // =========================

        private void btnCreateGroup_Click(
            object sender,
            EventArgs e)
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Tạo nhóm";
                dialog.StartPosition =
                    FormStartPosition.CenterParent;

                dialog.Size = new Size(380, 180);

                dialog.FormBorderStyle =
                    FormBorderStyle.FixedDialog;

                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label label = new Label();

                label.Text = "Tên nhóm:";
                label.Left = 25;
                label.Top = 25;
                label.Width = 100;

                TextBox textBox = new TextBox();

                textBox.Left = 25;
                textBox.Top = 55;
                textBox.Width = 300;

                Button ok = new Button();

                ok.Text = "Tạo nhóm";
                ok.Left = 225;
                ok.Top = 95;
                ok.Width = 100;

                ok.DialogResult =
                    DialogResult.OK;

                dialog.Controls.Add(label);
                dialog.Controls.Add(textBox);
                dialog.Controls.Add(ok);

                dialog.AcceptButton = ok;

                if (dialog.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    string groupName =
                        textBox.Text.Trim();

                    if (!string.IsNullOrEmpty(groupName))
                    {
                        AddGroup(groupName);

                        MessageBox.Show(
                            "Đã tạo nhóm: " + groupName,
                            "ChatP2P",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        // TODO:
                        // Sau này gọi GroupRepository
                        // và gửi group_invite qua PeerNode.
                    }
                }
            }
        }

        // =========================
        // LOAD HISTORY
        // =========================

        private void btnLoadHistory_Click(
            object sender,
            EventArgs e)
        {
            MessageBox.Show(
                "Chức năng này sẽ lấy thêm 20 tin nhắn cũ từ SQLite.",
                "Lịch sử chat",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            // TODO:
            // Gọi MessageRepository
            // với LIMIT 20 + OFFSET
        }

        // =========================
        // SEARCH
        // =========================

        private void txtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            string keyword =
                txtSearch.Text.Trim().ToLower();

            foreach (Control control
                in flpPeers.Controls)
            {
                if (control.Tag == null)
                    continue;

                string name =
                    control.Tag.ToString().ToLower();

                control.Visible =
                    string.IsNullOrEmpty(keyword) ||
                    name.Contains(keyword);
            }
        }

        // =========================
        // RESIZE
        // =========================

        private void MainForm_Resize(
            object sender,
            EventArgs e)
        {
            foreach (Control control
                in pnlMessages.Controls)
            {
                control.Width =
                    pnlMessages.ClientSize.Width - 25;
            }
        }
    }
}