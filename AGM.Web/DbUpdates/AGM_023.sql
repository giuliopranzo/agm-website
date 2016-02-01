BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_candidaticategorie] (
    [idcategoria] INT          IDENTITY (1, 1) NOT NULL,
    [nome]        VARCHAR (50) NULL,
    [isDeleted]   BIT          NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_candidaticategorie] PRIMARY KEY CLUSTERED ([idcategoria] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[candidaticategorie])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_candidaticategorie] ON;
        INSERT INTO [dbo].[tmp_ms_xx_candidaticategorie] ([idcategoria], [nome], [isDeleted])
        SELECT   [idcategoria],
                 [nome],
                 [isDeleted]
        FROM     [dbo].[candidaticategorie]
        ORDER BY [idcategoria] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_candidaticategorie] OFF;
    END

DROP TABLE [dbo].[candidaticategorie];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_candidaticategorie]', N'candidaticategorie';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_candidaticategorie]', N'PK_candidaticategorie', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
