if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CustInfoNotifyFailRecord]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[CustInfoNotifyFailRecord]
GO

CREATE TABLE [dbo].[CustInfoNotifyFailRecord] (
	[CustID] [varchar] (16) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[UserAccount] [varchar] (16) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Result] [int] NOT NULL ,
	[DealTime] [datetime] NOT NULL ,
	[Description] [varchar] (256) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

 CREATE  INDEX [IX_CustInfoNotifyFailRecord] ON [dbo].[CustInfoNotifyFailRecord]([CustID], [UserAccount]) ON [PRIMARY]
GO

----------------------------------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EnterpriseInfo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[EnterpriseInfo]
GO

CREATE TABLE [dbo].[EnterpriseInfo] (
	[EnterpriseID] [varchar] (30) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[EnterpriseName] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[EnterpriseType] [varchar] (2) COLLATE Chinese_PRC_CI_AS NULL ,
	[CustID] [varchar] (16) COLLATE Chinese_PRC_CI_AS NULL ,
	[UserAccount] [varchar] (16) COLLATE Chinese_PRC_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EnterpriseInfo] WITH NOCHECK ADD 
	CONSTRAINT [PK_EnterpriseInfo] PRIMARY KEY  CLUSTERED 
	(
		[EnterpriseID]
	)  ON [PRIMARY] 
GO

 CREATE  INDEX [IX_EnterpriseInfo] ON [dbo].[EnterpriseInfo]([CustID]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [IX_EnterpriseInfo_1] ON [dbo].[EnterpriseInfo]([UserAccount]) ON [PRIMARY]
GO

--------------------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CustLevelChangeRecord]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[CustLevelChangeRecord]
GO

CREATE TABLE [dbo].[CustLevelChangeRecord] (
	[CustID] [varchar] (16) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[ProvinceID] [varchar] (2) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[SPID] [varchar] (8) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[IntegralInfo] [bigint] NOT NULL ,
	[UpgradeOrFall] [varchar] (1) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Status] [varchar] (1) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[Description] [varchar] (256) COLLATE Chinese_PRC_CI_AS NULL ,
	[SendTimes] [int] NOT NULL ,
	[DealTime] [datetime] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CustLevelChangeRecord] WITH NOCHECK ADD 
	CONSTRAINT [PK_CustLevelChangeRecord] PRIMARY KEY  CLUSTERED 
	(
		[CustID]
	)  ON [PRIMARY] 
GO

