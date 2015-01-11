

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_Interface_UserSubScribe]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_Interface_UserSubScribe]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程dbo.up_BT_Interface_UserSubScribe
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-4-17
 * Remark:
 *
 */
 
 Create Procedure dbo.up_BT_Interface_UserSubScribe
(
	@ProvinceID varchar(2),
	@TransactionID varchar(36),
	@SubScribeStyle varchar(1),
	@CustID varchar(16),
	@UserAccount varchar(16),
	@SPID varchar(8),
	@ServiceID varchar(16),
	@ServiceName varchar(100),
	@Fee int,
	@SubscribeDate dateTime,
	@StartTime dateTime,
	@EndTime dateTime,
	@Result int out,
	@ErrMsg varchar(100) out

)
as
	set @Result = -22500
	set @ErrMsg = ''
	
	declare @CurrentTime DateTime
	set @CurrentTime = getDate()
	
	if not Exists( select 1 from CustInfo where CustID = @CustID and UserAccount = @UserAccount )
	begin
		set @Result = -20504
		set @ErrMsg = '用户帐号不存在'
		return
	end
	
	
	if Exists(select 1 from ServiceSubscriptionInfo where TransactionID = @TransactioniD )
	begin
		set @Result = -30006
		set @ErrMsg = '交易号已存在'
		return
	end

	if Exists( select 1 from ServiceSubscriptionInfo where CustID = @CustID and UserAccount= @UserAccount and ServiceID = @ServiceID and ( @StartTime Between StartTime And EndTime ) )
	begin
		set @Result = -21010
		set @ErrMsg = '重复订购'
		return
	end
	
	Insert Into ServiceSubscriptionInfo(CustID,UserAccount,SPID,ServiceID,ServiceName,TransactionID,Fee,SubscribeDate,
		StartTime,EndTime,DealTime,Status,RecordSource,SubScribeStyle)
	values(@CustID,@UserAccount,@SPID,@ServiceID,@ServiceName,@TransactionID,@Fee,@SubscribeDate,
		@StartTime,@EndTime,@CurrentTime,'00','01',@SubScribeStyle )

	if(@@RowCount = 1)
		set @Result = 0

Set QUOTED_IDENTIFIER Off
go
Set ANSI_NULLS Off
go
