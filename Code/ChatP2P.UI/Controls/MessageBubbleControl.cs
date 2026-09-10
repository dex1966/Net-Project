using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ChatP2P.Core.Models;

namespace ChatP2P.UI.Controls;

public class MessageBubbleControl : UserControl
{
    private readonly Panel _bubble = new();
    private readonly Label _sender = new();
    private readonly Label _forwarded = new();
    private readonly Panel _reply = new();
    private readonly Label _replyAuthor = new();
    private readonly Label _replyText = new();
    private readonly Label _content = new();
    private readonly Label _metadata = new();
    private bool _isMine;

    public ChatMessage? Message { get; private set; }
    public event EventHandler<ChatMessage>? MessageSelected;
    public event EventHandler<ChatMessage>? ReplyRequested;
    public event EventHandler<ChatMessage>? ForwardRequested;

    public MessageBubbleControl()
    {
        DoubleBuffered = true;
        BackColor = Color.Transparent;
        Margin = new Padding(0, 4, 0, 4);
        _bubble.Padding = new Padding(12, 8, 12, 7);
        _bubble.BackColor = Color.FromArgb(242, 242, 242);
        _sender.AutoSize = _forwarded.AutoSize = _replyAuthor.AutoSize = _replyText.AutoSize = _content.AutoSize = _metadata.AutoSize = true;
        _sender.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        _sender.ForeColor = Color.FromArgb(30, 100, 180);
        _forwarded.Text = "↪ Forwarded";
        _forwarded.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
        _forwarded.ForeColor = Color.DimGray;
        _reply.BackColor = Color.FromArgb(225, 230, 236);
        _reply.Padding = new Padding(7, 4, 7, 4);
        _replyAuthor.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        _replyText.Font = new Font("Segoe UI", 8F);
        _replyText.ForeColor = Color.DimGray;
        _reply.Controls.AddRange([_replyAuthor, _replyText]);
        _content.Font = new Font("Segoe UI", 10F);
        _content.ForeColor = Color.FromArgb(30, 30, 30);
        _metadata.Font = new Font("Segoe UI", 7.5F);
        _metadata.ForeColor = Color.Gray;
        _bubble.Controls.AddRange([_sender, _forwarded, _reply, _content, _metadata]);
        Controls.Add(_bubble);

        var menu = new ContextMenuStrip();
        menu.Items.Add("Reply", null, (_, _) => RaiseAction(ReplyRequested));
        menu.Items.Add("Forward", null, (_, _) => RaiseAction(ForwardRequested));
        ContextMenuStrip = menu;
        AttachInteraction(this);
    }

    public void SetData(ChatMessage message, bool isMyMessage, string? senderName = null, ChatMessage? replyTo = null, string? replyAuthor = null)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        _isMine = isMyMessage;
        _sender.Text = senderName ?? string.Empty;
        _sender.Visible = !_isMine && !string.IsNullOrWhiteSpace(senderName);
        _forwarded.Visible = !string.IsNullOrWhiteSpace(message.ForwardedFromId);
        _reply.Visible = replyTo is not null;
        if (replyTo is not null)
        {
            _replyAuthor.Text = replyAuthor ?? "Tin nhắn trước";
            _replyText.Text = Trim(replyTo.Content, 80);
        }
        _content.Text = message.Content ?? string.Empty;
        _metadata.Text = CreateMetadata(message);
        ApplyStyle();
        UpdateBubbleLayout();
    }

    private static string CreateMetadata(ChatMessage message) => message.DeliveryStatus switch
    {
        MessageDeliveryStatus.Sending => "Sending...",
        MessageDeliveryStatus.Failed => "Failed to send",
        MessageDeliveryStatus.Delivered => $"{message.Timestamp:HH:mm} ✓✓",
        _ => $"{message.Timestamp:HH:mm} ✓"
    };

    private void ApplyStyle()
    {
        _bubble.BackColor = _isMine ? Color.FromArgb(220, 248, 198) : Color.FromArgb(242, 242, 242);
        _metadata.ForeColor = Message?.DeliveryStatus == MessageDeliveryStatus.Failed ? Color.FromArgb(190, 55, 55) : (_isMine ? Color.FromArgb(80, 110, 80) : Color.Gray);
        _forwarded.ForeColor = _isMine ? Color.FromArgb(80, 110, 80) : Color.Gray;
    }

    public void UpdateBubbleLayout()
    {
        if (Message is null) return;
        var parentWidth = Parent?.ClientSize.Width ?? Width;
        var maxWidth = Math.Max(180, Math.Min(390, (int)(parentWidth * .68)));
        _content.MaximumSize = new Size(maxWidth, 0);
        _replyText.MaximumSize = new Size(maxWidth - 18, 0);
        var y = 8;
        foreach (var control in new Control[] { _sender, _forwarded })
        {
            if (!control.Visible) continue;
            control.Location = new Point(12, y); y = control.Bottom + 3;
        }
        if (_reply.Visible)
        {
            _replyAuthor.Location = new Point(7, 4);
            _replyText.Location = new Point(7, _replyAuthor.Bottom + 1);
            _reply.Size = new Size(Math.Max(130, Math.Max(_replyAuthor.Width, _replyText.Width) + 14), _replyText.Bottom + 4);
            _reply.Location = new Point(12, y); y = _reply.Bottom + 6;
        }
        _content.Location = new Point(12, y); y = _content.Bottom + 4;
        _metadata.Location = new Point(12, y);
        var itemWidth = new[] { _sender.Visible ? _sender.Width : 0, _forwarded.Visible ? _forwarded.Width : 0, _reply.Visible ? _reply.Width : 0, _content.Width, _metadata.Width }.Max();
        _bubble.Size = new Size(itemWidth + 24, _metadata.Bottom + 7);
        Width = Math.Max(_bubble.Width + 12, parentWidth - 24);
        Height = _bubble.Height + 4;
        _bubble.Location = new Point(_isMine ? Math.Max(0, Width - _bubble.Width - 6) : 6, 0);
        using var path = RoundedPath(new Rectangle(Point.Empty, _bubble.Size), 14);
        _bubble.Region = new Region(path);
    }

    protected override void OnResize(EventArgs e) { base.OnResize(e); UpdateBubbleLayout(); }
    protected override void OnLayout(LayoutEventArgs e) { base.OnLayout(e); UpdateBubbleLayout(); }

    private void AttachInteraction(Control control)
    {
        control.Click += (_, _) => { if (Message is not null) MessageSelected?.Invoke(this, Message); };
        control.MouseUp += (_, e) => { if (e.Button == MouseButtons.Right) ContextMenuStrip?.Show(control, e.Location); };
        foreach (Control child in control.Controls) AttachInteraction(child);
    }
    private void RaiseAction(EventHandler<ChatMessage>? action) { if (Message is not null) action?.Invoke(this, Message); }
    private static string Trim(string text, int limit) => text.Length <= limit ? text : text[..limit] + "…";
    private static GraphicsPath RoundedPath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath(); var d = radius * 2;
        path.AddArc(rect.Left, rect.Top, d, d, 180, 90); path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90); path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90); path.CloseFigure(); return path;
    }
}
