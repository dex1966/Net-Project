namespace ChatP2P.UI.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Panel pnlUser;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserStatus;

        private System.Windows.Forms.TextBox txtSearch;

        private System.Windows.Forms.Label lblPeers;
        private System.Windows.Forms.FlowLayoutPanel flpPeers;

        private System.Windows.Forms.Label lblGroups;
        private System.Windows.Forms.FlowLayoutPanel flpGroups;

        private System.Windows.Forms.Panel pnlConnection;
        private System.Windows.Forms.Label lblConnectionTitle;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnCreateGroup;
        private System.Windows.Forms.Label lblConnectionStatus;

        private System.Windows.Forms.Panel pnlChat;

        private System.Windows.Forms.Panel pnlChatHeader;
        private System.Windows.Forms.Label lblChatAvatar;
        private System.Windows.Forms.Label lblChatName;
        private System.Windows.Forms.Label lblChatStatus;

        private System.Windows.Forms.Panel pnlHistory;
        private System.Windows.Forms.Button btnLoadHistory;

        private System.Windows.Forms.Panel pnlMessages;

        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.Button btnEmoji;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;

        protected override void Dispose(bool disposing)
        {
            if (disposing &&
                (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components =
                new System.ComponentModel.Container();

            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.pnlUser = new System.Windows.Forms.Panel();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserStatus = new System.Windows.Forms.Label();

            this.txtSearch =
                new System.Windows.Forms.TextBox();

            this.lblPeers =
                new System.Windows.Forms.Label();

            this.flpPeers =
                new System.Windows.Forms.FlowLayoutPanel();

            this.lblGroups =
                new System.Windows.Forms.Label();

            this.flpGroups =
                new System.Windows.Forms.FlowLayoutPanel();

            this.pnlConnection =
                new System.Windows.Forms.Panel();

            this.lblConnectionTitle =
                new System.Windows.Forms.Label();

            this.lblIP =
                new System.Windows.Forms.Label();

            this.txtIP =
                new System.Windows.Forms.TextBox();

            this.lblPort =
                new System.Windows.Forms.Label();

            this.txtPort =
                new System.Windows.Forms.TextBox();

            this.btnConnect =
                new System.Windows.Forms.Button();

            this.btnCreateGroup =
                new System.Windows.Forms.Button();

            this.lblConnectionStatus =
                new System.Windows.Forms.Label();

            this.pnlChat =
                new System.Windows.Forms.Panel();

            this.pnlChatHeader =
                new System.Windows.Forms.Panel();

            this.lblChatAvatar =
                new System.Windows.Forms.Label();

            this.lblChatName =
                new System.Windows.Forms.Label();

            this.lblChatStatus =
                new System.Windows.Forms.Label();

            this.pnlHistory =
                new System.Windows.Forms.Panel();

            this.btnLoadHistory =
                new System.Windows.Forms.Button();

            this.pnlMessages =
                new System.Windows.Forms.Panel();

            this.pnlInput =
                new System.Windows.Forms.Panel();

            this.btnEmoji =
                new System.Windows.Forms.Button();

            this.txtMessage =
                new System.Windows.Forms.TextBox();

            this.btnSend =
                new System.Windows.Forms.Button();

            // =========================================
            // FORM
            // =========================================

            this.SuspendLayout();

            this.AutoScaleDimensions =
                new System.Drawing.SizeF(7F, 15F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;

            this.BackColor =
                System.Drawing.Color.FromArgb(
                    245, 246, 250);

            this.ClientSize =
                new System.Drawing.Size(1200, 720);

            this.MinimumSize =
                new System.Drawing.Size(950, 600);

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Text =
                "ChatP2P";

            // =========================================
            // SIDEBAR
            // =========================================

            this.pnlSidebar.BackColor =
                System.Drawing.Color.White;

            this.pnlSidebar.Dock =
                System.Windows.Forms.DockStyle.Left;

            this.pnlSidebar.Width = 300;

            this.pnlSidebar.Padding =
                new System.Windows.Forms.Padding(
                    18, 0, 18, 12);

            // =========================================
            // LOGO
            // =========================================

            this.pnlLogo.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.pnlLogo.Height = 65;

            this.lblLogo.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.lblLogo.Text =
                "💬  ChatP2P";

            this.lblLogo.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    17F,
                    System.Drawing.FontStyle.Bold);

            this.lblLogo.ForeColor =
                System.Drawing.Color.FromArgb(
                    65, 75, 160);

            this.lblLogo.TextAlign =
                System.Drawing.ContentAlignment.MiddleLeft;

            this.pnlLogo.Controls.Add(
                this.lblLogo);

            // =========================================
            // USER
            // =========================================

            this.pnlUser.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.pnlUser.Height = 60;

            this.pnlUser.BackColor =
                System.Drawing.Color.FromArgb(
                    246, 247, 252);

            this.lblUserName.Left = 15;
            this.lblUserName.Top = 9;
            this.lblUserName.Width = 230;
            this.lblUserName.Height = 23;

            this.lblUserName.Text =
                "Nguyễn Lê Gia Hân";

            this.lblUserName.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.lblUserName.ForeColor =
                System.Drawing.Color.FromArgb(
                    35, 35, 35);

            this.lblUserStatus.Left = 15;
            this.lblUserStatus.Top = 33;
            this.lblUserStatus.Width = 200;
            this.lblUserStatus.Height = 18;

            this.lblUserStatus.Text =
                "● Đang hoạt động";

            this.lblUserStatus.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F);

            this.lblUserStatus.ForeColor =
                System.Drawing.Color.FromArgb(
                    50, 170, 100);

            this.pnlUser.Controls.Add(
                this.lblUserName);

            this.pnlUser.Controls.Add(
                this.lblUserStatus);

            // =========================================
            // SEARCH
            // =========================================

            this.txtSearch.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.txtSearch.Height = 38;

            this.txtSearch.Margin =
                new System.Windows.Forms.Padding(
                    0, 10, 0, 10);

            this.txtSearch.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F);

            this.txtSearch.Text =
                "";

            this.txtSearch.PlaceholderText =
                "🔍  Tìm kiếm peer...";

            this.txtSearch.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;

            this.txtSearch.TextChanged +=
                new System.EventHandler(
                    this.txtSearch_TextChanged);

            // =========================================
            // PEERS LABEL
            // =========================================

            this.lblPeers.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.lblPeers.Height = 35;

            this.lblPeers.Text =
                "PEERS";

            this.lblPeers.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F,
                    System.Drawing.FontStyle.Bold);

            this.lblPeers.ForeColor =
                System.Drawing.Color.Gray;

            this.lblPeers.TextAlign =
                System.Drawing.ContentAlignment.BottomLeft;

            // =========================================
            // PEERS LIST
            // =========================================

            this.flpPeers.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.flpPeers.Height = 185;

            this.flpPeers.FlowDirection =
                System.Windows.Forms.FlowDirection.TopDown;

            this.flpPeers.WrapContents = false;

            this.flpPeers.AutoScroll = true;

            this.flpPeers.BackColor =
                System.Drawing.Color.White;

            // =========================================
            // GROUP LABEL
            // =========================================

            this.lblGroups.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.lblGroups.Height = 35;

            this.lblGroups.Text =
                "GROUPS";

            this.lblGroups.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F,
                    System.Drawing.FontStyle.Bold);

            this.lblGroups.ForeColor =
                System.Drawing.Color.Gray;

            this.lblGroups.TextAlign =
                System.Drawing.ContentAlignment.BottomLeft;

            // =========================================
            // GROUP LIST
            // =========================================

            this.flpGroups.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.flpGroups.Height = 145;

            this.flpGroups.FlowDirection =
                System.Windows.Forms.FlowDirection.TopDown;

            this.flpGroups.WrapContents = false;

            this.flpGroups.AutoScroll = true;

            this.flpGroups.BackColor =
                System.Drawing.Color.White;

            // =========================================
            // CONNECTION PANEL
            // =========================================

            this.pnlConnection.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.pnlConnection.Padding =
                new System.Windows.Forms.Padding(
                    0, 8, 0, 0);

            this.lblConnectionTitle.Left = 0;
            this.lblConnectionTitle.Top = 5;
            this.lblConnectionTitle.Width = 260;
            this.lblConnectionTitle.Height = 22;

            this.lblConnectionTitle.Text =
                "KẾT NỐI PEER";

            this.lblConnectionTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F,
                    System.Drawing.FontStyle.Bold);

            this.lblConnectionTitle.ForeColor =
                System.Drawing.Color.Gray;

            // IP
            this.lblIP.Left = 0;
            this.lblIP.Top = 33;
            this.lblIP.Width = 50;
            this.lblIP.Height = 22;

            this.lblIP.Text = "IP";

            this.lblIP.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F);

            this.txtIP.Left = 48;
            this.txtIP.Top = 29;
            this.txtIP.Width = 195;
            this.txtIP.Height = 28;

            this.txtIP.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F);

            this.txtIP.Text =
                "127.0.0.1";

            // Port
            this.lblPort.Left = 0;
            this.lblPort.Top = 68;
            this.lblPort.Width = 50;
            this.lblPort.Height = 22;

            this.lblPort.Text = "Port";

            this.lblPort.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F);

            this.txtPort.Left = 48;
            this.txtPort.Top = 64;
            this.txtPort.Width = 195;
            this.txtPort.Height = 28;

            this.txtPort.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F);

            this.txtPort.Text = "6000";

            // CONNECT
            this.btnConnect.Left = 0;
            this.btnConnect.Top = 101;
            this.btnConnect.Width = 120;
            this.btnConnect.Height = 34;

            this.btnConnect.Text =
                "🔌  Kết nối";

            this.btnConnect.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnConnect.FlatAppearance.BorderSize =
                0;

            this.btnConnect.BackColor =
                System.Drawing.Color.FromArgb(
                    88, 101, 242);

            this.btnConnect.ForeColor =
                System.Drawing.Color.White;

            this.btnConnect.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnConnect.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnConnect.Click +=
                new System.EventHandler(
                    this.btnConnect_Click);

            // CREATE GROUP
            this.btnCreateGroup.Left = 130;
            this.btnCreateGroup.Top = 101;
            this.btnCreateGroup.Width = 113;
            this.btnCreateGroup.Height = 34;

            this.btnCreateGroup.Text =
                "+ Tạo nhóm";

            this.btnCreateGroup.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnCreateGroup.FlatAppearance.BorderSize =
                1;

            this.btnCreateGroup.FlatAppearance.BorderColor =
                System.Drawing.Color.FromArgb(
                    220, 220, 230);

            this.btnCreateGroup.BackColor =
                System.Drawing.Color.White;

            this.btnCreateGroup.ForeColor =
                System.Drawing.Color.FromArgb(
                    70, 70, 80);

            this.btnCreateGroup.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F,
                    System.Drawing.FontStyle.Bold);

            this.btnCreateGroup.Click +=
                new System.EventHandler(
                    this.btnCreateGroup_Click);

            // STATUS
            this.lblConnectionStatus.Left = 0;
            this.lblConnectionStatus.Top = 143;
            this.lblConnectionStatus.Width = 250;
            this.lblConnectionStatus.Height = 22;

            this.lblConnectionStatus.Text =
                "● Chưa kết nối";

            this.lblConnectionStatus.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F);

            this.lblConnectionStatus.ForeColor =
                System.Drawing.Color.Gray;

            this.pnlConnection.Controls.Add(
                this.lblConnectionTitle);

            this.pnlConnection.Controls.Add(
                this.lblIP);

            this.pnlConnection.Controls.Add(
                this.txtIP);

            this.pnlConnection.Controls.Add(
                this.lblPort);

            this.pnlConnection.Controls.Add(
                this.txtPort);

            this.pnlConnection.Controls.Add(
                this.btnConnect);

            this.pnlConnection.Controls.Add(
                this.btnCreateGroup);

            this.pnlConnection.Controls.Add(
                this.lblConnectionStatus);

            // =========================================
            // ADD SIDEBAR
            // =========================================

            this.pnlSidebar.Controls.Add(
                this.pnlConnection);

            this.pnlSidebar.Controls.Add(
                this.flpGroups);

            this.pnlSidebar.Controls.Add(
                this.lblGroups);

            this.pnlSidebar.Controls.Add(
                this.flpPeers);

            this.pnlSidebar.Controls.Add(
                this.lblPeers);

            this.pnlSidebar.Controls.Add(
                this.txtSearch);

            this.pnlSidebar.Controls.Add(
                this.pnlUser);

            this.pnlSidebar.Controls.Add(
                this.pnlLogo);

            // =========================================
            // CHAT PANEL
            // =========================================

            this.pnlChat.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.pnlChat.BackColor =
                System.Drawing.Color.FromArgb(
                    245, 246, 250);

            // =========================================
            // CHAT HEADER
            // =========================================

            this.pnlChatHeader.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.pnlChatHeader.Height = 75;

            this.pnlChatHeader.BackColor =
                System.Drawing.Color.White;

            // Avatar
            this.lblChatAvatar.Left = 20;
            this.lblChatAvatar.Top = 14;
            this.lblChatAvatar.Width = 48;
            this.lblChatAvatar.Height = 48;

            this.lblChatAvatar.Text = "👤";

            this.lblChatAvatar.Font =
                new System.Drawing.Font(
                    "Segoe UI Emoji",
                    25F);

            this.lblChatAvatar.TextAlign =
                System.Drawing.ContentAlignment.MiddleCenter;

            // Name
            this.lblChatName.Left = 82;
            this.lblChatName.Top = 14;
            this.lblChatName.Width = 500;
            this.lblChatName.Height = 27;

            this.lblChatName.Text =
                "Chọn một cuộc trò chuyện";

            this.lblChatName.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    11F,
                    System.Drawing.FontStyle.Bold);

            this.lblChatName.ForeColor =
                System.Drawing.Color.FromArgb(
                    35, 35, 35);

            // Status
            this.lblChatStatus.Left = 82;
            this.lblChatStatus.Top = 42;
            this.lblChatStatus.Width = 400;
            this.lblChatStatus.Height = 20;

            this.lblChatStatus.Text =
                "";

            this.lblChatStatus.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F);

            this.lblChatStatus.ForeColor =
                System.Drawing.Color.Gray;

            this.pnlChatHeader.Controls.Add(
                this.lblChatAvatar);

            this.pnlChatHeader.Controls.Add(
                this.lblChatName);

            this.pnlChatHeader.Controls.Add(
                this.lblChatStatus);

            // =========================================
            // HISTORY
            // =========================================

            this.pnlHistory.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.pnlHistory.Height = 42;

            this.pnlHistory.BackColor =
                System.Drawing.Color.FromArgb(
                    245, 246, 250);

            this.btnLoadHistory.Width = 180;
            this.btnLoadHistory.Height = 30;

            this.btnLoadHistory.Top = 6;

            this.btnLoadHistory.Text =
                "↓  Tải thêm lịch sử";

            this.btnLoadHistory.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnLoadHistory.FlatAppearance.BorderSize =
                0;

            this.btnLoadHistory.BackColor =
                System.Drawing.Color.Transparent;

            this.btnLoadHistory.ForeColor =
                System.Drawing.Color.FromArgb(
                    80, 90, 170);

            this.btnLoadHistory.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F,
                    System.Drawing.FontStyle.Bold);

            this.btnLoadHistory.Anchor =
                System.Windows.Forms.AnchorStyles.Top;

            this.btnLoadHistory.Click +=
                new System.EventHandler(
                    this.btnLoadHistory_Click);

            this.pnlHistory.Controls.Add(
                this.btnLoadHistory);

            // =========================================
            // MESSAGES
            // =========================================

            this.pnlMessages.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.pnlMessages.AutoScroll = true;

            this.pnlMessages.Padding =
                new System.Windows.Forms.Padding(
                    15, 10, 15, 10);

            this.pnlMessages.BackColor =
                System.Drawing.Color.FromArgb(
                    245, 246, 250);

            // =========================================
            // INPUT
            // =========================================

            this.pnlInput.Dock =
                System.Windows.Forms.DockStyle.Bottom;

            this.pnlInput.Height = 65;

            this.pnlInput.BackColor =
                System.Drawing.Color.White;

            this.pnlInput.Padding =
                new System.Windows.Forms.Padding(
                    12, 12, 12, 12);

            // EMOJI
            this.btnEmoji.Dock =
                System.Windows.Forms.DockStyle.Left;

            this.btnEmoji.Width = 45;

            this.btnEmoji.Text =
                "😊";

            this.btnEmoji.Font =
                new System.Drawing.Font(
                    "Segoe UI Emoji",
                    17F);

            this.btnEmoji.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnEmoji.FlatAppearance.BorderSize =
                0;

            this.btnEmoji.BackColor =
                System.Drawing.Color.White;

            this.btnEmoji.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnEmoji.Click +=
                new System.EventHandler(
                    this.btnEmoji_Click);

            // SEND
            this.btnSend.Dock =
                System.Windows.Forms.DockStyle.Right;

            this.btnSend.Width = 55;

            this.btnSend.Text =
                "➤";

            this.btnSend.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    16F,
                    System.Drawing.FontStyle.Bold);

            this.btnSend.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnSend.FlatAppearance.BorderSize =
                0;

            this.btnSend.BackColor =
                System.Drawing.Color.FromArgb(
                    88, 101, 242);

            this.btnSend.ForeColor =
                System.Drawing.Color.White;

            this.btnSend.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnSend.Click +=
                new System.EventHandler(
                    this.btnSend_Click);

            // MESSAGE TEXTBOX
            this.txtMessage.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.txtMessage.Multiline = true;

            this.txtMessage.Height = 40;

            this.txtMessage.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F);

            this.txtMessage.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;

            this.txtMessage.PlaceholderText =
                "Nhập tin nhắn...";

            this.pnlInput.Controls.Add(
                this.txtMessage);

            this.pnlInput.Controls.Add(
                this.btnSend);

            this.pnlInput.Controls.Add(
                this.btnEmoji);

            // =========================================
            // CHAT ADD
            // =========================================

            this.pnlChat.Controls.Add(
                this.pnlMessages);

            this.pnlChat.Controls.Add(
                this.pnlHistory);

            this.pnlChat.Controls.Add(
                this.pnlInput);

            this.pnlChat.Controls.Add(
                this.pnlChatHeader);

            // =========================================
            // FORM ADD
            // =========================================

            this.Controls.Add(
                this.pnlChat);

            this.Controls.Add(
                this.pnlSidebar);

            this.ResumeLayout(false);
        }
    }
}