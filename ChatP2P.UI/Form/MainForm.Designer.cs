namespace ChatP2P.UI.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlChat;
        private System.Windows.Forms.Panel pnlChatHeader;
        private System.Windows.Forms.Panel pnlComposer;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label lblPeerTitle;
        private System.Windows.Forms.Label lblGroupTitle;

        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnConnect;

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnCreateGroup;

        private System.Windows.Forms.FlowLayoutPanel flpPeers;
        private System.Windows.Forms.FlowLayoutPanel flpGroups;

        private System.Windows.Forms.Label lblChatName;
        private System.Windows.Forms.Label lblChatStatus;
        private System.Windows.Forms.Button btnLoadHistory;

        private System.Windows.Forms.Panel pnlMessages;

        private System.Windows.Forms.Button btnEmoji;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlChat = new System.Windows.Forms.Panel();
            this.pnlChatHeader = new System.Windows.Forms.Panel();
            this.pnlComposer = new System.Windows.Forms.Panel();

            this.lblTitle = new System.Windows.Forms.Label();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.lblPeerTitle = new System.Windows.Forms.Label();
            this.lblGroupTitle = new System.Windows.Forms.Label();

            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();

            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnCreateGroup = new System.Windows.Forms.Button();

            this.flpPeers = new System.Windows.Forms.FlowLayoutPanel();
            this.flpGroups = new System.Windows.Forms.FlowLayoutPanel();

            this.lblChatName = new System.Windows.Forms.Label();
            this.lblChatStatus = new System.Windows.Forms.Label();
            this.btnLoadHistory = new System.Windows.Forms.Button();

            this.pnlMessages = new System.Windows.Forms.Panel();

            this.btnEmoji = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();

            this.pnlSidebar.SuspendLayout();
            this.pnlChat.SuspendLayout();
            this.pnlChatHeader.SuspendLayout();
            this.pnlComposer.SuspendLayout();
            this.SuspendLayout();

            // =====================================================
            // MAIN FORM
            // =====================================================

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.MinimumSize = new System.Drawing.Size(900, 600);

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChatP2P";
            this.BackColor = System.Drawing.Color.White;

            // =====================================================
            // SIDEBAR
            // =====================================================

            this.pnlSidebar.BackColor =
                System.Drawing.Color.FromArgb(245, 246, 250);

            this.pnlSidebar.Dock =
                System.Windows.Forms.DockStyle.Left;

            this.pnlSidebar.Width = 300;

            this.pnlSidebar.Padding =
                new System.Windows.Forms.Padding(15);

            // =====================================================
            // TITLE
            // =====================================================

            this.lblTitle.AutoSize = true;

            this.lblTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    18F,
                    System.Drawing.FontStyle.Bold);

            this.lblTitle.ForeColor =
                System.Drawing.Color.FromArgb(45, 45, 55);

            this.lblTitle.Location =
                new System.Drawing.Point(15, 15);

            this.lblTitle.Text = "ChatP2P";

            // =====================================================
            // CONNECTION STATUS
            // =====================================================

            this.lblConnectionStatus.AutoSize = true;

            this.lblConnectionStatus.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F);

            this.lblConnectionStatus.ForeColor =
                System.Drawing.Color.Gray;

            this.lblConnectionStatus.Location =
                new System.Drawing.Point(17, 50);

            this.lblConnectionStatus.Text =
                "● Chưa kết nối";

            // =====================================================
            // IP TEXTBOX
            // =====================================================

            this.txtIP.Location =
                new System.Drawing.Point(15, 78);

            this.txtIP.Size =
                new System.Drawing.Size(150, 27);

            this.txtIP.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F);

            this.txtIP.Text = "127.0.0.1";

            // =====================================================
            // PORT TEXTBOX
            // =====================================================

            this.txtPort.Location =
                new System.Drawing.Point(172, 78);

            this.txtPort.Size =
                new System.Drawing.Size(55, 27);

            this.txtPort.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F);

            this.txtPort.Text = "5000";

            // =====================================================
            // CONNECT BUTTON
            // =====================================================

            this.btnConnect.Location =
                new System.Drawing.Point(15, 112);

            this.btnConnect.Size =
                new System.Drawing.Size(212, 32);

            this.btnConnect.Text =
                "Kết nối Peer";

            this.btnConnect.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnConnect.BackColor =
                System.Drawing.Color.FromArgb(88, 101, 242);

            this.btnConnect.ForeColor =
                System.Drawing.Color.White;

            this.btnConnect.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnConnect.FlatAppearance.BorderSize = 0;

            this.btnConnect.Click +=
                new System.EventHandler(
                    this.btnConnect_Click);

            // =====================================================
            // SEARCH
            // =====================================================

            this.txtSearch.Location =
                new System.Drawing.Point(15, 158);

            this.txtSearch.Size =
                new System.Drawing.Size(260, 30);

            this.txtSearch.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9.5F);

            this.txtSearch.PlaceholderText =
                "🔍  Tìm kiếm Peer...";

            this.txtSearch.TextChanged +=
                new System.EventHandler(
                    this.txtSearch_TextChanged);

            // =====================================================
            // PEER TITLE
            // =====================================================

            this.lblPeerTitle.AutoSize = true;

            this.lblPeerTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.lblPeerTitle.ForeColor =
                System.Drawing.Color.FromArgb(70, 70, 80);

            this.lblPeerTitle.Location =
                new System.Drawing.Point(15, 200);

            this.lblPeerTitle.Text =
                "PEERS";

            // =====================================================
            // PEERS FLOW PANEL
            // =====================================================

            this.flpPeers.Location =
                new System.Drawing.Point(15, 228);

            this.flpPeers.Size =
                new System.Drawing.Size(260, 210);

            this.flpPeers.BackColor =
                System.Drawing.Color.Transparent;

            this.flpPeers.FlowDirection =
                System.Windows.Forms.FlowDirection.TopDown;

            this.flpPeers.WrapContents = false;

            this.flpPeers.AutoScroll = true;

            // =====================================================
            // GROUP TITLE
            // =====================================================

            this.lblGroupTitle.AutoSize = true;

            this.lblGroupTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.lblGroupTitle.ForeColor =
                System.Drawing.Color.FromArgb(70, 70, 80);

            this.lblGroupTitle.Location =
                new System.Drawing.Point(15, 450);

            this.lblGroupTitle.Text =
                "NHÓM";

            // =====================================================
            // CREATE GROUP BUTTON
            // =====================================================

            this.btnCreateGroup.Location =
                new System.Drawing.Point(215, 445);

            this.btnCreateGroup.Size =
                new System.Drawing.Size(60, 28);

            this.btnCreateGroup.Text =
                "+ Nhóm";

            this.btnCreateGroup.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8F,
                    System.Drawing.FontStyle.Bold);

            this.btnCreateGroup.BackColor =
                System.Drawing.Color.White;

            this.btnCreateGroup.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnCreateGroup.Click +=
                new System.EventHandler(
                    this.btnCreateGroup_Click);

            // =====================================================
            // GROUP FLOW PANEL
            // =====================================================

            this.flpGroups.Location =
                new System.Drawing.Point(15, 480);

            this.flpGroups.Size =
                new System.Drawing.Size(260, 150);

            this.flpGroups.BackColor =
                System.Drawing.Color.Transparent;

            this.flpGroups.FlowDirection =
                System.Windows.Forms.FlowDirection.TopDown;

            this.flpGroups.WrapContents = false;

            this.flpGroups.AutoScroll = true;

            // =====================================================
            // ADD SIDEBAR CONTROLS
            // =====================================================

            this.pnlSidebar.Controls.Add(
                this.lblTitle);

            this.pnlSidebar.Controls.Add(
                this.lblConnectionStatus);

            this.pnlSidebar.Controls.Add(
                this.txtIP);

            this.pnlSidebar.Controls.Add(
                this.txtPort);

            this.pnlSidebar.Controls.Add(
                this.btnConnect);

            this.pnlSidebar.Controls.Add(
                this.txtSearch);

            this.pnlSidebar.Controls.Add(
                this.lblPeerTitle);

            this.pnlSidebar.Controls.Add(
                this.flpPeers);

            this.pnlSidebar.Controls.Add(
                this.lblGroupTitle);

            this.pnlSidebar.Controls.Add(
                this.btnCreateGroup);

            this.pnlSidebar.Controls.Add(
                this.flpGroups);

            // =====================================================
            // CHAT PANEL
            // =====================================================

            this.pnlChat.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.pnlChat.BackColor =
                System.Drawing.Color.FromArgb(
                    245,
                    247,
                    251);

            // =====================================================
            // CHAT HEADER
            // =====================================================

            this.pnlChatHeader.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.pnlChatHeader.Height = 75;

            this.pnlChatHeader.BackColor =
                System.Drawing.Color.White;

            // =====================================================
            // CHAT NAME
            // =====================================================

            this.lblChatName.AutoSize = true;

            this.lblChatName.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    14F,
                    System.Drawing.FontStyle.Bold);

            this.lblChatName.ForeColor =
                System.Drawing.Color.FromArgb(
                    40,
                    40,
                    50);

            this.lblChatName.Location =
                new System.Drawing.Point(25, 12);

            this.lblChatName.Text =
                "Chọn một cuộc trò chuyện";

            // =====================================================
            // CHAT STATUS
            // =====================================================

            this.lblChatStatus.AutoSize = true;

            this.lblChatStatus.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F);

            this.lblChatStatus.ForeColor =
                System.Drawing.Color.FromArgb(
                    50,
                    170,
                    100);

            this.lblChatStatus.Location =
                new System.Drawing.Point(27, 42);

            this.lblChatStatus.Text =
                "● Online";

            // =====================================================
            // LOAD HISTORY BUTTON
            // =====================================================

            this.btnLoadHistory.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Right;

            this.btnLoadHistory.Location =
                new System.Drawing.Point(650, 20);

            this.btnLoadHistory.Size =
                new System.Drawing.Size(130, 35);

            this.btnLoadHistory.Text =
                "Lịch sử chat";

            this.btnLoadHistory.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F);

            this.btnLoadHistory.BackColor =
                System.Drawing.Color.White;

            this.btnLoadHistory.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnLoadHistory.Click +=
                new System.EventHandler(
                    this.btnLoadHistory_Click);

            // =====================================================
            // ADD HEADER CONTROLS
            // =====================================================

            this.pnlChatHeader.Controls.Add(
                this.lblChatName);

            this.pnlChatHeader.Controls.Add(
                this.lblChatStatus);

            this.pnlChatHeader.Controls.Add(
                this.btnLoadHistory);

            // =====================================================
            // MESSAGE PANEL
            // =====================================================

            this.pnlMessages.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.pnlMessages.AutoScroll = true;

            this.pnlMessages.BackColor =
                System.Drawing.Color.FromArgb(
                    245,
                    247,
                    251);

            this.pnlMessages.Padding =
                new System.Windows.Forms.Padding(10);

            // =====================================================
            // COMPOSER
            // =====================================================

            this.pnlComposer.Dock =
                System.Windows.Forms.DockStyle.Bottom;

            this.pnlComposer.Height = 80;

            this.pnlComposer.BackColor =
                System.Drawing.Color.White;

            // =====================================================
            // EMOJI BUTTON
            // =====================================================

            this.btnEmoji.Anchor =
                System.Windows.Forms.AnchorStyles.Left;

            this.btnEmoji.Location =
                new System.Drawing.Point(15, 18);

            this.btnEmoji.Size =
                new System.Drawing.Size(45, 45);

            this.btnEmoji.Text =
                "😊";

            this.btnEmoji.Font =
                new System.Drawing.Font(
                    "Segoe UI Emoji",
                    15F);

            this.btnEmoji.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnEmoji.Click +=
                new System.EventHandler(
                    this.btnEmoji_Click);

            // =====================================================
            // MESSAGE TEXTBOX
            // =====================================================

            this.txtMessage.Anchor =
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;

            this.txtMessage.Location =
                new System.Drawing.Point(70, 15);

            this.txtMessage.Size =
                new System.Drawing.Size(650, 50);

            this.txtMessage.Multiline = true;

            this.txtMessage.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F);

            this.txtMessage.ScrollBars =
                System.Windows.Forms.ScrollBars.Vertical;

            this.txtMessage.PlaceholderText =
                "Nhập tin nhắn...";

            // =====================================================
            // SEND BUTTON
            // =====================================================

            this.btnSend.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Right;

            this.btnSend.Location =
                new System.Drawing.Point(730, 15);

            this.btnSend.Size =
                new System.Drawing.Size(90, 50);

            this.btnSend.Text =
                "Gửi";

            this.btnSend.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.btnSend.BackColor =
                System.Drawing.Color.FromArgb(
                    88,
                    101,
                    242);

            this.btnSend.ForeColor =
                System.Drawing.Color.White;

            this.btnSend.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnSend.FlatAppearance.BorderSize = 0;

            this.btnSend.Click +=
                new System.EventHandler(
                    this.btnSend_Click);

            // =====================================================
            // ADD COMPOSER CONTROLS
            // =====================================================

            this.pnlComposer.Controls.Add(
                this.btnEmoji);

            this.pnlComposer.Controls.Add(
                this.txtMessage);

            this.pnlComposer.Controls.Add(
                this.btnSend);

            // =====================================================
            // ADD CHAT CONTROLS
            // =====================================================

            this.pnlChat.Controls.Add(
                this.pnlMessages);

            this.pnlChat.Controls.Add(
                this.pnlComposer);

            this.pnlChat.Controls.Add(
                this.pnlChatHeader);

            // =====================================================
            // ADD TO FORM
            // =====================================================

            this.Controls.Add(
                this.pnlChat);

            this.Controls.Add(
                this.pnlSidebar);

            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();

            this.pnlChat.ResumeLayout(false);

            this.pnlChatHeader.ResumeLayout(false);
            this.pnlChatHeader.PerformLayout();

            this.pnlComposer.ResumeLayout(false);
            this.pnlComposer.PerformLayout();

            this.ResumeLayout(false);
        }
    }
}