﻿BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

UPDATE 
	f 
SET 
	date = convert(DATETIME,r.giorno,103)  
FROM 
	rappfestivi f
INNER JOIN
	rappfestivi r 
ON r.idgiorno = f.idgiorno;

ALTER TABLE [dbo].[rappfestivi] 
	ALTER COLUMN [date] DATE NOT NULL;

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
