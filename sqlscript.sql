USE [model]
GO
/****** Object:  Table [dbo].[Department_tbl]    Script Date: 17/03/2023 23:39:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department_tbl](
	[Dept_Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Dept_Name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Department_tbl] PRIMARY KEY CLUSTERED 
(
	[Dept_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Job_Details_tbl]    Script Date: 17/03/2023 23:39:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Job_Details_tbl](
	[Job_Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Job_Code] [nvarchar](50) NULL,
	[Job_Title] [nvarchar](50) NULL,
	[Job_Description] [nvarchar](max) NULL,
	[Job_Location_Id] [bigint] NULL,
	[Job_Dept_Id] [bigint] NULL,
	[Job_PostedDate] [datetime] NULL,
	[Job_ClosingDate] [datetime] NULL,
 CONSTRAINT [PK_Job_Details_tbl] PRIMARY KEY CLUSTERED 
(
	[Job_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Job_Location_tbl]    Script Date: 17/03/2023 23:39:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Job_Location_tbl](
	[Location_Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Location_Title] [nvarchar](50) NULL,
	[Location_City] [nvarchar](50) NULL,
	[Location_State] [nvarchar](50) NULL,
	[Location_Country] [nvarchar](50) NULL,
	[Location_Zipcode] [int] NULL,
 CONSTRAINT [PK_Job_Location_tbl] PRIMARY KEY CLUSTERED 
(
	[Location_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Department_tbl] ON 

INSERT [dbo].[Department_tbl] ([Dept_Id], [Dept_Name]) VALUES (2085, N'Software Development')
INSERT [dbo].[Department_tbl] ([Dept_Id], [Dept_Name]) VALUES (2086, N'Project Management')
INSERT [dbo].[Department_tbl] ([Dept_Id], [Dept_Name]) VALUES (2087, N'HR')
SET IDENTITY_INSERT [dbo].[Department_tbl] OFF
GO
SET IDENTITY_INSERT [dbo].[Job_Details_tbl] ON 

INSERT [dbo].[Job_Details_tbl] ([Job_Id], [Job_Code], [Job_Title], [Job_Description], [Job_Location_Id], [Job_Dept_Id], [Job_PostedDate], [Job_ClosingDate]) VALUES (1, N'JOB-01', N'Software Developer', N'Job Description here...', 10030, 2085, CAST(N'2023-03-17T23:18:58.070' AS DateTime), CAST(N'2023-03-30T18:43:31.877' AS DateTime))
INSERT [dbo].[Job_Details_tbl] ([Job_Id], [Job_Code], [Job_Title], [Job_Description], [Job_Location_Id], [Job_Dept_Id], [Job_PostedDate], [Job_ClosingDate]) VALUES (2, N'JOB-02', N'Project Manager', N'Job Descripton here', 10029, 2086, CAST(N'2023-03-17T23:26:14.960' AS DateTime), CAST(N'2023-03-29T18:43:31.877' AS DateTime))
SET IDENTITY_INSERT [dbo].[Job_Details_tbl] OFF
GO
SET IDENTITY_INSERT [dbo].[Job_Location_tbl] ON 

INSERT [dbo].[Job_Location_tbl] ([Location_Id], [Location_Title], [Location_City], [Location_State], [Location_Country], [Location_Zipcode]) VALUES (10029, N'India Office', N'Verna', N'Goa', N'India', 403710)
INSERT [dbo].[Job_Location_tbl] ([Location_Id], [Location_Title], [Location_City], [Location_State], [Location_Country], [Location_Zipcode]) VALUES (10030, N'US Head Office', N'Baltimore', N'MD', N'United States', 21202)
INSERT [dbo].[Job_Location_tbl] ([Location_Id], [Location_Title], [Location_City], [Location_State], [Location_Country], [Location_Zipcode]) VALUES (10031, N'US Office', N'California', N'MD', N'United States', 21202)
SET IDENTITY_INSERT [dbo].[Job_Location_tbl] OFF
GO
/****** Object:  StoredProcedure [dbo].[GET_JOBSDETAILS]    Script Date: 17/03/2023 23:39:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GET_JOBSDETAILS]
	-- Add the parameters for the stored procedure here
	@Id bigint=null,
	@query nvarchar=null,
	@LocationId bigint=null,
	@DepartmentId bigint=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	if(@Id is null)
	begin
	if(@locationID is not null and @departmentID is not null and @query is not null)
	begin 
	SELECT JB.Job_Id as id ,JB.Job_Code as code,JB.Job_Title as title ,LT.Location_Title as location,DT.Dept_Name as department,CONVERT(VARCHAR(50), JB.Job_PostedDate, 127)+'Z' as postedDate,CONVERT(VARCHAR(50), JB.Job_ClosingDate, 127)+'Z' as closingDate
FROM  [Job_Details_tbl] JB
INNER JOIN [Job_Location_tbl] LT ON LT.Location_Id = JB.Job_Location_Id
inner join [Department_tbl] DT ON DT.Dept_Id= JB.Job_Dept_Id
WHERE JB.Job_Location_Id = @locationID AND JB.Job_Dept_Id = @departmentID and  Job_Code LIKE '%' + @query + '%' or
Job_Title LIKE '%' + @query + '%' or Job_PostedDate LIKE '%' + @query + '%' or Job_ClosingDate LIKE '%' + @query + '%'
or LT.Location_Title LIKE '%' + @query + '%' or DT.Dept_Name LIKE '%' + @query + '%'

	end

	else if(@locationID is not null and @departmentID is not null and @query is  null )
	begin 
	SELECT JB.Job_Id as id ,JB.Job_Code as code,JB.Job_Title as title ,LT.Location_Title as location,DT.Dept_Name as department,CONVERT(VARCHAR(50), JB.Job_PostedDate, 127)+'Z' as postedDate,CONVERT(VARCHAR(50), JB.Job_ClosingDate, 127)+'Z' as closingDate FROM  [Job_Details_tbl] JB
INNER JOIN [Job_Location_tbl] LT ON LT.Location_Id = JB.Job_Location_Id
inner join [Department_tbl] DT ON DT.Dept_Id= JB.Job_Dept_Id
WHERE JB.Job_Location_Id = @locationID AND JB.Job_Dept_Id = @departmentID 
	end

	else if(@locationID is not null and @departmentID is  null and @query is not null)
	begin 
	SELECT JB.Job_Id as id ,JB.Job_Code as code,JB.Job_Title as title ,LT.Location_Title as location,DT.Dept_Name as department,CONVERT(VARCHAR(50), JB.Job_PostedDate, 127)+'Z' as postedDate,CONVERT(VARCHAR(50), JB.Job_ClosingDate, 127)+'Z' as closingDate FROM  [Job_Details_tbl] JB
INNER JOIN [Job_Location_tbl] LT ON LT.Location_Id = JB.Job_Location_Id
inner join [Department_tbl] DT ON DT.Dept_Id= JB.Job_Dept_Id
WHERE JB.Job_Location_Id = @locationID and  Job_Code LIKE '%' + @query + '%'  or
Job_Title LIKE '%' + @query + '%' or Job_PostedDate LIKE '%' + @query + '%' or Job_ClosingDate LIKE '%' + @query + '%'
or LT.Location_Title LIKE '%' + @query + '%' or DT.Dept_Name LIKE '%' + @query + '%'
	end

	else if(@locationID is  null and @departmentID is not null and @query is not null )
	begin 
	SELECT JB.Job_Id as id ,JB.Job_Code as code,JB.Job_Title as title ,LT.Location_Title as location,DT.Dept_Name as department,CONVERT(VARCHAR(50), JB.Job_PostedDate, 127)+'Z' as postedDate,CONVERT(VARCHAR(50), JB.Job_ClosingDate, 127)+'Z' as closingDate FROM  [Job_Details_tbl] JB
INNER JOIN [Job_Location_tbl] LT ON LT.Location_Id = JB.Job_Location_Id
inner join [Department_tbl] DT ON DT.Dept_Id= JB.Job_Dept_Id
WHERE JB.Job_Dept_Id = @departmentID and  Job_Code LIKE '%' + @query + '%'  or
Job_Title LIKE '%' + @query + '%' or Job_PostedDate LIKE '%' + @query + '%' or Job_ClosingDate LIKE '%' + @query + '%'
or LT.Location_Title LIKE '%' + @query + '%' or DT.Dept_Name LIKE '%' + @query + '%'
	end


	else if(@locationID is  null and @departmentID is  null and @query is not null )
	begin 
	SELECT JB.Job_Id as id ,JB.Job_Code as code,JB.Job_Title as title ,LT.Location_Title as location,DT.Dept_Name as department,CONVERT(VARCHAR(50), JB.Job_PostedDate, 127)+'Z' as postedDate,CONVERT(VARCHAR(50), JB.Job_ClosingDate, 127)+'Z' as closingDate FROM  [Job_Details_tbl] JB
INNER JOIN [Job_Location_tbl] LT ON LT.Location_Id = JB.Job_Location_Id
inner join [Department_tbl] DT ON DT.Dept_Id= JB.Job_Dept_Id
WHERE  Job_Code LIKE '%' + @query + '%'  or Job_Id LIKE '%' + @query + '%' or
Job_Title LIKE '%' + @query + '%' or Job_PostedDate LIKE '%' + @query + '%' or Job_ClosingDate LIKE '%' + @query + '%'
or LT.Location_Title LIKE '%' + @query + '%' or DT.Dept_Name LIKE '%' + @query + '%'
	end

	else if(@locationID is  null and @departmentID is not  null and @query is  null )
	begin 
	SELECT JB.Job_Id as id ,JB.Job_Code as code,JB.Job_Title as title ,LT.Location_Title as location,DT.Dept_Name as department,CONVERT(VARCHAR(50), JB.Job_PostedDate, 127)+'Z' as postedDate,CONVERT(VARCHAR(50), JB.Job_ClosingDate, 127)+'Z' as closingDate FROM  [Job_Details_tbl] JB
INNER JOIN [Job_Location_tbl] LT ON LT.Location_Id = JB.Job_Location_Id
inner join [Department_tbl] DT ON DT.Dept_Id= JB.Job_Dept_Id
WHERE JB.Job_Dept_Id = @departmentID 
	end

	else if(@locationID is not null and @departmentID is  null and @query is  null )
	begin 
	SELECT JB.Job_Id as id ,JB.Job_Code as code,JB.Job_Title as title ,LT.Location_Title as location,DT.Dept_Name as department,CONVERT(VARCHAR(50), JB.Job_PostedDate, 127)+'Z' as postedDate,CONVERT(VARCHAR(50), JB.Job_ClosingDate, 127)+'Z' as closingDate FROM  [Job_Details_tbl] JB
INNER JOIN [Job_Location_tbl] LT ON LT.Location_Id = JB.Job_Location_Id
inner join [Department_tbl] DT ON DT.Dept_Id= JB.Job_Dept_Id
WHERE JB.Job_Location_Id = @locationID 

	end
	else if(@locationID is  null and @departmentID is  null and @query is  null)
	begin 
	SELECT JB.Job_Id as id ,JB.Job_Code as code,JB.Job_Title as title ,LT.Location_Title as location,DT.Dept_Name as department, CONVERT(VARCHAR(50), JB.Job_PostedDate, 127)+'Z' as postedDate,CONVERT(VARCHAR(50), JB.Job_ClosingDate, 127)+'Z' as closingDate FROM  [Job_Details_tbl] JB
INNER JOIN [Job_Location_tbl] LT ON LT.Location_Id = JB.Job_Location_Id
inner join [Department_tbl] DT ON DT.Dept_Id= JB.Job_Dept_Id

	end
	end
	else
	begin
	SELECT JB.Job_Id as id,JB.Job_Code as code,JB.Job_Title as title,JB.Job_Description as description,LT.Location_Id as lid,LT.Location_Title as ltitle,LT.Location_City as city,LT.Location_State as state
	,LT.Location_Country as country,LT.Location_Zipcode as zip,DT.Dept_Id as did,DT.Dept_Name as dtitle, CONVERT(VARCHAR(50), JB.Job_PostedDate, 127)+'Z',  CONVERT(VARCHAR(50), JB.Job_ClosingDate, 127)+'Z'
FROM  [Job_Details_tbl] JB
INNER JOIN [Job_Location_tbl] LT ON LT.Location_Id = JB.Job_Location_Id
inner join [Department_tbl] DT ON DT.Dept_Id= JB.Job_Dept_Id
WHERE JB.Job_Id=@Id
	end
END
GO
/****** Object:  StoredProcedure [dbo].[INSERT_JOBSDETAILS]    Script Date: 17/03/2023 23:39:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_JOBSDETAILS]
	-- Add the parameters for the stored procedure here
	@ID bigint=null,
	@Title nvarchar(50),
	@Description nvarchar(max),
	@LocationID bigint,
	@DepartmentID bigint,
	@ClosingDate nvarchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	IF EXISTS (SELECT Job_Id FROM Job_Details_tbl WHERE Job_Id = @ID)
	BEGIN
	UPDATE Job_Details_tbl 
	SET Job_Title = @Title,
	Job_Description = @Description,
	Job_Location_Id = @LocationID,
	Job_Dept_Id = @DepartmentID,
	Job_ClosingDate = Cast(@ClosingDate as Datetime)
	WHERE Job_Id = @ID
	END

	ELSE
	BEGIN
	declare @code nvarchar(10)
	declare @count nvarchar(10)
	set @count=(select count(Job_Id) from Job_Details_tbl )+1
	if(Len(@count)=1)
	begin
	set @count='0'+ @count
	end
	set @code='JOB-'+@count
	INSERT INTO Job_Details_tbl(Job_Code,Job_Title,Job_Description,Job_Location_Id,Job_Dept_Id,Job_ClosingDate,Job_PostedDate)
			VALUES(@code,@Title,@Description,@LocationID,@DepartmentID,Cast(@ClosingDate as Datetime),getdate())

			select top 1(Job_ID) from Job_Details_tbl order by Job_Id desc
	END

END
GO
/****** Object:  StoredProcedure [dbo].[Insert_Update_Department]    Script Date: 17/03/2023 23:39:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[Insert_Update_Department]
	-- Add the parameters for the stored procedure here
	@ID bigint=null,
	@Title nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	IF EXISTS (SELECT Dept_Id FROM Department_tbl WHERE Dept_Id = @ID)
	BEGIN
	UPDATE Department_tbl 
	SET Dept_Name = @Title
	WHERE Dept_Id = @ID
	END

	ELSE
	BEGIN

	INSERT INTO Department_tbl values(@Title)
	END

END
GO
/****** Object:  StoredProcedure [dbo].[Insert_Update_LocationDetails]    Script Date: 17/03/2023 23:39:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[Insert_Update_LocationDetails]
	-- Add the parameters for the stored procedure here
	@ID bigint=null,
	@Title nvarchar(50),
	@City nvarchar(50),
	@State nvarchar(50),
	@Country nvarchar(50),
	@Zip int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	IF EXISTS (SELECT Location_Id FROM Job_Location_tbl WHERE Location_Id = @ID)
	BEGIN
	UPDATE Job_Location_tbl 
	SET Location_Title = @Title,
	Location_City = @City,
	Location_State = @State,
	Location_Country = @Country,
	Location_Zipcode = @Zip
	WHERE Location_Id = @ID
	END

	ELSE
	BEGIN

	INSERT INTO Job_Location_tbl values(@Title,@City,@State,@Country,@Zip)
	END

END
GO
