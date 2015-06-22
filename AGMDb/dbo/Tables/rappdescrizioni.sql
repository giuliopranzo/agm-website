CREATE TABLE [dbo].[rappdescrizioni] (
    [iddescrizione] INT            IDENTITY (1, 1) NOT NULL,
    [idutente]      INT            NULL,
    [giorno]        INT            NULL,
    [mese]          INT            NULL,
    [anno]          INT            NULL,
    [campo]         NVARCHAR (300) NULL
);

