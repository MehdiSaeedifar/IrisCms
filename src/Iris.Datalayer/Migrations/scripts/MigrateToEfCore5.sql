IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

CREATE FULLTEXT CATALOG ftCatalog AS DEFAULT;
GO

CREATE FULLTEXT INDEX ON Posts(Title Statistical_Semantics, Body Statistical_Semantics) KEY INDEX [PK_dbo.Posts];
GO

CREATE FULLTEXT INDEX ON Books(Name Statistical_Semantics, Description Statistical_Semantics, Author Statistical_Semantics, ISBN Statistical_Semantics, Publisher Statistical_Semantics) KEY INDEX [PK_dbo.Books];
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210507050442_AddFullTextIndexToPostAndBook', N'5.0.6');
GO

COMMIT;
GO

ALTER DATABASE EbooksWorldDb
SET COMPATIBILITY_LEVEL = 120
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210507072306_ChangeDatabaseCompatibilityLevelTo120', N'5.0.6');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Posts]') AND [c].[name] = N'Status');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Posts] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Posts] ALTER COLUMN [Status] nvarchar(50) NULL;
GO

CREATE INDEX [IX_Posts_CreatedDate] ON [Posts] ([CreatedDate]);
GO

CREATE INDEX [IX_Posts_Status] ON [Posts] ([Status]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210513090836_AddCreatedDateAndStatusIndexToPost', N'5.0.6');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [__MigrationHistory];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210514072806_DropOldMigrationHistoryTable', N'5.0.6');
GO

COMMIT;
GO

