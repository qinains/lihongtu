

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V2_Interface_InsertCustInfoNotifyFailRecord]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V2_Interface_InsertCustInfoNotifyFailRecord]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程dbo.up_BT_V2_Interface_InsertCustInfoNotifyFailRecord
 *
 * 功能描述: 
 *			 
 *
 * Author: 
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-12-20
 * Remark:
 *
 */

 
 Create Procedure dbo.up_BT_V2_Interface_InsertCustInfoNotifyFailRecord
(
	@CustID varchar(16),
	@UserAccount varchar(16),
	@Result int,
	@ErrMsg varchar(256)
)
as
	insert into CustInfoNotifyFailRecord ( CustID, UserAccount,Result, Description, DealTime)
		values(@CustID, @UserAccount,@Result, @ErrMsg, GetDate())
