USE [AdminDemo]
GO
/****** Object:  Table [dbo].[CountryMaster]    Script Date: 28-03-2018 13:03:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CountryMaster](
	[CountryId] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CountryMaster] PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CurrencyMaster]    Script Date: 28-03-2018 13:03:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CurrencyMaster](
	[CurrencyId] [int] IDENTITY(1,1) NOT NULL,
	[CurrencyName] [varchar](225) NULL,
	[IsActive] [bit] NULL,
	[IsBase] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[CurrencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[CountryMaster] ON 

INSERT [dbo].[CountryMaster] ([CountryId], [CountryName], [IsActive]) VALUES (1, N'India', 1)
INSERT [dbo].[CountryMaster] ([CountryId], [CountryName], [IsActive]) VALUES (4, N'test', 0)
SET IDENTITY_INSERT [dbo].[CountryMaster] OFF
/****** Object:  StoredProcedure [dbo].[GetDataForGridWeb]    Script Date: 28-03-2018 13:03:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDataForGridWeb]
	@TableName NVARCHAR(MAX)=''
	,@ColumnsName NVARCHAR(MAX)='*'
	,@SortOrder NVARCHAR(50)='ASC'
	,@SortColumn NVARCHAR(50)='RowNumber'
	,@PageNumber INT=0
	,@RecordPerPage INT=10
	,@WhereClause NVARCHAR(MAX)=''

AS
BEGIN
	
	SET NOCOUNT ON;
	IF ISNULL(@ColumnsName,'')=''
		BEGIN
			SET @ColumnsName='*'
		END
	IF ISNULL(@SortOrder,'')=''
		BEGIN
			SET @SortOrder='ASC'
		END
	IF @PageNumber<0
		BEGIN
			SET @PageNumber=-1
		END
	ELSE
		BEGIN
			SET @PageNumber=(@PageNumber/@RecordPerPage)+1
		END
	DECLARE @StartNo INT=0
	DECLARE @EndNo INT=10
	SET @StartNo=(@PageNumber-1)*@RecordPerPage
	SET @EndNo=@PageNumber*@RecordPerPage
	IF ISNULL(@WhereClause,'')!=''
		BEGIN
			SET @WhereClause=' AND '+@WhereClause
		END
	DECLARE @RowClause NVARCHAR(100)='1=1'
	IF @PageNumber>0
		BEGIN
			SET @RowClause+=' AND RowNumber>'+CONVERT(NVARCHAR,@StartNo)+' AND RowNumber<='+CONVERT(NVARCHAR,@EndNo)
		END
	DECLARE @SQL NVARCHAR(MAX)=''
	DECLARE @TotalRows INT =0
	SET @SQL='SELECT @TotalRows=COUNT(1) FROM(SELECT '+@ColumnsName+' FROM '+@TableName+')T WHERE 1=1 '+@WhereClause
	--print @SQL
	EXECUTE sp_executesql @SQL,N'@TotalRows INT out',@TotalRows OUT
	SET @SQL='SELECT '+CONVERT(NVARCHAR,@TotalRows)+' as TotalRows,* FROM (
	SELECT Row_Number() over (order by '+@SortColumn+' '+@SortOrder+') as RowNumber,* FROM(
	SELECT '+@ColumnsName+' FROM '+@TableName+')TMP WHERE 1=1 '+@WhereClause+')T
	WHERE '+@RowClause
	PRINT @SQL
	EXEC(@SQL)
END

GO
