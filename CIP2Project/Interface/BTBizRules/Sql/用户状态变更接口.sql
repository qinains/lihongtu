USE [BestTone]
GO
/****** 对象:  StoredProcedure [dbo].[up_BT_V2_Interface_CustStatusChange]    脚本日期: 04/24/2008 16:56:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





/*
 * 存储过程dbo.up_BT_V2_Interface_CustStatusChange
 *
 * 功能描述: 
 *			 
 *
 * Author: Li Ye
 * Company: Linkage Technology CO., LTD.
 * Create: 2008-01-02
 * Remark:Status 00 - 正常 01 - 冻结 02 - 暂停 03 - 注销
 *
 */
 
ALTER  Procedure [dbo].[up_BT_V2_Interface_CustStatusChange]
(	
	@CustID varchar(16),
	@SPID varchar(8),	
	@ProvinceID varchar(2),
	@Status varchar(2),
	@Description varchar(256),
	@Result int out,
	@ErrMsg varchar(256) out,
	@RealName varchar(30) out

)
AS
BEGIN
	
	declare @UserAccount varchar(16)
	declare @OriginalStatus varchar(2)
	
	set @UserAccount = ''
	set @OriginalStatus = ''
	set @Result = -22500
	set @ErrMsg = ''
	set @RealName = ''

	if not exists ( select 1 From CustInfo Where CustID = @CustID)
	Begin
		set @Result = -20504
		set @ErrMsg = '无此帐号'
		return
	End
	
	
	select @OriginalStatus=Status, @RealName=RealName from CustInfo Where CustID = @CustID
	select @UserAccount = UserAccount from CustInfo where  CustID = @CustID
	
	----- 不被允许的状态变更操作-----------------------------------------------
	if ( @OriginalStatus = '03')
	Begin
		set @Result = -20504
		set @ErrMsg = '该卡已注销，无法进行状态变更'
		return
	End
	
	if ( @OriginalStatus = '01' and @Status = '02')
	Begin
		set @Result = -20504
		set @ErrMsg = '无法由冻结态变更为暂停态'
		return
	End
	
	if ( @OriginalStatus = '02' and @Status = '01')
	Begin
		set @Result = -20504
		set @ErrMsg = '无法由暂停态变更为冻结态'
		return
	End
	
	
	
	----- End 不被允许的状态变更操作-----------------------------------------------
	
	
	begin tran
	-- 更新客户信息表	
	update custInfo set Status=@Status where CustID = @CustID	
	if @@error<>0
	begin
		set @Result = -20504
		set @ErrMsg = '数据库异常，客户信息表出错'
		rollback
		return
	end
	
	-- 更新客户用户信息表
	
	if exists (select 1 from CustUserInfo where CustID = @CustID and UserAccount = @UserAccount)
	begin
		update CustUserInfo set Status=@Status,Description = @Description where CustID = @CustID and UserAccount = @UserAccount
		if @@error<>0
		begin
			set @Result = -20504
			set @ErrMsg = '数据库异常，客户用户信息表出错'
			rollback
			return
		end
	end
	
	-- 写入客户状态变更历史表
	insert into CustStatusChangeRecord(CustID,UserAccount,ProvinceID,SPID,OriginalStatus,Status,Description,DealTime)
	      values(@CustID,@UserAccount,@ProvinceID,@SPID,@OriginalStatus,@Status,@Description,getdate())
	if @@error<>0
		begin
			set @Result = -20504
			set @ErrMsg = '数据库异常，客户状态变更历史表出错'
			rollback
			return
		end
	
    commit
    

	set @Result = 0

	
END



