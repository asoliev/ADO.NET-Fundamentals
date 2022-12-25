/*
Post-Deployment Script Template
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.
 Use SQLCMD syntax to include a file in the post-deployment script.
 Example:      :r .\myfile.sql
 Use SQLCMD syntax to reference a variable in the post-deployment script.
 Example:      :setvar TableName MyTable
               SELECT * FROM [$(TableName)]
--------------------------------------------------------------------------------------
*/
BEGIN TRANSACTION
    INSERT INTO [dbo].[Product] ([Name], [Description], [Weight], [Height], [Width], [Length])
                         VALUES ('Apple', 'nice product', 5, 3, 2, 6);

    INSERT INTO [Order] ([Status], [CreatedDate], [UpdatedDate], [ProductId])
                    VALUES (2, '2000-11-11', CURRENT_TIMESTAMP, IDENT_CURRENT('Product'));
COMMIT;