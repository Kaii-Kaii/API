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
