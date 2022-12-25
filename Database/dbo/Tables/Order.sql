CREATE TABLE [dbo].[Order] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Status]      INT           NOT NULL,
    [CreatedDate] DATE NOT NULL,
    [UpdatedDate] DATE NOT NULL,
    [ProductId]   INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Order_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);