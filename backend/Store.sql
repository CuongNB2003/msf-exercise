USE [MsfDatabase]
GO
/****** Object:  StoredProcedure [dbo].[Log_GetAll]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CuongNB
-- Create date: 25/9/2024
-- Description:	Stored procedure to get paged logs
-- =============================================
CREATE PROCEDURE [dbo].[Log_GetAll] 
    @Page INT,
    @Limit INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Kiểm tra @Page và @Limit phải tối thiểu là 1
        IF @Page < 1 OR @Limit < 1
        BEGIN
            RAISERROR ('Page và Limit phải lớn hơn hoặc bằng 1.', 16, 50004);
            RETURN;
        END

        -- Tính toán offset
        DECLARE @Offset INT;
        SET @Offset = (@Page - 1) * @Limit;

        -- lấy dữ liệu log
        SELECT 
			(SELECT COUNT(*) 
			FROM Logs) AS TotalLog,
			* 
        FROM Logs
        ORDER BY createdAt DESC
        OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;
    END TRY
    BEGIN CATCH
        -- Xử lý lỗi
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Log_GetById]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CuongNB
-- Create date: 7/10/2024
-- Description:	Lấy log theo id
-- =============================================
CREATE PROCEDURE [dbo].[Log_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
        -- Kiểm tra xem Id có hợp lệ không
        IF @Id <= 0
        BEGIN
            RAISERROR ('Id phải lớn hơn 0.', 16, 50004);
            RETURN;
        END

        -- Truy vấn để lấy thông tin log theo Id
        SELECT *
        FROM Logs
        WHERE Id = @Id;

        -- Kiểm tra nếu không có bản ghi nào được tìm thấy
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR ('Không tìm thấy Log với Id = %d.', 16, 50004, @Id);
        END
    END TRY
    BEGIN CATCH
        -- Xử lý lỗi
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Role_Create]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CườngNB
-- Create date: 7/10/2024
-- Description:	tạo role
-- =============================================
CREATE PROCEDURE [dbo].[Role_Create]
    @Name NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Chuyển tên vai trò thành chữ thường
    SET @Name = LOWER(@Name);

    BEGIN TRY
        -- Kiểm tra name có trùng không 
        IF EXISTS (SELECT 1 FROM Roles WHERE LOWER(Name) = @Name)
        BEGIN
            -- Trùng thì trả về mã lỗi 
            RAISERROR ('Role đã tồn tại không thể thêm.', 16, 50001);
            RETURN;
        END

        -- Thêm vào db
        INSERT INTO Roles (Name)
        VALUES (@Name);

    END TRY
    BEGIN CATCH
        -- Bắt lỗi
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
            
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[Role_Delete]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CườngNB
-- Create date: 7/10/2024
-- Description:	Xóa role
-- =============================================
CREATE PROCEDURE [dbo].[Role_Delete]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Kiểm tra xem role có tồn tại không 
        IF NOT EXISTS (SELECT 1 FROM Roles WHERE Id = @Id)
        BEGIN
            -- Nếu role không tồn tại, trả về mã lỗi
            RAISERROR ('Role không tồn tại, không thể xóa.', 16, 50002);
            RETURN;
        END

        -- Kiểm tra xem có user nào liên quan đến role không 
        DECLARE @UserCount INT;
        -- SELECT @UserCount = COUNT(*) FROM Users WHERE RoleId = @Id;

        IF @UserCount > 0
        BEGIN
            -- Nếu có user liên quan, trả về mã lỗi
            RAISERROR ('Không thể xóa Role vì có %d User liên quan.', 16, 50003, @UserCount);
            RETURN;
        END

        -- Xóa role
        DELETE FROM Roles WHERE Id = @Id;

    END TRY
    BEGIN CATCH
        -- Bắt lỗi
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        -- Trả về lỗi bắt được
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Role_GetAll]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CuongNB
-- Create date: 20/9/2024
-- Description:	Lấy tất cả role
-- =============================================
CREATE PROCEDURE [dbo].[Role_GetAll] 
    @Page INT,
    @Limit INT
AS
BEGIN
    SET NOCOUNT ON;

	BEGIN TRY
        -- Kiểm tra @Page và @Limit phải tối thiểu là 1
        IF @Page < 1 OR @Limit < 1
        BEGIN
            RAISERROR ('Page và Limit phải lớn hơn hoặc bằng 1.', 16, 50004);
            RETURN;
        END

        -- Tính toán offset
        DECLARE @Offset INT;
        SET @Offset = (@Page - 1) * @Limit;

        -- lấy dữ liệu log
        SELECT 
			(SELECT COUNT(*) 
			FROM Roles) AS TotalRole,
			r.Id,
			r.Name,
			--(SELECT COUNT(*) FROM Users u WHERE u.RoleId = r.Id) AS CountUser,
			r.CreatedAt 
        FROM Roles r
		ORDER BY r.CreatedAt DESC
		OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;
    END TRY
    BEGIN CATCH
        -- Xử lý lỗi
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Role_GetById]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Cường
-- Create date: 10/7/2024
-- Description: Lấy thông tin role theo id
-- =============================================
CREATE PROCEDURE [dbo].[Role_GetById] 
	@Id INT
AS
BEGIN
	-- Không tính tổng tăng tốc độ truy vấn 
	SET NOCOUNT ON;

	BEGIN TRY
        -- Kiểm tra xem Id có hợp lệ không
        IF @Id <= 0
        BEGIN
            RAISERROR ('Id phải lớn hơn 0.', 16, 50004);
            RETURN;
        END

        -- Truy vấn để lấy thông tin log theo Id
        -- Lấy thông tin role và đếm số lượng người dùng
	SELECT 
        r.Id, 
        r.Name, 
       -- COUNT(u.Id) AS CountUser, 
        r.CreatedAt
    FROM 
        Roles r
    --LEFT JOIN 
        --Users u ON u.RoleId = r.Id
    WHERE 
        r.Id = @Id
    GROUP BY 
        r.Id, r.Name, r.CreatedAt

        -- Kiểm tra nếu không có bản ghi nào được tìm thấy
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR ('Không tìm thấy Role với Id = %d.', 16, 50004, @Id);
        END
    END TRY
    BEGIN CATCH
        -- Xử lý lỗi
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Role_Update]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CườngNB
-- Create date: 7/10/2024
-- Description:	Sửa role
-- =============================================
CREATE PROCEDURE [dbo].[Role_Update]
    @Id INT,
    @Name NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Chuyển tên vai trò thành chữ thường
    SET @Name = LOWER(@Name);

    BEGIN TRY
        -- Kiểm tra xem ID có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM Roles WHERE Id = @Id)
        BEGIN
            -- Nếu ID không tồn tại, trả về mã lỗi
            RAISERROR ('Role không tồn tại không thể sửa.', 16, 50002);
            RETURN;
        END

        -- Kiểm tra name có trùng không với ID khác
        IF EXISTS (SELECT 1 FROM Roles WHERE LOWER(Name) = @Name AND Id <> @Id)
        BEGIN
            -- Nếu trùng name, trả về mã lỗi
            RAISERROR ('Name đã tồn tại không thể sửa.', 16, 50001);
            RETURN;
        END

        -- Sửa role
        UPDATE Roles 
        SET Name = @Name, UpdatedAt = GETDATE() 
        WHERE Id = @Id;

    END TRY
    BEGIN CATCH
        -- Bắt lỗi
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        -- Trả về lỗi bắt được
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Token_DeleteByUserId]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
-- Author:		CườngNB
-- Create date: 7/10/2024
-- Description:	Xóa token theo user id (string -> int conversion)
-- ================================================
CREATE PROCEDURE [dbo].[Token_DeleteByUserId]
    @UserIdString NVARCHAR(50) -- Tham số đầu vào dạng chuỗi
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE @UserId INT;

        -- Chuyển đổi chuỗi thành số nguyên
        SET @UserId = TRY_CAST(@UserIdString AS INT);

        -- Nếu không chuyển đổi được, trả về lỗi
        IF @UserId IS NULL
        BEGIN
            RAISERROR('UserId không hợp lệ.', 16, 50002);
            RETURN;
        END

        -- Xóa các token liên quan đến UserId
        DELETE FROM Tokens WHERE UserId = @UserId;

        -- Kiểm tra nếu không có token nào bị xóa
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('Không tìm thấy token cho người dùng này.', 16, 50001);
            RETURN;
        END
    END TRY
    BEGIN CATCH
        -- Bắt lỗi và trả về thông tin lỗi chi tiết
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        -- Trả về lỗi bắt được
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Token_GetByRefreshToken]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CuongNB
-- Create date: 7/10/2024
-- Description:	Lấy token theo RefreshToken
-- =============================================
CREATE PROCEDURE [dbo].[Token_GetByRefreshToken]
    @RefreshToken NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Truy vấn token từ bảng Tokens dựa trên RefreshToken
        SELECT *
        FROM Tokens
        WHERE RefreshToken = @RefreshToken;

        -- Nếu không tìm thấy token, trả về lỗi
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('Không tìm thấy Token.', 16, 50001);
			RETURN;
        END
    END TRY
    BEGIN CATCH
        -- Bắt lỗi và trả về thông tin lỗi chi tiết
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        -- Trả về lỗi bắt được
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[Token_Save]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CuongNB
-- Create date: 24/9/2024
-- Description:	Sử lý thêm và sửa token 
-- =============================================
CREATE PROCEDURE [dbo].[Token_Save]
    @JsonInput NVARCHAR(MAX) -- Nhận tham số JSON
AS
BEGIN
    SET NOCOUNT ON;

    -- Biến để lưu trữ các giá trị sau khi phân tích JSON
    DECLARE @UserId INT;
    DECLARE @RefreshToken NVARCHAR(MAX);
    DECLARE @ExpirationDate DATETIME;

    -- Phân tích chuỗi JSON để lấy các giá trị
    SELECT 
        @UserId = JSON_VALUE(@JsonInput, '$.UserId'),
        @RefreshToken = JSON_VALUE(@JsonInput, '$.RefreshToken'),
        @ExpirationDate = TRY_CONVERT(DATETIME, JSON_VALUE(@JsonInput, '$.ExpirationDate'), 126);

    -- Kiểm tra xem @ExpirationDate có phải là NULL không
    IF @ExpirationDate IS NULL
    BEGIN
        RAISERROR('ExpirationDate không hợp lệ hoặc không thể chuyển đổi.', 16, 50002);
        RETURN;
    END

    -- Kiểm tra xem userId tồn tại không
    IF NOT EXISTS (SELECT 1 FROM Tokens WHERE UserId = @UserId)
    BEGIN
        -- Thêm mới nếu không tồn tại
        INSERT INTO Tokens (UserId, RefreshToken, ExpirationDate, CreatedAt)
        VALUES (@UserId, @RefreshToken, @ExpirationDate, GETDATE());
    END
    ELSE
    BEGIN
        -- Sửa nếu tồn tại
        UPDATE Tokens
        SET RefreshToken = @RefreshToken, ExpirationDate = @ExpirationDate, UpdatedAt = GETDATE()
        WHERE UserId = @UserId;
    END
END

GO
/****** Object:  StoredProcedure [dbo].[User_Create]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CuongNB
-- Create date: 26/9/2024
-- Description:	Hàm tạo user
-- =============================================
CREATE PROCEDURE [dbo].[User_Create]
    @UserJson NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE @Name NVARCHAR(100),
                @Email NVARCHAR(100),
                @Password NVARCHAR(100),
                @Avatar NVARCHAR(255),
                @Salt NVARCHAR(100);

        -- Phân tích cú pháp JSON
        SELECT 
            @Name = JSON_VALUE(@UserJson, '$.Name'),
            @Email = JSON_VALUE(@UserJson, '$.Email'),
            @Password = JSON_VALUE(@UserJson, '$.Password'),
            @Avatar = JSON_VALUE(@UserJson, '$.Avatar'),
            @Salt = JSON_VALUE(@UserJson, '$.Salt');

        -- Kiểm tra email đã tồn tại
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
        BEGIN
			RAISERROR ('Email đã tồn tại.', 16, 50000);
            RETURN;
        END

        -- Insert dữ liệu
        INSERT INTO Users (Name, Email, Password, Avatar, Salt)
        VALUES (@Name, @Email, @Password, @Avatar, @Salt);
    END TRY
    BEGIN CATCH
        -- Xử lý lỗi
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[User_Delete]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CuongNB
-- Create date: 8/10/2024
-- Description:	Xóa user
-- =============================================
CREATE PROCEDURE [dbo].[User_Delete]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Kiểm tra xem người dùng có tồn tại hay không
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @Id)
        BEGIN
            RAISERROR ('Người dùng không tồn tại.', 16, 1);
            RETURN;
        END

        -- Xóa người dùng
        DELETE FROM Users WHERE Id = @Id;
    END TRY
    BEGIN CATCH
        -- Xử lý lỗi
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[User_GetAll]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CuongNB
-- Create date: 20/9/2024
-- Description:	Stored procedure to get paged users ( phân trang )
-- =============================================
CREATE PROCEDURE [dbo].[User_GetAll] 
    @Page INT,
    @Limit INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Kiểm tra @Page và @Limit phải tối thiểu là 1
        IF @Page < 1 OR @Limit < 1
        BEGIN
            RAISERROR ('Page và Limit phải lớn hơn hoặc bằng 1.', 16, 50004);
            RETURN;
        END

        -- Tính toán offset
        DECLARE @Offset INT;
        SET @Offset = (@Page - 1) * @Limit;

        -- lấy dữ liệu log
        SELECT 
			(SELECT COUNT(*) 
			FROM Users) AS TotalUser,
			*
		FROM Users
		--LEFT JOIN Roles r ON u.RoleId = r.Id
		ORDER BY CreatedAt DESC
		OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;
    END TRY
    BEGIN CATCH
        -- Xử lý lỗi
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[User_GetByEmail]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CườngNB
-- Create date: 8/10/2024
-- Description:	Lấy người dùng theo email
-- =============================================
CREATE PROCEDURE [dbo].[User_GetByEmail]
    @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Kiểm tra email có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
        BEGIN
            RAISERROR ('Email không tồn tại.', 16, 1);
            RETURN;
        END

        -- Lấy thông tin người dùng
        SELECT * 
        FROM Users 
        WHERE Email = @Email;
    END TRY
    BEGIN CATCH
        -- Xử lý lỗi
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[User_GetById]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CuongNB
-- Create date: 8/10/2024
-- Description:	Lấy thông tin người dùng và vai trò theo ID
-- =============================================
CREATE PROCEDURE [dbo].[User_GetById]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Kiểm tra xem người dùng có tồn tại hay không
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @Id)
        BEGIN
            RAISERROR ('Người dùng không tồn tại.', 16, 1);
            RETURN;
        END

        -- Lấy thông tin người dùng
        SELECT * FROM Users WHERE Id = @Id;

    END TRY
    BEGIN CATCH
        -- Xử lý lỗi
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[User_Update]    Script Date: 10/10/2024 9:48:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CuongNB
-- Create date: 8/10/2024
-- Description:	Sửa user
-- =============================================
CREATE PROCEDURE [dbo].[User_Update]
    @UserJson NVARCHAR(MAX),
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE @Name NVARCHAR(100),
                @Email NVARCHAR(100),
                @Avatar NVARCHAR(255);

        -- Kiểm tra xem người dùng có tồn tại hay không
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @Id)
        BEGIN
            RAISERROR ('Người dùng không tồn tại.', 16, 1);
            RETURN;
        END

        -- Phân tích cú pháp JSON
        SELECT 
            @Name = JSON_VALUE(@UserJson, '$.Name'),
            @Email = JSON_VALUE(@UserJson, '$.Email'),
            @Avatar = JSON_VALUE(@UserJson, '$.Avatar');

        -- Kiểm tra email mới có trùng với email hiện tại không
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND Id <> @Id)
        BEGIN
            RAISERROR ('Email đã tồn tại.', 16, 1);
            RETURN;
        END

        -- Cập nhật dữ liệu
        UPDATE Users
        SET Name = @Name, Email = @Email, Avatar = @Avatar, UpdatedAt = GETDATE()
        WHERE Id = @Id;
    END TRY
    BEGIN CATCH
        -- Xử lý lỗi
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO
