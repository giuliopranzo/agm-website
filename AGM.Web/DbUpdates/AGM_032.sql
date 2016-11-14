USE [agmsolutions_net_site]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'isShiftWorker'
      AND Object_ID = Object_ID(N'utenti'))
BEGIN
    ALTER TABLE utenti ADD isShiftWorker INT NULL;
	EXEC('UPDATE utenti SET utenti.isShiftWorker = 0');
END

GO