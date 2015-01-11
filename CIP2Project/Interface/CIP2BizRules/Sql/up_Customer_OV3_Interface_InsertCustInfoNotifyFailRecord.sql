USE [CIP2]
GO
/****** 对象:  StoredProcedure [dbo].[up_Customer_OV3_Interface_InsertCustInfoNotifyFailRecord]    脚本日期: 10/22/2009 15:36:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER procedure [dbo].[up_Customer_OV3_Interface_InsertCustInfoNotifyFailRecord] (
 	@CustID varchar(16),
 	@ProvinceID varchar(2),
 	@CustType varchar(2),
 	@Result int,
    @DealType int,
 	@ToSPID varchar(8),
 	
 	@Description varchar(256)
 ) as
insert into CustInfoNotify (CustID,ProvinceID,CustType,OPType,Result,ToSPID,DealTime,Description,NotifyCount,DealType)
    values (@CustID,@ProvinceID,@CustType,'2',@Result,@ToSPID,getdate(),@Description,0,@DealType)
