
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/15/2014 18:46:56
-- Generated from EDMX file: C:\Users\lucas.ontivero\Documents\GitHub\BlockchainParser\BlockchainToSql\Blockchain.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [blockchain];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Inputs_Inputs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Inputs] DROP CONSTRAINT [FK_Inputs_Inputs];
GO
IF OBJECT_ID(N'[dbo].[FK_Outputs_Transactions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Outputs] DROP CONSTRAINT [FK_Outputs_Transactions];
GO
IF OBJECT_ID(N'[dbo].[FK_Transactions_blocks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions] DROP CONSTRAINT [FK_Transactions_blocks];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[blocks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[blocks];
GO
IF OBJECT_ID(N'[dbo].[Inputs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Inputs];
GO
IF OBJECT_ID(N'[dbo].[Outputs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Outputs];
GO
IF OBJECT_ID(N'[dbo].[Transactions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'blocks'
CREATE TABLE [dbo].[blocks] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [Length] int  NOT NULL,
    [LockTime] bigint  NOT NULL,
    [Nonce] bigint  NOT NULL,
    [PreviousBlockHash] binary(32)  NOT NULL,
    [TargetDifficulty] bigint  NOT NULL,
    [TimeStamp] datetime  NOT NULL,
    [MerkleRoot] binary(32)  NOT NULL
);
GO

-- Creating table 'Inputs'
CREATE TABLE [dbo].[Inputs] (
    [ID] bigint  NOT NULL,
    [TransactionID] bigint  NOT NULL,
    [TransactionHash] binary(32)  NOT NULL,
    [TransactionIndex] bigint  NOT NULL,
    [Script] varbinary(max)  NOT NULL,
    [SequenceNumber] bigint  NOT NULL
);
GO

-- Creating table 'Outputs'
CREATE TABLE [dbo].[Outputs] (
    [ID] bigint  NOT NULL,
    [TransactionID] bigint  NOT NULL,
    [Value] bigint  NOT NULL,
    [Script] varbinary(max)  NOT NULL
);
GO

-- Creating table 'Transactions'
CREATE TABLE [dbo].[Transactions] (
    [ID] bigint  NOT NULL,
    [BlockID] bigint  NOT NULL,
    [Version] bigint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'blocks'
ALTER TABLE [dbo].[blocks]
ADD CONSTRAINT [PK_blocks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Inputs'
ALTER TABLE [dbo].[Inputs]
ADD CONSTRAINT [PK_Inputs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Outputs'
ALTER TABLE [dbo].[Outputs]
ADD CONSTRAINT [PK_Outputs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Transactions'
ALTER TABLE [dbo].[Transactions]
ADD CONSTRAINT [PK_Transactions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [BlockID] in table 'Transactions'
ALTER TABLE [dbo].[Transactions]
ADD CONSTRAINT [FK_Transactions_blocks]
    FOREIGN KEY ([BlockID])
    REFERENCES [dbo].[blocks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Transactions_blocks'
CREATE INDEX [IX_FK_Transactions_blocks]
ON [dbo].[Transactions]
    ([BlockID]);
GO

-- Creating foreign key on [TransactionID] in table 'Inputs'
ALTER TABLE [dbo].[Inputs]
ADD CONSTRAINT [FK_Inputs_Inputs]
    FOREIGN KEY ([TransactionID])
    REFERENCES [dbo].[Transactions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Inputs_Inputs'
CREATE INDEX [IX_FK_Inputs_Inputs]
ON [dbo].[Inputs]
    ([TransactionID]);
GO

-- Creating foreign key on [TransactionID] in table 'Outputs'
ALTER TABLE [dbo].[Outputs]
ADD CONSTRAINT [FK_Outputs_Transactions]
    FOREIGN KEY ([TransactionID])
    REFERENCES [dbo].[Transactions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Outputs_Transactions'
CREATE INDEX [IX_FK_Outputs_Transactions]
ON [dbo].[Outputs]
    ([TransactionID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------