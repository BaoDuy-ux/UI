# Hướng dẫn Test Multiplayer bằng Radmin VPN

## Chuẩn bị

### 1. Cài đặt Radmin VPN
- Tải Radmin VPN: https://www.radmin-vpn.com/
- Cài đặt trên cả 2 máy (máy chủ và máy client)

### 2. Tạo mạng Radmin VPN
- Mở Radmin VPN trên máy chủ
- Click "Create Network"
- Đặt tên mạng và mật khẩu
- Ghi lại tên mạng và mật khẩu

### 3. Kết nối máy client
- Mở Radmin VPN trên máy client
- Click "Join Network"
- Nhập tên mạng và mật khẩu
- Đợi kết nối thành công

## Cấu hình Backend Server

### Trên máy chủ (Host):

1. **Tìm IP Radmin của máy chủ:**
   - Mở Radmin VPN
   - Xem IP address (ví dụ: `26.xxx.xxx.xxx`)
   - Ghi lại IP này

2. **Cấu hình backend để lắng nghe trên tất cả interfaces:**
   - Mở `backend/Program.cs`
   - Đảm bảo server lắng nghe trên `0.0.0.0` hoặc IP Radmin:
   ```csharp
   app.Urls.Add("http://0.0.0.0:5000");
   // hoặc
   app.Urls.Add($"http://26.xxx.xxx.xxx:5000");
   ```

3. **Chạy backend server:**
   ```bash
   cd backend
   dotnet run
   ```

4. **Kiểm tra firewall:**
   - Mở Windows Firewall
   - Cho phép port 5000 (TCP) qua firewall
   - Hoặc tạm thời tắt firewall để test

## Cấu hình Unity Client

### Trên máy chủ (Host):

1. Mở Unity project
2. Tìm `TcpClientManager` trong scene
3. Đặt `serverURL` = `http://26.xxx.xxx.xxx:5000` (IP Radmin của máy chủ)

### Trên máy client:

1. Mở Unity project (hoặc build)
2. Tìm `TcpClientManager` trong scene
3. Đặt `serverURL` = `http://26.xxx.xxx.xxx:5000` (IP Radmin của máy chủ)

## Test kết nối

### Bước 1: Test đăng ký/đăng nhập

**Trên máy chủ:**
1. Chạy backend server
2. Chạy Unity game
3. Đăng ký tài khoản mới (ví dụ: `test1@test.com` / `password123`)

**Trên máy client:**
1. Chạy Unity game
2. Đăng ký tài khoản khác (ví dụ: `test2@test.com` / `password123`)
3. Đăng nhập với tài khoản vừa tạo

### Bước 2: Test multiplayer

**Trên máy chủ:**
1. Đăng nhập vào game
2. Di chuyển character
3. Kiểm tra console logs xem có nhận được vị trí từ client không

**Trên máy client:**
1. Đăng nhập vào game
2. Di chuyển character
3. Kiểm tra xem có thấy character của máy chủ không
4. Kiểm tra console logs

### Bước 3: Test chat

**Trên máy chủ:**
1. Gửi tin nhắn trong game
2. Kiểm tra xem client có nhận được không

**Trên máy client:**
1. Gửi tin nhắn trong game
2. Kiểm tra xem server có nhận được không

## Troubleshooting

### Lỗi: Không kết nối được đến server

**Kiểm tra:**
1. Radmin VPN đã kết nối chưa? (cả 2 máy phải cùng network)
2. Backend server đã chạy chưa?
3. IP address trong Unity có đúng không?
4. Firewall có chặn port 5000 không?

**Giải pháp:**
```bash
# Test kết nối từ máy client
ping 26.xxx.xxx.xxx  # Thay bằng IP Radmin của máy chủ

# Test HTTP connection
curl http://26.xxx.xxx.xxx:5000/api/auth/login
```

### Lỗi: Connection timeout

**Giải pháp:**
1. Tắt Windows Firewall tạm thời để test
2. Đảm bảo backend lắng nghe trên `0.0.0.0:5000`
3. Kiểm tra Radmin VPN connection status

### Lỗi: 401 Unauthorized

**Nguyên nhân:**
- Email/password không đúng
- Database chưa có user

**Giải pháp:**
1. Đăng ký tài khoản mới
2. Kiểm tra database có user chưa
3. Kiểm tra logs backend để xem lỗi chi tiết

## Kiểm tra Logs

### Backend logs:
- Xem console output của `dotnet run`
- Tìm các dòng `[Login]`, `[Register]`, `[Chat]`

### Unity logs:
- Mở Unity Console
- Tìm các dòng `[Login]`, `[Register]`, `[MultiplayerManager]`

## Lưu ý quan trọng

1. **IP Radmin thay đổi:** Mỗi lần kết nối Radmin VPN, IP có thể thay đổi. Cần cập nhật lại trong Unity.

2. **Port forwarding:** Radmin VPN tự động forward ports, không cần cấu hình router.

3. **Security:** Radmin VPN chỉ dùng để test. Không dùng cho production.

4. **Performance:** Radmin VPN có thể có độ trễ. Đây là bình thường khi test qua VPN.

## Test nhanh với PowerShell

**Trên máy client, test kết nối:**
```powershell
# Test ping
Test-Connection -ComputerName 26.xxx.xxx.xxx -Count 2

# Test HTTP
Invoke-WebRequest -Uri "http://26.xxx.xxx.xxx:5000/api/auth/login" -Method POST -ContentType "application/json" -Body '{"email":"test@test.com","password":"test123"}'
```

Thay `26.xxx.xxx.xxx` bằng IP Radmin của máy chủ.

