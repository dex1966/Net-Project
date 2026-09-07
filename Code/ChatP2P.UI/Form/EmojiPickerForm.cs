using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ChatP2P.UI.Form
{
    public class EmojiPickerForm : System.Windows.Forms.Form
    {
        public event Action<string>? EmojiSelected;

        private TextBox _txtSearch = null!;
        private FlowLayoutPanel _flowPanel = null!;
        private Panel _categoryBar = null!;
        private Label _lblStatus = null!;

        private static readonly Dictionary<string, string[]> Categories = new()
        {
            ["😃 Biểu cảm"] = new[]
            {
                "😀", "😁", "😂", "😃", "😄", "😅", "😆", "😇", "😈", "😉", "😊", "😋", "😌", "😍", "😎", "😏",
                "😐", "😑", "😒", "😓", "😔", "😕", "😖", "😗", "😘", "😙", "😚", "😛", "😜", "😝", "😞", "😟",
                "😠", "😡", "😢", "😣", "😤", "😥", "😦", "😧", "😨", "😩", "😪", "😫", "😬", "😭", "😮", "😯",
                "😰", "😱", "😲", "😳", "😴", "😵", "😶", "😷", "😸", "😹", "😺", "😻", "😼", "😽", "😾", "😿",
                "🙀", "🙁", "🙂", "🙃", "🙄", "🥺", "🥳", "🤩", "🤪", "🤫", "🤬", "🤯", "🥶", "🥵", "🥸"
            },
            ["👍 Cử chỉ"] = new[]
            {
                "👍", "👎", "👌", "👊", "✊", "✌️", "🖐️", "✋", "👐", "👏", "💪", "🤝", "🙏", "🤞", "🤟", "🤘",
                "🤙", "👈", "👉", "👆", "👇", "🖕", "✍️", "🤳", "💅", "🦵", "🦶", "👂", "👃", "🧠", "👀"
            },
            ["❤️ Cảm xúc"] = new[]
            {
                "❤️", "🧡", "💛", "💚", "💙", "💜", "🖤", "🤍", "🤎", "💔", "❣️", "💕", "💞", "💓", "💗", "💖",
                "💘", "💝", "💟", "☮️", "✝️", "☪️", "🕉️", "☸️", "✡️", "🔯", "🕎", "☯️", "☦️", "🛐", "⛎"
            },
            ["🐱 Động vật"] = new[]
            {
                "🐶", "🐱", "🐭", "🐹", "🐰", "🦊", "🐻", "🐼", "🐨", "🐯", "🦁", "🐮", "🐷", "🐸", "🐵", "🐔",
                "🐧", "🐦", "🐤", "🦆", "🦅", "🦉", "🦇", "🐺", "🐗", "🐴", "🦄", "🐝", "🐛", "🦋", "🐌", "🐞"
            },
            ["🍕 Đồ ăn"] = new[]
            {
                "🍏", "🍎", "🍐", "🍊", "🍋", "🍌", "🍉", "🍇", "🍓", "🫐", "🍈", "🍒", "🍑", "🥭", "🍍", "🥥",
                "🥝", "🍅", "🍆", "🥑", "🥦", "🥒", "🌶️", "🌽", "🥕", "🧄", "🧅", "🥔", "🍠", "🥐", "🥯", "🍞",
                "🥖", "🥨", "🧀", "🥚", "🍳", "🧈", "🥞", "🧇", "🥓", "🥩", "🍗", "🍖", "🌭", "🍔", "🍟", "🍕"
            }
        };

        private string _activeCategory = "😃 Biểu cảm";

        public EmojiPickerForm()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            Text = "Chọn Emoji";
            Size = new Size(360, 420);
            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            TopMost = true;
            BackColor = Color.FromArgb(248, 249, 250);

            // 1. Khung Tìm kiếm
            Panel searchContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                Padding = new Padding(10, 8, 10, 8),
                BackColor = Color.White
            };

            _txtSearch = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "🔍 Tìm kiếm emoji...",
                BorderStyle = BorderStyle.FixedSingle
            };
            _txtSearch.TextChanged += (s, e) => FilterEmojis();

            searchContainer.Controls.Add(_txtSearch);

            // 2. Thanh Danh mục (Tabs)
            _categoryBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = Color.FromArgb(238, 242, 246)
            };

            int btnWidth = 70;
            int x = 4;
            foreach (var category in Categories.Keys)
            {
                Button catBtn = new Button
                {
                    Text = category,
                    Location = new Point(x, 4),
                    Size = new Size(btnWidth, 28),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 8.25F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Tag = category
                };
                catBtn.FlatAppearance.BorderSize = 0;
                catBtn.Click += CategoryBtn_Click;
                _categoryBar.Controls.Add(catBtn);
                x += btnWidth + 2;
            }

            // 3. Panel chứa emoji (FlowLayoutPanel)
            _flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(8),
                BackColor = Color.White,
                WrapContents = true
            };

            // 4. Thanh trạng thái ở dưới
            _lblStatus = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 24,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = Color.DimGray,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                Text = "Nhấp vào emoji để chèn"
            };

            Controls.Add(_flowPanel);
            Controls.Add(_categoryBar);
            Controls.Add(searchContainer);
            Controls.Add(_lblStatus);

            UpdateCategoryTabs();
            LoadCategory(_activeCategory);
        }

        private void CategoryBtn_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is string category)
            {
                _activeCategory = category;
                _txtSearch.Clear();
                UpdateCategoryTabs();
                LoadCategory(_activeCategory);
            }
        }

        private void UpdateCategoryTabs()
        {
            foreach (Control ctrl in _categoryBar.Controls)
            {
                if (ctrl is Button btn && btn.Tag is string cat)
                {
                    if (cat == _activeCategory)
                    {
                        btn.BackColor = Color.FromArgb(0, 122, 255);
                        btn.ForeColor = Color.White;
                    }
                    else
                    {
                        btn.BackColor = Color.Transparent;
                        btn.ForeColor = Color.FromArgb(70, 70, 70);
                    }
                }
            }
        }

        private void LoadCategory(string category)
        {
            if (!Categories.ContainsKey(category)) return;
            DisplayEmojis(Categories[category]);
        }

        private void FilterEmojis()
        {
            string keyword = _txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadCategory(_activeCategory);
                return;
            }

            // Gom tất cả emoji khi đang search
            var allEmojis = Categories.Values.SelectMany(x => x).Distinct();
            DisplayEmojis(allEmojis);
        }

        private void DisplayEmojis(IEnumerable<string> emojis)
        {
            _flowPanel.SuspendLayout();
            _flowPanel.Controls.Clear();

            foreach (string emoji in emojis)
            {
                Button btn = new Button
                {
                    Text = emoji,
                    Size = new Size(38, 38),
                    Margin = new Padding(2),
                    Font = new Font("Segoe UI Emoji", 14F, FontStyle.Regular),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 235, 250);
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(190, 215, 245);

                btn.Click += (s, e) =>
                {
                    EmojiSelected?.Invoke(emoji);
                    _lblStatus.Text = $"Đã chọn: {emoji}";
                };

                _flowPanel.Controls.Add(btn);
            }

            _flowPanel.ResumeLayout(true);
        }

        public void ShowAtLocation(Point screenPoint)
        {
            Location = screenPoint;
            Show();
        }
    }
}
