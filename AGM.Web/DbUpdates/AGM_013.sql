﻿BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[token]
(
	[Id] VARCHAR(32) NOT NULL PRIMARY KEY, 
    [IsConsumed] BIT NOT NULL, 
    [ExpirationDate] DATETIME2 (7) NOT NULL
);

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;