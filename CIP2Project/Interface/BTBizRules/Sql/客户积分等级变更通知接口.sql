if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V2_Interface_IntegralTigerGrade]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V2_Interface_IntegralTigerGrade]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





/*
 * �洢����dbo.up_BT_V2_Interface_IntegralTigerGrade
 *
 * ��������: ���ֱ��֪ͨ�ӿ� ��ע���ͻ���֤���Ķ����Զ��ţ��ʼ����绰֪ͨ�ȷ�ʽ��ʾ�û�����������
 * ��һ��ݷ�һ�Σ����������Σ�������
 *
 * sendTimes < 3
 * currtime-dealtime>7day *			 
 *
 * Author: Li Ye
 * Company: Linkage Technology CO., LTD.
 * Create: 2008-01-03
 * Remark: 
 * Result 0-���� 1-���� 2-���𲻱�
 * ����ʱ��ֻ�������Ͷ�������²ż�¼��֪ͨ�����ݿ�
 * 1=��ʯ���ͻ����ߣ���2=���𿨿ͻ�(��)��3=ˮ�����ͻ����ͣ���0=�޼��𿨿ͻ���ͬ3��
 *
 * �ݲ�֧������
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
	declare @CurrCustLevel varchar(1) --�ͻ����ǰ����
	declare @MaxLevel varchar(1)      --�ͻ���ʷ��߼���
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
		set @ErrMsg = '�ͻ������ڻ��Ѷ��ᡢע��'  
		return
	end
	
	-- ���𲻱�
	select @CurrCustLevel = CustLevel from CustInfo  where CustID = @CustID
	if( @CurrCustLevel = 0)
		begin
			set @CurrCustLevel='4'
		end
	if(@CurrCustLevel = @UpgradeOrFall)
	begin
		set @Result = 2
		set @ErrMsg = '�ͻ����Ǹü��𣬲���Ҫ����'
		-- �Ƿ���Ҫ�ڱ�CustLevelChangeRecord�м�¼��
		return
	end
	
	-- ���� �ݲ�֧��  
	if(@CurrCustLevel < @UpgradeOrFall)
	begin
		update CustLevelChangeRecord set sendTimes = 0 ,status = '0' where CustID = @CustID
		set @Result = 1
		set @ErrMsg = '�ͻ��������ݲ�֧��'
		-- �Ƿ���Ҫ�ڱ�CustLevelChangeRecord�м�¼�����ÿͻ���Ӧ��sendTimes �� 0 ��
		return
	end
	
	-- ����ҵ���߼� -- ÿ�ζ�����һ����¼
	if not exists (select 1 from CustLevelChangeRecord where CustID = @CustID and UpgradeOrFall =@UpgradeOrFall)
	begin
		insert into CustLevelChangeRecord(CustID,ProvinceID,SPID,IntegralInfo,UpgradeOrFall,Status,Description,DealTime,SendTimes)
							values(@CustID,@ProvinceID,@SPID,@IntegralInfo,@UpgradeOrFall,'0',' ',getdate(),1)
		set @Result = 0 
		--set @ErrMsg = '��¼���ɹ�'  
		select @ErrMsg = RealName from CustInfo where CustID = @CustID
		return  
	end	
	
	-- ��ȡ�����¼�¼ǰ ���ʹ�������ͬ����������ʱ�䡢״̬
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
		set @ErrMsg = '�Ѿ��ﵽ����ʹ��������ܷ���'
		return  
	end
	
	set @PrevDealTime = dateadd(day,7,@PrevDealTime)
	if(@PrevDealTime > @CurrentTime)
	begin
		set @Result = -22504  
		set @ErrMsg = '7�����Ѿ����͹�һ�Σ����ܷ���'
		return  
	end	
	
	set @TmpErrMsg = rtrim(@TmpErrMsg)
	if((@TmpErrMsg = '') or (@TmpErrMsg = null))
	begin
		set @TmpErrMsg='���ܷ��ͣ�ԭ��δ֪'
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
		set @ErrMsg = '�û��Ѿ��������'
		return  
	end
	-- ���Է��ͣ������¼
	
	 begin tran
		insert into CustLevelChangeRecord(CustID,ProvinceID,SPID,IntegralInfo,UpgradeOrFall,Status,Description,DealTime,SendTimes)
							values(@CustID,@ProvinceID,@SPID,@IntegralInfo,@UpgradeOrFall,'1',' ',getdate(),@TmpSendTimes+1)	
		if(@@error<>0)
		begin
			set @Result = -22504
			set @ErrMsg = '������ֵȼ������¼����'
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

