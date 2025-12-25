# Hướng dẫn Test Game 2 Máy ở Xa Nhau bằng Radmin VPN

## Bước 1: Cài đặt Radmin VPN

### Trên cả 2 máy (Máy Server và Máy Client):

1. **Tải Radmin VPN:**
   - Truy cập: https://www.radmin-vpn.com/
   - Tải và cài đặt Radmin VPN

2. **Đăng ký tài khoản (miễn phí):**
   - Mở Radmin VPN
   - Click **"Sign Up"** hoặc **"Create Account"**
   - Điền thông tin và đăng ký (miễn phí cho tối đa 100 máy)

## Bước 2: Tạo/Tham gia Network

### Trên Máy Server (máy chạy game server):

1. Mở **Radmin VPN**
2. Click nút **"Create Network"** (hoặc **"+"** → **"Create Network"**)
3. Đặt tên network (ví dụ: `MyGarden-Test`)
4. Đặt **mật khẩu** cho network (ghi nhớ mật khẩu này)
5. Click **"Create"**
6. **Ghi lại Network ID** (sẽ hiển thị sau khi tạo, ví dụ: `1234567890`)

### Trên Máy Client (máy chơi game):

1. Mở **Radmin VPN**
2. Click nút **"Join Network"** (hoặc **"+"** → **"Join Network"**)
3. Nhập **Network ID** từ máy server
4. Nhập **mật khẩu** network
5. Click **"Connect"** hoặc **"Join"**

## Bước 3: Kiểm tra kết nối

### Trên cả 2 máy:

1. Sau khi kết nối thành công, bạn sẽ thấy:
   - Tên network trong danh sách
   - **IP Address** của máy mình (thường là `26.x.x.x`)
   - Danh sách các máy khác trong network

2. **Trên máy Server:**
   - Ghi lại **IP Address** của máy server (ví dụ: `26.123.45.67`)

3. **Trên máy Client:**
   - Thử ping máy server để kiểm tra:
     ```powershell
     ping 26.123.45.67
     ```
   - Nếu ping thành công → Kết nối OK ✅

## Bước 4: Chạy Backend Server

### Trên Máy Server:

1. **Mở MySQL Server** (nếu chưa chạy)

2. **Chạy Backend Server:**
   ```powershell
   cd G:\Documents\FE\MyGarden-FE_201225\MyGarden-FE_201225\backend
   dotnet run
   ```
   
   Server sẽ chạy tại: `http://0.0.0.0:5000` (đã được cấu hình sẵn)

3. **Mở Firewall cho port 5000:**
   ```powershell
   netsh advfirewall firewall add rule name="MyGarden Server" dir=in action=allow protocol=TCP localport=5000
   ```

4. **Kiểm tra server đang chạy:**
   - Mở trình duyệt: `http://localhost:5000`
   - Hoặc test API: `http://localhost:5000/api/auth/register`

## Bước 5: Cấu hình Unity Client

### Trên Máy Client:

1. **Mở Unity Editor** và mở project game

2. **Tìm GameObject có component `TcpClientManager`:**
   - Trong Hierarchy, tìm GameObject có script `TcpClientManager`
   - Hoặc tìm trong scene có Register/Login UI

3. **Trong Inspector, tìm field "Server URL":**
   - Hiện tại có thể là: `http://localhost:5000`

4. **Đổi thành IP Radmin của máy server:**
   - Đổi thành: `http://[IP_RADMIN_SERVER]:5000`
   - Ví dụ: `http://26.123.45.67:5000`
   - (Thay bằng IP Radmin thực tế của máy server)

5. **Lưu scene** (Ctrl+S)

## Bước 6: Test kết nối

### Trên Máy Client:

1. **Chạy game** (Play trong Unity hoặc Build)

2. **Kiểm tra Console log:**
   - Nếu thấy: `[TcpClientManager] Server URL: http://26.x.x.x:5000` → OK ✅
   - Nếu thấy lỗi kết nối → Kiểm tra lại IP và firewall

3. **Test đăng ký/đăng nhập:**
   - Thử đăng ký tài khoản mới
   - Thử đăng nhập
   - Nếu thành công → Kết nối hoạt động! ✅

## Bước 7: Test Multiplayer

### Trên cả 2 máy:

1. **Máy Server:** Chạy game (có thể dùng Unity Editor hoặc Build)

2. **Máy Client:** Chạy game (Unity Editor hoặc Build)

3. **Cả 2 máy đều kết nối đến cùng server:**
   - Server URL: `http://[IP_RADMIN_SERVER]:5000`
   - Cả 2 sẽ thấy nhau trong game!

## Khắc phục sự cố

### ❌ Không ping được máy server

**Nguyên nhân:**
- Radmin VPN chưa kết nối đúng
- Firewall block

**Giải pháp:**
1. Kiểm tra cả 2 máy đã vào cùng network chưa
2. Thử disconnect và reconnect lại Radmin VPN
3. Kiểm tra Windows Firewall trên máy server

### ❌ Không kết nối được server từ client

**Nguyên nhân:**
- IP Radmin sai
- Server chưa chạy
- Firewall chưa mở port

**Giải pháp:**
1. Kiểm tra IP Radmin trên máy server (mở Radmin VPN xem)
2. Kiểm tra server đang chạy: `http://localhost:5000` trên máy server
3. Kiểm tra firewall đã mở port 5000 chưa
4. Thử ping từ client đến server IP

### ❌ IP Radmin thay đổi

**Nguyên nhân:**
- IP Radmin có thể thay đổi mỗi lần reconnect

**Giải pháp:**
- Mỗi lần reconnect Radmin, kiểm tra lại IP và cập nhật trong Unity

### ❌ Server không nhận kết nối

**Nguyên nhân:**
- Server chỉ bind localhost
- Firewall block

**Giải pháp:**
1. Kiểm tra `backend/Program.cs` có dòng:
   ```csharp
   app.Run("http://0.0.0.0:5000");
   ```
   (Đã được cấu hình sẵn)

2. Kiểm tra firewall:
   ```powershell
   netsh advfirewall firewall show rule name="MyGarden Server"
   ```

## Lưu ý quan trọng

✅ **Radmin VPN là miễn phí** cho tối đa 100 máy  
✅ **Không cần cấu hình router** - Radmin tự động tạo mạng LAN ảo  
✅ **Bảo mật** - Kết nối được mã hóa  
✅ **Ổn định** - Hoạt động tốt cho test multiplayer  

⚠️ **IP Radmin có thể thay đổi** mỗi lần reconnect  
⚠️ **Cần đảm bảo cả 2 máy đều online** và Radmin VPN đang chạy  
⚠️ **MySQL trên máy server** cần cho phép kết nối từ xa (nếu cần)  

## Tóm tắt các bước

1. ✅ Cài Radmin VPN trên cả 2 máy
2. ✅ Tạo network trên máy server, join trên máy client
3. ✅ Ghi lại IP Radmin của máy server
4. ✅ Chạy backend server trên máy server
5. ✅ Mở firewall port 5000
6. ✅ Đổi Server URL trong Unity client thành IP Radmin server
7. ✅ Test kết nối và chơi game!

## Test thành công khi:

- ✅ Ping được từ client đến server
- ✅ Unity client kết nối được đến server
- ✅ Đăng ký/đăng nhập thành công
- ✅ Cả 2 người chơi thấy nhau trong game

Chúc bạn test thành công! 🎮

