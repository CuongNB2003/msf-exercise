USE [MsfDatabase]
GO
/****** Object:  StoredProcedure [dbo].[Get_LogMethodByMonth]    Script Date: 11/11/2024 9:08:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_LogMethodByMonth] 
    @InputDate DATE   -- Ngày mà người dùng nhập vào
AS
BEGIN
    -- Lấy ngày đầu tiên và ngày cuối cùng của tháng từ ngày người dùng nhập vào
    DECLARE @StartDate DATE = DATEFROMPARTS(YEAR(@InputDate), MONTH(@InputDate), 1);
    DECLARE @EndDate DATE = EOMONTH(@InputDate); -- EOMONTH trả về ngày cuối cùng của tháng

    -- CTE để lấy tất cả các ngày trong tháng đó
    WITH DateRange AS (
        SELECT @StartDate AS AccessDate
        UNION ALL
        SELECT DATEADD(DAY, 1, AccessDate)
        FROM DateRange
        WHERE AccessDate < @EndDate
    )
    
    -- Truy vấn số lượng truy cập của từng phương thức theo ngày
    SELECT 
        d.AccessDate,
        SUM(CASE WHEN l.Method = 'POST' THEN 1 ELSE 0 END) AS PostCount,
        SUM(CASE WHEN l.Method = 'PUT' THEN 1 ELSE 0 END) AS PutCount,
        SUM(CASE WHEN l.Method = 'GET' THEN 1 ELSE 0 END) AS GetCount,
        SUM(CASE WHEN l.Method = 'DELETE' THEN 1 ELSE 0 END) AS DeleteCount
    FROM DateRange d
    LEFT JOIN Logs l ON CAST(l.CreatedAt AS DATE) = d.AccessDate
    GROUP BY d.AccessDate
    ORDER BY d.AccessDate;
END;
GO
/****** Object:  StoredProcedure [dbo].[Get_LogMethodByYear]    Script Date: 11/11/2024 9:08:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CườngNB
-- Create date: 10/22/2024
-- Description:	Thống kê trạng thái nhật ký
-- =============================================
CREATE PROCEDURE [dbo].[Get_LogMethodByYear]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    -- Khai báo bảng để lưu trữ kết quả
    DECLARE @Result TABLE (YearMonth VARCHAR(7), PostCount INT, PutCount INT, GetCount INT, DeleteCount INT);

    -- Khai báo biến để lưu trữ tháng bắt đầu và tháng kết thúc
    DECLARE @CurrentDate DATE = @StartDate;
    
    -- Tạo bảng tạm với tất cả các tháng trong khoảng thời gian
    WHILE @CurrentDate <= @EndDate
    BEGIN
        INSERT INTO @Result (YearMonth, PostCount, PutCount, GetCount, DeleteCount)
        VALUES (
            FORMAT(@CurrentDate, 'yyyy-MM'),
            0, -- Khởi tạo PostCount
            0, -- Khởi tạo PutCount
            0, -- Khởi tạo GetCount
            0  -- Khởi tạo DeleteCount
        );
        
        -- Tiến đến tháng tiếp theo
        SET @CurrentDate = DATEADD(MONTH, 1, @CurrentDate);
    END

    -- Tính số lượng log theo tháng trong khoảng thời gian được chỉ định
    UPDATE @Result
    SET 
        PostCount = COALESCE((SELECT COUNT(*) FROM Logs WHERE Method = 'POST' AND FORMAT(CreatedAt, 'yyyy-MM') = YearMonth), 0),
        PutCount = COALESCE((SELECT COUNT(*) FROM Logs WHERE Method = 'PUT' AND FORMAT(CreatedAt, 'yyyy-MM') = YearMonth), 0),
        GetCount = COALESCE((SELECT COUNT(*) FROM Logs WHERE Method = 'GET' AND FORMAT(CreatedAt, 'yyyy-MM') = YearMonth), 0),
        DeleteCount = COALESCE((SELECT COUNT(*) FROM Logs WHERE Method = 'DELETE' AND FORMAT(CreatedAt, 'yyyy-MM') = YearMonth), 0)
    WHERE 
        YearMonth IN (SELECT FORMAT(CreatedAt, 'yyyy-MM') FROM Logs WHERE CreatedAt BETWEEN @StartDate AND @EndDate);

    -- Trả về kết quả
    SELECT * FROM @Result ORDER BY YearMonth;
END
GO
/****** Object:  StoredProcedure [dbo].[Get_RoleCountUser]    Script Date: 11/11/2024 9:08:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CườngNB
-- Create date: 10/22/2024
-- Description:	Thống kê xem role có bao nhiêu user sử dụng 
-- =============================================
CREATE PROCEDURE [dbo].[Get_RoleCountUser] 
AS
BEGIN
	SELECT 
        r.Name AS RoleName,
        COUNT(ur.UserId) AS TotalUsers
    FROM 
        Roles r
    LEFT JOIN 
        User_Role ur ON r.Id = ur.RoleId
    WHERE 
        r.IsDeleted = 0
    GROUP BY 
        r.Name
    ORDER BY 
        TotalUsers DESC;  -- Sắp xếp theo số lượng người dùng giảm dần

END
GO
/****** Object:  StoredProcedure [dbo].[Log_GetAll]    Script Date: 11/11/2024 9:08:22 AM ******/
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
            THROW 50004, '400/Page và Limit phải lớn hơn hoặc bằng 1.', 1;

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
/****** Object:  StoredProcedure [dbo].[Log_GetById]    Script Date: 11/11/2024 9:08:22 AM ******/
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
        -- Truy vấn để lấy thông tin log theo Id, chỉ lấy bản ghi chưa bị xóa
        SELECT *
        FROM Logs
        WHERE Id = @Id AND IsDeleted = 0;  -- Thêm điều kiện kiểm tra IsDeleted

        -- Kiểm tra nếu không có bản ghi nào được tìm thấy
        IF @@ROWCOUNT = 0
			THROW 50002, '404/Không tìm thấy Log.', 1;
    END TRY
    BEGIN CATCH
        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Menu_Create]    Script Date: 11/11/2024 9:08:22 AM ******/
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
        THROW 50000, '409/Menu đã tồn tại, không thể thêm.', 1; -- Sử dụng THROW

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
/****** Object:  StoredProcedure [dbo].[Menu_Delete]    Script Date: 11/11/2024 9:08:22 AM ******/
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
            THROW 50000, '404/Menu không tồn tại hoặc đã bị xóa.', 1; -- Sử dụng THROW thay vì RAISERROR

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
/****** Object:  StoredProcedure [dbo].[Menu_GetAll]    Script Date: 11/11/2024 9:08:22 AM ******/
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
            THROW 50004, '400/Page và Limit phải lớn hơn hoặc bằng 1.', 1;

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
/****** Object:  StoredProcedure [dbo].[Menu_GetById]    Script Date: 11/11/2024 9:08:22 AM ******/
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
            THROW 50002, '404/Không tìm thấy Menu.', 1; -- Đảm bảo rằng số lỗi ở đây là khác

    END TRY
    BEGIN CATCH
        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Menu_Update]    Script Date: 11/11/2024 9:08:22 AM ******/
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
            THROW 50000, '404/Không tìm thấy Menu.', 1;

        -- Kiểm tra DisplayName có trùng không (bỏ qua menu hiện tại)
        IF EXISTS (SELECT 1 FROM Menu WHERE LOWER(DisplayName) = LOWER(@DisplayName) AND Id <> @Id)
            THROW 50001, '409/Menu đã tồn tại.', 1;

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
/****** Object:  StoredProcedure [dbo].[Permission_Create]    Script Date: 11/11/2024 9:08:22 AM ******/
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
				@Name NVARCHAR(50),
				@GroupName NVARCHAR(50),
                @Id INT;

        -- Phân tích cú pháp JSON
        SELECT 
            @Name = JSON_VALUE(@PermissionJson, '$.Name'),
            @Description = JSON_VALUE(@PermissionJson, '$.Description'),
			@PermissionName = JSON_VALUE(@PermissionJson, '$.PermissionName'),
			@GroupName = JSON_VALUE(@PermissionJson, '$.GroupName');

        -- Kiểm tra PermissionName đã tồn tại và chưa bị xóa mềm
        IF EXISTS (SELECT 1 FROM Permissions WHERE LOWER(PermissionName) = LOWER(@PermissionName) AND IsDeleted = 0)
			THROW 50000, '409/Permission đã tồn tại, không thể thêm.', 1; 

        -- Kiểm tra PermissionName đã tồn tại nhưng bị xóa mềm
        ELSE IF EXISTS (SELECT 1 FROM Permissions WHERE LOWER(PermissionName) = LOWER(@PermissionName) AND IsDeleted = 1)
        BEGIN
            -- Lấy Id của Permission đã bị xóa mềm
            SELECT @Id = Id 
            FROM Permissions 
            WHERE LOWER(PermissionName) = LOWER(@PermissionName) AND IsDeleted = 1;

            -- Cập nhật IsDeleted thành 0 (khôi phục)
            UPDATE Permissions
            SET IsDeleted = 0,
                DeletedAt = NULL,     
                CreatedAt = GETDATE(), 
                Description = @Description,
				Name = @Name,
				GroupName = @GroupName
            WHERE Id = @Id;

            -- Commit transaction nếu tất cả thành công
            COMMIT TRANSACTION;
            RETURN;
        END

        -- Insert dữ liệu permission nếu chưa tồn tại
        INSERT INTO Permissions (PermissionName, Description, Name, GroupName)
        VALUES (@PermissionName, @Description, @Name, @GroupName);

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
/****** Object:  StoredProcedure [dbo].[Permission_Delete]    Script Date: 11/11/2024 9:08:22 AM ******/
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
			THROW 50000, '404/Permission không tồn tại hoặc đã bị xóa.', 1;

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
/****** Object:  StoredProcedure [dbo].[Permission_GetAll]    Script Date: 11/11/2024 9:08:22 AM ******/
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
			THROW 50004, '400/Page và Limit phải lớn hơn hoặc bằng 1.', 1;

        -- Tính toán offset
        DECLARE @Offset INT = (@Page - 1) * @Limit;

        -- Lấy danh sách quyền (permissions)
        SELECT 
            COUNT(*) OVER() AS Total,
			(SELECT COUNT(*) FROM Role_Permission rm WHERE rm.PermissionId = p.Id) AS CountRole,
            p.Id,
            p.PermissionName,
            p.Description,
			p.GroupName,
			p.Name,
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
/****** Object:  StoredProcedure [dbo].[Permission_GetById]    Script Date: 11/11/2024 9:08:22 AM ******/
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
			THROW 50001, '400/Id phải lớn hơn 0.', 1;

        -- Lấy thông tin permission theo ID, chỉ lấy bản ghi chưa bị xóa        
        SELECT 
            p.Id,
            p.PermissionName,
            p.Description,
            p.CreatedAt,
			p.GroupName,
			p.Name,
            ISNULL((SELECT COUNT(*) FROM Role_Permission WHERE PermissionId = p.Id), 0) AS CountRole
        FROM 
            Permissions p
        WHERE 
            Id = @Id AND IsDeleted = 0;

        -- Kiểm tra nếu không có bản ghi nào được tìm thấy
        IF @@ROWCOUNT = 0
        	THROW 50002, '404/Không tìm thấy Permission.', 1;
    END TRY
    BEGIN CATCH
		-- Sử dụng THROW để ném lại lỗi
		THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Permission_Update]    Script Date: 11/11/2024 9:08:22 AM ******/
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
                @Description NVARCHAR(255),
				@Name NVARCHAR(50),
				@GroupName NVARCHAR(50);

        -- Phân tích cú pháp JSON
        SELECT 
            @Name = JSON_VALUE(@PermissionJson, '$.Name'),
            @Description = JSON_VALUE(@PermissionJson, '$.Description'),
			@PermissionName = JSON_VALUE(@PermissionJson, '$.PermissionName'),
			@GroupName = JSON_VALUE(@PermissionJson, '$.GroupName');

        -- Kiểm tra Id có tồn tại không (và không bị xóa mềm)
        IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Id = @Id AND IsDeleted = 0)
			THROW 50001, '404/Không tìm thấy Permission.', 1;

        -- Kiểm tra PermissionName có trùng không (bỏ qua permission hiện tại)
        IF EXISTS (SELECT 1 FROM Permissions WHERE LOWER(PermissionName) = LOWER(@PermissionName) AND Id <> @Id)
			THROW 50002, '409/Permission đã tồn tại.', 1;

        -- Cập nhật dữ liệu permission
        UPDATE Permissions
        SET 
            PermissionName = @PermissionName,
            Description = @Description,
			Name = @Name,
			GroupName = @GroupName,
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
/****** Object:  StoredProcedure [dbo].[Role_CheckPermission]    Script Date: 11/11/2024 9:08:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CuongNB
-- Create date: 25/9/2024
-- Description:	Kiểm tra quyền của vai trò
-- =============================================
CREATE PROCEDURE [dbo].[Role_CheckPermission]
    @roleId INT,
    @permissionName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Kiểm tra quyền của vai trò, trả về 1 nếu có quyền, 0 nếu không có
         SELECT CASE 
                    WHEN EXISTS (
                        SELECT 1
                        FROM Roles r
                        INNER JOIN Role_Permission rp ON rp.RoleId = r.Id
                        INNER JOIN Permissions p ON rp.PermissionId = p.Id
                        WHERE p.PermissionName = @permissionName
                          AND r.Id = @roleId
                          AND r.IsDeleted = 0
                          AND p.IsDeleted = 0
                    ) THEN 1
                    ELSE 0
                END;
    END TRY
    BEGIN CATCH
        -- Bắt lỗi và sử dụng THROW để ném lỗi
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Role_Create]    Script Date: 11/11/2024 9:08:22 AM ******/
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
            THROW 50000, '409/Role đã tồn tại, không thể thêm.', 1;

        -- Kiểm tra MenuIds và PermissionIds
        IF (@MenuIds IS NULL OR LEN(@MenuIds) = 0 OR NOT EXISTS (SELECT 1 FROM OPENJSON(@MenuIds))) OR
           (@PermissionIds IS NULL OR LEN(@PermissionIds) = 0 OR NOT EXISTS (SELECT 1 FROM OPENJSON(@PermissionIds)))
            THROW 50001, '400/Bạn cần phải chọn ít nhất 1 Menu và 1 Permission.', 1;

        -- Kiểm tra xem có Menu hoặc Permission nào đã bị xóa mềm không
        IF EXISTS (
            SELECT 1 
            FROM OPENJSON(@MenuIds) AS M
            WHERE EXISTS (SELECT 1 FROM Menu WHERE Id = CAST(M.value AS INT) AND IsDeleted = 1)
        )
            THROW 50002, '409/Có Menu đã bị xóa, không thể thêm vào Role.', 1;

        IF EXISTS (
            SELECT 1 
            FROM OPENJSON(@PermissionIds) AS P
            WHERE EXISTS (SELECT 1 FROM Permissions WHERE Id = CAST(P.value AS INT) AND IsDeleted = 1)
        )
            THROW 50003, '409/Có Permission đã bị xóa, không thể thêm vào Role.', 1;

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
/****** Object:  StoredProcedure [dbo].[Role_Delete]    Script Date: 11/11/2024 9:08:22 AM ******/
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
        THROW 50002, '404/Role không tồn tại hoặc đã bị xóa.', 1;

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
/****** Object:  StoredProcedure [dbo].[Role_GetAll]    Script Date: 11/11/2024 9:08:22 AM ******/
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
            THROW 50004, '400/Page và Limit phải lớn hơn hoặc bằng 1.', 1;

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
        JOIN @RoleIds t ON t.Id = r.Id
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
			@RoleIds t ON t.Id = rm.RoleId
        JOIN 
            Menu m ON rm.MenuId = m.Id
        AND 
			m.IsDeleted = 0;

        -- Lấy danh sách permissions cho mỗi role
        SELECT 
            rp.RoleId,
            p.Id AS PermissionId,
            p.PermissionName,
            p.Description
        FROM 
            Role_Permission rp
		JOIN	
			@RoleIds t ON t.Id = rp.RoleId
		JOIN 
            Permissions p ON rp.PermissionId = p.Id
        AND 
			p.IsDeleted = 0;

    END TRY
    BEGIN CATCH
        -- Sử dụng THROW để ném lại lỗi
        THROW;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[Role_GetById]    Script Date: 11/11/2024 9:08:22 AM ******/
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
            THROW 50001, '404/Không tìm thấy Role.', 1;

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
/****** Object:  StoredProcedure [dbo].[Role_Update]    Script Date: 11/11/2024 9:08:22 AM ******/
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
            THROW 50000, '404/Không tìm thấy Role.', 1;

        -- Kiểm tra tên quyền mới có trùng không và không bị xóa mềm
        IF EXISTS (SELECT 1 FROM Roles WHERE LOWER(Name) = LOWER(@Name) AND Id <> @Id AND IsDeleted = 0)
            THROW 50001, '409/Role đã tồn tại.', 1;

		-- Kiểm tra MenuIds và PermissionIds
        IF (@MenuIds IS NULL OR LEN(@MenuIds) = 0 OR NOT EXISTS (SELECT 1 FROM OPENJSON(@MenuIds))) OR
           (@PermissionIds IS NULL OR LEN(@PermissionIds) = 0 OR NOT EXISTS (SELECT 1 FROM OPENJSON(@PermissionIds)))
            THROW 50001, '400/Bạn cần phải chọn ít nhất 1 Menu và 1 Permission.', 1;

		
        -- Kiểm tra xem có Menu hoặc Permission nào đã bị xóa mềm không
        IF EXISTS (
            SELECT 1 
            FROM OPENJSON(@MenuIds) AS M
            WHERE EXISTS (SELECT 1 FROM Menu WHERE Id = CAST(M.value AS INT) AND IsDeleted = 1)
        )
            THROW 50002, '409/Có Menu đã bị xóa, không thể thêm vào vai trò.', 1;

        IF EXISTS (
            SELECT 1 
            FROM OPENJSON(@PermissionIds) AS P
            WHERE EXISTS (SELECT 1 FROM Permissions WHERE Id = CAST(P.value AS INT) AND IsDeleted = 1)
        )
            THROW 50003, '409/Có Permission đã bị xóa, không thể thêm vào vai trò.', 1;

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
/****** Object:  StoredProcedure [dbo].[Token_DeleteByUserId]    Script Date: 11/11/2024 9:08:22 AM ******/
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
            THROW 50002, '400/UserId không hợp lệ.', 1;

        -- Xóa các token liên quan đến UserId
        DELETE FROM Tokens WHERE UserId = @UserId;

        -- Kiểm tra nếu không có token nào bị xóa
        IF @@ROWCOUNT = 0
            THROW 50001, '404/Không tìm thấy token cho người dùng này.', 1;

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
/****** Object:  StoredProcedure [dbo].[Token_GetByRefreshToken]    Script Date: 11/11/2024 9:08:22 AM ******/
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
        THROW 50001, '404/Không tìm thấy Token.', 1;
    END TRY
    BEGIN CATCH
        -- Ném lại lỗi nếu có
        THROW;
    END CATCH
END


GO
/****** Object:  StoredProcedure [dbo].[Token_Save]    Script Date: 11/11/2024 9:08:22 AM ******/
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
		THROW 50002, '400/ExpirationDate không hợp lệ hoặc không thể chuyển đổi.', 1;
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
/****** Object:  StoredProcedure [dbo].[User_Create]    Script Date: 11/11/2024 9:08:22 AM ******/
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

    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @Name NVARCHAR(100),
                @Email NVARCHAR(100),
                @Password NVARCHAR(100),
                @Avatar NVARCHAR(255),
                @RoleIds NVARCHAR(MAX),
                @UserId INT,
                @UserRoleId INT;

        -- Phân tích cú pháp JSON
        SELECT 
            @Name = JSON_VALUE(@UserJson, '$.Name'),
            @Email = JSON_VALUE(@UserJson, '$.Email'),
            @Password = JSON_VALUE(@UserJson, '$.Password'),
            @Avatar = JSON_VALUE(@UserJson, '$.Avatar'),
            @RoleIds = JSON_QUERY(@UserJson, '$.RoleIds'); -- Lấy RoleIds dưới dạng JSON

        -- Kiểm tra email đã tồn tại
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND IsDeleted = 0)
        THROW 50000, '409/Email đã tồn tại.', 1;

        -- Nếu @RoleIds không tồn tại, lấy RoleId của role có tên 'user', nếu không có thì tạo mới
        IF @RoleIds IS NULL OR LEN(@RoleIds) = 0 OR NOT EXISTS (SELECT 1 FROM OPENJSON(@RoleIds))
        BEGIN
            SELECT @UserRoleId = Id FROM Roles WHERE Name = 'user' AND IsDeleted = 0;

            -- Nếu role 'user' không tồn tại, tạo role này
            IF @UserRoleId IS NULL
            BEGIN
                INSERT INTO Roles (Name, IsDeleted) VALUES ('user', 0);
                SET @UserRoleId = SCOPE_IDENTITY();
            END

            -- Gán @RoleIds là JSON với Id của role 'user' mới tạo
            SET @RoleIds = CONCAT('[', @UserRoleId, ']');
        END

        -- Kiểm tra xem tất cả các RoleIds có tồn tại trong bảng Roles và chưa bị xóa không
        IF EXISTS (
            SELECT value 
            FROM OPENJSON(@RoleIds)
            WHERE value NOT IN (SELECT Id FROM Roles WHERE IsDeleted = 0) 
        )
        THROW 50000, '409/Có Role đã bị xóa, không thể thêm vào User.', 1;

        -- Kiểm tra email có bị xóa hay không, nếu có thì sửa lại IsDeleted thành false
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND IsDeleted = 1)
        BEGIN
            UPDATE Users
            SET Name = @Name, Password = @Password, Avatar = @Avatar, IsDeleted = 0, CreatedAt = GETDATE(), DeletedAt = NULL
            WHERE Email = @Email;

            SET @UserId = (SELECT Id FROM Users WHERE Email = @Email);

            DELETE FROM User_Role WHERE UserId = @UserId;
        END
        ELSE
        BEGIN
            INSERT INTO Users (Name, Email, Password, Avatar)
            VALUES (@Name, @Email, @Password, @Avatar);

            SET @UserId = SCOPE_IDENTITY();
        END

        -- Thêm các quyền vào bảng User_Role
        INSERT INTO User_Role (UserId, RoleId)
        SELECT @UserId, CAST(value AS INT)
        FROM OPENJSON(@RoleIds) 
        WHERE ISNUMERIC(value) = 1 
        AND NOT EXISTS (SELECT 1 FROM Roles WHERE Id = CAST(value AS INT) AND IsDeleted = 1);

        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[User_Delete]    Script Date: 11/11/2024 9:08:22 AM ******/
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
        THROW 50002, '404/User không tồn tại hoặc đã bị xóa.', 1;

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
/****** Object:  StoredProcedure [dbo].[User_GetAll]    Script Date: 11/11/2024 9:08:22 AM ******/
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
            THROW 50004, '400/Page và Limit phải lớn hơn hoặc bằng 1.', 1;

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
/****** Object:  StoredProcedure [dbo].[User_GetByEmail]    Script Date: 11/11/2024 9:08:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author:        CườngNB 
-- Create date:   8/10/2024 
-- Description:   Lấy người dùng theo email 
-- ============================================= 
CREATE PROCEDURE [dbo].[User_GetByEmail]
    @Email NVARCHAR(100),
	@PassWord NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId INT; -- Biến để lưu trữ Id của người dùng

    BEGIN TRY

        -- Kiểm tra nếu người dùng không tồn tại hoặc đã bị xóa
        -- IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND IsDeleted = 0)
            -- THROW 50000, '404/Email hoặc mật khẩu không đúng.', 1;

        -- Gán giá trị Id của người dùng vào biến @UserId
        SELECT 
            @UserId = u.Id
        FROM 
            Users u
        WHERE 
            u.Email = @Email AND u.IsDeleted = 0 AND u.Password = @PassWord;

        -- Trả về bảng 1: Thông tin người dùng
        SELECT 
            u.Id,
            u.Name,
            u.Email,
            u.CreatedAt,
            u.Avatar,
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
/****** Object:  StoredProcedure [dbo].[User_GetById]    Script Date: 11/11/2024 9:08:22 AM ******/
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
			THROW 50000, '404/Không tìm thấy User.', 1;


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
/****** Object:  StoredProcedure [dbo].[User_Update]    Script Date: 11/11/2024 9:08:22 AM ******/
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
			THROW 50000, '404/Không tìm thấy User.', 1;

        -- Kiểm tra email mới có trùng với email hiện tại không
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND Id <> @Id AND IsDeleted = 0)
		THROW 50001, '409/Email đã tồn tại.', 1;

		-- Kiểm tra MenuIds và PermissionIds
        IF (@RoleIds IS NULL OR LEN(@RoleIds) = 0 OR NOT EXISTS (SELECT 1 FROM OPENJSON(@RoleIds)))
            THROW 50001, '400/Bạn cần phải chọn ít nhất 1 Role.', 1;

		-- Kiểm tra xem có Role nào đã bị xóa mềm không
        IF EXISTS (
            SELECT 1 
            FROM OPENJSON(@RoleIds) AS R
            WHERE EXISTS (SELECT 1 FROM Roles WHERE Id = CAST(R.value AS INT) AND IsDeleted = 1)
        )
            THROW 50002, '409/Có Role đã bị xóa, không thể thêm vào vai trò.', 1;

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
