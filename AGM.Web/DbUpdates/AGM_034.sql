USE [agmsolutions_net_site]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.utentitipi', 'U') IS NOT NULL 
  DROP TABLE dbo.utentitipi;

CREATE TABLE [dbo].[utentitipi](
	[idtipo] [int] IDENTITY(1,1) NOT NULL,
	[nome] [varchar](50) NULL
 CONSTRAINT [PK_utentitipi] PRIMARY KEY CLUSTERED 
(
	[idtipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[utentitipi](nome) VALUES('Amministratore')
INSERT INTO [dbo].[utentitipi](nome) VALUES('HR')
INSERT INTO [dbo].[utentitipi](nome) VALUES('Utente')

IF NOT EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'userType'
      AND Object_ID = Object_ID(N'utenti'))
BEGIN
    ALTER TABLE utenti ADD userType INT NULL;
	EXEC('UPDATE utenti SET utenti.userType = 3');
END

IF EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'isAdmin'
      AND Object_ID = Object_ID(N'utenti'))
BEGIN
    EXEC('UPDATE utenti SET utenti.userType = 1 WHERE utenti.isAdmin = 1');
	ALTER TABLE utenti DROP COLUMN isAdmin;
END

GO

