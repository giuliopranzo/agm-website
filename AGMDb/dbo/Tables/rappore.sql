CREATE TABLE [dbo].[rappore] (
    [idutente]  INT           NULL,
    [idcausale] INT           NULL,
    [giorno]    INT           NULL,
    [mese]      INT           NULL,
    [anno]      INT           NULL,
    [ore]       NVARCHAR (50) NULL,
    [idrappore] INT           IDENTITY (1, 1) NOT NULL
);

