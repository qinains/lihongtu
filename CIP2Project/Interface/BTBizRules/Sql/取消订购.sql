

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_Interface_CancelBySubscription]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_Interface_CancelBySubscription]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程dbo.up_BT_Interface_CancelBySubscription
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
 
 Create Procedure dbo.up_BT_Interface_CancelBySubscription
(
	@ProvinceID varchar(2),
	@TransactionID varchar(36),
	@CustID varchar(16),
	@UserAccount varchar(16),
	@SPID varchar(8),
	@ServiceID varchar(16),
	@EndTime dateTime,
	@Result int out,
	@ErrMsg varchar(100) out

)
as
	set @Result = -22500
	set @ErrMsg = ''
	
	declare @CurrentTime DateTime
	set @CurrentTime = getDate()
	
	declare @tmpCustID varchar(16)
	Set @tmpCustID= 'a'
	declare @tmpSPID varchar(8)
	set @tmpSPID = 'a'
	declare @tmpServiceID varchar(16)
	set @tmpServiceID = 'a'
	
	declare @tmpUserAccount varchar(16)
	Set @tmpUserAccount = 'a'

	declare @tmpStatus varchar(2)
	Set @tmpStatus = 'a'
	
	select @tmpCustID=CustID,@tmpUserAccount=UserAccount,@tmpSPID=SPID,@tmpServiceID = ServiceID, @tmpStatus=Status from ServiceSubscriptionInfo where TransactionID = @TransactioniD  
	if(@@RowCount <=0)
	begin
		set @Result = -20514
		set @ErrMsg = '订购记录不存在'
		return
	end
	
	if( @tmpCustID != @CustID)
	begin
		set @Result = -20514
		set @ErrMsg = '订购记录不存在，CustID不正确'
		return
	end
	
	if( @tmpSPID != @SPID)
	begin
		set @Result = -20514
		set @ErrMsg = '订购记录不存在,SPID不正确'
		return
	end
	

	if( @tmpServiceID != @ServiceID)
	begin
		set @Result = -20514
		set @ErrMsg = '订购记录不存在，ServiceID不正确'
		return
	end
	
	if( @tmpUserAccount != @UserAccount)
	begin
		set @Result = -20514
		set @ErrMsg = '订购记录不存在，UserAccount不正确'
		return
	end
	
	if(@tmpStatus != '00')
	begin
		set @Result = -21011
		set @ErrMsg = '该账号已经退订了该产品,退订请求为重复请求'
		return
	end
	
	
	
	--退订
	update ServiceSubscriptionInfo 
	set EndTime=
		case 
			when  EndTime > @EndTime then  @EndTime
			Else EndTime
		end,
		Status='01',DealTime = @CurrentTime
	where TransactionID = @TransactionID

	if(@@RowCount = 1)
		set @Result = 0

Set QUOTED_IDENTIFIER Off
go
Set ANSI_NULLS Off
go
