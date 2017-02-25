CREATE TABLE [dbo].[SchemaChanges](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Major] [int] NOT NULL,
	[Minor] [int] NOT NULL,
	[Point] [int] NOT NULL,
	[ScriptName] [varchar](50) NOT NULL,
	[DateApplied] [datetime] NOT NULL,
 CONSTRAINT [PK_SchemaChanges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[SchemaChanges]
           ([Major]
           ,[Minor]
           ,[Point]
           ,[ScriptName]
           ,[DateApplied])
     VALUES
           (0
           ,0
           ,1
           ,'CreateSchemaChangeLog.sql'
           ,GETDATE())
GO
