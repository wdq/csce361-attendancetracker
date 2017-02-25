IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Course_LocationRoomId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Course]'))
ALTER TABLE [dbo].[Course] DROP CONSTRAINT [FK_Course_LocationRoomId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND type in (N'U'))
DROP TABLE [dbo].[Course]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[Id] [uniqueidentifier] NOT NULL,
	[CourseCode] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CourseNumber] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CourseSection] [int] NOT NULL,
	[CourseName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ClassNumber] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsOnSunday] [bit] NOT NULL,
	[IsOnMonday] [bit] NOT NULL,
	[IsOnTuesday] [bit] NOT NULL,
	[IsOnWednesday] [bit] NOT NULL,
	[IsOnThursday] [bit] NOT NULL,
	[IsOnFriday] [bit] NOT NULL,
	[IsOnSaturday] [bit] NOT NULL,
	[StartTime] [int] NOT NULL,
	[StopTime] [int] NOT NULL,
	[Semester] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Year] [int] NOT NULL,
	[LocationRoomId] [uniqueidentifier] NOT NULL,
	[ActiveAttendanceCode] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_LocationRoomId] FOREIGN KEY([LocationRoomId])
REFERENCES [Room] ([Id])
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_LocationRoomId]
GO
