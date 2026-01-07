
--Author : Shadab Shaikh
--CreatedDate : 20th Dec 2025

CREATE TABLE [dbo].[TradeTransactions](
	[TransactionID] [int] IDENTITY(1,1) NOT NULL,
	[TradeID] [int] NOT NULL,
	[Version] [int] NOT NULL,
	[SecurityCode] [nvarchar](10) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Action] [nvarchar](10) NOT NULL,
	[BuySell] [nvarchar](4) NOT NULL,
	[CreatedAt] [datetime] NOT NULL default GetDate(),
	Constraint PK_TXNID Primary Key(TransactionID)
)


select * from TradeTransactions



