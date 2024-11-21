CREATE TABLE [dbo].[Sample]
(
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Description] VARCHAR(50) NOT NULL,
    CONSTRAINT [PK_Sample] PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO

CREATE UNIQUE NONCLUSTERED INDEX [UK_Sample_Description] ON [dbo].[Sample]
(
	[Description] ASC
)