USE [AGM_prod]
GO

/****** Object:  Table [dbo].[rappcausali]    Script Date: 05/29/2015 09:03:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rappcausali]') AND type in (N'U'))
DROP TABLE [dbo].[rappcausali]
GO

USE [AGM_prod]
GO

/****** Object:  Table [dbo].[rappcausali]    Script Date: 05/29/2015 09:03:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[rappcausali](
	[idcausale] INT          NOT NULL IDENTITY,
    [nome]      VARCHAR (50) NULL,
	[isDeleted] BIT NOT NULL DEFAULT 0,  
	CONSTRAINT [PK_rappcausali] PRIMARY KEY ([idcausale])
) ON [PRIMARY]

GO

INSERT INTO [dbo].[rappcausali] ([nome]) VALUES (N'ordinarie')
INSERT INTO [dbo].[rappcausali] ([nome]) VALUES (N'straordinarie (solo se approvate)')
INSERT INTO [dbo].[rappcausali] ([nome]) VALUES (N'r.o.l.')
INSERT INTO [dbo].[rappcausali] ([nome]) VALUES (N'ferie')
INSERT INTO [dbo].[rappcausali] ([nome]) VALUES (N'malattia')
INSERT INTO [dbo].[rappcausali] ([nome]) VALUES (N'infortunio')
INSERT INTO [dbo].[rappcausali] ([nome]) VALUES (N'donazione sangue')
INSERT INTO [dbo].[rappcausali] ([nome]) VALUES (N'congedo matrimoniale')
INSERT INTO [dbo].[rappcausali] ([nome]) VALUES (N'D.Lgs. 151')
INSERT INTO [dbo].[rappcausali] ([nome]) VALUES (N'Permessi ex-festività')

SET ANSI_PADDING OFF
GO
