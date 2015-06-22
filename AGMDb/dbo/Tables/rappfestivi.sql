CREATE TABLE [dbo].[rappfestivi] (
    [idgiorno] INT          IDENTITY (1, 1) NOT NULL,
    [giorno]   VARCHAR (10) NOT NULL,
    CONSTRAINT [PK__rappfestivi] PRIMARY KEY CLUSTERED ([idgiorno] ASC, [giorno] ASC)
);

