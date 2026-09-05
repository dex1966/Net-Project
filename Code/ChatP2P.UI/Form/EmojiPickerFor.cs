using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ChatP2P.UI.Form
{
    public class EmojiPickerFor : System.Windows.Forms.Form
    {
        private readonly TextBox txtSearch;
        private readonly FlowLayoutPanel emojiPanel;

        // Emoji được người dùng chọn
        public string? SelectedEmoji { get; private set; }

        private readonly string[] emojis =
        {
            // Smileys & Emotion
            "😀", "😃", "😄", "😁", "😆", "😅", "😂", "🤣",
            "😊", "😇", "🙂", "🙃", "😉", "😌", "😍", "🥰",
            "😘", "😗", "😙", "😚", "😋", "😛", "😝", "😜",
            "🤪", "🤨", "🧐", "🤓", "😎", "🤩", "🥳", "😏",
            "😒", "😞", "😔", "😟", "😕", "🙁", "☹️", "😣",
            "😖", "😫", "😩", "🥺", "😢", "😭", "😤", "😠",
            "😡", "🤬", "🤯", "😳", "🥵", "🥶", "😱", "😨",
            "😰", "😥", "😓", "🤗", "🤔", "🫣", "🤭", "🤫",
            "🤥", "😶", "😐", "😑", "😬", "🙄", "😯", "😦",
            "😧", "😮", "😲", "🥱", "😴", "🤤", "😪", "😵",

            // Hands & People
            "👍", "👎", "👌", "✌️", "🤞", "🤟", "🤘", "🤙",
            "👈", "👉", "👆", "👇", "☝️", "✋", "🤚", "🖐️",
            "🖖", "👋", "🤏", "💪", "👏", "🙌", "👐", "🤲",
            "🙏", "❤️", "🩷", "🧡", "💛", "💚", "💙", "💜",
            "🖤", "🤍", "🤎", "💔", "💕", "💞", "💓", "💗",

            // Animals
            "🐶", "🐱", "🐭", "🐹", "🐰", "🦊", "🐻", "🐼",
            "🐨", "🐯", "🦁", "🐮", "🐷", "🐸", "🐵", "🙈",
            "🙉", "🙊", "🐔", "🐧", "🐦", "🐤", "🦄", "🐝",
            "🦋", "🐌", "🐞", "🐢", "🐍", "🦖", "🐙", "🦀",

            // Food
            "🍎", "🍐", "🍊", "🍋", "🍌", "🍉", "🍇", "🍓",
            "🫐", "🍒", "🍑", "🥭", "🍍", "🥥", "🥝", "🍅",
            "🍕", "🍔", "🍟", "🌭", "🍿", "🍩", "🍪", "🎂",
            "🍰", "🍫", "🍭", "🍬", "☕", "🧋", "🍜", "🍚",

            // Activities & Objects
            "⚽", "🏀", "🏈", "⚾", "🎾", "🏆", "🎮", "🎲",
            "🎸", "🎵", "🎶", "🎤", "🎧", "📱", "💻", "📷",
            "💡", "🎁", "🎈", "🎉", "🎊", "🔥", "✨", "⭐",
            "🌟", "💯", "💥", "💫", "💦", "💤", "💎", "🚀"
        };

        public EmojiPickerFor()
        {
            // ===== FORM =====
            Text = "Emoji";
            Width = 430;
            Height = 470;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            BackColor = Color.White;

            // ===== TITLE =====
            Label lblTitle = new Label
            {
                Text = "Emoji",
                Dock = DockStyle.Top,
                Height = 45,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.White
            };

            // ===== SEARCH =====
            txtSearch = new TextBox
            {
                Dock = DockStyle.Bottom,
                Height = 35,
                Font = new Font("Segoe UI", 11),
                PlaceholderText = "🔍  Tìm emoji..."
            };

            // ===== EMOJI PANEL =====
            emojiPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(8),
                BackColor = Color.White
            };

            Controls.Add(emojiPanel);
            Controls.Add(lblTitle);
            Controls.Add(txtSearch);

            // Khi nhập vào ô tìm kiếm
            txtSearch.TextChanged += SearchEmoji;

            // Hiển thị emoji
            LoadEmojis(emojis);
        }

        // Hiển thị danh sách emoji
        private void LoadEmojis(string[] emojiList)
        {
            emojiPanel.Controls.Clear();

            foreach (string emoji in emojiList)
            {
                Button button = new Button
                {
                    Text = emoji,
                    Width = 48,
                    Height = 48,
                    Font = new Font("Segoe UI Emoji", 20),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    Margin = new Padding(2),
                    Cursor = Cursors.Hand
                };

                button.FlatAppearance.BorderSize = 0;

                // Khi click emoji
                button.Click += (sender, e) =>
                {
                    SelectedEmoji = emoji;
                    DialogResult = DialogResult.OK;
                    Close();
                };

                emojiPanel.Controls.Add(button);
            }
        }

        // Tìm emoji
        private void SearchEmoji(object? sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadEmojis(emojis);
                return;
            }

            // Tìm theo emoji mà người dùng nhập trực tiếp
            string[] result = emojis
                .Where(x => x.Contains(keyword))
                .ToArray();

            LoadEmojis(result);
        }
    }
}