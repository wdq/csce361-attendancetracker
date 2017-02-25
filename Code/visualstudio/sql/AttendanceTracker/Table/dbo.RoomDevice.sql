IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoomDevice_Room]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoomDevice]'))
ALTER TABLE [dbo].[RoomDevice] DROP CONSTRAINT [FK_RoomDevice_Room]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoomDevice]') AND type in (N'U'))
DROP TABLE [dbo].[RoomDevice]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoomDevice](
	[Id] [uniqueidentifier] NOT NULL,
	[IpAddress] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[RoomId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_RoomDevice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RoomDevice]  WITH CHECK ADD  CONSTRAINT [FK_RoomDevice_Room] FOREIGN KEY([RoomId])
REFERENCES [Room] ([Id])
GO
ALTER TABLE [dbo].[RoomDevice] CHECK CONSTRAINT [FK_RoomDevice_Room]
GO
