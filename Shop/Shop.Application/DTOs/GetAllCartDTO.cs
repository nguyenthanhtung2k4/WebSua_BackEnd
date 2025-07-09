namespace Shop.Application.DTOs
{
    public class GetAllCartDTO
    {
        public int MaGh { get; set; }
        public int MaSua { get; set; }
        public string? TenSua { get; set; }
        public string? HinhAnh { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public DateTime? NgayTao { get; set; }
    }
}
