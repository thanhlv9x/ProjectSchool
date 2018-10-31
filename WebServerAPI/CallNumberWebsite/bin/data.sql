--INSERT INTO BOPHAN VALUES (N'Lĩnh vực tài nguyên và đất đai')
INSERT INTO BOPHAN VALUES (N'Lĩnh vực xây dựng')
INSERT INTO BOPHAN VALUES (N'Lĩnh vực tư pháp')
INSERT INTO BOPHAN VALUES (N'Lĩnh vực lao động, thương binh và xã hội')
INSERT INTO BOPHAN VALUES (N'Lĩnh vực cấp phép đăng ký xây dựng')


SELECT * FROM CANBO

insert into MUCDODANHGIA values (1, N'Rất hài lòng')
insert into MUCDODANHGIA values (2, N'Hài lòng')
insert into MUCDODANHGIA values (3, N'Bình thường')
insert into MUCDODANHGIA values (4, N'Không hài lòng')

update CANBO set HINHANH = N'E:\Luận văn\Source\WebServerAPI\WebServerAPI\resources\user1.png'

insert into CANBO values (N'Nguyễn Văn A', 1, N'E:\Luận văn\Source\WebServerAPI\WebServerAPI\resources\user1.png', N'nva-1-TNDD', '202CB962AC59075B964B07152D234B70')
insert into CANBO values (N'Nguyễn Thị B', 1, N'E:\Luận văn\Source\WebServerAPI\WebServerAPI\resources\user1.png', N'ntb-2-TNDD', '202CB962AC59075B964B07152D234B70')
insert into CANBO values (N'Lê Văn C', 2, N'E:\Luận văn\Source\WebServerAPI\WebServerAPI\resources\user1.png', N'lvc-3-XD', '202CB962AC59075B964B07152D234B70')
insert into CANBO values (N'Lê Thị D', 2, N'E:\Luận văn\Source\WebServerAPI\WebServerAPI\resources\user1.png', N'ltd-4-XD', '202CB962AC59075B964B07152D234B70')
insert into CANBO values (N'Trần Văn E', 3, N'E:\Luận văn\Source\WebServerAPI\WebServerAPI\resources\user1.png', N'tve-5-TP', '202CB962AC59075B964B07152D234B70')
insert into CANBO values (N'Trần Thị F', 3, N'E:\Luận văn\Source\WebServerAPI\WebServerAPI\resources\user1.png', N'ttf-6-TP', '202CB962AC59075B964B07152D234B70')
insert into CANBO values (N'Lý Văn G', 4, N'E:\Luận văn\Source\WebServerAPI\WebServerAPI\resources\user1.png', N'lvg-7-LDTBXA', '202CB962AC59075B964B07152D234B70')
insert into CANBO values (N'Lý Thị H', 4, N'E:\Luận văn\Source\WebServerAPI\WebServerAPI\resources\user1.png', N'lth-8-LDTBXA', '202CB962AC59075B964B07152D234B70')
insert into CANBO values (N'Huỳnh Văn I', 5, N'E:\Luận văn\Source\WebServerAPI\WebServerAPI\resources\user1.png', N'hvi-9-LDTBXA', '202CB962AC59075B964B07152D234B70')
insert into CANBO values (N'Huỳnh Thị J', 5, N'E:\Luận văn\Source\WebServerAPI\WebServerAPI\resources\user1.png', N'htj-10-LDTBXA', '202CB962AC59075B964B07152D234B70')
select * from SOTHUTU
select * from MUCDODANHGIA
select * from KETQUADANHGIA
select * from GOPY
insert into SOTHUTU values (1, 1, GETDATE())
insert into KETQUADANHGIA values (1, GETDATE(), 1)
insert into GOPY values (N'Thái độ nhân viên tốt', 4)

insert into SOTHUTU values (1, 3, GETDATE())
insert into KETQUADANHGIA values (4, GETDATE(), 2)
insert into GOPY values (N'Thái độ nhân viên không tốt', 5)

insert into SOTHUTU values (1, 5, GETDATE())
insert into KETQUADANHGIA values (3, GETDATE(), 3)
insert into GOPY values (N'Thái độ nhân viên bình thường', 6)

insert into SOTHUTU values (1, 7, GETDATE())
insert into KETQUADANHGIA values (4, GETDATE(), 4)
insert into GOPY values (N'Giải quyết thủ tục không hiệu quả', 7)

insert into SOTHUTU values (1, 9, GETDATE())
insert into KETQUADANHGIA values (2, GETDATE(), 5)
insert into GOPY values (N'Thời gian giải quyết nhanh', 8)

insert into SOTHUTU values (2, 1, GETDATE())
insert into KETQUADANHGIA values (1, GETDATE(), 6)
insert into GOPY values (N'Thái độ nhân viên tốt', 9)

insert into SOTHUTU values (3, 1, GETDATE())
insert into KETQUADANHGIA values (1, GETDATE(), 7)
insert into GOPY values (N'Thái độ nhân viên tốt', 11)

insert into SOTHUTU values (4, 1, GETDATE())
insert into KETQUADANHGIA values (2, GETDATE(), 8)
insert into GOPY values (N'Thái độ nhân viên tốt', 12)

insert into SOTHUTU values (1, 10, GETDATE())
insert into KETQUADANHGIA values (2, GETDATE(), 10)
insert into GOPY values (N'Thời gian giải quyết nhanh', 14)

insert into SOTHUTU values (2, 9, GETDATE())
insert into KETQUADANHGIA values (3, GETDATE(), 11)
insert into GOPY values (N'Giải quyết thủ tục hiệu quả', 15)

insert into SOTHUTU values (1, 6, '10-05-2018')
insert into KETQUADANHGIA values (4, '10-05-2018', 12)
insert into GOPY values (N'Thời gian giải quyết chậm', 16)

insert into SOTHUTU values (1, 5, '10-06-2018')
insert into KETQUADANHGIA values (1, '10-06-2018', 13)
insert into GOPY values (N'Giải quyết thủ tục hiệu quả', 17)


insert into KETQUADANHGIA values (4, GETDATE(), 9)
insert into GOPY values (N'Thời gian giải quyết chậm', 13)

select C.MACB, S.MASTT, K.MADG from SOTHUTU as S, KETQUADANHGIA as K, GOPY as G, CANBO as C where S.MASTT = K.MASTT AND K.MADG = G.MADG AND S.MACB = C.MACB

insert into CANBO values (N'Nguyễn Văn K', 1)
insert into CANBO values (N'Nguyễn Thị L', 1)
insert into CANBO values (N'Lê Văn M', 2)
insert into CANBO values (N'Lê Thị N', 2)
insert into CANBO values (N'Trần Văn O', 3)
insert into CANBO values (N'Trần Thị P', 3)
insert into CANBO values (N'Lý Văn Q', 4)
insert into CANBO values (N'Lý Thị R', 4)
insert into CANBO values (N'Huỳnh Văn S', 5)
insert into CANBO values (N'Huỳnh Thị T', 5)

insert into CANBO values (N'Nguyễn Văn X', 1)
insert into CANBO values (N'Nguyễn Thị V', 1)
insert into CANBO values (N'Lê Văn Y', 2)
insert into CANBO values (N'Lê Thị Z', 2)
insert into CANBO values (N'Trần Văn W', 3)
insert into CANBO values (N'Trần Thị AB', 3)
insert into CANBO values (N'Lý Văn BC', 4)
insert into CANBO values (N'Lý Thị CD', 4)
insert into CANBO values (N'Huỳnh Văn DE', 5)
insert into CANBO values (N'Huỳnh Thị EF', 5)

insert into SOTHUTU values (1, 1, GETDATE())
insert into SOTHUTU values (2, 2, GETDATE())
insert into SOTHUTU values (1, 3, GETDATE())
insert into SOTHUTU values (2, 4, GETDATE())
insert into SOTHUTU values (1, 5, GETDATE())
insert into SOTHUTU values (2, 6, GETDATE())
insert into SOTHUTU values (1, 7, GETDATE())
insert into SOTHUTU values (2, 8, GETDATE())
insert into SOTHUTU values (1, 9, GETDATE())
insert into SOTHUTU values (2, 10, GETDATE())

select * from SOTHUTU, CANBO where SOTHUTU.MACB = CANBO.MACB and MABP = 1

insert into SOTHUTU values (1, 6, '2018-07-01')
insert into SOTHUTU values (2, 6, '2018-07-02')
insert into SOTHUTU values (1, 7, '2018-07-03')
insert into SOTHUTU values (1, 8, '2018-09-04')
insert into SOTHUTU values (2, 8, '2018-09-05')
insert into SOTHUTU values (1, 9, '2018-09-06')
insert into SOTHUTU values (1, 9, '2018-05-01')
insert into SOTHUTU values (1, 10, '2018-05-16')
insert into SOTHUTU values (2, 10, '2018-05-16')

insert into SOTHUTU values (1, 6, '2019-01-01')
insert into SOTHUTU values (2, 6, '2017-02-02')
insert into SOTHUTU values (3, 7, '2015-03-03')
insert into SOTHUTU values (4, 8, '2013-04-04')
insert into SOTHUTU values (5, 8, '2011-11-05')
insert into SOTHUTU values (6, 9, '2020-12-06')
insert into SOTHUTU values (7, 9, '2018-06-01')
insert into SOTHUTU values (8, 10, '2014-11-22')
insert into SOTHUTU values (9, 10, '2002-02-28')

insert into KETQUADANHGIA values (1, N'Tốt', GETDATE(), 1)
insert into KETQUADANHGIA values (2, NULL, GETDATE(), 2)
insert into KETQUADANHGIA values (3, N'Giải quyết tạm ổn', GETDATE(), 3)
insert into KETQUADANHGIA values (4, N'Chờ đợi quá lâu', GETDATE(), 4)
insert into KETQUADANHGIA values (1, NULL, GETDATE(), 5)
insert into KETQUADANHGIA values (1, NULL, GETDATE(), 6)
insert into KETQUADANHGIA values (3, N'Cần trả lời cụ thể hơn', GETDATE(), 7)
insert into KETQUADANHGIA values (2, N'Năng động', GETDATE(), 8)
insert into KETQUADANHGIA values (1, NULL, GETDATE(), 9)
insert into KETQUADANHGIA values (4, N'Sai thông tin', GETDATE(), 10)

select * from KETQUADANHGIA

insert into KETQUADANHGIA values (1, N'Tốt', '2018-07-01', 6)
insert into KETQUADANHGIA values (2, NULL, '2018-07-02', 6)
insert into KETQUADANHGIA values (3, N'Giải quyết tạm ổn', '2018-07-03', 7)
insert into KETQUADANHGIA values (4, N'Chờ đợi quá lâu', '2018-09-04', 8)
insert into KETQUADANHGIA values (1, NULL, '2018-09-05', 8)
insert into KETQUADANHGIA values (1, NULL, '2018-09-06', 9)
insert into KETQUADANHGIA values (3, N'Cần trả lời cụ thể hơn', '2018-05-01', 9)
insert into KETQUADANHGIA values (2, N'Năng động', '2018-05-16', 10)
insert into KETQUADANHGIA values (1, NULL, '2018-03-01', 10)

insert into KETQUADANHGIA values (1, N'Tốt', '2019-01-01', 6)
insert into KETQUADANHGIA values (2, NULL, '2017-02-02', 6)
insert into KETQUADANHGIA values (3, N'Giải quyết tạm ổn', '2015-03-03', 7)
insert into KETQUADANHGIA values (4, N'Chờ đợi quá lâu', '2013-04-04', 8)
insert into KETQUADANHGIA values (1, NULL, '2011-11-05', 8)
insert into KETQUADANHGIA values (1, NULL, '2020-12-06', 9)
insert into KETQUADANHGIA values (3, N'Cần trả lời cụ thể hơn', '2018-06-01', 9)
insert into KETQUADANHGIA values (2, N'Năng động', '2014-11-22', 10)
insert into KETQUADANHGIA values (1, NULL, '2002-02-28', 10)

select * from KETQUADANHGIA as A, CANBO as B, BOPHAN as C where A.MACB = B.MACB AND B.MABP = C.MABP AND C.MABP = 4
SELECT * FROM KETQUADANHGIA AS A, CANBO AS B, SOTHUTU AS C WHERE A.MASTT = C.MASTT AND C.MACB = B.MACB AND B.MABP = 4
insert into TAIKHOAN values ('1', '202cb962ac59075b964b07152d234b70', 1)
select * from TAIKHOAN

select * from GOPY, KETQUADANHGIA, SOTHUTU where GOPY.MADG = KETQUADANHGIA.MADG AND KETQUADANHGIA.MASTT = SOTHUTU.MASTT

select * from SOTOIDA

insert into SOTOIDA values (10, 1, GETDATE())
insert into SOTOIDA values (2, 1, GETDATE())
insert into SOTOIDA values (3, 1, GETDATE())
insert into SOTOIDA values (4, 1, GETDATE())
insert into SOTOIDA values (23, 1, GETDATE())

insert into MAYDANHGIA values (2, '20-47-47-66-2F-31')
select * from MAYDANHGIA where MAC = '20-47-47-66-2F-31'
select * from TRANGTHAIDANGNHAP
select * from KETQUADANHGIA
select * from SOTOIDA
select * from SOTHUTU