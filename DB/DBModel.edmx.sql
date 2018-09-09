
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/09/2018 17:17:22
-- Generated from EDMX file: F:\Repos\Hospital\DB\DBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Patients];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PatientDocuments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PDFDocuments] DROP CONSTRAINT [FK_PatientDocuments];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientVisitLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VisitLog] DROP CONSTRAINT [FK_PatientVisitLog];
GO
IF OBJECT_ID(N'[dbo].[FK_VisitLogDoctors]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Doctors] DROP CONSTRAINT [FK_VisitLogDoctors];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Patient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Patient];
GO
IF OBJECT_ID(N'[dbo].[PDFDocuments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PDFDocuments];
GO
IF OBJECT_ID(N'[dbo].[VisitLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VisitLog];
GO
IF OBJECT_ID(N'[dbo].[Doctors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Doctors];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Patient'
CREATE TABLE [dbo].[Patient] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [PatronymicName] nvarchar(max)  NOT NULL,
    [Sity] nvarchar(max)  NOT NULL,
    [Street] nvarchar(max)  NOT NULL,
    [BuildingNumber] nvarchar(max)  NOT NULL,
    [FlatNumber] nvarchar(max)  NOT NULL,
    [PatientType] int  NOT NULL,
    [CreateDateTime] datetime  NOT NULL,
    [LastVisitDateTime] datetime  NULL,
    [IsCured] bit  NOT NULL,
    [SNILS] nvarchar(max)  NULL,
    [InsuranceNumber] nvarchar(max)  NULL,
    [INN] nvarchar(max)  NULL
);
GO

-- Creating table 'PDFDocuments'
CREATE TABLE [dbo].[PDFDocuments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Document] varbinary(max)  NOT NULL,
    [Patient_Id] int  NOT NULL
);
GO

-- Creating table 'VisitLog'
CREATE TABLE [dbo].[VisitLog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [VisitDateTime] datetime  NOT NULL,
    [Patient_Id] int  NOT NULL
);
GO

-- Creating table 'Doctors'
CREATE TABLE [dbo].[Doctors] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [PatronymicName] nvarchar(max)  NOT NULL,
    [Position] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Parents'
CREATE TABLE [dbo].[Parents] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [PatronymicName] nvarchar(max)  NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NOT NULL,
    [Patient_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Patient'
ALTER TABLE [dbo].[Patient]
ADD CONSTRAINT [PK_Patient]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PDFDocuments'
ALTER TABLE [dbo].[PDFDocuments]
ADD CONSTRAINT [PK_PDFDocuments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VisitLog'
ALTER TABLE [dbo].[VisitLog]
ADD CONSTRAINT [PK_VisitLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Doctors'
ALTER TABLE [dbo].[Doctors]
ADD CONSTRAINT [PK_Doctors]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Parents'
ALTER TABLE [dbo].[Parents]
ADD CONSTRAINT [PK_Parents]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Patient_Id] in table 'PDFDocuments'
ALTER TABLE [dbo].[PDFDocuments]
ADD CONSTRAINT [FK_PatientDocuments]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Patient]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientDocuments'
CREATE INDEX [IX_FK_PatientDocuments]
ON [dbo].[PDFDocuments]
    ([Patient_Id]);
GO

-- Creating foreign key on [Patient_Id] in table 'VisitLog'
ALTER TABLE [dbo].[VisitLog]
ADD CONSTRAINT [FK_PatientVisitLog]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Patient]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientVisitLog'
CREATE INDEX [IX_FK_PatientVisitLog]
ON [dbo].[VisitLog]
    ([Patient_Id]);
GO

-- Creating foreign key on [Id] in table 'Doctors'
ALTER TABLE [dbo].[Doctors]
ADD CONSTRAINT [FK_VisitLogDoctors]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[VisitLog]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Patient_Id] in table 'Parents'
ALTER TABLE [dbo].[Parents]
ADD CONSTRAINT [FK_PatientParents]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Patient]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientParents'
CREATE INDEX [IX_FK_PatientParents]
ON [dbo].[Parents]
    ([Patient_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------