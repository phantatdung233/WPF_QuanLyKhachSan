/* =========================================================================
   AURA HOTEL — Hệ thống Quản lý Khách sạn (IT008)
   Script khởi tạo CSDL SQL Server 8 bảng & dữ liệu mẫu chuẩn (Idempotent)
   Chạy trực tiếp trên SSMS (F5) nhiều lần liên tiếp không phát sinh lỗi.
   ========================================================================= */

USE [master];
GO

IF DB_ID(N'HotelManagementDb') IS NOT NULL
BEGIN
    EXEC(N'ALTER DATABASE [HotelManagementDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [HotelManagementDb];');
END
GO

CREATE DATABASE [HotelManagementDb];
GO

USE [HotelManagementDb];
GO

SET NOCOUNT ON;
GO

/* =========================================================================
   1. XÓA BẢNG THEO THỨ TỰ KHÓA NGOẠI (HỖ TRỢ CHẠY LẠI TRONG CÙNG DB)
   ========================================================================= */
DROP TABLE IF EXISTS [dbo].[Invoices];
DROP TABLE IF EXISTS [dbo].[BookingServices];
DROP TABLE IF EXISTS [dbo].[Bookings];
DROP TABLE IF EXISTS [dbo].[Services];
DROP TABLE IF EXISTS [dbo].[Customers];
DROP TABLE IF EXISTS [dbo].[Rooms];
DROP TABLE IF EXISTS [dbo].[RoomTypes];
DROP TABLE IF EXISTS [dbo].[Users];
GO

/* =========================================================================
   2. ĐỊNH NGHĨA LƯỢC ĐỒ 8 BẢNG CỐT LÕI & RÀNG BUỘC TOÀN VẸN (GLOSSARY.md)
   ========================================================================= */

-- Bảng 1: Users (Tài khoản truy cập & phân quyền Administrator / Staff)
CREATE TABLE [dbo].[Users]
(
    [UserId]   INT            IDENTITY(1, 1) NOT NULL,
    [Username] NVARCHAR(50)   NOT NULL,
    [Password] NVARCHAR(100)  NOT NULL,
    [FullName] NVARCHAR(100)  NOT NULL,
    [Role]     NVARCHAR(20)   NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [UQ_Users_Username] UNIQUE ([Username]),
    CONSTRAINT [CK_Users_Role] CHECK ([Role] IN (N'Administrator', N'Staff'))
);
GO

-- Bảng 2: RoomTypes (Danh mục loại phòng & đơn giá cơ bản theo đêm)
CREATE TABLE [dbo].[RoomTypes]
(
    [RoomTypeId]  INT            IDENTITY(1, 1) NOT NULL,
    [TypeName]    NVARCHAR(100)  NOT NULL,
    [PricePerDay] DECIMAL(18, 2) NOT NULL,
    CONSTRAINT [PK_RoomTypes] PRIMARY KEY CLUSTERED ([RoomTypeId] ASC),
    CONSTRAINT [UQ_RoomTypes_TypeName] UNIQUE ([TypeName]),
    CONSTRAINT [CK_RoomTypes_PricePerDay] CHECK ([PricePerDay] > 0)
);
GO

-- Bảng 3: Rooms (Danh mục phòng & 4 trạng thái RoomStatus bất biến)
CREATE TABLE [dbo].[Rooms]
(
    [RoomId]     INT           IDENTITY(1, 1) NOT NULL,
    [RoomNumber] NVARCHAR(20)  NOT NULL,
    [RoomTypeId] INT           NOT NULL,
    [Status]     NVARCHAR(30)  NOT NULL CONSTRAINT [DF_Rooms_Status] DEFAULT (N'Trống'),
    CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED ([RoomId] ASC),
    CONSTRAINT [UQ_Rooms_RoomNumber] UNIQUE ([RoomNumber]),
    CONSTRAINT [FK_Rooms_RoomTypes] FOREIGN KEY ([RoomTypeId]) REFERENCES [dbo].[RoomTypes] ([RoomTypeId]),
    CONSTRAINT [CK_Rooms_Status] CHECK ([Status] IN (N'Trống', N'Đang ở', N'Đã đặt', N'Bảo trì'))
);
GO

-- Bảng 4: Customers (Hồ sơ khách lưu trú với CCCD/CMND duy nhất & Email tùy chọn)
CREATE TABLE [dbo].[Customers]
(
    [CustomerId] INT           IDENTITY(1, 1) NOT NULL,
    [FullName]   NVARCHAR(100) NOT NULL,
    [CitizenId]  VARCHAR(20)   NOT NULL,
    [Phone]      VARCHAR(15)   NOT NULL,
    [Email]      NVARCHAR(100) NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([CustomerId] ASC),
    CONSTRAINT [UQ_Customers_CitizenId] UNIQUE ([CitizenId])
);
GO

-- Bảng 5: Services (Danh mục dịch vụ khách sạn với đơn giá >= 0)
CREATE TABLE [dbo].[Services]
(
    [ServiceId]   INT            IDENTITY(1, 1) NOT NULL,
    [ServiceName] NVARCHAR(100)  NOT NULL,
    [Price]       DECIMAL(18, 2) NOT NULL,
    CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED ([ServiceId] ASC),
    CONSTRAINT [UQ_Services_ServiceName] UNIQUE ([ServiceName]),
    CONSTRAINT [CK_Services_Price] CHECK ([Price] >= 0)
);
GO

-- Bảng 6: Bookings (Hồ sơ đặt phòng & lưu trú theo 4 trạng thái BookingStatus)
CREATE TABLE [dbo].[Bookings]
(
    [BookingId]    INT          IDENTITY(1, 1) NOT NULL,
    [CustomerId]   INT          NOT NULL,
    [RoomId]       INT          NOT NULL,
    [CheckInDate]  DATETIME     NOT NULL,
    [CheckOutDate] DATETIME     NOT NULL,
    [Status]       NVARCHAR(30) NOT NULL CONSTRAINT [DF_Bookings_Status] DEFAULT (N'Đã đặt'),
    CONSTRAINT [PK_Bookings] PRIMARY KEY CLUSTERED ([BookingId] ASC),
    CONSTRAINT [FK_Bookings_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([CustomerId]),
    CONSTRAINT [FK_Bookings_Rooms] FOREIGN KEY ([RoomId]) REFERENCES [dbo].[Rooms] ([RoomId]),
    CONSTRAINT [CK_Bookings_Status] CHECK ([Status] IN (N'Đã đặt', N'Đang ở', N'Hoàn thành', N'Đã hủy')),
    CONSTRAINT [CK_Bookings_DateRange] CHECK (
        [CheckOutDate] > [CheckInDate]
        AND ([Status] <> N'Đã đặt' OR CAST([CheckOutDate] AS DATE) > CAST([CheckInDate] AS DATE))
    )
);
GO

-- Bảng 7: BookingServices (Chi tiết dịch vụ đã sử dụng kèm snapshot UnitPrice)
CREATE TABLE [dbo].[BookingServices]
(
    [BookingServiceId] INT            IDENTITY(1, 1) NOT NULL,
    [BookingId]        INT            NOT NULL,
    [ServiceId]        INT            NOT NULL,
    [Quantity]         INT            NOT NULL,
    [UnitPrice]        DECIMAL(18, 2) NOT NULL,
    CONSTRAINT [PK_BookingServices] PRIMARY KEY CLUSTERED ([BookingServiceId] ASC),
    CONSTRAINT [FK_BookingServices_Bookings] FOREIGN KEY ([BookingId]) REFERENCES [dbo].[Bookings] ([BookingId]),
    CONSTRAINT [FK_BookingServices_Services] FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Services] ([ServiceId]),
    CONSTRAINT [CK_BookingServices_Quantity] CHECK ([Quantity] >= 1),
    CONSTRAINT [CK_BookingServices_UnitPrice] CHECK ([UnitPrice] >= 0)
);
GO

-- Bảng 8: Invoices (Hóa đơn thanh toán theo 2 trạng thái InvoiceStatus)
CREATE TABLE [dbo].[Invoices]
(
    [InvoiceId]     INT            IDENTITY(1, 1) NOT NULL,
    [BookingId]     INT            NOT NULL,
    [RoomCharge]    DECIMAL(18, 2) NOT NULL CONSTRAINT [DF_Invoices_RoomCharge] DEFAULT (0),
    [ServiceCharge] DECIMAL(18, 2) NOT NULL CONSTRAINT [DF_Invoices_ServiceCharge] DEFAULT (0),
    [TotalAmount]   DECIMAL(18, 2) NOT NULL CONSTRAINT [DF_Invoices_TotalAmount] DEFAULT (0),
    [Status]        NVARCHAR(30)   NOT NULL CONSTRAINT [DF_Invoices_Status] DEFAULT (N'Chưa thanh toán'),
    [PaymentMethod] NVARCHAR(30)   NULL,
    [PaymentDate]   DATETIME       NULL,
    CONSTRAINT [PK_Invoices] PRIMARY KEY CLUSTERED ([InvoiceId] ASC),
    CONSTRAINT [UQ_Invoices_BookingId] UNIQUE ([BookingId]),
    CONSTRAINT [FK_Invoices_Bookings] FOREIGN KEY ([BookingId]) REFERENCES [dbo].[Bookings] ([BookingId]),
    CONSTRAINT [CK_Invoices_RoomCharge] CHECK ([RoomCharge] >= 0),
    CONSTRAINT [CK_Invoices_ServiceCharge] CHECK ([ServiceCharge] >= 0),
    CONSTRAINT [CK_Invoices_TotalAmount] CHECK ([TotalAmount] >= 0),
    CONSTRAINT [CK_Invoices_Status] CHECK ([Status] IN (N'Chưa thanh toán', N'Đã thanh toán')),
    CONSTRAINT [CK_Invoices_PaymentMethod] CHECK ([PaymentMethod] IS NULL OR [PaymentMethod] IN (N'Tiền mặt', N'Chuyển khoản'))
);
GO

-- Chỉ mục hỗ trợ truy vấn lọc phòng, kiểm tra trùng lịch và báo cáo doanh thu
CREATE NONCLUSTERED INDEX [IX_Rooms_RoomTypeId_Status]
    ON [dbo].[Rooms] ([RoomTypeId], [Status]);

CREATE NONCLUSTERED INDEX [IX_Bookings_RoomId_Status_Dates]
    ON [dbo].[Bookings] ([RoomId], [Status], [CheckInDate], [CheckOutDate]);

CREATE NONCLUSTERED INDEX [IX_Bookings_CustomerId]
    ON [dbo].[Bookings] ([CustomerId]);

CREATE NONCLUSTERED INDEX [IX_BookingServices_BookingId]
    ON [dbo].[BookingServices] ([BookingId]);

CREATE NONCLUSTERED INDEX [IX_Invoices_Status_PaymentDate]
    ON [dbo].[Invoices] ([Status], [PaymentDate]);
GO

/* =========================================================================
   3. NẠP DỮ LIỆU MẪU ĐỊNH SẴN (DETERMINISTIC DEMO SEED DATA)
   ========================================================================= */

-- 3.1. Tài khoản mẫu: admin / 123456 (Administrator) & staff / 123456 (Staff)
SET IDENTITY_INSERT [dbo].[Users] ON;
INSERT INTO [dbo].[Users] ([UserId], [Username], [Password], [FullName], [Role])
VALUES
    (1, N'admin', N'123456', N'Trần Minh Quân', N'Administrator'),
    (2, N'staff', N'123456', N'Lê Thu Ngân',   N'Staff');
SET IDENTITY_INSERT [dbo].[Users] OFF;
GO

-- 3.2. 4 Loại phòng chuẩn: Đơn (500.000), Đôi (700.000), Gia đình (950.000), VIP (1.200.000)
SET IDENTITY_INSERT [dbo].[RoomTypes] ON;
INSERT INTO [dbo].[RoomTypes] ([RoomTypeId], [TypeName], [PricePerDay])
VALUES
    (1, N'Đơn',      500000),
    (2, N'Đôi',      700000),
    (3, N'Gia đình', 950000),
    (4, N'VIP',      1200000);
SET IDENTITY_INSERT [dbo].[RoomTypes] OFF;
GO

-- 3.3. 12 Phòng chuẩn: 6 Trống (Phòng 101 là Đơn 500.000 đ sẵn sàng cho kịch bản 1.070.000 đ),
--      2 Đang ở (103, 203), 3 Đã đặt (104, 204, 304), 1 Bảo trì (302)
SET IDENTITY_INSERT [dbo].[Rooms] ON;
INSERT INTO [dbo].[Rooms] ([RoomId], [RoomNumber], [RoomTypeId], [Status])
VALUES
    (1,  N'101', 1, N'Trống'),
    (2,  N'102', 1, N'Trống'),
    (3,  N'103', 1, N'Đang ở'),
    (4,  N'104', 2, N'Đã đặt'),
    (5,  N'201', 2, N'Trống'),
    (6,  N'202', 2, N'Trống'),
    (7,  N'203', 2, N'Đang ở'),
    (8,  N'204', 3, N'Đã đặt'),
    (9,  N'301', 3, N'Trống'),
    (10, N'302', 3, N'Bảo trì'),
    (11, N'303', 4, N'Trống'),
    (12, N'304', 4, N'Đã đặt');
SET IDENTITY_INSERT [dbo].[Rooms] OFF;
GO

-- 3.4. 5 Khách hàng mẫu (bao gồm cả CCCD 12 số, CMND 9 số và Email NULL)
SET IDENTITY_INSERT [dbo].[Customers] ON;
INSERT INTO [dbo].[Customers] ([CustomerId], [FullName], [CitizenId], [Phone], [Email])
VALUES
    (1, N'Nguyễn Văn An',      '079201001234', '0901234567', N'an.nguyen@gmail.com'),
    (2, N'Trần Thị Bích Ngọc', '079202005678', '0912345678', N'ngoc.tran@gmail.com'),
    (3, N'Lê Hoàng Nam',       '079199009012', '0987654321', NULL),
    (4, N'Phạm Minh Châu',     '079203003456', '0938112233', N'chau.pham@outlook.com'),
    (5, N'Hoàng Quốc Bảo',     '025489123',    '0977556688', N'bao.hoang@aurahotel.vn');
SET IDENTITY_INSERT [dbo].[Customers] OFF;
GO

-- 3.5. 5 Dịch vụ khách sạn mẫu (2 × Nước suối Lavie 10.000 + 1 × Ăn sáng buffet 50.000 = 70.000 đ)
SET IDENTITY_INSERT [dbo].[Services] ON;
INSERT INTO [dbo].[Services] ([ServiceId], [ServiceName], [Price])
VALUES
    (1, N'Nước suối Lavie',  10000),
    (2, N'Ăn sáng buffet',   50000),
    (3, N'Giặt ủi nhanh',    40000),
    (4, N'Thuê xe máy ngày', 150000),
    (5, N'Đưa đón sân bay',  250000);
SET IDENTITY_INSERT [dbo].[Services] OFF;
GO

-- 3.6. Dữ liệu Bookings, BookingServices & Invoices đồng bộ tuyệt đối với trạng thái 12 phòng
DECLARE @Today DATETIME = CAST(CAST(GETDATE() AS DATE) AS DATETIME);

SET IDENTITY_INSERT [dbo].[Bookings] ON;
INSERT INTO [dbo].[Bookings] ([BookingId], [CustomerId], [RoomId], [CheckInDate], [CheckOutDate], [Status])
VALUES
    -- 2 lượt lưu trú đã hoàn tất trên phòng 102 & 201 (phòng hiện đã về Trống theo Settlement Invariant)
    (1, 1, 2,  DATEADD(HOUR, 14, DATEADD(DAY, -5, @Today)), DATEADD(HOUR, 12, DATEADD(DAY, -3, @Today)), N'Hoàn thành'),
    (2, 2, 5,  DATEADD(HOUR, 14, DATEADD(DAY, -3, @Today)), DATEADD(HOUR, 12, DATEADD(DAY, -1, @Today)), N'Hoàn thành'),
    -- 2 lượt đang lưu trú khớp với 2 phòng Đang ở (103, 203 — CheckInDate luôn ở quá khứ kể cả chạy sáng sớm)
    (3, 3, 3,  DATEADD(HOUR, 14, DATEADD(DAY, -1, @Today)), DATEADD(HOUR, 12, DATEADD(DAY,  1, @Today)), N'Đang ở'),
    (4, 4, 7,  DATEADD(HOUR, 14, DATEADD(DAY, -2, @Today)), DATEADD(HOUR, 12, DATEADD(DAY,  1, @Today)), N'Đang ở'),
    -- 3 lượt đặt trước khớp với 3 phòng Đã đặt (104, 204, 304)
    (5, 5, 4,  DATEADD(HOUR, 14, DATEADD(DAY,  1, @Today)), DATEADD(HOUR, 12, DATEADD(DAY,  3, @Today)), N'Đã đặt'),
    (6, 1, 8,  DATEADD(HOUR, 14, DATEADD(DAY,  2, @Today)), DATEADD(HOUR, 12, DATEADD(DAY,  4, @Today)), N'Đã đặt'),
    (7, 2, 12, DATEADD(HOUR, 14, DATEADD(DAY,  3, @Today)), DATEADD(HOUR, 12, DATEADD(DAY,  5, @Today)), N'Đã đặt');
SET IDENTITY_INSERT [dbo].[Bookings] OFF;

SET IDENTITY_INSERT [dbo].[BookingServices] ON;
INSERT INTO [dbo].[BookingServices] ([BookingServiceId], [BookingId], [ServiceId], [Quantity], [UnitPrice])
VALUES
    -- Dịch vụ đã sử dụng của Booking 1 (2 × 10.000 + 1 × 50.000 = 70.000 đ)
    (1, 1, 1, 2, 10000),
    (2, 1, 2, 1, 50000),
    -- Dịch vụ đã sử dụng của Booking 2 (2 × 40.000 = 80.000 đ)
    (3, 2, 3, 2, 40000),
    -- Dịch vụ đã sử dụng của phòng 103 đang lưu trú (Booking 3: 3 × 10.000 + 1 × 50.000 = 80.000 đ)
    (4, 3, 1, 3, 10000),
    (5, 3, 2, 1, 50000);
SET IDENTITY_INSERT [dbo].[BookingServices] OFF;

SET IDENTITY_INSERT [dbo].[Invoices] ON;
INSERT INTO [dbo].[Invoices]
    ([InvoiceId], [BookingId], [RoomCharge], [ServiceCharge], [TotalAmount], [Status], [PaymentMethod], [PaymentDate])
VALUES
    -- Hóa đơn đã thanh toán của Booking 1 (2 đêm × 500.000 + 70.000 = 1.070.000 đ)
    (1, 1, 1000000, 70000, 1070000, N'Đã thanh toán', N'Tiền mặt',     DATEADD(HOUR, 12, DATEADD(DAY, -3, @Today))),
    -- Hóa đơn đã thanh toán của Booking 2 (2 đêm × 700.000 + 80.000 = 1.480.000 đ)
    (2, 2, 1400000, 80000, 1480000, N'Đã thanh toán', N'Chuyển khoản', DATEADD(HOUR, 12, DATEADD(DAY, -1, @Today)));
SET IDENTITY_INSERT [dbo].[Invoices] OFF;
GO
