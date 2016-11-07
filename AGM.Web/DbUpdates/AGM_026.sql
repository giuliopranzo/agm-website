BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[Messages] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [FromUserId] INT           NOT NULL,
    [IsArchived] BIT	NOT NULL, 
    [InsertDate] DATETIME      NOT NULL,
    [Subject]    VARCHAR (50)  NOT NULL,
    [Text]       VARCHAR (255) NOT NULL,
    [IsDeleted]  BIT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

ALTER TABLE [dbo].[Messages] WITH NOCHECK
    ADD CONSTRAINT [FK_Messages_ToUtenti] FOREIGN KEY ([FromUserId]) REFERENCES [dbo].[utenti] ([idutente]);

ALTER TABLE [dbo].[Messages] WITH CHECK CHECK CONSTRAINT [FK_Messages_ToUtenti];

CREATE TABLE [dbo].[Message_receivers] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [MessageId]  INT NOT NULL,
    [ToUserId]   INT NOT NULL,
    [IsArchived] BIT NOT NULL,
    [IsDeleted]  BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

ALTER TABLE [dbo].[Message_receivers] WITH NOCHECK
    ADD CONSTRAINT [FK_Message_receivers_ToUtenti] FOREIGN KEY ([ToUserId]) REFERENCES [dbo].[utenti] ([idutente]);

ALTER TABLE [dbo].[Message_receivers] WITH NOCHECK
    ADD CONSTRAINT [FK_Message_receivers_ToMessages] FOREIGN KEY ([MessageId]) REFERENCES [dbo].[Messages] ([Id]);

ALTER TABLE [dbo].[Message_receivers] WITH CHECK CHECK CONSTRAINT [FK_Message_receivers_ToUtenti];

ALTER TABLE [dbo].[Message_receivers] WITH CHECK CHECK CONSTRAINT [FK_Message_receivers_ToMessages];

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
