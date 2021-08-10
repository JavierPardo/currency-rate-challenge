USE [master]
GO


CREATE DATABASE [ExchangeDB]
Go
USE [ExchangeDB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExchangeTransactions](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[AmountInput] [decimal](18, 2) NOT NULL,
	[AmountOutput] [decimal](18, 2) NOT NULL,
	[CurrencyCodeInput] [nvarchar](max) NULL,
	[CurrencyCodeOutput] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[DateTime] [datetime2](7) NOT NULL,
	[Comment] [nvarchar](max) NULL,
 CONSTRAINT [PK_ExchangeTransactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
