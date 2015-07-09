CREATE TABLE [dbo].[version]
(
	[Version] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [UpdateDate] DATETIME NULL, 
    [UpdateSucceeded] BIT NULL, 
    [LastUpdateTryDate] DATETIME NULL
)
