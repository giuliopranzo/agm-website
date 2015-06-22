CREATE TABLE [dbo].[annunci] (
    [idannuncio]  INT           IDENTITY (1, 1) NOT NULL,
    [indirizzo]   VARCHAR (100) NULL,
    [titolo]      VARCHAR (100) NULL,
    [dove]        VARCHAR (100) NULL,
    [datainizio]  NVARCHAR (10) NULL,
    [datafine]    NVARCHAR (10) NULL,
    [riferimento] NVARCHAR (50) NULL
);

