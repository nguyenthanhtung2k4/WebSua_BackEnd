CREATE TABLE KhachHang (
    khach_hang_id INT PRIMARY KEY AUTO_INCREMENT,
    ho_ten VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    mat_khau VARCHAR(255) NOT NULL,
    dia_chi TEXT,
    sdt VARCHAR(15) NOT NULL,
    ngay_dang_ky DATE
);

CREATE TABLE LoaiSua (
    loai_sua_id INT PRIMARY KEY AUTO_INCREMENT,
    ten_loai VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE SanPhamSua (
    sanpham_id INT PRIMARY KEY AUTO_INCREMENT,
    ten_san_pham VARCHAR(100) NOT NULL,
    gia DECIMAL(10,2) NOT NULL,
    mo_ta TEXT,
    so_luong_ton INT NOT NULL DEFAULT 0,
    loai_sua_id INT NOT NULL,
    FOREIGN KEY (loai_sua_id) REFERENCES LoaiSua(loai_sua_id)
);

CREATE TABLE DonHang (
    don_hang_id INT PRIMARY KEY AUTO_INCREMENT,
    khach_hang_id INT NOT NULL,
    tong_tien DECIMAL(10,2) NOT NULL,
    trang_thai VARCHAR(50) NOT NULL DEFAULT 'Chờ xử lý',
    ngay_dat DATE NOT NULL,
    FOREIGN KEY (khach_hang_id) REFERENCES KhachHang(khach_hang_id)
);

CREATE TABLE ChiTietDonHang (
    chi_tiet_id INT PRIMARY KEY AUTO_INCREMENT,
    don_hang_id INT NOT NULL,
    sanpham_id INT NOT NULL,
    so_luong INT NOT NULL,
    don_gia DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (don_hang_id) REFERENCES DonHang(don_hang_id),
    FOREIGN KEY (sanpham_id) REFERENCES SanPhamSua(sanpham_id)
);

CREATE TABLE DangKyTuVan (
    dang_ky_id INT PRIMARY KEY AUTO_INCREMENT,
    khach_hang_id INT NOT NULL,
    noi_dung TEXT NOT NULL,
    ngay_dang_ky DATE NOT NULL,
    trang_thai VARCHAR(50) NOT NULL DEFAULT 'Chưa xử lý',
    FOREIGN KEY (khach_hang_id) REFERENCES KhachHang(khach_hang_id)
);

CREATE TABLE NguoiDung (
    nguoi_dung_id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(50) UNIQUE NOT NULL,
    mat_khau VARCHAR(255) NOT NULL,
    vai_tro VARCHAR(50) NOT NULL DEFAULT 'KhachHang'
);