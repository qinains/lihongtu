SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO







/*
 * 存储过程[dbo].[up_BT_V2_Interface_CustLevelChange]
 *
 * 功能描述: 客户升级请求			 
 *
 * Author: Li Ye
 * Company: Linkage Technology CO., LTD.
 * Create: 2008-01-02
 * Remark:
 *
 */
ALTER  procedure [dbo].[up_BT_V2_Interface_CustLevelChange]
(
@CustID     varchar(16),
@UserAccount varchar(16),
@ProvinceID varchar(2),
@SPID	    varchar(8),
@CustLevel  varchar(1) out,
@Result      int out,
@ErrMsg      varchar(256) out
)

AS
BEGIN
	
    declare @TmpStatus varchar
	declare @TmpErrMsg varchar(256)
	declare @UpgradeOrFall varchar(1)
	declare @CustInfoStatus varchar(2)
	declare @MaxDateTime datetime

	set @TmpStatus=''
	set @Result = -22500  
	set @ErrMsg = ''  
	set @TmpErrMsg = ''
	set @UpgradeOrFall= ''
	set @CustInfoStatus = ''
	set @MaxDateTime = getdate()
	
	if not exists (select 1 from CustLevelChangeRecord where CustID=@CustID )
	begin
		set @Result = -22504  
		set @ErrMsg = '没有该客户级别变更信息'  
		return
	end
	
	select @MaxDateTime = max(DealTime) from CustLevelChangeRecord where CustID = @CustID	
	select @TmpStatus=Status from CustLevelChangeRecord where CustID=@CustID 
							and DealTime = @MaxDateTime
	select @TmpErrMsg = ltrim(Description) from CustLevelChangeRecord where CustID = @CustID
							and DealTime = @MaxDateTime
	set @TmpErrMsg = rtrim(@TmpErrMsg)
	if((@TmpErrMsg = '') or (@TmpErrMsg = null))
	begin
		set @TmpErrMsg='不能升级原因未知'
	end
	if(@TmpStatus = '3')
	begin
		set @Result = -22504  
		set @ErrMsg = @TmpErrMsg
		return
	end
	
	if(@TmpStatus = '2')
	begin
		set @Result = -22504  
		set @ErrMsg = '客户已经升级完毕'
		return
	end
	
	if not exists (select 1 from CustInfo where CustID = @CustID )
	begin
		set @Result = -22504  
		set @ErrMsg = '该客户不存在'  
		return
	end
	select @CustInfoStatus = Status from CustInfo where CustID = @CustID 
	if(@CustInfoStatus <> '00')
	begin
		set @Result = -22504  
		set @ErrMsg = '客户已经冻结、暂停或注销，无法升级'  
		return
	end

	begin tran
		select top 1 @UpgradeOrFall = UpgradeOrFall from CustLevelChangeRecord where CustID=@CustID and DealTime = @MaxDateTime
		update 	CustInfo set CustLevel = @UpgradeOrFall , DealTime = getdate() where CustID = @CustID 
		if(@@error <> 0)
		begin
			set @Result = -22504  
			set @ErrMsg = '系统异常，升级失败'  
			rollback
			return
		end
		
		set @CustLevel = @UpgradeOrFall
		--更新客户升级变更表状态 为已升级	2
		update CustLevelChangeRecord set Status = '2' , DealTime = getdate() where CustID=@CustID 
											and DealTime = @MaxDateTime
		if(@@error <> 0)
		begin
			set @Result = -22504  
			set @ErrMsg = '系统异常，升级失败'  
			rollback
			return
		end

		--更新 状态为升级卡，须制卡
		Update CustUserInfo set IsHaveCard='3' where CustID = @CustID and UserAccount = @UserAccount
											
	commit 
	select @ErrMsg = RealName from CustInfo where CustID = @CustID
	set @Result = 0
END






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

