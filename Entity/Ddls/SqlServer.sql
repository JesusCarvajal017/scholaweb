USE [segurity];

CREATE TABLE [Person] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [LastName] nvarchar(150)  NOT NULL,
    [Email] nvarchar(150)  NULL,
    [Identification] nvarchar(14)  NOT NULL,
    [Age] int NOT NULL,
    [Status] int NOT NULL,

    CONSTRAINT [PK_Person] PRIMARY KEY ([Id]),
    CONSTRAINT [UQ_Person_Email] UNIQUE ([Email]),
    CONSTRAINT [UQ_Person_Identification] UNIQUE ([Identification])
);

CREATE TABLE [User] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(150) NOT NULL, 
    [Password] nvarchar(max) NOT NULL,
    [Status] Int NOT NULL,
    [PersonId] int  NOT NULL,

    CONSTRAINT [PK_User] PRIMARY KEY ([Id]),   
    CONSTRAINT [UQ_User_Name] UNIQUE ([UserName]),
    CONSTRAINT [FK_Person_User] FOREIGN KEY ([PersonId]) REFERENCES [Person]([Id])

);

CREATE TABLE [Rol] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [Description] nvarchar(150)  NOT NULL,
    [Status] int NOT NULL,

    CONSTRAINT [PK_Rol] PRIMARY KEY ([Id])   
);

CREATE TABLE [Form] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [Description] nvarchar(150)  NOT NULL,
    [Status] int NOT NULL,

    CONSTRAINT [PK_Form] PRIMARY KEY ([Id])  
);

CREATE TABLE [Module] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [Descripcion] nvarchar(150)  NOT NULL,
    [Status] int NOT NULL,

    CONSTRAINT [PK_Module] PRIMARY KEY ([Id])  
);

CREATE TABLE [Permission] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [Description] nvarchar(150)  NOT NULL,
    [Status] int NOT NULL,

    CONSTRAINT [PK_Permission] PRIMARY KEY ([Id])  
);

CREATE TABLE [ModuleForm] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FormId] int  NOT NULL,
    [ModuleId] int  NOT NULL,

    CONSTRAINT [PK_ModuleForm] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Form] FOREIGN KEY ([FormId]) REFERENCES [Form]([Id]),
    CONSTRAINT [FK_Module] FOREIGN KEY ([ModuleId]) REFERENCES [Module]([Id])
);


CREATE TABLE [UserRol] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [RolId] int  NOT NULL,
    
    CONSTRAINT [PK_UserRol] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
    CONSTRAINT [FK_Rol] FOREIGN KEY ([RolId]) REFERENCES [Rol]([Id])
);



CREATE TABLE [RolFormPermission] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Form_Id] int  NOT NULL,
    [Rol_Id] int  NOT NULL,
    [Permision_Id] int  NOT NULL,

    CONSTRAINT [PK_RolFormPermission] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Form_Rfp] FOREIGN KEY ([Form_Id]) REFERENCES [Form]([Id]),
    CONSTRAINT [FK_Rol_Rfp] FOREIGN KEY ([Rol_Id]) REFERENCES [Rol]([Id]),
    CONSTRAINT [FK_Permision_Rfp] FOREIGN KEY ([Permision_Id]) REFERENCES [Permission]([Id])
);
