DELETE FROM [dbo].[SchemaChanges]
GO
SET IDENTITY_INSERT [dbo].[SchemaChanges] ON 

GO
INSERT [dbo].[SchemaChanges] ([Id], [Major], [Minor], [Point], [ScriptName], [DateApplied]) VALUES (1, 0, 0, 1, N'CreateSchemaChangeLog.sql', CAST(0x0000A725009AEA52 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[SchemaChanges] OFF
GO
