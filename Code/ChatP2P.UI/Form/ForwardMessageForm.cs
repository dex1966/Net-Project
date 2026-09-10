using System.Drawing;
using System.Windows.Forms;
using ChatP2P.Core.Models;

namespace ChatP2P.UI.Form;

public sealed class ForwardDestination
{
    public string Id { get; init; } = string.Empty;
    public bool IsGroup { get; init; }
    public string Name { get; init; } = string.Empty;
}

public class ForwardMessageForm : System.Windows.Forms.Form
{
    private readonly List<ForwardDestination> _destinations;
    private readonly CheckedListBox _list = new();
    private readonly TextBox _search = new();
    public IReadOnlyList<ForwardDestination> SelectedDestinations { get; private set; } = Array.Empty<ForwardDestination>();

    public ForwardMessageForm(IEnumerable<Peer> peers, IEnumerable<GroupChat> groups)
    {
        _destinations = peers.Select(p => new ForwardDestination { Id = p.Id, Name = p.Name, IsGroup = false })
            .Concat(groups.Select(g => new ForwardDestination { Id = g.Id, Name = g.Name, IsGroup = true })).ToList();
        Text = "Chuyển tiếp tin nhắn";
        ClientSize = new Size(380, 440);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        BackColor = Color.White;
        var title = new Label { Text = "Chọn người nhận", AutoSize = true, Location = new Point(20, 18), Font = new Font("Segoe UI", 14, FontStyle.Bold) };
        var hint = new Label { Text = "Có thể chọn nhiều người hoặc nhóm.", AutoSize = true, Location = new Point(21, 47), Font = new Font("Segoe UI", 9), ForeColor = Color.DimGray };
        _search.SetBounds(20, 76, 340, 33); _search.PlaceholderText = "Tìm peer hoặc group..."; _search.TextChanged += (_, _) => Reload();
        _list.SetBounds(20, 120, 340, 252); _list.CheckOnClick = true; _list.Font = new Font("Segoe UI", 10); _list.BorderStyle = BorderStyle.FixedSingle;
        var cancel = new Button { Text = "Hủy", Location = new Point(190, 388), Size = new Size(80, 32), DialogResult = DialogResult.Cancel };
        var forward = new Button { Text = "Forward", Location = new Point(280, 388), Size = new Size(80, 32), BackColor = Color.FromArgb(0, 122, 255), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
        forward.Click += (_, _) => Confirm();
        Controls.AddRange([title, hint, _search, _list, cancel, forward]);
        Reload();
    }

    private void Reload()
    {
        var checkedIds = _list.CheckedItems.Cast<ForwardDestination>().Select(d => d.Id).ToHashSet();
        _list.Items.Clear();
        var term = _search.Text.Trim();
        foreach (var destination in _destinations.Where(d => string.IsNullOrEmpty(term) || d.Name.Contains(term, StringComparison.OrdinalIgnoreCase)))
            _list.Items.Add(destination, checkedIds.Contains(destination.Id));
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        _list.Format += (_, args) => { if (args.ListItem is ForwardDestination d) args.Value = d.IsGroup ? $"👥  {d.Name}" : $"👤  {d.Name}"; };
    }

    private void Confirm()
    {
        SelectedDestinations = _list.CheckedItems.Cast<ForwardDestination>().ToList();
        if (SelectedDestinations.Count == 0)
        {
            MessageBox.Show(this, "Chọn ít nhất một người nhận.", "Chuyển tiếp", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        DialogResult = DialogResult.OK;
        Close();
    }
}
