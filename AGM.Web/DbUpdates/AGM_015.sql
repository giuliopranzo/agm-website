﻿BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

ALTER TABLE [dbo].[export] ALTER COLUMN [HourReport] NVARCHAR (MAX) NULL;

ALTER TABLE [dbo].[export] ALTER COLUMN [RetributionItems] NVARCHAR (MAX) NULL;

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
