CREATE DATABASE QL_THUCHI;
USE QL_THUCHI;

CREATE TABLE QuyenTruyCap (
    MAQUYEN CHAR(20) PRIMARY KEY, 
    TENQUYEN NVARCHAR(100) NOT NULL UNIQUE,
);

CREATE TABLE TaiKhoan (
    MATAIKHOAN CHAR(20) PRIMARY KEY, 
    MAQUYEN CHAR(20),
	TENDANGNHAP VARCHAR(40),
	MATKHAU VARCHAR(40),
	EMAIL VARCHAR(40),
	ISEMAILCONFIRMED INT,
	EMAILCONFIRMATIONTOKEN NVARCHAR(100),
	OTP INT,
	 CONSTRAINT FK_TK_QTC FOREIGN KEY (MAQUYEN) REFERENCES QuyenTruyCap(MAQUYEN)
);
CREATE TABLE KhachHang (
    MAKH CHAR(20) PRIMARY KEY, 
	MATAIKHOAN CHAR(20),
    HOTEN NVARCHAR(50),
	NGAYSINH DATE,
	SODT CHAR(11),
	XU INT,
	AVATAR VARCHAR(150),
	CONSTRAINT FK_KH_TK FOREIGN KEY (MATAIKHOAN) REFERENCES TaiKhoan(MATAIKHOAN)
);

INSERT INTO QuyenTruyCap (MAQUYEN, TENQUYEN)
VALUES 
    ('Q001', N'Ph? Huynh'),
    ('Q002', N'Con Cái');

INSERT INTO TaiKhoan (MATAIKHOAN, MAQUYEN, TENDANGNHAP, MATKHAU, EMAIL, ISEMAILCONFIRMED, EMAILCONFIRMATIONTOKEN, OTP)
VALUES 
    ('TK0001', 'Q001', 'admin', '123456', 'khangt2110@gmail.com', 1, NULL, NULL),
    ('TK0002', 'Q002', 'Khang12', '123456', 'khangtuong2110@gmail.com', 1, NULL, NULL);
INSERT INTO KhachHang (MAKH, MATAIKHOAN, HOTEN, NGAYSINH, SODT, XU, AVATAR)
VALUES 
('KH0001', 'TK0001', N'Tý?ng T?n Khang', '2004-08-09', '0374075809', 100, 'images/avatar4.png'),
('KH0002', 'TK0002', N'Nguy?n Výõng Ðào', '1999-10-11', '09808208', 100, Null)

select* from QuyenTruyCap
select* from TaiKhoan
select* from KhachHang

UPDATE QuyenTruyCap
SET TENQUYEN = N'Ph? Huynh'
WHERE MAQUYEN = 'Q001';

DELETE FROM TaiKhoan WHERE MATAIKHOAN = 'tk0003              ';

SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TaiKhoan';

CREATE TABLE Vi (
    MaVi INT PRIMARY KEY IDENTITY(1,1), -- Mã ví tự tăng
    TenVi NVARCHAR(100) NOT NULL,       -- Tên ví (vd: Tiền mặt, Ngân hàng...)
    LoaiVi NVARCHAR(50),                -- Loại ví (vd: Chi tiêu, tiết kiệm,... nếu có)
    IconVi NVARCHAR(100)                -- Tên icon Flutter (vd: Icons.wallet, Icons.credit_card,...)
);
INSERT INTO Vi (TenVi, LoaiVi, IconVi) VALUES 
(N'Tiền mặt', N'Chi tiêu', 'Icons.money'),
(N'Tài khoản ngân hàng', N'Tiết kiệm', 'Icons.account_balance'),
(N'Thẻ tín dụng', N'Chi tiêu', 'Icons.credit_card'),
(N'Tài khoản đầu tư', N'Đầu tư', 'Icons.trending_up'),
(N'Ví điện tử', N'Chi tiêu', 'Icons.phone_android'),
(N'Khác', N'Khác', 'Icons.more_horiz');
CREATE TABLE LoaiTien (
    MaLoai INT PRIMARY KEY IDENTITY(1,1),   -- Mã loại tự tăng
    TenLoai NVARCHAR(100) NOT NULL,         -- Tên loại tiền (VD: Việt Nam Đồng, United States Dollar)
    MenhGia NVARCHAR(10) NOT NULL,          -- Mệnh giá (VD: VND, USD, ...)
    KyHieu NVARCHAR(5) NOT NULL             -- Ký hiệu (VD: đ, $, €,...)
);

INSERT INTO LoaiTien (TenLoai, MenhGia, KyHieu) VALUES
(N'Việt Nam Đồng', 'VND', 'đ'),
(N'United States Dollar', 'USD', '$'),
(N'Euro', 'EUR', '€'),
(N'Japanese Yen', 'JPY', '¥'),
(N'British Pound', 'GBP', '£'),
(N'Korean Won', 'KRW', '₩'),
(N'Swiss Franc', 'CHF', 'Fr'),
(N'Chinese Yuan', 'CNY', '¥'),
(N'Canadian Dollar', 'CAD', '$'),
(N'Australian Dollar', 'AUD', '$'),
(N'Singapore Dollar', 'SGD', '$'),
(N'Thai Baht', 'THB', '฿'),
(N'Indian Rupee', 'INR', '₹'),
(N'Malaysian Ringgit', 'MYR', 'RM'),
(N'Indonesian Rupiah', 'IDR', 'Rp'),
(N'Hong Kong Dollar', 'HKD', '$'),
(N'Philippine Peso', 'PHP', '₱'),
(N'New Zealand Dollar', 'NZD', '$'),
(N'Russian Ruble', 'RUB', '₽'),
(N'South African Rand', 'ZAR', 'R'),
(N'Saudi Riyal', 'SAR', 'ر.س'),
(N'United Arab Emirates Dirham', 'AED', 'د.إ');


CREATE TABLE ViNguoiDung (
    MaNguoiDung CHAR(20) NOT NULL,           -- Mã người dùng
    MaVi INT NOT NULL,                      -- Mã ví (tham chiếu đến bảng Vi)
    MaLoaiTien INT NOT NULL,                -- Mã loại tiền (tham chiếu bảng LoaiTien)
    TenTaiKhoan NVARCHAR(100) NOT NULL,     -- Tên tài khoản (do người dùng đặt)
    DienGiai NVARCHAR(255),                 -- Diễn giải hoặc ghi chú
	SoDu DECIMAL(18, 2),
    PRIMARY KEY (MaNguoiDung, MaVi, TenTaiKhoan),  -- Khóa chính kết hợp
    FOREIGN KEY (MaVi) REFERENCES Vi(MaVi),
    FOREIGN KEY (MaLoaiTien) REFERENCES LoaiTien(MaLoai),
	FOREIGN KEY (MaNguoiDung) REFERENCES KhachHang(MAKH)
);
INSERT INTO ViNguoiDung (MaNguoiDung, MaVi, MaLoaiTien, TenTaiKhoan, DienGiai, SoDu) VALUES
						('KH0001              ',		1,		1,		N'Ví Tiền Mặt', N'Ví Tiền Mặt', 5000000),
						('KH0001              ',		2,		1,		N'Tài khoản Vietcombank', N'Tài khoản tiết kiệm', 15000000),
						('KH0002              ',		3,		1,		N'Thẻ tín dụng Techcombank', N'Thẻ tín dụng chính', 0),
						('KH0002              ',		5,		1,		N'Ví điện tử Momo', N'Ví điện tử sử dụng thường xuyên', 1200000),
						('KH0002              ',		1,		1,		N'Ví Tiền Mặt', N' ', 0);
-- update lần 3--

-- Bảng Loại
CREATE TABLE Loai (
    MaLoai INT PRIMARY KEY IDENTITY(1,1),
    TenLoai NVARCHAR(50) NOT NULL CHECK (TenLoai IN ('Thu', 'Chi'))
);

-- Bảng Danh Mục
CREATE TABLE DanhMuc (
    MaDanhMuc INT PRIMARY KEY IDENTITY(1,1),
    TenDanhMuc NVARCHAR(100) NOT NULL,
    MaLoaiDanhMuc INT NOT NULL,
    FOREIGN KEY (MaLoaiDanhMuc) REFERENCES Loai(MaLoai)
);

-- Bảng Danh Mục Người Dùng
CREATE TABLE DanhMucNguoiDung (
    MaDanhMucNguoiDung INT,
    MaNguoiDung CHAR(20) NOT NULL,
    TenDanhMucNguoiDung NVARCHAR(100) NOT NULL,
    ToiDa DECIMAL(18, 2),    -- số tiền tối đa mà người dùng set
    SoTienHienTai DECIMAL(18, 2),    -- số tiền hiện tại mà người dùng đã nhập
    PRIMARY KEY (MaDanhMucNguoiDung, MaNguoiDung),
    FOREIGN KEY (MaNguoiDung) REFERENCES KhachHang(MAKH)
);

-- Bảng Giao Dịch
CREATE TABLE GiaoDich (
    MaGiaoDich INT PRIMARY KEY IDENTITY(1,1),
    MaNguoiDung CHAR(20) NOT NULL,
    MaVi INT NOT NULL,
    MaDanhMucNguoiDung INT,
    SoTien DECIMAL(18, 2) NOT NULL,
    SoTienCu DECIMAL(18, 2),
    SoTienMoi DECIMAL(18, 2),
    GhiChu NVARCHAR(255),
    NgayGiaoDich DATE NOT NULL,
    LoaiGiaoDich NVARCHAR(50) NOT NULL CHECK (LoaiGiaoDich IN ('Thu', 'Chi')),
    MaViNhan INT, -- Cho trường hợp chuyển khoản
    FOREIGN KEY (MaNguoiDung) REFERENCES KhachHang(MAKH),
    FOREIGN KEY (MaVi) REFERENCES Vi(MaVi),
    FOREIGN KEY (MaDanhMucNguoiDung, MaNguoiDung) REFERENCES DanhMucNguoiDung(MaDanhMucNguoiDung, MaNguoiDung),
    FOREIGN KEY (MaViNhan) REFERENCES Vi(MaVi)
);

-- Bảng Anh Hóa Đơn
CREATE TABLE AnhHoaDon (
    MaAnh INT PRIMARY KEY IDENTITY(1,1),
    MaGiaoDich INT NOT NULL,
    DuongDanAnh NVARCHAR(255) NOT NULL,
    NgayTaiLen DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaGiaoDich) REFERENCES GiaoDich(MaGiaoDich)
);

-- Bảng Lịch Sử Giao Dịch
CREATE TABLE LichSuGiaoDich (
    MaLichSu INT PRIMARY KEY IDENTITY(1,1),
    MaGiaoDich INT NOT NULL,
    HanhDong NVARCHAR(50) NOT NULL CHECK (HanhDong IN ('TaoMoi', 'CapNhat', 'Xoa')),
    SoTienCu DECIMAL(18, 2),
    SoTienMoi DECIMAL(18, 2),
    ThucHienBoi CHAR(20) NOT NULL,
    ThoiGian DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaGiaoDich) REFERENCES GiaoDich(MaGiaoDich),
    FOREIGN KEY (ThucHienBoi) REFERENCES KhachHang(MAKH)
);

-- Thêm dữ liệu mẫu cho Loai
INSERT INTO Loai (TenLoai) VALUES
(N'Thu'),
(N'Chi');

-- Thêm dữ liệu mẫu cho DanhMuc
INSERT INTO DanhMuc (TenDanhMuc, MaLoaiDanhMuc) VALUES
(N'Lương', 1),
(N'Thưởng', 1),
(N'Đầu tư', 1),
(N'Ăn uống', 2),
(N'Di chuyển', 2),
(N'Mua sắm', 2),
(N'Giải trí', 2),
(N'Y tế', 2),
(N'Học tập', 2),
(N'Khác', 2);

-- Thêm dữ liệu mẫu cho DanhMucNguoiDung
INSERT INTO DanhMucNguoiDung (MaDanhMucNguoiDung, MaNguoiDung, TenDanhMucNguoiDung, ToiDa, SoTienHienTai) VALUES
(1, 'KH0001', N'Lương tháng 3', 15000000, 15000000),
(2, 'KH0001', N'Ăn trưa văn phòng', 2000000, 1500000),
(3, 'KH0001', N'Di chuyển', 1000000, 800000),
(4, 'KH0001', N'Mua sắm cá nhân', 3000000, 2500000),
(1, 'KH0002', N'Lương tháng 3', 12000000, 12000000),
(2, 'KH0002', N'Ăn uống', 1500000, 1200000),
(3, 'KH0002', N'Di chuyển', 800000, 600000);

-- Thêm dữ liệu mẫu cho GiaoDich
INSERT INTO GiaoDich (MaNguoiDung, MaVi, MaDanhMucNguoiDung, SoTien, SoTienCu, SoTienMoi, GhiChu, NgayGiaoDich, LoaiGiaoDich) VALUES
('KH0001', 1, 1, 15000000, 0, 15000000, N'Lương tháng 3', '2024-03-01', 'Thu'),
('KH0001', 1, 2, 50000, 15000000, 14950000, N'Ăn trưa tại canteen', '2024-03-02', 'Chi'),
('KH0001', 1, 3, 200000, 14950000, 14750000, N'Đổ xăng xe', '2024-03-03', 'Chi'),
('KH0001', 2, 4, 1000000, 15000000, 14000000, N'Mua quần áo', '2024-03-04', 'Chi'),
('KH0001', 1, NULL, 5000000, 14750000, 9750000, 'Chuyển tiền vào tài khoản tiết kiệm', '2024-03-05', 'ChuyenKhoan'),
('KH0002', 3, 1, 12000000, 0, 12000000, N'Lương tháng 3', '2024-03-01', 'Thu'),
('KH0002', 3, 2, 30000, 12000000, 11970000, N'Ăn sáng', '2024-03-02', 'Chi'),
('KH0002', 5, 3, 150000, 1200000, 1050000, N'Đi taxi', '2024-03-03', 'Chi');

-- Thêm dữ liệu mẫu cho AnhHoaDon
INSERT INTO AnhHoaDon (MaGiaoDich, DuongDanAnh, NgayTaiLen) VALUES
(2, 'receipts/lunch_20240302.jpg', '2024-03-02 12:30:00'),
(3, 'receipts/gas_20240303.jpg', '2024-03-03 15:45:00'),
(4, 'receipts/shopping_20240304.jpg', '2024-03-04 16:20:00'),
(7, 'receipts/breakfast_20240302.jpg', '2024-03-02 08:15:00');

-- Thêm dữ liệu mẫu cho LichSuGiaoDich
INSERT INTO LichSuGiaoDich (MaGiaoDich, HanhDong, SoTienCu, SoTienMoi, ThucHienBoi, ThoiGian) VALUES
(1, 'TaoMoi', NULL, 15000000, 'KH0001', '2024-03-01 09:00:00'),
(2, 'TaoMoi', NULL, 50000, 'KH0001', '2024-03-02 12:30:00'),
(3, 'TaoMoi', NULL, 200000, 'KH0001', '2024-03-03 15:45:00'),
(4, 'TaoMoi', NULL, 1000000, 'KH0001', '2024-03-04 16:20:00'),
(4, 'CapNhat', 1000000, 1200000, 'KH0001', '2024-03-04 16:25:00'),
(5, 'TaoMoi', NULL, 5000000, 'KH0001', '2024-03-05 10:00:00'),
(6, 'TaoMoi', NULL, 12000000, 'KH0002', '2024-03-01 09:00:00'),
(7, 'TaoMoi', NULL, 30000, 'KH0002', '2024-03-02 08:15:00'),
(8, 'TaoMoi', NULL, 150000, 'KH0002', '2024-03-03 14:30:00');

