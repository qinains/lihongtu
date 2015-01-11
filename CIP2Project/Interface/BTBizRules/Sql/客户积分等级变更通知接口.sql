if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V2_Interface_IntegralTigerGrade]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V2_Interface_IntegralTigerGrade]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





/*
 * 存储过程dbo.up_BT_V2_Interface_IntegralTigerGrade
 *
 * 功能描述: 积分变更通知接口 备注：客户认证中心定期以短信，邮件，电话通知等方式提示用户可以升级。
 * （一礼拜发一次，不超过三次）条件：
 *
 * sendTimes < 3
 * currtime-dealtime>7day *			 
 *
 * Author: Li Ye
 * Company: Linkage Technology CO., LTD.
 * Create: 2008-01-03
 * Remark: 
 * Result 0-升级 1-降级 2-级别不变
 * 升级时，只有允许发送短信情况下才记录该通知到数据库
 * 1=钻石卡客户（高），2=铂金卡客户(中)，3=水晶卡客户（低），0=无级别卡客户（同3）
 *
 * 暂不支持信用
 *
 */

Create Procedure [dbo].[up_BT_V2_Interface_IntegralTigerGrade]
(
 @CustID varchar(16),
 @ProvinceID varchar(2),
 @SPID varchar(8),
 @IntegralInfo bigint,
 @GradeOrCredit varchar(2),
 @UpgradeOrFall varchar(2),
 @Result int out,           
 @ErrMsg varchar(256) out
)

AS
BEGIN
	declare @TmpSendTimes int
	declare @PrevDealTime datetime
    declare @Status varchar
	declare @CurrentTime  datetime
	declare @TmpErrMsg varchar(256)
	declare @CurrCustLevel varchar(1) --客户变更前级别
	declare @MaxLevel varchar(1)      --客户历史最高级别
	declare @MaxDealTime datetime
	
	
	set @Result = -22500  
	set @ErrMsg = ''  
	set @TmpErrMsg = ' '
	set @CurrCustLevel = ''
	set @MaxLevel = ''
	set @MaxDealTime = getdate()
	
	
	if not exists (select 1 from CustInfo where CustID = @CustID and Status = '00')
	begin
		set @Result = -22504  
		set @ErrMsg = '客户不存在或已冻结、注销'  
		return
	end
	
	-- 级别不变
	select @CurrCustLevel = CustLevel from CustInfo  where CustID = @CustID
	if( @CurrCustLevel = 0)
		begin
			set @CurrCustLevel='4'
		end
	if(@CurrCustLevel = @UpgradeOrFall)
	begin
		set @Result = 2
		set @ErrMsg = '客户已是该级别，不需要处理'
		-- 是否需要在表CustLevelChangeRecord中记录？
		return
	end
	
	-- 降级 暂不支持  
	if(@CurrCustLevel < @UpgradeOrFall)
	begin
		update CustLevelChangeRecord set sendTimes = 0 ,status = '0' where CustID = @CustID
		set @Result = 1
		set @ErrMsg = '客户降级，暂不支持'
		-- 是否需要在表CustLevelChangeRecord中记录？将该客户对应的sendTimes 置 0 ，
		return
	end
	
	-- 升级业务逻辑 -- 每次都新增一条记录
	if not exists (select 1 from CustLevelChangeRecord where CustID = @CustID and UpgradeOrFall =@UpgradeOrFall)
	begin
		insert into CustLevelChangeRecord(CustID,ProvinceID,SPID,IntegralInfo,UpgradeOrFall,Status,Description,DealTime,SendTimes)
							values(@CustID,@ProvinceID,@SPID,@IntegralInfo,@UpgradeOrFall,'0',' ',getdate(),1)
		set @Result = 0 
		--set @ErrMsg = '记录入库成功'  
		select @ErrMsg = RealName from CustInfo where CustID = @CustID
		return  
	end	
	
	-- 获取插入新记录前 发送次数、相同级别最后操作时间、状态
	select @MaxDealTime = max(DealTime) from CustLevelChangeRecord where CustID = @CustID and UpgradeOrFall = @UpgradeOrFall
	
	select top 1 @TmpSendTimes = SendTimes from CustLevelChangeRecord where CustID = @CustID and UpgradeOrFall =@UpgradeOrFall 
		and DealTime = @MaxDealTime		
	
	select top 1 @PrevDealTime = DealTime from CustLevelChangeRecord where CustID = @CustID and UpgradeOrFall =@UpgradeOrFall
				and DealTime = @MaxDealTime			
	
	select @Status = Status from CustLevelChangeRecord where CustID = @CustID and UpgradeOrFall =@UpgradeOrFall
		and DealTime = @MaxDealTime
		
	select @TmpErrMsg = ltrim(Description) from CustLevelChangeRecord where CustID = @CustID 
			and DealTime = @MaxDealTime		
		
	if(@TmpSendTimes='' or @TmpSendTimes=null)
	begin
		set @TmpSendTimes=0
	end
					
	set @PrevDealTime = convert(varchar(10),@PrevDealTime,120)
	set @CurrentTime = convert(varchar(10),getdate(),120)	
		
	if(@TmpSendTimes >= 3)
	begin
		set @Result = -22504 
		set @ErrMsg = '已经达到最大发送次数，不能发送'
		return  
	end
	
	set @PrevDealTime = dateadd(day,7,@PrevDealTime)
	if(@PrevDealTime > @CurrentTime)
	begin
		set @Result = -22504  
		set @ErrMsg = '7天内已经发送过一次，不能发送'
		return  
	end	
	
	set @TmpErrMsg = rtrim(@TmpErrMsg)
	if((@TmpErrMsg = '') or (@TmpErrMsg = null))
	begin
		set @TmpErrMsg='不能发送，原因未知'
	end

	if(@Status = '3')
	begin
		set @Result = -22503 	
		set @ErrMsg = @TmpErrMsg
		return  
	end

	if(@Status = '2')
	begin
		set @Result = -22504
		set @ErrMsg = '用户已经升级完毕'
		return  
	end
	-- 可以发送，插入记录
	
	 begin tran
		insert into CustLevelChangeRecord(CustID,ProvinceID,SPID,IntegralInfo,UpgradeOrFall,Status,Description,DealTime,SendTimes)
							values(@CustID,@ProvinceID,@SPID,@IntegralInfo,@UpgradeOrFall,'1',' ',getdate(),@TmpSendTimes+1)	
		if(@@error<>0)
		begin
			set @Result = -22504
			set @ErrMsg = '插入积分等级变更记录错误'
			Rollback
			return  
		end
	commit    
    
	select @ErrMsg = RealName from CustInfo where CustID = @CustID
	set @Result = 0
	
END




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

