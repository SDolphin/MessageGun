CREATE TABLE [dbo].[UsersPhones] (
    [Id]          INT        NOT NULL,
    [FirstName]   NCHAR (10) NULL,
    [LastName]    NCHAR (10) NULL,
    [PhoneNumber] NCHAR (20) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
