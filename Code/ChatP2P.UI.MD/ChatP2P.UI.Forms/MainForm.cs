using System;
using System.Windows.Forms;
using ChatP2P.Core.Network;

namespace ChatP2P.UI.Forms
{
    public partial class MainForm : Form
    {
        private readonly PeerNode _peerNode;

        public MainForm()
        {
            InitializeComponent();
            _peerNode = new PeerNode();

            // 
            _peerNode.OnMessageReceived += PeerNode_OnMessageReceived;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            // 
            int myListenPort = 6000;
            try
            {
                await _peerNode.StartListeningAsync(myListenPort);
                lblStatus.Text = $"Đang lắng nghe tại cổng: {myListenPort}";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi khi khởi tạo lắng nghe";
                MessageBox.Show($"Lỗi khi bắt đầu lắng nghe: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            if (!int.TryParse(txtPort.Text.Trim(), out int port))
            {
                MessageBox.Show("Cổng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool connected = await _peerNode.ConnectToPeerAsync(ip, port);
                if (connected)
                {
                    string entry = $"{ip}:{port}";
                    if (!lstPeers.Items.Contains(entry))
                        lstPeers.Items.Add(entry);

                    MessageBox.Show("Kết nối thành công!");
                }
                else
                {
                    MessageBox.Show("Không thể kết nối tới Peer mục tiêu!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý nút Gửi tin nhắn 1-1
        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (lstPeers.SelectedItem == null) return;

            string target = lstPeers.SelectedItem.ToString();
            string[] parts = target.Split(':');
            if (parts.Length != 2 || !int.TryParse(parts[1], out int port))
            {
                MessageBox.Show("Định dạng peer không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ip = parts[0];

            var packet = new
            {
                type = "private_message",
                content = txtMessage.Text
            };

            try
            {
                await _peerNode.SendMessageAsync(ip, port, packet);

                // 
                rtbChatHistory.AppendText($"[Tôi]: {txtMessage.Text}\n");
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 
        private void PeerNode_OnMessageReceived(string senderEndPoint, string jsonContent)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => PeerNode_OnMessageReceived(senderEndPoint, jsonContent)));
                return;
            }

            // 
            rtbChatHistory.AppendText($"[{senderEndPoint}]: {jsonContent}\n");
        }
    }
