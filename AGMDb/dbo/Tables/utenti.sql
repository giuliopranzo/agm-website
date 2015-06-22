CREATE TABLE [dbo].[utenti] (
    [utente]     VARCHAR (50)  NULL,
    [nome]       VARCHAR (50)  NULL,
    [cognome]    VARCHAR (50)  NULL,
    [utenti]     INT           NULL,
    [annunci]    INT           NULL,
    [idutente]   INT           IDENTITY (1, 1) NOT NULL,
    [pass]       VARCHAR (50)  NULL,
    [rapportini] INT           NULL,
    [candidati]  INT           NULL,
    [attivo]     INT           DEFAULT ((1)) NULL,
    [email]      VARCHAR (200) NULL,
    [isDeleted]  BIT           DEFAULT ((0)) NOT NULL,
    [image]      VARCHAR (250) NULL
);

