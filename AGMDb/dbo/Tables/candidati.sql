﻿CREATE TABLE [dbo].[candidati] (
    [idcandidato]      INT            IDENTITY (1, 1) NOT NULL,
    [nome]             VARCHAR (30)   NULL,
    [cognome]          VARCHAR (30)   NULL,
    [colloquio]        VARCHAR (50)   NULL,
    [anno]             VARCHAR (10)   NULL,
    [profilo]          VARCHAR (8000) NULL,
    [lingua1]          INT            NULL,
    [lingua2]          INT            NULL,
    [lingua2livello]   INT            NULL,
    [lingua3]          INT            NULL,
    [lingua3livello]   INT            NULL,
    [contrattoimporto] VARCHAR (30)   NULL,
    [contrattotipo]    INT            NULL,
    [idcategoria]      INT            NULL,
    [idstato]          INT            NULL,
    [idselezionatore]  INT            NULL,
    [idluogo]          INT            NULL,
    [aggiornamento]    VARCHAR (10)   NULL,
    [disponibilita]    VARCHAR (50)   NULL,
    [luogonascita]     VARCHAR (80)   NULL,
    [testinglese]      INT            NULL,
    [idmotivo]         INT            NULL,
    [idluogolavoro]    INT            NULL
);
