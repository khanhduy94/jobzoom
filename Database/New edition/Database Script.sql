USE [master]
GO
/****** Object:  Database [JobZoomEvolution]    Script Date: 10/14/2011 17:16:19 ******/
CREATE DATABASE [JobZoomEvolution] ON  PRIMARY 
( NAME = N'JobZoomEvolution', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\JobZoomEvolution.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'JobZoomEvolution_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\JobZoomEvolution_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [JobZoomEvolution] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [JobZoomEvolution].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [JobZoomEvolution] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [JobZoomEvolution] SET ANSI_NULLS OFF
GO
ALTER DATABASE [JobZoomEvolution] SET ANSI_PADDING OFF
GO
ALTER DATABASE [JobZoomEvolution] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [JobZoomEvolution] SET ARITHABORT OFF
GO
ALTER DATABASE [JobZoomEvolution] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [JobZoomEvolution] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [JobZoomEvolution] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [JobZoomEvolution] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [JobZoomEvolution] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [JobZoomEvolution] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [JobZoomEvolution] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [JobZoomEvolution] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [JobZoomEvolution] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [JobZoomEvolution] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [JobZoomEvolution] SET  DISABLE_BROKER
GO
ALTER DATABASE [JobZoomEvolution] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [JobZoomEvolution] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [JobZoomEvolution] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [JobZoomEvolution] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [JobZoomEvolution] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [JobZoomEvolution] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [JobZoomEvolution] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [JobZoomEvolution] SET  READ_WRITE
GO
ALTER DATABASE [JobZoomEvolution] SET RECOVERY SIMPLE
GO
ALTER DATABASE [JobZoomEvolution] SET  MULTI_USER
GO
ALTER DATABASE [JobZoomEvolution] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [JobZoomEvolution] SET DB_CHAINING OFF
GO
USE [JobZoomEvolution]
GO
/****** Object:  Table [dbo].[Tag]    Script Date: 10/14/2011 17:16:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Tag](
	[ID] [char](36) NOT NULL,
	[ObjectID] [char](36) NULL,
	[TagName] [nvarchar](50) NOT NULL,
	[ParentID] [char](36) NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IDX_ParentID] ON [dbo].[Tag] 
(
	[ParentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 10/14/2011 17:16:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[ID] [char](36) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[LastLoginDate] [datetime] NULL,
	[ModifiedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Job]    Script Date: 10/14/2011 17:16:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Job](
	[ID] [char](36) NOT NULL,
	[JobTitle] [nvarchar](50) NOT NULL,
	[CompanyName] [nvarchar](50) NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Company]    Script Date: 10/14/2011 17:16:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Company](
	[ID] [char](36) NOT NULL,
	[UserID] [char](36) NOT NULL,
	[Name] [nvarchar](10) NOT NULL,
	[IndustryID] [int] NOT NULL,
	[CompanyType] [nvarchar](50) NULL,
	[CompanySize] [nvarchar](50) NULL,
	[AddressLine1] [varchar](50) NOT NULL,
	[AddressLine2] [varchar](50) NULL,
	[CityID] [int] NOT NULL,
	[Phone] [nvarchar](50) NULL,
	[Fax] [nvarchar](50) NULL,
	[Founded] [datetime] NULL,
	[Website] [nvarchar](50) NULL,
	[Logo] [image] NULL,
	[ModifiedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TagDetail]    Script Date: 10/14/2011 17:16:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TagDetail](
	[ID] [char](36) NOT NULL,
	[TagID] [char](36) NOT NULL,
	[Location] [nvarchar](50) NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[Description] [nvarchar](100) NULL,
	[Attachment] [nvarchar](255) NULL,
 CONSTRAINT [PK_TagDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Profile]    Script Date: 10/14/2011 17:16:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Profile](
	[ID] [char](36) NOT NULL,
	[UserID] [char](36) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Gender] [nchar](1) NOT NULL,
	[Birthdate] [datetime] NOT NULL,
	[MaritalStatus] [nchar](1) NOT NULL,
	[Citizenship] [char](36) NOT NULL,
	[Picture] [image] NULL,
	[AddressLine1] [varchar](50) NOT NULL,
	[AddressLine2] [varchar](50) NULL,
	[CityID] [char](36) NOT NULL,
	[Phone] [nvarchar](50) NULL,
	[Mobile] [nvarchar](50) NULL,
	[AdditionalInfo] [nvarchar](100) NULL,
	[Website] [nvarchar](50) NULL,
	[ModifiedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[JobApproval]    Script Date: 10/14/2011 17:16:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[JobApproval](
	[ID] [char](36) NOT NULL,
	[ProfileID] [char](36) NOT NULL,
	[JobID] [char](36) NOT NULL,
	[IsApply] [bit] NULL,
	[IsApproved] [bit] NULL,
 CONSTRAINT [PK_JobApproval] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  ForeignKey [FK_Tag_ParentTag]    Script Date: 10/14/2011 17:16:21 ******/
ALTER TABLE [dbo].[Tag]  WITH NOCHECK ADD  CONSTRAINT [FK_Tag_ParentTag] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Tag] ([ID])
GO
ALTER TABLE [dbo].[Tag] NOCHECK CONSTRAINT [FK_Tag_ParentTag]
GO
/****** Object:  ForeignKey [FK_Company_User]    Script Date: 10/14/2011 17:16:21 ******/
ALTER TABLE [dbo].[Company]  WITH CHECK ADD  CONSTRAINT [FK_Company_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Company] CHECK CONSTRAINT [FK_Company_User]
GO
/****** Object:  ForeignKey [FK_TagDetail_Tag]    Script Date: 10/14/2011 17:16:21 ******/
ALTER TABLE [dbo].[TagDetail]  WITH CHECK ADD  CONSTRAINT [FK_TagDetail_Tag] FOREIGN KEY([TagID])
REFERENCES [dbo].[Tag] ([ID])
GO
ALTER TABLE [dbo].[TagDetail] CHECK CONSTRAINT [FK_TagDetail_Tag]
GO
/****** Object:  ForeignKey [FK_Profile_User]    Script Date: 10/14/2011 17:16:21 ******/
ALTER TABLE [dbo].[Profile]  WITH CHECK ADD  CONSTRAINT [FK_Profile_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Profile] CHECK CONSTRAINT [FK_Profile_User]
GO
/****** Object:  ForeignKey [FK_JobApproval_Job]    Script Date: 10/14/2011 17:16:21 ******/
ALTER TABLE [dbo].[JobApproval]  WITH CHECK ADD  CONSTRAINT [FK_JobApproval_Job] FOREIGN KEY([JobID])
REFERENCES [dbo].[Job] ([ID])
GO
ALTER TABLE [dbo].[JobApproval] CHECK CONSTRAINT [FK_JobApproval_Job]
GO
/****** Object:  ForeignKey [FK_JobApproval_Profile]    Script Date: 10/14/2011 17:16:21 ******/
ALTER TABLE [dbo].[JobApproval]  WITH CHECK ADD  CONSTRAINT [FK_JobApproval_Profile] FOREIGN KEY([ProfileID])
REFERENCES [dbo].[Profile] ([ID])
GO
ALTER TABLE [dbo].[JobApproval] CHECK CONSTRAINT [FK_JobApproval_Profile]
GO
