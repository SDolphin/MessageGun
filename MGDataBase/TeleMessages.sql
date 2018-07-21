CREATE TABLE [dbo].[TeleMessages] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [id_messages] INT             NOT NULL,
    [Sended]      BIT              NOT NULL,
    [Description] NVARCHAR (1000) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TeleMessages_ToTable] FOREIGN KEY ([id_messages]) REFERENCES [dbo].[MqMessages] ([Id])
);
