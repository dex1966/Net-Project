using System;
using System.Drawing;
using System.Windows.Forms;
using ChatP2P.Core.Models;
using ChatP2P.UI.Controls;

namespace ChatP2P.UI;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        LoadTestBubbles();
    }

    private void LoadTestBubbles()
    {
        Text = "Test MessageBubbleControl";
        Width = 480;
        Height = 550;
        StartPosition = FormStartPosition.CenterScreen;

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
            Content = "Chào bạn! Bạn khoẻ không?",
            Timestamp = DateTime.Now.AddMinutes(-10)
        }, isMyMessage: false);

        // 2. Tin nhắn gửi (ngắn)
        var msg2 = new MessageBubbleControl();
        msg2.SetData(new ChatMessage
        {
            Content = "Mình khoẻ, cảm ơn bạn!",
            Timestamp = DateTime.Now.AddMinutes(-8)
        }, isMyMessage: true);

        // 3. Tin nhắn nhận (dài - test tự động xuống dòng)
        var msg3 = new MessageBubbleControl();
        msg3.SetData(new ChatMessage
        {
            Content = "Đây là một tin nhắn thử nghiệm có nội dung khá dài để kiểm tra khả năng xuống dòng tự động của MessageBubbleControl.",
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
            Content = "Thông báo: Ngày mai họp nhóm Lập trình mạng lúc 9h sáng nhé!",
            ForwardedFromId = "msg-001",
            Timestamp = DateTime.Now
        }, isMyMessage: false);

        panel.Controls.Add(msg1);
        panel.Controls.Add(msg2);
        panel.Controls.Add(msg3);
        panel.Controls.Add(msg4);
        panel.Controls.Add(msg5);

        Controls.Add(panel);
        AdjustWidths();
    }
}

