﻿BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

ALTER TABLE [dbo].[rappvociretributive ]
	ALTER COLUMN [Amount] FLOAT NULL;

ALTER TABLE [dbo].[rappvociretributive ]
	ALTER COLUMN [Total] FLOAT NULL;

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;