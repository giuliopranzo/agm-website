﻿BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

ALTER TABLE [dbo].[rappfestivi]
    ADD [date] DATE NULL;

ALTER TABLE [dbo].[rappfestivi] 
	ALTER COLUMN [giorno] VARCHAR(10) NULL

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
