using System.Drawing;
using System.Windows.Forms;
using ChatP2P.Core.Models;
using ChatP2P.Data;
using ChatP2P.Data.Repositories;
using ChatP2P.UI.Controls;

namespace ChatP2P.UI.Form;

public class MainForm : System.Windows.Forms.Form
{
    private readonly string _currentUser;
    private readonly PeerRepository _peers = new(new AppDbContext());
    private readonly GroupRepository _groups = new(new AppDbContext());
    private readonly MessageRepository _messages = new(new AppDbContext());
    private readonly FlowLayoutPanel _peerList = CreateVerticalList();
    private readonly FlowLayoutPanel _groupList = CreateVerticalList();
    private readonly FlowLayoutPanel _history = CreateVerticalList();
    private readonly Panel _emptyState = new();
    private readonly Panel _chat = new();
    private readonly Label _chatTitle = new();
    private readonly Label _chatState = new();
    private readonly Label _connectionState = new();
    private readonly Label _composerNotice = new();
    private readonly TextBox _messageInput = new();
    private readonly Panel _replyBar = new();
    private readonly Label _replyLabel = new();
    private readonly TextBox _search = new();
    private readonly List<Peer> _allPeers = [];
    private readonly List<GroupChat> _allGroups = [];
    private Peer? _selectedPeer;
    private GroupChat? _selectedGroup;
    private ChatMessage? _replyTo;
    private EmojiPickerForm? _emojiPicker;

    public MainForm(string currentUser)
    {
        _currentUser = currentUser;
        Text = $"Chat P2P — {_currentUser}";
        MinimumSize = new Size(960, 650);
        Size = new Size(1180, 760);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.White;
        BuildLayout();
        LoadData();
    }

    private static FlowLayoutPanel CreateVerticalList() => new()
    {
        FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, BackColor = Color.White,
        Padding = new Padding(0), Margin = new Padding(0)
    };

    private void BuildLayout()
    {
        var sidebar = new Panel { Dock = DockStyle.Left, Width = 315, BackColor = Color.White, Padding = new Padding(12) };
        var sidebarHeader = new Panel { Dock = DockStyle.Top, Height = 92 };
        var brand = new Label { Text = "CHAT P2P", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.FromArgb(25, 90, 160), AutoSize = true, Location = new Point(4, 4) };
        var account = new Label { Text = $"Đăng nhập: {_currentUser}", Font = new Font("Segoe UI", 8.5F), ForeColor = Color.DimGray, AutoSize = true, Location = new Point(16, 45) };
        _connectionState.Text = "● Online · Connected";
        _connectionState.ForeColor = Color.FromArgb(35, 150, 80);
        _connectionState.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        _connectionState.AutoSize = true; _connectionState.Location = new Point(4, 56);
        account.Location = new Point(4, 35);
        sidebarHeader.Controls.AddRange([brand, account, _connectionState]);
        _search.Dock = DockStyle.Top; _search.Height = 34; _search.Font = new Font("Segoe UI", 9.5F); _search.PlaceholderText = "Tìm peer hoặc group...";
        _search.TextChanged += (_, _) => RenderSidebar();

        var peerTitle = SectionTitle("PEOPLE");
        peerTitle.Dock = DockStyle.Top;
        _peerList.Dock = DockStyle.Top; _peerList.Height = 268;
        var groupTitle = SectionTitle("GROUPS"); groupTitle.Dock = DockStyle.Top;
        _groupList.Dock = DockStyle.Fill;
        sidebar.Controls.Add(_groupList); sidebar.Controls.Add(groupTitle); sidebar.Controls.Add(_peerList); sidebar.Controls.Add(peerTitle); sidebar.Controls.Add(_search); sidebar.Controls.Add(sidebarHeader);

        _emptyState.Dock = DockStyle.Fill; _emptyState.BackColor = Color.FromArgb(249, 250, 252);
        var emptyIcon = new Label { Text = "💬", Font = new Font("Segoe UI Emoji", 34), AutoSize = true };
        var emptyText = new Label { Text = "Chọn một người hoặc nhóm để bắt đầu trò chuyện", Font = new Font("Segoe UI", 13), AutoSize = true, ForeColor = Color.DimGray };
        _emptyState.Controls.AddRange([emptyIcon, emptyText]);
        _emptyState.Resize += (_, _) => { emptyIcon.Location = new Point((_emptyState.Width - emptyIcon.Width) / 2, _emptyState.Height / 2 - 55); emptyText.Location = new Point((_emptyState.Width - emptyText.Width) / 2, _emptyState.Height / 2 + 8); };

        BuildChatPanel();
        var content = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(249, 250, 252) };
        content.Controls.Add(_emptyState); content.Controls.Add(_chat);
        _chat.Visible = false;
        Controls.Add(content); Controls.Add(sidebar);
    }

    private void BuildChatPanel()
    {
        _chat.Dock = DockStyle.Fill; _chat.BackColor = Color.FromArgb(249, 250, 252);
        var header = new Panel { Dock = DockStyle.Top, Height = 74, BackColor = Color.White, Padding = new Padding(22, 12, 14, 8) };
        var avatar = new Label { Text = "●", ForeColor = Color.FromArgb(35, 150, 80), Font = new Font("Segoe UI", 18), AutoSize = true, Location = new Point(22, 20) };
        _chatTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold); _chatTitle.AutoSize = true; _chatTitle.Location = new Point(52, 15);
        _chatState.Font = new Font("Segoe UI", 8.5F); _chatState.ForeColor = Color.DimGray; _chatState.AutoSize = true; _chatState.Location = new Point(53, 40);
        var reconnect = new Button { Text = "↻ Kết nối lại", AutoSize = true, Dock = DockStyle.Right, FlatStyle = FlatStyle.Flat, ForeColor = Color.FromArgb(25, 90, 160), Font = new Font("Segoe UI", 9) };
        reconnect.FlatAppearance.BorderSize = 0; reconnect.Click += (_, _) => Reconnect();
        header.Controls.AddRange([avatar, _chatTitle, _chatState, reconnect]);

        _history.Dock = DockStyle.Fill; _history.Padding = new Padding(18, 14, 18, 10); _history.BackColor = Color.FromArgb(249, 250, 252);
        _history.Resize += (_, _) => ResizeHistoryItems();

        var composer = BuildComposer();
        _chat.Controls.Add(_history); _chat.Controls.Add(composer); _chat.Controls.Add(header);
    }

    private Control BuildComposer()
    {
        var composer = new Panel { Dock = DockStyle.Bottom, Height = 122, BackColor = Color.White, Padding = new Padding(14, 8, 14, 10) };
        _replyBar.Dock = DockStyle.Top; _replyBar.Height = 30; _replyBar.Visible = false; _replyBar.BackColor = Color.FromArgb(235, 241, 248);
        _replyLabel.AutoSize = false; _replyLabel.Dock = DockStyle.Fill; _replyLabel.Padding = new Padding(9, 6, 0, 0); _replyLabel.Font = new Font("Segoe UI", 8.5F); _replyLabel.ForeColor = Color.FromArgb(45, 90, 130);
        var cancelReply = new Button { Text = "×", Dock = DockStyle.Right, Width = 34, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 12) }; cancelReply.FlatAppearance.BorderSize = 0; cancelReply.Click += (_, _) => ClearReply();
        _replyBar.Controls.Add(_replyLabel); _replyBar.Controls.Add(cancelReply);
        _composerNotice.Dock = DockStyle.Bottom; _composerNotice.Height = 19; _composerNotice.Font = new Font("Segoe UI", 8F); _composerNotice.ForeColor = Color.FromArgb(190, 60, 60);
        _messageInput.Dock = DockStyle.Fill; _messageInput.Font = new Font("Segoe UI Emoji", 10); _messageInput.PlaceholderText = "Nhập tin nhắn..."; _messageInput.Multiline = true; _messageInput.AcceptsReturn = true;
        _messageInput.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter && !e.Shift) { e.SuppressKeyPress = true; SendMessage(); } };
        var send = new Button { Text = "Gửi", Dock = DockStyle.Right, Width = 72, BackColor = Color.FromArgb(0, 122, 255), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold) }; send.Click += (_, _) => SendMessage();
        var emoji = new Button { Text = "😊", Dock = DockStyle.Left, Width = 43, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Emoji", 13) }; emoji.FlatAppearance.BorderSize = 0; emoji.Click += (_, _) => ShowEmojiPicker(emoji);
        var row = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 5, 0, 0) }; row.Controls.Add(_messageInput); row.Controls.Add(send); row.Controls.Add(emoji);
        composer.Controls.Add(row); composer.Controls.Add(_composerNotice); composer.Controls.Add(_replyBar);
        return composer;
    }

    private static Label SectionTitle(string text) => new() { Text = text, Height = 30, Padding = new Padding(4, 11, 0, 0), Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.Gray };

    private void LoadData()
    {
        _allPeers.Clear(); _allPeers.AddRange(_peers.GetAll());
        if (_allPeers.Count == 0)
        {
            _allPeers.AddRange([
                new Peer { Id = "minh", Name = "Minh", IpAddress = "127.0.0.1", Port = 6001, IsOnline = true },
                new Peer { Id = "lan", Name = "Lan", IpAddress = "192.168.1.12", Port = 6002, IsOnline = true },
                new Peer { Id = "tuan", Name = "Tuấn", IpAddress = "192.168.1.23", Port = 6003, IsOnline = false, LastSeen = DateTime.Now.AddMinutes(-35) }
            ]);
            foreach (var peer in _allPeers) _peers.AddOrUpdate(peer);
        }
        _allGroups.Clear(); _allGroups.AddRange(_groups.GetAll());
        if (_allGroups.Count == 0)
        {
            _allGroups.AddRange([
                new GroupChat { Id = "network-class", Name = "Nhóm Lập Trình Mạng", MemberPeerIds = _allPeers.Select(p => p.Id).ToList() },
                new GroupChat { Id = "team-02", Name = "Team 02", MemberPeerIds = ["minh", "lan"] },
                new GroupChat { Id = "friends", Name = "Bạn bè", MemberPeerIds = ["minh", "tuan"] }
            ]);
            foreach (var group in _allGroups) _groups.Add(group);
        }
        SeedExampleHistory();
        RenderSidebar();
    }

    private void SeedExampleHistory()
    {
        var minh = _allPeers.First(p => p.Id == "minh");
        if (!_messages.GetDirectHistory(_currentUser, minh.Id, 0).Any())
        {
            var first = new ChatMessage { SenderId = minh.Id, ReceiverId = _currentUser, Content = "Chào bạn! Nhóm mình họp lúc 9 giờ nhé.", Timestamp = DateTime.Now.AddMinutes(-12) };
            _messages.Add(first);
            _messages.Add(new ChatMessage { SenderId = _currentUser, ReceiverId = minh.Id, Content = "Mình đã nhận được, cảm ơn Minh!", ReplyToId = first.Id, Timestamp = DateTime.Now.AddMinutes(-9), DeliveryStatus = MessageDeliveryStatus.Delivered });
        }
        var group = _allGroups.First(g => g.Id == "network-class");
        if (!_messages.GetGroupHistory(group.Id, 0).Any())
            _messages.Add(new ChatMessage { SenderId = minh.Id, GroupId = group.Id, Content = "Hello mọi người, nhớ chuẩn bị phần socket nhé!", Timestamp = DateTime.Now.AddMinutes(-5) });
    }

    private void RenderSidebar()
    {
        var term = _search.Text.Trim();
        _peerList.SuspendLayout(); _peerList.Controls.Clear();
        foreach (var peer in _allPeers.Where(p => string.IsNullOrEmpty(term) || p.Name.Contains(term, StringComparison.OrdinalIgnoreCase)))
        {
            var item = new PeerListItemControl { Width = Math.Max(280, _peerList.ClientSize.Width - 4), IsSelected = _selectedPeer?.Id == peer.Id };
            item.SetData(peer, peer.IsOnline ? "Sẵn sàng trò chuyện" : "Đang offline", 0);
            item.PeerSelected += (_, selected) => SelectPeer(selected);
            _peerList.Controls.Add(item);
        }
        _peerList.ResumeLayout();
        _groupList.SuspendLayout(); _groupList.Controls.Clear();
        foreach (var group in _allGroups.Where(g => string.IsNullOrEmpty(term) || g.Name.Contains(term, StringComparison.OrdinalIgnoreCase)))
        {
            var item = new Button { Text = $"👥  {group.Name}\n     {group.MemberPeerIds.Count} thành viên", TextAlign = ContentAlignment.MiddleLeft, Width = Math.Max(280, _groupList.ClientSize.Width - 4), Height = 56, FlatStyle = FlatStyle.Flat, BackColor = _selectedGroup?.Id == group.Id ? Color.FromArgb(230, 240, 255) : Color.White, Font = new Font("Segoe UI", 9F), Cursor = Cursors.Hand };
            item.FlatAppearance.BorderColor = Color.FromArgb(238, 238, 238); item.Click += (_, _) => SelectGroup(group);
            _groupList.Controls.Add(item);
        }
        _groupList.ResumeLayout();
    }

    private void SelectPeer(Peer peer)
    {
        _selectedPeer = peer; _selectedGroup = null; ClearReply(); _chatTitle.Text = peer.Name;
        _chatState.Text = peer.IsOnline ? $"● Online · Connected · {peer.IpAddress}:{peer.Port}" : "○ Offline · Connection lost";
        _chatState.ForeColor = peer.IsOnline ? Color.FromArgb(35, 150, 80) : Color.FromArgb(190, 90, 45);
        _emptyState.Visible = false; _chat.Visible = true; _composerNotice.Text = peer.IsOnline ? string.Empty : "Peer đang offline. Tin nhắn mới sẽ báo Failed to send.";
        RenderSidebar(); RenderConversation();
    }

    private void SelectGroup(GroupChat group)
    {
        _selectedGroup = group; _selectedPeer = null; ClearReply(); _chatTitle.Text = group.Name;
        _chatState.Text = $"● Group connected · {group.MemberPeerIds.Count} thành viên"; _chatState.ForeColor = Color.FromArgb(35, 150, 80);
        _emptyState.Visible = false; _chat.Visible = true; _composerNotice.Text = string.Empty;
        RenderSidebar(); RenderConversation();
    }

    private void RenderConversation()
    {
        _history.SuspendLayout(); _history.Controls.Clear();
        var items = _selectedPeer is not null ? _messages.GetDirectHistory(_currentUser, _selectedPeer.Id, 0, 100).OrderBy(m => m.Timestamp).ToList() : _selectedGroup is not null ? _messages.GetGroupHistory(_selectedGroup.Id, 0, 100).OrderBy(m => m.Timestamp).ToList() : [];
        foreach (var message in items)
        {
            var isMine = message.SenderId == _currentUser;
            var reply = string.IsNullOrWhiteSpace(message.ReplyToId) ? null : items.FirstOrDefault(m => m.Id == message.ReplyToId);
            var sender = _selectedGroup is not null && !isMine ? PeerName(message.SenderId) : null;
            var bubble = new MessageBubbleControl { Width = Math.Max(300, _history.ClientSize.Width - 36) };
            bubble.SetData(message, isMine, sender, reply, reply is null ? null : PeerName(reply.SenderId));
            bubble.ReplyRequested += (_, selected) => StartReply(selected);
            bubble.ForwardRequested += (_, selected) => Forward(selected);
            _history.Controls.Add(bubble);
        }
        _history.ResumeLayout(); ResizeHistoryItems();
        if (_history.Controls.Count > 0) _history.ScrollControlIntoView(_history.Controls[^1]);
    }

    private void ResizeHistoryItems()
    {
        var width = Math.Max(280, _history.ClientSize.Width - _history.Padding.Horizontal - 25);
        foreach (Control item in _history.Controls) { item.Width = width; if (item is MessageBubbleControl bubble) bubble.UpdateBubbleLayout(); }
    }

    private void SendMessage()
    {
        var text = _messageInput.Text.Trim();
        if (string.IsNullOrEmpty(text) || (_selectedPeer is null && _selectedGroup is null)) return;
        var status = _selectedPeer is { IsOnline: false } ? MessageDeliveryStatus.Failed : MessageDeliveryStatus.Sending;
        var message = new ChatMessage { SenderId = _currentUser, ReceiverId = _selectedPeer?.Id, GroupId = _selectedGroup?.Id, Content = text, ReplyToId = _replyTo?.Id, Timestamp = DateTime.Now, DeliveryStatus = status };
        _messages.Add(message); _messageInput.Clear(); ClearReply(); RenderConversation();
        if (status == MessageDeliveryStatus.Failed)
        {
            _composerNotice.Text = "Không thể kết nối tới peer. Thử lại khi peer online.";
            return;
        }
        var timer = new System.Windows.Forms.Timer { Interval = 450 };
        timer.Tick += (_, _) =>
        {
            timer.Stop(); timer.Dispose();
            message.DeliveryStatus = MessageDeliveryStatus.Sent;
            _messages.UpdateDeliveryStatus(message.Id, message.DeliveryStatus);
            RenderConversation();
        };
        timer.Start();
    }

    private void StartReply(ChatMessage message)
    {
        _replyTo = message; _replyLabel.Text = $"Reply to {PeerName(message.SenderId)}: {ShortText(message.Content)}"; _replyBar.Visible = true; _messageInput.Focus();
    }
    private void ClearReply() { _replyTo = null; _replyBar.Visible = false; }
    private void Forward(ChatMessage source)
    {
        using var form = new ForwardMessageForm(_allPeers.Where(p => p.Id != _selectedPeer?.Id), _allGroups);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        foreach (var destination in form.SelectedDestinations)
        {
            var message = new ChatMessage { SenderId = _currentUser, ReceiverId = destination.IsGroup ? null : destination.Id, GroupId = destination.IsGroup ? destination.Id : null, Content = source.Content, ForwardedFromId = source.Id, DeliveryStatus = MessageDeliveryStatus.Sent };
            _messages.Add(message);
        }
        _composerNotice.Text = $"Đã chuyển tiếp đến {form.SelectedDestinations.Count} cuộc hội thoại.";
        RenderConversation();
    }

    private void ShowEmojiPicker(Control anchor)
    {
        if (_emojiPicker is null || _emojiPicker.IsDisposed)
        {
            _emojiPicker = new EmojiPickerForm();
            _emojiPicker.EmojiSelected += emoji => { var start = _messageInput.SelectionStart; _messageInput.Text = _messageInput.Text.Insert(start, emoji); _messageInput.SelectionStart = start + emoji.Length; _messageInput.Focus(); };
        }
        _emojiPicker.ShowAtLocation(anchor.PointToScreen(new Point(0, anchor.Height + 4)));
    }

    private void Reconnect()
    {
        if (_selectedPeer is { IsOnline: false }) { _composerNotice.Text = "Peer đang offline. Không thể kết nối tới peer."; MessageBox.Show(this, "Peer đang offline. Vui lòng thử lại sau.", "Connection lost", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        _connectionState.Text = "● Online · Connected"; _connectionState.ForeColor = Color.FromArgb(35, 150, 80); _composerNotice.Text = "Đã kết nối mạng P2P.";
    }

    private string PeerName(string id) => id == _currentUser ? "Bạn" : _allPeers.FirstOrDefault(p => p.Id == id)?.Name ?? id;
    private static string ShortText(string content) => content.Length <= 50 ? content : content[..50] + "…";
}
