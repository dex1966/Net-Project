using System.Drawing;
using System.Windows.Forms;

namespace ChatP2P.UI.Form;

public class EmojiPickerForm : System.Windows.Forms.Form
{
    private sealed record EmojiEntry(string Glyph, string Category, string Keywords);
    private static readonly EmojiEntry[] Emoji =
    [
        new("😀", "Biểu cảm", "vui cuoi smile happy"), new("😂", "Biểu cảm", "cuoi tears laugh"), new("😍", "Biểu cảm", "yeu heart mat"), new("😢", "Biểu cảm", "buon cry"), new("😡", "Biểu cảm", "gian angry"), new("🤔", "Biểu cảm", "nghi think"), new("🥳", "Biểu cảm", "party sinh nhat"), new("😎", "Biểu cảm", "cool"),
        new("👍", "Cử chỉ", "like dong y"), new("👎", "Cử chỉ", "dislike khong"), new("👏", "Cử chỉ", "vo tay clap"), new("🙏", "Cử chỉ", "cam on xin"), new("💪", "Cử chỉ", "manh strong"), new("👋", "Cử chỉ", "chao hello wave"),
        new("❤️", "Cảm xúc", "tim yeu heart"), new("💔", "Cảm xúc", "that tinh broken"), new("✨", "Cảm xúc", "sparkles sao"), new("🎉", "Cảm xúc", "chuc mung party"), new("🔥", "Cảm xúc", "fire nong"), new("✅", "Cảm xúc", "done ok check"),
        new("🐱", "Động vật", "meo cat"), new("🐶", "Động vật", "cho dog"), new("🦊", "Động vật", "cao fox"), new("🐼", "Động vật", "panda"),
        new("🍕", "Đồ ăn", "pizza"), new("☕", "Đồ ăn", "coffee ca phe"), new("🍜", "Đồ ăn", "pho noodles"), new("🍰", "Đồ ăn", "cake banh")
    ];
    private readonly TextBox _search = new();
    private readonly FlowLayoutPanel _items = new();
    private readonly FlowLayoutPanel _categories = new();
    private readonly Label _status = new();
    private string _activeCategory = "Biểu cảm";
    public event Action<string>? EmojiSelected;

    public EmojiPickerForm()
    {
        Text = "Chọn emoji"; ClientSize = new Size(365, 410); FormBorderStyle = FormBorderStyle.FixedToolWindow; MaximizeBox = false; MinimizeBox = false; ShowInTaskbar = false; TopMost = true; BackColor = Color.White;
        var searchPanel = new Panel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(10, 9, 10, 6) };
        _search.Dock = DockStyle.Fill; _search.PlaceholderText = "🔍 Tìm emoji: vui, tim, mèo..."; _search.Font = new Font("Segoe UI", 10); _search.TextChanged += (_, _) => Render(); searchPanel.Controls.Add(_search);
        _categories.Dock = DockStyle.Top; _categories.Height = 39; _categories.Padding = new Padding(7, 4, 4, 3); _categories.WrapContents = false; _categories.BackColor = Color.FromArgb(242, 245, 249);
        foreach (var category in Emoji.Select(e => e.Category).Distinct())
        {
            var button = new Button { Text = category, Tag = category, AutoSize = true, Height = 28, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 8) }; button.FlatAppearance.BorderSize = 0;
            button.Click += (_, _) => { _activeCategory = category; _search.Clear(); Render(); }; _categories.Controls.Add(button);
        }
        _items.Dock = DockStyle.Fill; _items.AutoScroll = true; _items.Padding = new Padding(9); _items.BackColor = Color.White;
        _status.Dock = DockStyle.Bottom; _status.Height = 24; _status.Padding = new Padding(9, 0, 0, 0); _status.TextAlign = ContentAlignment.MiddleLeft; _status.Font = new Font("Segoe UI", 8); _status.ForeColor = Color.DimGray;
        Controls.Add(_items); Controls.Add(_categories); Controls.Add(searchPanel); Controls.Add(_status); Render();
    }

    private void Render()
    {
        var term = _search.Text.Trim();
        var visible = Emoji.Where(e => string.IsNullOrWhiteSpace(term) ? e.Category == _activeCategory : e.Glyph.Contains(term, StringComparison.OrdinalIgnoreCase) || e.Keywords.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
        _items.SuspendLayout(); _items.Controls.Clear();
        foreach (var entry in visible)
        {
            var button = new Button { Text = entry.Glyph, Size = new Size(42, 42), Margin = new Padding(2), Font = new Font("Segoe UI Emoji", 15), FlatStyle = FlatStyle.Flat, BackColor = Color.White, Cursor = Cursors.Hand };
            button.FlatAppearance.BorderSize = 0; button.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 235, 250);
            button.Click += (_, _) => { EmojiSelected?.Invoke(entry.Glyph); _status.Text = $"Đã chọn: {entry.Glyph}"; };
            _items.Controls.Add(button);
        }
        _items.ResumeLayout();
        _status.Text = visible.Count == 0 ? "Không tìm thấy emoji phù hợp." : $"{visible.Count} emoji";
        foreach (Control control in _categories.Controls) if (control is Button button && button.Tag is string category) { button.BackColor = category == _activeCategory && string.IsNullOrWhiteSpace(term) ? Color.FromArgb(210, 228, 250) : Color.Transparent; }
    }

    public void ShowAtLocation(Point screenPoint)
    {
        Location = screenPoint;
        if (!Visible) Show(); else Activate();
    }
}
