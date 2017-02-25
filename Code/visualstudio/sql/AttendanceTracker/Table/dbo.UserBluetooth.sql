IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserBluetooth_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserBluetooth]'))
ALTER TABLE [dbo].[UserBluetooth] DROP CONSTRAINT [FK_UserBluetooth_User]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserBluetooth]') AND type in (N'U'))
DROP TABLE [dbo].[UserBluetooth]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserBluetooth](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Address] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_UserBluetooth] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserBluetooth]  WITH CHECK ADD  CONSTRAINT [FK_UserBluetooth_User] FOREIGN KEY([UserId])
REFERENCES [User] ([Id])
GO
ALTER TABLE [dbo].[UserBluetooth] CHECK CONSTRAINT [FK_UserBluetooth_User]
GO
