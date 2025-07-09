namespace Shop.Application.DTOs
{
    public class GetAllOrderAdminDTO
    {
        public int MaDh { get; set; }
        public string? TenKhachHang { get; set; }
        public DateTime? NgayDat { get; set; }
        public decimal? TongTien { get; set; }
        public string? TrangThai { get; set; }
    }
}
