IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseAttendance_CourseId]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseAttendance]'))
ALTER TABLE [dbo].[CourseAttendance] DROP CONSTRAINT [FK_CourseAttendance_CourseId]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseAttendance_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseAttendance]'))
ALTER TABLE [dbo].[CourseAttendance] DROP CONSTRAINT [FK_CourseAttendance_UserId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CourseAttendance]') AND type in (N'U'))
DROP TABLE [dbo].[CourseAttendance]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseAttendance](
	[Id] [uniqueidentifier] NOT NULL,
	[CourseId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Attendance] [bit] NOT NULL,
 CONSTRAINT [PK_CourseAttendance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CourseAttendance]  WITH CHECK ADD  CONSTRAINT [FK_CourseAttendance_CourseId] FOREIGN KEY([CourseId])
REFERENCES [Course] ([Id])
GO
ALTER TABLE [dbo].[CourseAttendance] CHECK CONSTRAINT [FK_CourseAttendance_CourseId]
GO
ALTER TABLE [dbo].[CourseAttendance]  WITH CHECK ADD  CONSTRAINT [FK_CourseAttendance_UserId] FOREIGN KEY([UserId])
REFERENCES [User] ([Id])
GO
ALTER TABLE [dbo].[CourseAttendance] CHECK CONSTRAINT [FK_CourseAttendance_UserId]
GO
