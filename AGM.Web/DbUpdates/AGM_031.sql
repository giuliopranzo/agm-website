USE [agmsolutions_net_site]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.rappreperibilita', 'U') IS NOT NULL 
  DROP TABLE dbo.rappreperibilita;

CREATE TABLE [dbo].[rappreperibilita]
(
	[idreperibilita] [int] IDENTITY(1,1) NOT NULL,
	[idutente] [int] NULL,
	[giorno] [int] NULL,
	[mese] [int] NULL,
	[anno] [int] NULL,
	[campo] [bit] NULL
) ON [PRIMARY]

GO