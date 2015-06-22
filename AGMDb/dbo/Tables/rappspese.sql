CREATE TABLE [dbo].[rappspese] (
    [idutente]    INT           NULL,
    [idspesa]     INT           NULL,
    [giorno]      INT           NULL,
    [mese]        INT           NULL,
    [anno]        INT           NULL,
    [importo]     NVARCHAR (50) NULL,
    [idrappspese] INT           IDENTITY (1, 1) NOT NULL
);

