USE [agmsolutions_net_site]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.comunicazioni', 'U') IS NOT NULL 
  DROP TABLE dbo.comunicazioni;

CREATE TABLE [dbo].[comunicazioni](
	[idcomunicazione] [int] IDENTITY(1,1) NOT NULL,
	[idutente] [int] NOT NULL,
	[data] [datetime] NOT NULL,
	[oggetto] [varchar](50) NOT NULL,
	[testo] [nvarchar](MAX) NOT NULL,
	[isDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idcomunicazione] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[comunicazioni]  WITH CHECK ADD  CONSTRAINT [FK_Comunicazioni_ToUtenti] FOREIGN KEY([idutente])
REFERENCES [dbo].[utenti] ([idutente])
GO

ALTER TABLE [dbo].[Comunicazioni] CHECK CONSTRAINT [FK_Comunicazioni_ToUtenti]
GO

