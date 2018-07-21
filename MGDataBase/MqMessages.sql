CREATE TABLE [dbo].[MqMessages] (
    [Id]     INT         IDENTITY (1, 1) NOT NULL,
    [Time]   DATETIME    NULL,
    [Phone]  NCHAR (20)  NULL,
    [Header] NCHAR (100) NOT NULL,
    [Body]   NCHAR (300) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
