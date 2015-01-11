USE [BestTone]
GO
/****** ����:  StoredProcedure [dbo].[up_BT_V2_Interface_CustStatusChange]    �ű�����: 04/24/2008 16:56:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





/*
 * �洢����dbo.up_BT_V2_Interface_CustStatusChange
 *
 * ��������: 
 *			 
 *
 * Author: Li Ye
 * Company: Linkage Technology CO., LTD.
 * Create: 2008-01-02
 * Remark:Status 00 - ���� 01 - ���� 02 - ��ͣ 03 - ע��
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
		set @ErrMsg = '�޴��ʺ�'
		return
	End
	
	
	select @OriginalStatus=Status, @RealName=RealName from CustInfo Where CustID = @CustID
	select @UserAccount = UserAccount from CustInfo where  CustID = @CustID
	
	----- ���������״̬�������-----------------------------------------------
	if ( @OriginalStatus = '03')
	Begin
		set @Result = -20504
		set @ErrMsg = '�ÿ���ע�����޷�����״̬���'
		return
	End
	
	if ( @OriginalStatus = '01' and @Status = '02')
	Begin
		set @Result = -20504
		set @ErrMsg = '�޷��ɶ���̬���Ϊ��̬ͣ'
		return
	End
	
	if ( @OriginalStatus = '02' and @Status = '01')
	Begin
		set @Result = -20504
		set @ErrMsg = '�޷�����̬ͣ���Ϊ����̬'
		return
	End
	
	
	
	----- End ���������״̬�������-----------------------------------------------
	
	
	begin tran
	-- ���¿ͻ���Ϣ��	
	update custInfo set Status=@Status where CustID = @CustID	
	if @@error<>0
	begin
		set @Result = -20504
		set @ErrMsg = '���ݿ��쳣���ͻ���Ϣ�����'
		rollback
		return
	end
	
	-- ���¿ͻ��û���Ϣ��
	
	if exists (select 1 from CustUserInfo where CustID = @CustID and UserAccount = @UserAccount)
	begin
		update CustUserInfo set Status=@Status,Description = @Description where CustID = @CustID and UserAccount = @UserAccount
		if @@error<>0
		begin
			set @Result = -20504
			set @ErrMsg = '���ݿ��쳣���ͻ��û���Ϣ�����'
			rollback
			return
		end
	end
	
	-- д��ͻ�״̬�����ʷ��
	insert into CustStatusChangeRecord(CustID,UserAccount,ProvinceID,SPID,OriginalStatus,Status,Description,DealTime)
	      values(@CustID,@UserAccount,@ProvinceID,@SPID,@OriginalStatus,@Status,@Description,getdate())
	if @@error<>0
		begin
			set @Result = -20504
			set @ErrMsg = '���ݿ��쳣���ͻ�״̬�����ʷ�����'
			rollback
			return
		end
	
    commit
    

	set @Result = 0

	
END



