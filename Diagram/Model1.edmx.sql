
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/01/2025 17:15:35
-- Generated from EDMX file: C:\Users\JesusCarvajal\Documents\segurity\Diagram\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [segurity];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [State] nvarchar(max)  NOT NULL,
    [UserRolId] int  NOT NULL,
    [UserRolId1] int  NOT NULL
);
GO

-- Creating table 'PersonSet'
CREATE TABLE [dbo].[PersonSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Identification] nvarchar(max)  NOT NULL,
    [Age] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'ModuleSet'
CREATE TABLE [dbo].[ModuleSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Code] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FormSet'
CREATE TABLE [dbo].[FormSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RolSet'
CREATE TABLE [dbo].[RolSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Active] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PermisosSet'
CREATE TABLE [dbo].[PermisosSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Action] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'moduleFormSet'
CREATE TABLE [dbo].[moduleFormSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Form_Id] int  NOT NULL,
    [Module_Id] int  NOT NULL
);
GO

-- Creating table 'PermisionSet'
CREATE TABLE [dbo].[PermisionSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserRolSet'
CREATE TABLE [dbo].[UserRolSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [User_Id] int  NOT NULL,
    [Rol_Id] int  NOT NULL
);
GO

-- Creating table 'RolFormPermissionSet'
CREATE TABLE [dbo].[RolFormPermissionSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserRolPersmisionPermisos_UserRolPersmision_Id] int  NOT NULL,
    [Form_Id] int  NOT NULL,
    [Rol_Id] int  NOT NULL,
    [Permision_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PersonSet'
ALTER TABLE [dbo].[PersonSet]
ADD CONSTRAINT [PK_PersonSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ModuleSet'
ALTER TABLE [dbo].[ModuleSet]
ADD CONSTRAINT [PK_ModuleSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FormSet'
ALTER TABLE [dbo].[FormSet]
ADD CONSTRAINT [PK_FormSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RolSet'
ALTER TABLE [dbo].[RolSet]
ADD CONSTRAINT [PK_RolSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PermisosSet'
ALTER TABLE [dbo].[PermisosSet]
ADD CONSTRAINT [PK_PermisosSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'moduleFormSet'
ALTER TABLE [dbo].[moduleFormSet]
ADD CONSTRAINT [PK_moduleFormSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PermisionSet'
ALTER TABLE [dbo].[PermisionSet]
ADD CONSTRAINT [PK_PermisionSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserRolSet'
ALTER TABLE [dbo].[UserRolSet]
ADD CONSTRAINT [PK_UserRolSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RolFormPermissionSet'
ALTER TABLE [dbo].[RolFormPermissionSet]
ADD CONSTRAINT [PK_RolFormPermissionSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserRolPersmisionPermisos_UserRolPersmision_Id] in table 'RolFormPermissionSet'
ALTER TABLE [dbo].[RolFormPermissionSet]
ADD CONSTRAINT [FK_UserRolPersmisionPermisos]
    FOREIGN KEY ([UserRolPersmisionPermisos_UserRolPersmision_Id])
    REFERENCES [dbo].[PermisosSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRolPersmisionPermisos'
CREATE INDEX [IX_FK_UserRolPersmisionPermisos]
ON [dbo].[RolFormPermissionSet]
    ([UserRolPersmisionPermisos_UserRolPersmision_Id]);
GO

-- Creating foreign key on [UserId] in table 'PersonSet'
ALTER TABLE [dbo].[PersonSet]
ADD CONSTRAINT [FK_UserPerson]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPerson'
CREATE INDEX [IX_FK_UserPerson]
ON [dbo].[PersonSet]
    ([UserId]);
GO

-- Creating foreign key on [User_Id] in table 'UserRolSet'
ALTER TABLE [dbo].[UserRolSet]
ADD CONSTRAINT [FK_UserRolUser]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRolUser'
CREATE INDEX [IX_FK_UserRolUser]
ON [dbo].[UserRolSet]
    ([User_Id]);
GO

-- Creating foreign key on [Rol_Id] in table 'UserRolSet'
ALTER TABLE [dbo].[UserRolSet]
ADD CONSTRAINT [FK_UserRolRol]
    FOREIGN KEY ([Rol_Id])
    REFERENCES [dbo].[RolSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRolRol'
CREATE INDEX [IX_FK_UserRolRol]
ON [dbo].[UserRolSet]
    ([Rol_Id]);
GO

-- Creating foreign key on [Form_Id] in table 'moduleFormSet'
ALTER TABLE [dbo].[moduleFormSet]
ADD CONSTRAINT [FK_moduleFormForm]
    FOREIGN KEY ([Form_Id])
    REFERENCES [dbo].[FormSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_moduleFormForm'
CREATE INDEX [IX_FK_moduleFormForm]
ON [dbo].[moduleFormSet]
    ([Form_Id]);
GO

-- Creating foreign key on [Module_Id] in table 'moduleFormSet'
ALTER TABLE [dbo].[moduleFormSet]
ADD CONSTRAINT [FK_moduleFormModule]
    FOREIGN KEY ([Module_Id])
    REFERENCES [dbo].[ModuleSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_moduleFormModule'
CREATE INDEX [IX_FK_moduleFormModule]
ON [dbo].[moduleFormSet]
    ([Module_Id]);
GO

-- Creating foreign key on [Form_Id] in table 'RolFormPermissionSet'
ALTER TABLE [dbo].[RolFormPermissionSet]
ADD CONSTRAINT [FK_RolFormPermissionForm]
    FOREIGN KEY ([Form_Id])
    REFERENCES [dbo].[FormSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RolFormPermissionForm'
CREATE INDEX [IX_FK_RolFormPermissionForm]
ON [dbo].[RolFormPermissionSet]
    ([Form_Id]);
GO

-- Creating foreign key on [Rol_Id] in table 'RolFormPermissionSet'
ALTER TABLE [dbo].[RolFormPermissionSet]
ADD CONSTRAINT [FK_RolFormPermissionRol]
    FOREIGN KEY ([Rol_Id])
    REFERENCES [dbo].[RolSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RolFormPermissionRol'
CREATE INDEX [IX_FK_RolFormPermissionRol]
ON [dbo].[RolFormPermissionSet]
    ([Rol_Id]);
GO

-- Creating foreign key on [Permision_Id] in table 'RolFormPermissionSet'
ALTER TABLE [dbo].[RolFormPermissionSet]
ADD CONSTRAINT [FK_RolFormPermissionPermision]
    FOREIGN KEY ([Permision_Id])
    REFERENCES [dbo].[PermisionSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RolFormPermissionPermision'
CREATE INDEX [IX_FK_RolFormPermissionPermision]
ON [dbo].[RolFormPermissionSet]
    ([Permision_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------