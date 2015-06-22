CREATE TABLE [dbo].[rappcausali] (
    [idcausale] INT          IDENTITY (1, 1) NOT NULL,
    [nome]      VARCHAR (50) NULL,
    [isDeleted] BIT          DEFAULT 0 NOT NULL,
    [disabled] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_rappcausali] PRIMARY KEY CLUSTERED ([idcausale] ASC)
);

