IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseOwner_Course]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseOwner]'))
ALTER TABLE [dbo].[CourseOwner] DROP CONSTRAINT [FK_CourseOwner_Course]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CourseOwner_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[CourseOwner]'))
ALTER TABLE [dbo].[CourseOwner] DROP CONSTRAINT [FK_CourseOwner_User]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CourseOwner]') AND type in (N'U'))
DROP TABLE [dbo].[CourseOwner]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseOwner](
	[Id] [uniqueidentifier] NOT NULL,
	[CourseId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CourseOwner] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CourseOwner]  WITH CHECK ADD  CONSTRAINT [FK_CourseOwner_Course] FOREIGN KEY([CourseId])
REFERENCES [Course] ([Id])
GO
ALTER TABLE [dbo].[CourseOwner] CHECK CONSTRAINT [FK_CourseOwner_Course]
GO
ALTER TABLE [dbo].[CourseOwner]  WITH CHECK ADD  CONSTRAINT [FK_CourseOwner_User] FOREIGN KEY([UserId])
REFERENCES [User] ([Id])
GO
ALTER TABLE [dbo].[CourseOwner] CHECK CONSTRAINT [FK_CourseOwner_User]
GO
