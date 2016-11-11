USE [agmsolutions_net_site]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'IsShiftWorker'
      AND Object_ID = Object_ID(N'utenti'))
BEGIN
    ALTER TABLE utenti ADD IsShiftWorker BIT NULL;
	EXEC('UPDATE utenti SET utenti.IsShiftWorker = 0');
END

GO