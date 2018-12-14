IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE TABLE [LibraryBranches] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(30) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Telephone] nvarchar(max) NOT NULL,
        [Decription] nvarchar(max) NULL,
        [OpenDate] datetime2 NOT NULL,
        [ImageUrl] nvarchar(max) NULL,
        CONSTRAINT [PK_LibraryBranches] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE TABLE [LibraryCards] (
        [Id] int NOT NULL IDENTITY,
        [Fees] decimal(18,2) NOT NULL,
        [Created] datetime2 NOT NULL,
        CONSTRAINT [PK_LibraryCards] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE TABLE [Statuses] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Statuses] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE TABLE [BranchHours] (
        [Id] int NOT NULL IDENTITY,
        [branchId] int NULL,
        [DayOfWeek] int NOT NULL,
        [OpenTime] int NOT NULL,
        [CloseTime] int NOT NULL,
        CONSTRAINT [PK_BranchHours] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BranchHours_LibraryBranches_branchId] FOREIGN KEY ([branchId]) REFERENCES [LibraryBranches] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE TABLE [Patrons] (
        [Id] int NOT NULL IDENTITY,
        [FirstName] nvarchar(max) NULL,
        [LastName] nvarchar(max) NULL,
        [Address] nvarchar(max) NULL,
        [DateOfBirth] datetime2 NOT NULL,
        [PhoneNo] nvarchar(max) NULL,
        [LibraryCardId] int NULL,
        [HomeLibraryBranchId] int NULL,
        CONSTRAINT [PK_Patrons] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Patrons_LibraryBranches_HomeLibraryBranchId] FOREIGN KEY ([HomeLibraryBranchId]) REFERENCES [LibraryBranches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Patrons_LibraryCards_LibraryCardId] FOREIGN KEY ([LibraryCardId]) REFERENCES [LibraryCards] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE TABLE [LibraryAssets] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(max) NOT NULL,
        [Year] int NOT NULL,
        [StatusId] int NOT NULL,
        [Cost] decimal(18,2) NOT NULL,
        [ImageUrl] nvarchar(max) NULL,
        [NumberOfCopies] int NOT NULL,
        [LocationId] int NULL,
        [Discriminator] nvarchar(max) NOT NULL,
        [ISBN] nvarchar(max) NULL,
        [Author] nvarchar(max) NULL,
        [DeweyIndex] nvarchar(max) NULL,
        [Director] nvarchar(max) NULL,
        CONSTRAINT [PK_LibraryAssets] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_LibraryAssets_LibraryBranches_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [LibraryBranches] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_LibraryAssets_Statuses_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [Statuses] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE TABLE [CheckOutHistories] (
        [Id] int NOT NULL IDENTITY,
        [LibraryAssetId] int NOT NULL,
        [LibraryCardId] int NOT NULL,
        [CheckedOut] datetime2 NOT NULL,
        [CheckedIn] datetime2 NULL,
        CONSTRAINT [PK_CheckOutHistories] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CheckOutHistories_LibraryAssets_LibraryAssetId] FOREIGN KEY ([LibraryAssetId]) REFERENCES [LibraryAssets] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CheckOutHistories_LibraryCards_LibraryCardId] FOREIGN KEY ([LibraryCardId]) REFERENCES [LibraryCards] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE TABLE [CheckOuts] (
        [Id] int NOT NULL IDENTITY,
        [LibraryAssetId] int NOT NULL,
        [LibraryCardId] int NULL,
        [Since] datetime2 NOT NULL,
        [Until] datetime2 NOT NULL,
        CONSTRAINT [PK_CheckOuts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CheckOuts_LibraryAssets_LibraryAssetId] FOREIGN KEY ([LibraryAssetId]) REFERENCES [LibraryAssets] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CheckOuts_LibraryCards_LibraryCardId] FOREIGN KEY ([LibraryCardId]) REFERENCES [LibraryCards] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE TABLE [Holds] (
        [Id] int NOT NULL IDENTITY,
        [LibraryAssetId] int NULL,
        [LibraryCardId] int NULL,
        [HoldPlaced] datetime2 NOT NULL,
        CONSTRAINT [PK_Holds] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Holds_LibraryAssets_LibraryAssetId] FOREIGN KEY ([LibraryAssetId]) REFERENCES [LibraryAssets] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Holds_LibraryCards_LibraryCardId] FOREIGN KEY ([LibraryCardId]) REFERENCES [LibraryCards] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE INDEX [IX_BranchHours_branchId] ON [BranchHours] ([branchId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE INDEX [IX_CheckOutHistories_LibraryAssetId] ON [CheckOutHistories] ([LibraryAssetId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE INDEX [IX_CheckOutHistories_LibraryCardId] ON [CheckOutHistories] ([LibraryCardId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE INDEX [IX_CheckOuts_LibraryAssetId] ON [CheckOuts] ([LibraryAssetId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE INDEX [IX_CheckOuts_LibraryCardId] ON [CheckOuts] ([LibraryCardId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE INDEX [IX_Holds_LibraryAssetId] ON [Holds] ([LibraryAssetId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE INDEX [IX_Holds_LibraryCardId] ON [Holds] ([LibraryCardId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE INDEX [IX_LibraryAssets_LocationId] ON [LibraryAssets] ([LocationId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE INDEX [IX_LibraryAssets_StatusId] ON [LibraryAssets] ([StatusId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE INDEX [IX_Patrons_HomeLibraryBranchId] ON [Patrons] ([HomeLibraryBranchId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    CREATE INDEX [IX_Patrons_LibraryCardId] ON [Patrons] ([LibraryCardId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180915211544_initialMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20180915211544_initialMigration', N'2.1.1-rtm-30846');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20180924223707_Added checkout')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20180924223707_Added checkout', N'2.1.1-rtm-30846');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001220659_added identity login')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181001220659_added identity login', N'2.1.1-rtm-30846');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181003103618_Added loginModel')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181003103618_Added loginModel', N'2.1.1-rtm-30846');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181015111430_changed stausId to Id')
BEGIN
    EXEC sp_rename N'[Statuses].[StatusId]', N'Id', N'COLUMN';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181015111430_changed stausId to Id')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181015111430_changed stausId to Id', N'2.1.1-rtm-30846');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181015115711_Changed Id to statusId')
BEGIN
    EXEC sp_rename N'[Statuses].[Id]', N'StatusId', N'COLUMN';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181015115711_Changed Id to statusId')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181015115711_Changed Id to statusId', N'2.1.1-rtm-30846');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181015214752_Added StatusId to LibrabryAsset model')
BEGIN
    ALTER TABLE [LibraryAssets] DROP CONSTRAINT [FK_LibraryAssets_Statuses_StatusId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181015214752_Added StatusId to LibrabryAsset model')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LibraryAssets]') AND [c].[name] = N'StatusId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [LibraryAssets] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [LibraryAssets] ALTER COLUMN [StatusId] int NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181015214752_Added StatusId to LibrabryAsset model')
BEGIN
    ALTER TABLE [LibraryAssets] ADD [StatuId] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181015214752_Added StatusId to LibrabryAsset model')
BEGIN
    ALTER TABLE [LibraryAssets] ADD CONSTRAINT [FK_LibraryAssets_Statuses_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [Statuses] ([StatusId]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181015214752_Added StatusId to LibrabryAsset model')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181015214752_Added StatusId to LibrabryAsset model', N'2.1.1-rtm-30846');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181016182622_Corrected statuId to StatusId')
BEGIN
    ALTER TABLE [LibraryAssets] DROP CONSTRAINT [FK_LibraryAssets_Statuses_StatusId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181016182622_Corrected statuId to StatusId')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LibraryAssets]') AND [c].[name] = N'StatuId');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [LibraryAssets] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [LibraryAssets] DROP COLUMN [StatuId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181016182622_Corrected statuId to StatusId')
BEGIN
    DROP INDEX [IX_LibraryAssets_StatusId] ON [LibraryAssets];
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LibraryAssets]') AND [c].[name] = N'StatusId');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [LibraryAssets] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [LibraryAssets] ALTER COLUMN [StatusId] int NOT NULL;
    CREATE INDEX [IX_LibraryAssets_StatusId] ON [LibraryAssets] ([StatusId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181016182622_Corrected statuId to StatusId')
BEGIN
    ALTER TABLE [LibraryAssets] ADD CONSTRAINT [FK_LibraryAssets_Statuses_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [Statuses] ([StatusId]) ON DELETE CASCADE;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181016182622_Corrected statuId to StatusId')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181016182622_Corrected statuId to StatusId', N'2.1.1-rtm-30846');
END;

GO

