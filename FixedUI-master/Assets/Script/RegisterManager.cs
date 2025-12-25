using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUI : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_InputField confirmField;

    [Header("UI Message Text (tuỳ chọn)")]
    public Text messageText;

    [Header("Network Manager Reference")]
    public TcpClientManager tcpManager;

    void Start()
    {
        // Tìm TcpClientManager nếu chưa gán
        if (tcpManager == null)
        {
            tcpManager = FindObjectOfType<TcpClientManager>();
        }
        
        // Đăng ký lắng nghe kết quả đăng ký
        if (tcpManager != null)
        {
            tcpManager.OnRegisterResult += HandleRegisterResult;
        }
    }

    void OnDestroy()
    {
        // Hủy đăng ký khi object bị destroy
        if (tcpManager != null)
        {
            tcpManager.OnRegisterResult -= HandleRegisterResult;
        }
    }

    public void OnRegisterButtonClick()
    {
        string email = emailField.text.Trim();
        string password = passwordField.text;
        string confirm = confirmField.text;

        // Kiểm tra rỗng
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirm))
        {
            ShowMessage("Vui lòng nhập đầy đủ thông tin!");
            return;
        }

        // Kiểm tra định dạng email
        if (!IsValidEmail(email))
        {
            ShowMessage("Email không hợp lệ! Vui lòng nhập đúng định dạng email (ví dụ: user@gmail.com)");
            return;
        }

        // Kiểm tra độ dài password
        if (password.Length < 6)
        {
            ShowMessage("Mật khẩu phải có ít nhất 6 ký tự!");
            return;
        }

        // Kiểm tra xác nhận mật khẩu
        if (password != confirm)
        {
            ShowMessage("Mật khẩu xác nhận không khớp!");
            return;
        }

        // Nếu chưa gán tcpManager trong Unity, thì tự tìm
        if (tcpManager == null)
        {
            tcpManager = FindFirstObjectByType<TcpClientManager>();
        }

        if (tcpManager != null)
        {
            ShowMessage("⏳ Đang gửi dữ liệu...");
            tcpManager.RegisterAccount(email, password);
        }
        else
        {
            ShowMessage("❌ Không tìm thấy TcpClientManager trong scene!");
        }
    }

    // Kiểm tra email hợp lệ
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    // Xử lý kết quả đăng ký
    private void HandleRegisterResult(string message, bool success)
    {
        if (success)
        {
            ShowMessage("✅ " + message);
        }
        else
        {
            ShowMessage("❌ " + message);
        }
    }

    private void ShowMessage(string msg)
    {
        Debug.Log(msg);
        if (messageText != null)
            messageText.text = msg;
    }
}
