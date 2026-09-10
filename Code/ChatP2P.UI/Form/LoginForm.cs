using System.Drawing;
using System.Windows.Forms;
using ChatP2P.Data;
using ChatP2P.Data.Repositories;

namespace ChatP2P.UI.Form;

public class LoginForm : System.Windows.Forms.Form
{
    private readonly AccountRepository _accounts = new(new AppDbContext());
    private readonly TextBox _username = new();
    private readonly TextBox _password = new();
    private readonly Label _feedback = new();

    public string Username { get; private set; } = string.Empty;

    public LoginForm()
    {
        Text = "Chat P2P | Đăng nhập";
        ClientSize = new Size(420, 430);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(246, 248, 252);

        var card = new Panel { Size = new Size(348, 330), Location = new Point(36, 48), BackColor = Color.White, Padding = new Padding(30) };
        var title = new Label { Text = "Chat P2P", Font = new Font("Segoe UI", 22, FontStyle.Bold), ForeColor = Color.FromArgb(31, 78, 121), AutoSize = true, Location = new Point(30, 28) };
        var subtitle = new Label { Text = "Đăng nhập để bắt đầu trò chuyện", Font = new Font("Segoe UI", 10), ForeColor = Color.DimGray, AutoSize = true, Location = new Point(31, 68) };
        var userLabel = new Label { Text = "Tên đăng nhập", AutoSize = true, Location = new Point(30, 112), Font = new Font("Segoe UI", 9, FontStyle.Bold) };
        var passwordLabel = new Label { Text = "Mật khẩu", AutoSize = true, Location = new Point(30, 184), Font = new Font("Segoe UI", 9, FontStyle.Bold) };

        _username.SetBounds(30, 136, 288, 34);
        _username.Font = new Font("Segoe UI", 10);
        _username.PlaceholderText = "Nhập username";
        _password.SetBounds(30, 208, 288, 34);
        _password.Font = new Font("Segoe UI", 10);
        _password.PasswordChar = '●';
        _password.PlaceholderText = "Nhập mật khẩu";
        _password.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) Login(); };

        var login = CreateButton("Đăng nhập", new Point(30, 258), Color.FromArgb(0, 122, 255));
        login.Click += (_, _) => Login();
        var register = CreateButton("Đăng ký", new Point(181, 258), Color.FromArgb(90, 98, 104));
        register.Click += (_, _) => Register();
        _feedback.SetBounds(30, 302, 288, 22);
        _feedback.Font = new Font("Segoe UI", 8.5F);
        _feedback.TextAlign = ContentAlignment.MiddleCenter;

        card.Controls.AddRange([title, subtitle, userLabel, _username, passwordLabel, _password, login, register, _feedback]);
        Controls.Add(card);
        Shown += (_, _) => _username.Focus();
    }

    private static Button CreateButton(string text, Point location, Color color) => new()
    {
        Text = text, Location = location, Size = new Size(137, 36), BackColor = color, ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand
    };

    private bool ValidateInput()
    {
        var user = _username.Text.Trim();
        if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(_password.Text))
        {
            ShowFeedback("Vui lòng nhập username và mật khẩu.", true);
            return false;
        }
        if (user.Length < 3 || _password.Text.Length < 3)
        {
            ShowFeedback("Username và mật khẩu cần có ít nhất 3 ký tự.", true);
            return false;
        }
        return true;
    }

    private void Login()
    {
        if (!ValidateInput()) return;
        try
        {
            if (!_accounts.Login(_username.Text.Trim(), _password.Text))
            {
                ShowFeedback("Username hoặc mật khẩu không đúng.", true);
                return;
            }
            Username = _username.Text.Trim();
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception)
        {
            ShowFeedback("Không thể truy cập dữ liệu đăng nhập. Thử lại.", true);
        }
    }

    private void Register()
    {
        if (!ValidateInput()) return;
        try
        {
            if (!_accounts.Register(_username.Text.Trim(), _password.Text))
            {
                ShowFeedback("Username này đã được sử dụng.", true);
                return;
            }
            ShowFeedback("Đăng ký thành công. Bấm Đăng nhập để tiếp tục.", false);
            _password.Clear();
            _password.Focus();
        }
        catch (Exception)
        {
            ShowFeedback("Không thể tạo tài khoản. Thử lại.", true);
        }
    }

    private void ShowFeedback(string message, bool isError)
    {
        _feedback.Text = message;
        _feedback.ForeColor = isError ? Color.FromArgb(200, 55, 55) : Color.FromArgb(35, 135, 75);
    }
}
