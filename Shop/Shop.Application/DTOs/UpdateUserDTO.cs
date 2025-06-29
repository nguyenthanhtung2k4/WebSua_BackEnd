using System;
using System.ComponentModel.DataAnnotations; // Thêm namespace này cho validation

namespace Shop.Application.DTOs
{
    public class UpdateUserDTO
    {
        [Required] // Đảm bảo ID được gửi đến
        public int MaNd { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải có từ 3 đến 50 ký tự.")]
        public string TenDangNhap { get; set; } = null!;

        // Bỏ MatKhau khỏi DTO này để tránh hiển thị mật khẩu và xử lý cập nhật mật khẩu phức tạp
         //public string MatKhau { get; set; } = null!; // Xóa hoặc comment dòng này

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        public string? Email { get; set; }

        [StringLength(20, ErrorMessage = "Vai trò không được vượt quá 20 ký tự.")]
        public string? VaiTro { get; set; } // = "KhachHang"; // Không nên gán giá trị mặc định ở đây nếu nó được chọn từ dropdown

        public string? status { get; set; } // Giả sử 'status' là string để khớp với HTML options
    }
}