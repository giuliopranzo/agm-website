BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_rappcausali] (
    [idcausale] INT          IDENTITY (1, 1) NOT NULL,
    [nome]      VARCHAR (50) NULL,
    [isDeleted] BIT          DEFAULT 0 NOT NULL,
    [disabled]  BIT          DEFAULT 0 NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_rappcausali] PRIMARY KEY CLUSTERED ([idcausale] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[rappcausali])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_rappcausali] ON;
        INSERT INTO [dbo].[tmp_ms_xx_rappcausali] ([idcausale], [nome])
        SELECT   [idcausale],
                 [nome]
        FROM     [dbo].[rappcausali]
        ORDER BY [idcausale] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_rappcausali] OFF;
    END

DROP TABLE [dbo].[rappcausali];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_rappcausali]', N'rappcausali';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_rappcausali]', N'PK_rappcausali', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
