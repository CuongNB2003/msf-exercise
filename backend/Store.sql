USE [MsfDatabase]
GO
/****** Object:  StoredProcedure [dbo].[Log_GetAll]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CuongNB
-- Create date: 25/9/2024
-- Description:	Lấy tất cả dữ liệu Logs
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
            THROW 50004, 'Page và Limit phải lớn hơn hoặc bằng 1.', 1;

        -- Tính toán offset
        DECLARE @Offset INT = (@Page - 1) * @Limit;

        -- lấy dữ liệu log
        SELECT 
            COUNT(*) OVER() AS Total, -- Sử dụng COUNT() OVER() để tính tổng số bản ghi
            * 
        FROM Logs
        WHERE IsDeleted = 0  -- Chỉ lấy những bản ghi chưa bị xóa
        ORDER BY createdAt DESC
        OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;
    END TRY
    BEGIN CATCH
        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Log_GetById]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CuongNB
-- Create date: 7/10/2024
-- Description:	Lấy thông tin log theo id 
-- =============================================
CREATE PROCEDURE [dbo].[Log_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
        -- Kiểm tra xem Id có hợp lệ không
        IF @Id <= 0
        THROW 50001, 'Id phải lớn hơn 0.', 1; -- Sử dụng THROW để ném lỗi

        -- Truy vấn để lấy thông tin log theo Id, chỉ lấy bản ghi chưa bị xóa
        SELECT *
        FROM Logs
        WHERE Id = @Id AND IsDeleted = 0;  -- Thêm điều kiện kiểm tra IsDeleted

        -- Kiểm tra nếu không có bản ghi nào được tìm thấy
        IF @@ROWCOUNT = 0
			THROW 50002, 'Không tìm thấy Log.', 1;
    END TRY
    BEGIN CATCH
        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Menu_Create]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        CườngNB
-- Create date:   10/11/2024
-- Description:   Tạo hoặc phục hồi Menu
-- =============================================
CREATE PROCEDURE [dbo].[Menu_Create]
    @MenuJson NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    -- Bắt đầu transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @DisplayName NVARCHAR(100),
                @Url NVARCHAR(100),
                @IconName NVARCHAR(50),
                @IsDeleted BIT;

        -- Phân tích cú pháp JSON
        SELECT 
            @DisplayName = JSON_VALUE(@MenuJson, '$.DisplayName'),
            @Url = JSON_VALUE(@MenuJson, '$.Url'),
            @IconName = JSON_VALUE(@MenuJson, '$.IconName');

        -- Kiểm tra trạng thái của menu
        SELECT @IsDeleted = IsDeleted
        FROM Menu 
        WHERE LOWER(DisplayName) = LOWER(@DisplayName);

        -- Kiểm tra nếu menu đã tồn tại
        IF @IsDeleted = 0
        THROW 50000, 'Menu đã tồn tại, không thể thêm.', 1; -- Sử dụng THROW

        -- Phục hồi menu nếu đã bị xóa mềm
        IF @IsDeleted = 1
        BEGIN
            UPDATE Menu
            SET IsDeleted = 0,
				Url = @Url,
				IconName = @IconName,
                DeletedAt = NULL,      -- Đặt DeletedAt về NULL khi phục hồi
                CreatedAt = GETDATE()  -- Cập nhật thời gian cập nhật
            WHERE LOWER(DisplayName) = LOWER(@DisplayName);
            
            -- Commit transaction nếu phục hồi thành công
            COMMIT TRANSACTION;
            RETURN;  -- Kết thúc thủ tục sau khi phục hồi
        END

        -- Insert dữ liệu menu mới
        INSERT INTO Menu (DisplayName, Url, IconName)
        VALUES (@DisplayName, @Url, @IconName);

        -- Commit transaction nếu tất cả thành công
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        -- Hoàn tác transaction nếu có lỗi
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;

        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Menu_Delete]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        CườngNB
-- Create date:   11/10/2024
-- Description:   Xóa mềm Menu
-- =============================================
CREATE PROCEDURE [dbo].[Menu_Delete]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Bắt đầu transaction
        BEGIN TRANSACTION;

        -- Kiểm tra xem menu có tồn tại hay không
        IF NOT EXISTS (SELECT 1 FROM Menu WHERE Id = @Id AND IsDeleted = 0)
            THROW 50000, 'Menu không tồn tại hoặc đã bị xóa.', 1; -- Sử dụng THROW thay vì RAISERROR

        -- Cập nhật trường IsDeleted thành 1 (đã xóa mềm) và DeletedAt với thời gian hiện tại
        UPDATE Menu
        SET IsDeleted = 1,
            DeletedAt = GETDATE()
        WHERE Id = @Id;

		DELETE FROM Role_Menu WHERE MenuId = @Id;

        -- Commit transaction
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Hoàn tác transaction nếu có lỗi
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;

        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Menu_GetAll]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        CuongNB
-- Create date:   11/10/2024
-- Description:    Lấy tất cả dữ liệu Menu
-- =============================================
CREATE PROCEDURE [dbo].[Menu_GetAll]
    @Page INT,
    @Limit INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Kiểm tra @Page và @Limit phải tối thiểu là 1
        IF @Page < 1 OR @Limit < 1
            THROW 50004, 'Page và Limit phải lớn hơn hoặc bằng 1.', 1;

        -- Tính toán offset
        DECLARE @Offset INT = (@Page - 1) * @Limit;

        -- Lấy danh sách menu
        SELECT 
            COUNT(*) OVER() AS Total, -- Chỉ đếm bản ghi chưa bị xóa
			(SELECT COUNT(*) FROM Role_Menu rm WHERE rm.MenuId = m.Id) AS CountRole,
            m.Id,
            m.DisplayName,
            m.Url,
            m.IconName,
            m.Status,
            m.CreatedAt
        FROM 
            Menu m
        WHERE 
            m.IsDeleted = 0  -- Chỉ lấy bản ghi chưa bị xóa
        ORDER BY 
            m.CreatedAt DESC
        OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;

    END TRY
    BEGIN CATCH
        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Menu_GetById]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author:        Cường 
-- Create date:   10/11/2024 
-- Description:   Lấy thông tin menu theo id 
-- ============================================= 
CREATE PROCEDURE [dbo].[Menu_GetById] 
   @Id INT
AS
BEGIN
    -- Không tính tổng tăng tốc độ truy vấn 
    SET NOCOUNT ON;

    BEGIN TRY
        -- Kiểm tra xem Id có hợp lệ không
        IF @Id <= 0
            THROW 50001, 'Id phải lớn hơn 0.', 1;

        -- Truy vấn để lấy thông tin menu theo Id
        SELECT 
            m.Id,
            m.DisplayName,
            m.Url,
            m.IconName,
            m.Status,
            m.CreatedAt,
            ISNULL((SELECT COUNT(*) FROM Role_Menu WHERE MenuId = m.Id), 0) AS CountRole 
        FROM 
            Menu m
        WHERE 
            m.Id = @Id;

        -- Kiểm tra nếu không có bản ghi nào được tìm thấy
        IF @@ROWCOUNT = 0
            THROW 50002, 'Không tìm thấy Menu.', 1; -- Đảm bảo rằng số lỗi ở đây là khác

    END TRY
    BEGIN CATCH
        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Menu_Update]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author:        Cường 
-- Create date:   10/11/2024 
-- Description:   Cập nhật Menu 
-- ============================================= 
CREATE PROCEDURE [dbo].[Menu_Update] 
    @MenuJson NVARCHAR(MAX), 
    @Id INT 
AS 
BEGIN 
    SET NOCOUNT ON;

    BEGIN TRY
        -- Bắt đầu transaction
        BEGIN TRANSACTION;

        DECLARE @DisplayName NVARCHAR(100), 
                @Url NVARCHAR(100), 
                @IconName NVARCHAR(50), 
                @Status BIT;

        -- Phân tích cú pháp JSON
        SELECT 
            @DisplayName = JSON_VALUE(@MenuJson, '$.DisplayName'), 
            @Url = JSON_VALUE(@MenuJson, '$.Url'), 
            @IconName = JSON_VALUE(@MenuJson, '$.IconName'), 
            @Status = JSON_VALUE(@MenuJson, '$.Status');

        -- Kiểm tra Id có tồn tại không (và không bị xóa mềm)
        IF NOT EXISTS (SELECT 1 FROM Menu WHERE Id = @Id AND IsDeleted = 0)
            THROW 50000, 'Menu không tồn tại hoặc đã bị xóa.', 1;

        -- Kiểm tra DisplayName có trùng không (bỏ qua menu hiện tại)
        IF EXISTS (SELECT 1 FROM Menu WHERE LOWER(DisplayName) = LOWER(@DisplayName) AND Id <> @Id)
            THROW 50001, 'DisplayName đã tồn tại, không thể cập nhật.', 1;

        -- Cập nhật dữ liệu menu
        UPDATE Menu
        SET 
            DisplayName = @DisplayName, 
            Url = @Url, 
            IconName = @IconName, 
            Status = @Status, 
            UpdatedAt = GETDATE()
        WHERE Id = @Id;

        -- Commit transaction nếu tất cả thành công
        COMMIT TRANSACTION;

    END TRY 
    BEGIN CATCH 
        -- Hoàn tác transaction nếu có lỗi
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;

        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH 
END
GO
/****** Object:  StoredProcedure [dbo].[Permission_Create]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        CườngNB
-- Create date:   11/10/2024
-- Description:   Thêm Permission
-- =============================================
CREATE PROCEDURE [dbo].[Permission_Create]
    @PermissionJson NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    -- Bắt đầu transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @PermissionName NVARCHAR(50),
                @Description NVARCHAR(255),
                @Id INT;

        -- Phân tích cú pháp JSON
        SELECT 
            @PermissionName = LOWER(JSON_VALUE(@PermissionJson, '$.PermissionName')),
            @Description = JSON_VALUE(@PermissionJson, '$.Description');

        -- Kiểm tra PermissionName đã tồn tại và chưa bị xóa mềm
        IF EXISTS (SELECT 1 FROM Permissions WHERE LOWER(PermissionName) = @PermissionName AND IsDeleted = 0)
			THROW 50000, 'Permission đã tồn tại, không thể thêm.', 1; 

        -- Kiểm tra PermissionName đã tồn tại nhưng bị xóa mềm
        ELSE IF EXISTS (SELECT 1 FROM Permissions WHERE LOWER(PermissionName) = @PermissionName AND IsDeleted = 1)
        BEGIN
            -- Lấy Id của Permission đã bị xóa mềm
            SELECT @Id = Id 
            FROM Permissions 
            WHERE LOWER(PermissionName) = @PermissionName AND IsDeleted = 1;

            -- Cập nhật IsDeleted thành 0 (khôi phục)
            UPDATE Permissions
            SET IsDeleted = 0,
                DeletedAt = NULL,     
                CreatedAt = GETDATE(), 
                Description = @Description
            WHERE Id = @Id;

            -- Commit transaction nếu tất cả thành công
            COMMIT TRANSACTION;
            RETURN;
        END

        -- Insert dữ liệu permission nếu chưa tồn tại
        INSERT INTO Permissions (PermissionName, Description)
        VALUES (@PermissionName, @Description);

        -- Commit transaction nếu tất cả thành công
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        -- Hoàn tác transaction nếu có lỗi
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;

        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Permission_Delete]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        CườngNB
-- Create date:   11/10/2024
-- Description:   Xóa mềm permission
-- =============================================
CREATE PROCEDURE [dbo].[Permission_Delete]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Bắt đầu transaction
        BEGIN TRANSACTION;

        -- Kiểm tra xem permission có tồn tại hay không
        IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Id = @Id AND IsDeleted = 0)
			THROW 50000, 'Permission không tồn tại hoặc đã bị xóa.', 1;

        -- Xóa mềm permission
        UPDATE Permissions 
        SET IsDeleted = 1,
            DeletedAt = GETDATE()  -- Lưu thời gian xóa
        WHERE Id = @Id;

		DELETE FROM Role_Permission WHERE PermissionId = @Id;

        -- Commit transaction
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Hoàn tác transaction nếu có lỗi
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;

		-- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Permission_GetAll]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        CuongNB
-- Create date:   11/10/2024
-- Description:   Lấy tất cả dữ liệu Permissions
-- =============================================
CREATE PROCEDURE [dbo].[Permission_GetAll]
    @Page INT,
    @Limit INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Kiểm tra @Page và @Limit phải tối thiểu là 1
        IF @Page < 1 OR @Limit < 1
			THROW 50004, 'Page và Limit phải lớn hơn hoặc bằng 1.', 1;

        -- Tính toán offset
        DECLARE @Offset INT = (@Page - 1) * @Limit;

        -- Lấy danh sách quyền (permissions)
        SELECT 
            COUNT(*) OVER() AS Total,
            (SELECT COUNT(*) FROM Role_Permission rm WHERE rm.PermissionId = p.Id) AS CountRole,
            p.Id,
            p.PermissionName,
            p.Description,
            p.CreatedAt
        FROM 
            Permissions p
		WHERE 
			p.IsDeleted = 0
        ORDER BY 
            p.CreatedAt DESC
        OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;

    END TRY
    BEGIN CATCH
       -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Permission_GetById]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author:        CườngNB 
-- Create date:   11/10/2024 
-- Description:   Lấy thông tin permission theo ID 
-- ============================================= 
CREATE PROCEDURE [dbo].[Permission_GetById]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
		-- Kiểm tra xem Id có hợp lệ không
        IF @Id <= 0
			THROW 50001, 'Id phải lớn hơn 0.', 1;

        -- Lấy thông tin permission theo ID, chỉ lấy bản ghi chưa bị xóa        
        SELECT 
            p.Id,
            p.PermissionName,
            p.Description,
            p.CreatedAt,
            ISNULL((SELECT COUNT(*) FROM Role_Permission WHERE PermissionId = p.Id), 0) AS CountRole
        FROM 
            Permissions p
        WHERE 
            Id = @Id AND IsDeleted = 0;

        -- Kiểm tra nếu không có bản ghi nào được tìm thấy
        IF @@ROWCOUNT = 0
        	THROW 50002, 'Không tìm thấy Permission.', 1;
    END TRY
    BEGIN CATCH
		-- Sử dụng THROW để ném lại lỗi
		THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Permission_Update]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        CườngNB
-- Create date:   10/11/2024
-- Description:   Cập nhật quyền từ JSON input
-- =============================================
CREATE   PROCEDURE [dbo].[Permission_Update]
    @PermissionJson NVARCHAR(MAX),
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Bắt đầu transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @PermissionName NVARCHAR(50),
                @Description NVARCHAR(255);

        -- Phân tích cú pháp JSON
        SELECT 
            @PermissionName = JSON_VALUE(@PermissionJson, '$.PermissionName'),
            @Description = JSON_VALUE(@PermissionJson, '$.Description');

        -- Kiểm tra Id có tồn tại không (và không bị xóa mềm)
        IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Id = @Id AND IsDeleted = 0)
			THROW 50001, 'Permission không tồn tại hoặc đã bị xóa.', 1;

        -- Kiểm tra PermissionName có trùng không (bỏ qua permission hiện tại)
        IF EXISTS (SELECT 1 FROM Permissions WHERE LOWER(PermissionName) = LOWER(@PermissionName) AND Id <> @Id)
			THROW 50002, 'PermissionName đã tồn tại, không thể cập nhật.', 1;

        -- Cập nhật dữ liệu permission
        UPDATE Permissions
        SET 
            PermissionName = @PermissionName,
            Description = @Description,
			UpdatedAt = GETDATE()
        WHERE Id = @Id;

        -- Commit transaction nếu tất cả thành công
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        -- Hoàn tác transaction nếu có lỗi
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;

        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Role_Create]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Role_Create]
    @RoleJson NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    -- Bắt đầu transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @Name NVARCHAR(100),
                @Description NVARCHAR(250),
                @MenuIds NVARCHAR(MAX),
                @PermissionIds NVARCHAR(MAX),
                @RoleId INT;

        -- Phân tích cú pháp JSON
        SELECT 
            @Name = LOWER(JSON_VALUE(@RoleJson, '$.Name')),
            @Description = JSON_VALUE(@RoleJson, '$.Description'),
            @MenuIds = JSON_QUERY(@RoleJson, '$.MenuIds'),
            @PermissionIds = JSON_QUERY(@RoleJson, '$.PermissionIds');

        -- Kiểm tra tên quyền đã tồn tại
        IF EXISTS (SELECT 1 FROM Roles WHERE LOWER(Name) = @Name AND IsDeleted = 0)
            THROW 50000, 'Quyền đã tồn tại không thể thêm.', 1;

        -- Kiểm tra MenuIds và PermissionIds
        IF (@MenuIds IS NULL OR LEN(@MenuIds) = 0 OR NOT EXISTS (SELECT 1 FROM OPENJSON(@MenuIds))) OR
           (@PermissionIds IS NULL OR LEN(@PermissionIds) = 0 OR NOT EXISTS (SELECT 1 FROM OPENJSON(@PermissionIds)))
            THROW 50001, 'Bạn cần phải chọn ít nhất 1 Menu và 1 Permission.', 1;

        -- Kiểm tra xem có Menu hoặc Permission nào đã bị xóa mềm không
        IF EXISTS (
            SELECT 1 
            FROM OPENJSON(@MenuIds) AS M
            WHERE EXISTS (SELECT 1 FROM Menu WHERE Id = CAST(M.value AS INT) AND IsDeleted = 1)
        )
            THROW 50002, 'Có Menu đã bị xóa, không thể thêm vào Role.', 1;

        IF EXISTS (
            SELECT 1 
            FROM OPENJSON(@PermissionIds) AS P
            WHERE EXISTS (SELECT 1 FROM Permissions WHERE Id = CAST(P.value AS INT) AND IsDeleted = 1)
        )
            THROW 50003, 'Có Permission đã bị xóa, không thể thêm vào Role.', 1;

        -- Trường hợp vai trò đã bị xóa mềm, cho phép cập nhật lại
        IF EXISTS (SELECT 1 FROM Roles WHERE LOWER(Name) = @Name AND IsDeleted = 1)
        BEGIN
            -- Cập nhật lại vai trò đã bị xóa mềm
            UPDATE Roles
            SET IsDeleted = 0, 
                Description = @Description,
				DeletedAt = NULL,
                CreatedAt = GETDATE()
            WHERE LOWER(Name) = @Name;

            -- Lấy ID của role sau khi cập nhật
            SET @RoleId = (SELECT Id FROM Roles WHERE LOWER(Name) = @Name);

            -- Xóa bỏ các menu và permission cũ trước khi thêm mới
            DELETE FROM Role_Menu WHERE RoleId = @RoleId;
            DELETE FROM Role_Permission WHERE RoleId = @RoleId;
        END
        ELSE
        BEGIN
            -- Insert dữ liệu role mới
            INSERT INTO Roles (Name, Description)
            VALUES (@Name, @Description);

            -- Lấy ID của role mới tạo
            SET @RoleId = SCOPE_IDENTITY();
        END

        -- Thêm các menu vào bảng Role_Menu sử dụng OPENJSON
        INSERT INTO Role_Menu (RoleId, MenuId)
        SELECT @RoleId, CAST(value AS INT)
        FROM OPENJSON(@MenuIds) 
        WHERE ISNUMERIC(value) = 1 
        AND NOT EXISTS (SELECT 1 FROM Menu WHERE Id = CAST(value AS INT) AND IsDeleted = 1);

        -- Thêm các quyền vào bảng Role_Permission sử dụng OPENJSON
        INSERT INTO Role_Permission (RoleId, PermissionId)
        SELECT @RoleId, CAST(value AS INT)
        FROM OPENJSON(@PermissionIds) 
        WHERE ISNUMERIC(value) = 1 
        AND NOT EXISTS (SELECT 1 FROM Permissions WHERE Id = CAST(value AS INT) AND IsDeleted = 1);

        -- Commit transaction nếu tất cả thành công
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        -- Hoàn tác transaction nếu có lỗi
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;

        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Role_Delete]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        CườngNB
-- Create date:   11/10/2024
-- Description:   Xóa role
-- =============================================
CREATE PROCEDURE [dbo].[Role_Delete]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Bắt đầu transaction
        BEGIN TRANSACTION;

        -- Kiểm tra xem role có tồn tại không 
        IF NOT EXISTS (SELECT 1 FROM Roles WHERE Id = @Id AND IsDeleted = 0)
        THROW 50002, 'Role không tồn tại, không thể xóa.', 1;

        -- Cập nhật trường IsDeleted thành 1 (đã xóa mềm) và DeletedAt với thời gian hiện tại
        UPDATE Roles
        SET IsDeleted = 1,
            DeletedAt = GETDATE()
        WHERE Id = @Id;

		DELETE FROM Role_Menu WHERE RoleId = @Id;
		DELETE FROM Role_Permission WHERE RoleId = @Id;

        -- Commit transaction
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        -- Hoàn tác transaction nếu có lỗi
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;

        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Role_GetAll]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        CườngNB
-- Create date:   11/10/2024
-- Description:   Lấy tất cả dữ liệu Role
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
            THROW 50004, 'Page và Limit phải lớn hơn hoặc bằng 1.', 1;

        -- Tính toán offset
        DECLARE @Offset INT = (@Page - 1) * @Limit;

        -- Tạo một bảng tạm chứa các RoleId dựa trên phân trang
        DECLARE @RoleIds TABLE (Id INT);

        -- Lấy danh sách quyền với phân trang
        INSERT INTO @RoleIds (Id)
        SELECT r.Id
        FROM Roles r
        WHERE r.IsDeleted = 0
		ORDER BY r.CreatedAt DESC
        OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;

        -- Lấy danh sách quyền và tổng số lượng người dùng trong từng quyền
        SELECT 
            COUNT(*) OVER() AS Total,
            r.Id,
            r.Name,
            (SELECT COUNT(*) FROM User_Role ur WHERE ur.RoleId = r.Id) AS CountUser,
            r.CreatedAt
        FROM 
            Roles r
        WHERE r.Id IN (SELECT Id FROM @RoleIds)
		ORDER BY r.CreatedAt DESC;

        -- Lấy danh sách menus cho mỗi role
        SELECT 
            rm.RoleId,
            m.Id AS MenuId,
            m.DisplayName,
            m.Url,
            m.IconName,
            m.Status
        FROM 
            Role_Menu rm
        JOIN 
            Menu m ON rm.MenuId = m.Id
        WHERE rm.RoleId IN (SELECT Id FROM @RoleIds)
          AND m.IsDeleted = 0;

        -- Lấy danh sách permissions cho mỗi role
        SELECT 
            rp.RoleId,
            p.Id AS PermissionId,
            p.PermissionName,
            p.Description
        FROM 
            Role_Permission rp
        JOIN 
            Permissions p ON rp.PermissionId = p.Id
        WHERE rp.RoleId IN (SELECT Id FROM @RoleIds)
          AND p.IsDeleted = 0;

    END TRY
    BEGIN CATCH
        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[Role_GetById]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author:        CườngNB 
-- Create date:   11/10/2024 
-- Description:   Lấy thông tin role và liên kết menu, permission theo ID 
-- ============================================= 
CREATE PROCEDURE [dbo].[Role_GetById]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Kiểm tra xem role có tồn tại và chưa bị xóa
        IF NOT EXISTS (SELECT 1 FROM Roles WHERE Id = @Id AND IsDeleted = 0)
            THROW 50001, 'Role không tồn tại hoặc đã bị xóa.', 1;

        -- Lấy thông tin role
        SELECT 
            r.Id,
            r.Name,
            (SELECT COUNT(*) FROM User_Role ur WHERE ur.RoleId = r.Id) AS CountUser,
            r.CreatedAt
        FROM 
            Roles r
        WHERE 
            r.Id = @Id AND r.IsDeleted = 0;

        -- Lấy danh sách menus của role
        SELECT 
            m.Id,
            m.DisplayName,
            m.Url,
            m.IconName,
            m.Status
            --m.CreatedAt,
            --COUNT(rm.RoleId) AS CountRole  -- Trực tiếp tính tổng số role sử dụng menu này
        FROM 
            Role_Menu rm
        JOIN 
            Menu m ON rm.MenuId = m.Id
        WHERE 
            rm.RoleId = @Id AND m.IsDeleted = 0  -- Kiểm tra IsDeleted trong cả 2 bảng
        GROUP BY 
            m.Id, m.DisplayName, m.Url, m.IconName, m.Status, m.CreatedAt;

        -- Lấy danh sách permissions của role
        SELECT 
            p.Id,
            p.PermissionName,
            p.Description
            --p.CreatedAt,
            --COUNT(rp.RoleId) AS CountRole  -- Trực tiếp tính tổng số role sử dụng permission này
        FROM 
            Role_Permission rp
        JOIN 
            Permissions p ON rp.PermissionId = p.Id
        WHERE 
            rp.RoleId = @Id AND p.IsDeleted = 0  -- Kiểm tra IsDeleted trong cả 2 bảng
        GROUP BY 
            p.Id, p.PermissionName, p.Description, p.CreatedAt;

    END TRY
    BEGIN CATCH
        -- Sử dụng THROW để ném lại lỗi khi xảy ra trong TRY
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Role_Update]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        CườngNB
-- Create date:   11/10/2024
-- Description:   Sửa role
-- =============================================
CREATE PROCEDURE [dbo].[Role_Update]
    @RoleJson NVARCHAR(MAX),
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Bắt đầu transaction
        BEGIN TRANSACTION;

        DECLARE @Name NVARCHAR(100),
                @Description NVARCHAR(250),
                @MenuIds NVARCHAR(MAX),
                @PermissionIds NVARCHAR(MAX);

        -- Phân tích cú pháp JSON
        SELECT 
            @Name = JSON_VALUE(@RoleJson, '$.Name'),
            @Description = JSON_VALUE(@RoleJson, '$.Description'),
            @MenuIds = JSON_QUERY(@RoleJson, '$.MenuIds'),
            @PermissionIds = JSON_QUERY(@RoleJson, '$.PermissionIds');

        -- Kiểm tra xem role có tồn tại và không bị xóa mềm
        IF NOT EXISTS (SELECT 1 FROM Roles WHERE Id = @Id AND IsDeleted = 0)
            THROW 50000, 'Quyền không tồn tại hoặc đã bị xóa.', 1;

        -- Kiểm tra tên quyền mới có trùng không và không bị xóa mềm
        IF EXISTS (SELECT 1 FROM Roles WHERE LOWER(Name) = LOWER(@Name) AND Id <> @Id AND IsDeleted = 0)
            THROW 50001, 'Tên quyền đã tồn tại.', 1;

		-- Kiểm tra MenuIds và PermissionIds
        IF (@MenuIds IS NULL OR LEN(@MenuIds) = 0 OR NOT EXISTS (SELECT 1 FROM OPENJSON(@MenuIds))) OR
           (@PermissionIds IS NULL OR LEN(@PermissionIds) = 0 OR NOT EXISTS (SELECT 1 FROM OPENJSON(@PermissionIds)))
            THROW 50001, 'Bạn cần phải chọn ít nhất 1 Menu và 1 Permission.', 1;

		
        -- Kiểm tra xem có Menu hoặc Permission nào đã bị xóa mềm không
        IF EXISTS (
            SELECT 1 
            FROM OPENJSON(@MenuIds) AS M
            WHERE EXISTS (SELECT 1 FROM Menu WHERE Id = CAST(M.value AS INT) AND IsDeleted = 1)
        )
            THROW 50002, 'Có Menu đã bị xóa, không thể thêm vào vai trò.', 1;

        IF EXISTS (
            SELECT 1 
            FROM OPENJSON(@PermissionIds) AS P
            WHERE EXISTS (SELECT 1 FROM Permissions WHERE Id = CAST(P.value AS INT) AND IsDeleted = 1)
        )
            THROW 50003, 'Có Permission đã bị xóa, không thể thêm vào vai trò.', 1;

        -- Cập nhật dữ liệu role
        UPDATE Roles
        SET Name = @Name, Description = @Description, UpdatedAt = GETDATE()
        WHERE Id = @Id;

        -- Cập nhật Role_Menu và Role_Permission
        DELETE FROM Role_Menu WHERE RoleId = @Id;
        INSERT INTO Role_Menu (RoleId, MenuId)
        SELECT @Id, MenuId
        FROM OPENJSON(@MenuIds) WITH (MenuId INT '$');

        DELETE FROM Role_Permission WHERE RoleId = @Id;
        INSERT INTO Role_Permission (RoleId, PermissionId)
        SELECT @Id, PermissionId
        FROM OPENJSON(@PermissionIds) WITH (PermissionId INT '$');

        -- Commit transaction
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        -- Hoàn tác transaction nếu có lỗi
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;

        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Token_DeleteByUserId]    Script Date: 10/14/2024 11:56:08 AM ******/
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
    @UserIdString NVARCHAR(50)  -- Tham số đầu vào dạng chuỗi
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Bắt đầu transaction
        BEGIN TRANSACTION;

        -- Chuyển đổi chuỗi thành số nguyên
        DECLARE @UserId INT = TRY_CAST(@UserIdString AS INT);

        -- Nếu không chuyển đổi được, ném lỗi ngay lập tức
        IF @UserId IS NULL
            THROW 50002, 'UserId không hợp lệ.', 1;

        -- Xóa các token liên quan đến UserId
        DELETE FROM Tokens WHERE UserId = @UserId;

        -- Kiểm tra nếu không có token nào bị xóa
        IF @@ROWCOUNT = 0
            THROW 50001, 'Không tìm thấy token cho người dùng này.', 1;

        -- Cam kết transaction
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        -- Nếu có lỗi, hoàn tác transaction
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Ném lại lỗi nếu có
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Token_GetByRefreshToken]    Script Date: 10/14/2024 11:56:08 AM ******/
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

        -- Nếu không tìm thấy token, ném lỗi
        IF @@ROWCOUNT = 0
        THROW 50001, 'Không tìm thấy Token.', 1;
    END TRY
    BEGIN CATCH
        -- Ném lại lỗi nếu có
        THROW;
    END CATCH
END


GO
/****** Object:  StoredProcedure [dbo].[Token_Save]    Script Date: 10/14/2024 11:56:08 AM ******/
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

	-- Bắt đầu transaction
    BEGIN TRANSACTION;

    BEGIN TRY
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
		THROW 50002, 'ExpirationDate không hợp lệ hoặc không thể chuyển đổi.', 1;
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

        -- Cam kết transaction
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Nếu có lỗi, hoàn tác transaction
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Ném lại lỗi nếu có
        THROW;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[User_Create]    Script Date: 10/14/2024 11:56:08 AM ******/
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

    -- Bắt đầu transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @Name NVARCHAR(100),
                @Email NVARCHAR(100),
                @Password NVARCHAR(100),
                @Avatar NVARCHAR(255),
                @Salt NVARCHAR(100),
                @RoleIds NVARCHAR(MAX),
                @UserId INT;

        -- Phân tích cú pháp JSON
        SELECT 
            @Name = JSON_VALUE(@UserJson, '$.Name'),
            @Email = JSON_VALUE(@UserJson, '$.Email'),
            @Password = JSON_VALUE(@UserJson, '$.Password'),
            @Avatar = JSON_VALUE(@UserJson, '$.Avatar'),
            @Salt = JSON_VALUE(@UserJson, '$.Salt'),
            @RoleIds = JSON_QUERY(@UserJson, '$.RoleIds'); -- Lấy RoleIds dưới dạng JSON

        -- Kiểm tra email đã tồn tại
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND IsDeleted = 0)
        THROW 50000, 'Email đã tồn tại.', 1;

        -- Kiểm tra danh sách RoleIds có tồn tại và hợp lệ (bao gồm kiểm tra rỗng)
        IF @RoleIds IS NULL OR LEN(@RoleIds) = 0 OR NOT EXISTS (SELECT 1 FROM OPENJSON(@RoleIds))
        THROW 50000, 'Bạn cần phải chọn ít nhất 1 Role.', 1;

        -- Kiểm tra xem tất cả các RoleIds có tồn tại trong bảng Roles và chưa bị xóa không
        IF EXISTS (
            SELECT value 
            FROM OPENJSON(@RoleIds)
            WHERE value NOT IN (SELECT Id FROM Roles WHERE IsDeleted = 0) -- chỉ chọn những Role chưa bị xóa
        )
        THROW 50000, 'Có Role đã bị xóa, không thể thêm vào User.', 1;

        -- Kiểm tra email có bị xóa hay không, nếu có thì sửa lại IsDeleted thành false
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND IsDeleted = 1)
        BEGIN
			-- Cập nhật lại vai trò đã bị xóa mềm
            UPDATE Users
            SET Name = @Name, Password = @Password, Avatar = @Avatar, Salt = @Salt, IsDeleted = 0, CreatedAt = GETDATE(), DeletedAt = NUll
            WHERE Email = @Email;

			-- Lấy ID của role sau khi cập nhật
            SET @UserId = (SELECT Id FROM Users WHERE Email = @Email);

            -- Xóa quyền cũ của người dùng trong User_Role
            DELETE FROM User_Role WHERE UserId = (SELECT Id FROM Users WHERE Email = @Email);
        END
        ELSE
        BEGIN
			-- Insert dữ liệu người dùng mới
			INSERT INTO Users (Name, Email, Password, Avatar, Salt)
			VALUES (@Name, @Email, @Password, @Avatar, @Salt);

			-- Lấy ID của người dùng mới tạo
			SET @UserId = SCOPE_IDENTITY();
		END

		-- Thêm các quyền vào bảng User_Role sử dụng OPENJSON
        INSERT INTO User_Role (UserId, RoleId)
        SELECT @UserId, CAST(value AS INT)
        FROM OPENJSON(@RoleIds) 
        WHERE ISNUMERIC(value) = 1 
        AND NOT EXISTS (SELECT 1 FROM Roles WHERE Id = CAST(value AS INT) AND IsDeleted = 1);

        -- Commit transaction nếu tất cả thành công
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        -- Nếu có lỗi, hoàn tác transaction
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Ném lại lỗi nếu có
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[User_Delete]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        CuongNB
-- Create date:   8/10/2024
-- Description:   Xóa user và các bản ghi liên quan
-- =============================================
CREATE PROCEDURE [dbo].[User_Delete]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Bắt đầu transaction
        BEGIN TRANSACTION;

        -- Kiểm tra xem role có tồn tại không 
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @Id AND IsDeleted = 0)
        THROW 50002, 'User không tồn tại, không thể xóa.', 1;

        -- Cập nhật trường IsDeleted thành 1 (đã xóa mềm) và DeletedAt với thời gian hiện tại
        UPDATE Users
        SET IsDeleted = 1,
            DeletedAt = GETDATE()
        WHERE Id = @Id;

		DELETE FROM User_Role WHERE UserId = @Id;

        -- Commit transaction
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Hoàn tác transaction nếu có lỗi
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;

        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[User_GetAll]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        CuongNB
-- Create date:   20/9/2024
-- Description:   Stored procedure to get paged users ( phân trang ) với vai trò ( 2 bảng )
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
            THROW 50004, 'Page và Limit phải lớn hơn hoặc bằng 1.', 1;

        -- Tính toán offset
        DECLARE @Offset INT = (@Page - 1) * @Limit;

        -- Tạo một bảng tạm chứa các RoleId dựa trên phân trang
        DECLARE @UserIds TABLE (Id INT);

		-- Lấy danh sách quyền với phân trang
        INSERT INTO @UserIds (Id)
        SELECT u.Id
        FROM Users u
        WHERE u.IsDeleted = 0
        ORDER BY u.CreatedAt DESC
        OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;

        -- Lấy danh sách người dùng
        SELECT 
            COUNT(*) OVER() AS Total,
            u.Id,
            u.Name,
            u.Email,
            u.CreatedAt,
            u.Avatar
        FROM 
            Users u
		WHERE u.Id IN (SELECT Id FROM @UserIds)
		ORDER BY u.CreatedAt DESC;

        -- Lấy danh sách vai trò của tất cả người dùng trong phạm vi phân trang
		SELECT 
			ur.UserId,
			r.Id,
			r.Name
		FROM 
			User_Role ur
		JOIN 
			Roles r ON ur.RoleId = r.Id
		WHERE ur.UserId IN (SELECT Id FROM @UserIds) -- Kiểm tra theo UserId, không phải RoleId
			AND r.IsDeleted = 0;

    END TRY
    BEGIN CATCH
        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[User_GetByEmail]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author:        CườngNB 
-- Create date:   8/10/2024 
-- Description:   Lấy người dùng theo email 
-- ============================================= 
ALTER PROCEDURE [dbo].[User_GetByEmail]
    @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId INT; -- Biến để lưu trữ Id của người dùng

    BEGIN TRY

        -- Kiểm tra nếu người dùng không tồn tại hoặc đã bị xóa
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND IsDeleted = 0)
            THROW 50000, 'Người dùng không tồn tại hoặc đã bị xóa.', 1;

        -- Gán giá trị Id của người dùng vào biến @UserId
        SELECT 
            @UserId = u.Id
        FROM 
            Users u
        WHERE 
            u.Email = @Email AND u.IsDeleted = 0;

        -- Trả về bảng 1: Thông tin người dùng
        SELECT 
            u.Id,
            u.Name,
            u.Email,
            u.CreatedAt,
            u.Avatar,
			u.Salt,
			u.Password
        FROM 
            Users u
        WHERE 
            u.Id = @UserId;

        -- Trả về bảng 2: Danh sách vai trò của người dùng dựa trên @UserId
        SELECT 
            r.Id,
            r.Name
        FROM 
            User_Role ur
        JOIN 
            Roles r ON ur.RoleId = r.Id
        WHERE 
            ur.UserId = @UserId AND r.IsDeleted = 0;

    END TRY
    BEGIN CATCH
        -- Xử lý lỗi
        THROW; -- Ném lại lỗi đã bắt được
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[User_GetById]    Script Date: 10/14/2024 11:56:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        CuongNB
-- Create date:   8/10/2024
-- Description:   Lấy thông tin người dùng và vai trò theo ID
-- =============================================
CREATE PROCEDURE [dbo].[User_GetById]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Kiểm tra nếu người dùng không tồn tại hoặc đã bị xóa
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @Id AND IsDeleted = 0)
			THROW 50000, 'Người dùng không tồn tại hoặc đã bị xóa.', 1;


		-- Trả về bảng 1: Thông tin người dùng
		SELECT 
			u.Id,
			u.Name,
			u.Email,
			u.CreatedAt,
			u.Avatar
		FROM 
			Users u
		WHERE 
			u.Id = @Id AND u.IsDeleted = 0;

		-- Trả về bảng 2: Danh sách vai trò của người dùng
		SELECT 
			r.Id,
			r.Name
		FROM 
			User_Role ur
		JOIN 
			Roles r ON ur.RoleId = r.Id
		WHERE 
			ur.UserId = @Id AND r.IsDeleted = 0;


    END TRY
    BEGIN CATCH
         -- Sử dụng THROW trong CATCH block để ném lỗi lại
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[User_Update]    Script Date: 10/14/2024 11:56:08 AM ******/
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
        -- Bắt đầu transaction
        BEGIN TRANSACTION;

        DECLARE @Name NVARCHAR(100),
                @Email NVARCHAR(100),
                @Avatar NVARCHAR(255),
                @RoleIds NVARCHAR(MAX)

		-- Phân tích cú pháp JSON
        SELECT 
            @Name = JSON_VALUE(@UserJson, '$.Name'),
            @Email = JSON_VALUE(@UserJson, '$.Email'),
            @Avatar = JSON_VALUE(@UserJson, '$.Avatar'),
			@RoleIds = JSON_QUERY(@UserJson, '$.RoleIds');

        -- Kiểm tra xem người dùng có tồn tại hay không
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @Id AND IsDeleted = 0)
			THROW 50000, 'Người dùng không tồn tại hoặc đã bị xóa.', 1;

        -- Kiểm tra email mới có trùng với email hiện tại không
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND Id <> @Id AND IsDeleted = 0)
		THROW 50001, 'Email đã tồn tại.', 1;

		-- Kiểm tra MenuIds và PermissionIds
        IF (@RoleIds IS NULL OR LEN(@RoleIds) = 0 OR NOT EXISTS (SELECT 1 FROM OPENJSON(@RoleIds)))
            THROW 50001, 'Bạn cần phải chọn ít nhất 1 Role.', 1;

		-- Kiểm tra xem có Role nào đã bị xóa mềm không
        IF EXISTS (
            SELECT 1 
            FROM OPENJSON(@RoleIds) AS R
            WHERE EXISTS (SELECT 1 FROM Roles WHERE Id = CAST(R.value AS INT) AND IsDeleted = 1)
        )
            THROW 50002, 'Có Menu đã bị xóa, không thể thêm vào vai trò.', 1;

        -- Cập nhật dữ liệu
        UPDATE Users
        SET Name = @Name, Email = @Email, Avatar = @Avatar, UpdatedAt = GETDATE()
        WHERE Id = @Id;

        -- Cập nhật Role_Menu và Role_Permission
        DELETE FROM User_Role WHERE UserId = @Id;
        INSERT INTO User_Role (UserId, RoleId)
        SELECT @Id, RoleId
        FROM OPENJSON(@RoleIds) WITH (RoleId INT '$');

        -- Commit transaction
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
       -- Hoàn tác transaction nếu có lỗi
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;

        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
