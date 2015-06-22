CREATE TABLE [dbo].[candidatiricerche] (
    [idricerca] INT            IDENTITY (1, 1) NOT NULL,
    [idsocieta] INT            NULL,
    [posizione] VARCHAR (100)  NULL,
    [note]      VARCHAR (8000) NULL,
    [attiva]    INT            DEFAULT ((0)) NULL
);

