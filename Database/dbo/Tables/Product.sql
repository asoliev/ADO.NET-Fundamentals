CREATE TABLE [dbo].[Product] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50) NOT NULL,
    [Description] NVARCHAR (50) NOT NULL,
    [Weight]      INT           NOT NULL,
    [Height]      INT           NOT NULL,
    [Width]       INT           NOT NULL,
    [Length]      INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);