﻿BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

ALTER TABLE [dbo].[rappfestivi]
DROP CONSTRAINT [PK__rappfestivi];

ALTER TABLE [dbo].[rappfestivi]
    ADD CONSTRAINT [PK__rappfestivi] PRIMARY KEY ([idgiorno] ASC);

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
