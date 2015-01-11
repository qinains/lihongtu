if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V2_InsertCustInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V2_InsertCustInfo]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO









/*
*SPID ����ͬ���̻���������ݣ�ÿ����Ҫ��������
*1��CustID Ϊ�գ�UserAccountΪ�գ��¿ͻ��Ǽ�	  CustID��UserAccount ���Զ����䡣 
*2��CustID Ϊ�գ�UserAccount��Ϊ�գ��ͻ��ÿ��Ǽ�  �Զ�����CustID
*select right('0000000'+cast(5 as varchar),5)
*/

Create Procedure [dbo].[up_BT_V2_InsertCustInfo]
(
 @RealName varchar(30),
 @UserAccount varchar(16),
 @EncryedPassword varchar(50),
 @CustType  varchar(2),
 @CustomType varchar(1), -- 0���� 1����
 @CustLevel varchar(1),
 @AreaID    varchar(3),
 @Sex       varchar(1),
 @CertificateType  varchar(2),
 @CertificateCode  varchar(20),
 @BirthDay  datetime,
 @MobilePhone      varchar(20),
 @MobileUserDate   datetime,
 @TelCode1         varchar(20),
 @TelCode1Date     datetime,
 @TelCode2         varchar(20),
 @TelCode2Date     datetime,
 @Email            varchar(100),
 @EduLevel         varchar(2),
 @IncomLevel       varchar(2),
 @Fav              varchar(256),
 @HCode            varchar(1),
 @RegType		   varchar(1),
 @RegSrc           varchar(2),
 @RegDate          datetime,
 @SPID			   varchar(8),
 @OUserAccount     varchar(16)   out,
 @OCustID		   varchar(16)   out,
 @Result           int           out,
 @ErrMsg           varchar(1024) out
)
-- liye
-- 
AS
BEGIN

	declare @Valid1			int
	declare @Valid2			int
	declare @InnerCardID     varchar(16)
	
	-- ����areaid
	declare @BTAreaID         varchar(3)  -- 2λ���ź����0   250
	declare @OriginalAreaCode varchar(3)  -- 2λ����ǰ���0   025
	declare @CustID			  varchar(16)
    declare @ProvinceID		  varchar(2)
    
    set @Valid1 = 0
    set @Valid2 = 0
    set @InnerCardID = ''
    
	set @BTAreaID = ''
	set @OriginalAreaCode = ''
	set @ProvinceID = ''
	set @Result = 0
	set @ErrMsg = ''

    if (len(@AreaID) = 2)
		begin
			set @BTAreaID = @AreaID + '0'
			set @OriginalAreaCode = '0'+@AreaID
		end
    else if(len(@AreaID) = 3 )
		begin
			set @BTAreaID = @AreaID
			set @OriginalAreaCode = @AreaID
		end 

    select @ProvinceID = ProvinceID from Area  where AreaID = @OriginalAreaCode
    if(@ProvinceID ='' or @ProvinceID = null)
    Begin
		Set @Result = -30001
		set @ErrMsg = '��ǰAreaID��Ч���޷�������ProvinceID'
		return
    End
    --set  @IncomLevel = '51'
	
	BEGIN tran
	-- ���������Ϣ

	--------------------------------------------------------------------------------------------------------
	if(@UserAccount != '')
	Begin
		Declare @ExeSql nvarchar(4000)
		Declare @BatchNumber int
		Declare @tmpStatus varChar(2)
		Declare @tmpAreaCode varchar(3)
		Declare @CardExpireTime DateTime
		Declare @CurrentTime	DateTime
		Set @tmpAreaCode = SubString(@UserAccount,5,3)
		Set @BatchNumber = 0
		Set @tmpStatus = 'kk'
		Set @CurrentTime = getdate()
		
		/* ����������У��

		if(@tmpAreaCode != @OriginalAreaCode)
		Begin
			Set @Result = -31008
			set @ErrMsg = '�ʺ��еĵ������봫������벻��'
			return
		End
		*/

		Set @ExeSql = ' select @BatchNumber=BatchNumber, @tmpStatus=Status, @CardExpireTime=ExpireDate from BestToneCard_'+@ProvinceID+'_'+@OriginalAreaCode + ' where CardID=''' + @UserAccount + ''''

		exec sp_executesql @ExeSql,N' @BatchNumber int output, @tmpStatus varchar(2) output, @CardExpireTime dateTime output', @BatchNumber output,@tmpStatus output, @CardExpireTime output
		if( @@rowcount <> 0 )
		Begin
			Set @Result = -31001
			set @ErrMsg = '�����Ѿ����ڣ����ܵ���'
			Rollback
			return
		End
		/*
		if(@tmpStatus = '01')
		Begin
			Set @Result = -30001
			set @ErrMsg = '�˿���ʹ��'
			rollback
			return
		End
		
		if( @CardExpireTime < @CurrentTime )
		Begin
			Set @Result = -21528
			set @ErrMsg = '�˿��ѹ���'
			rollback
			return
		End

		if(@tmpStatus = '02')
		Begin
			Set @Result = -21528
			set @ErrMsg = '�˿��Ѷ���'
			rollback
			return
		End
		
		if(@tmpStatus = '03')
		Begin
			Set @Result = -21528
			set @ErrMsg = '�˿��ѷ���'
			rollback
			return
		End
		
		if(@BatchNumber =0)
		Begin
			Set @Result = -30008
			set @ErrMsg = '�޴˿�'
			rollback
			return
		End
		
		declare @tmpBatchStatus varchar(2)
		set @tmpBatchStatus = 'kk'
		Select @tmpBatchStatus =BatchStatus from BestToneCardBatch where BatchNumber=@BatchNumber

		if(@tmpBatchStatus = 'kk')
		Begin
			Set @Result = -30008
			set @ErrMsg = '�޴˿�����'
			rollback
			return
		End
		
		if(@tmpBatchStatus = '00')
		Begin
			Set @Result = -30008
			set @ErrMsg = '�����ο�δ�������ע��'
			rollback
			return
		End
		
		if(@tmpBatchStatus = '02')
		Begin
			Set @Result = -30008
			set @ErrMsg = '�����ο��Ѷ��ᣬ����ע��'
			rollback
			return
		End
		
		if(@tmpBatchStatus = '03')
		Begin
			Set @Result = -30008
			set @ErrMsg = '�����ο��ѷ���������ע��'
			rollback
			return
		End
		
		if(@tmpBatchStatus = '04')
		Begin
			Set @Result = -30008
			set @ErrMsg = '�����ο���ɾ��������ע��'
			rollback
			return
		End
        */
		
		select @CustID = SequenceID+1 from ParSequenceNumber with(updlock)where SequenceType = '0101'
		update ParSequenceNumber set SequenceID = @CustID where SequenceType = '0101'
		if ( @@error <> 0)
		begin
			set @Result = -22500
			set @ErrMsg = '�Զ�����CustIDʧ��'
			rollback
			return
		end

		
		set @OCustID = @CustID
		set @OUserAccount = @UserAccount
		set @InnerCardID = '8600'+@UserAccount
	End
	

	--���û�������Զ�Ϊ�ͻ����ɿ�
	---------------------------------------------------------------------------------------------------
	if(@UserAccount ='')
	Begin
		--Set @RegistrationType = '1'
		
		declare @cardSerial	int  --���� i~mλ		
		declare @cardPre	varchar(8)
		declare @i	int
		declare @retainStr char(1)
		set @i = 0
		set @retainStr = '1'     -- H λ�� 1 
		-- 0~4 �����һλ
	
		--����SPID��status���ж������ڸ���ͨ������ͻ��������ÿ��û��Լ�hλ��ȡֵ
		--����SPID���ж�hλ��ȡֵ,Ŀǰֻ������ͻ�
		/*
		if(@SPID='35000001')
			Begin
				set @retainStr = '1'
			End
		else
			Begin
				set @Result = -20000
				set @ErrMsg = '������Ч��spid'
				return
			End		
		*/

		set @CustID=''
				
		set @cardPre = '86'+'0'+'0'+@BTAreaID+@retainStr
		
		while(1=1)
		begin
			select @cardSerial=SequenceID+1 from AutoDistributeSequence with(updlock) where ProvinceID=@ProvinceID and AreaID=@OriginalAreaCode
			if (@cardSerial =100000)
			Begin
					set @Result = -22500
					set @ErrMsg = '�Զ����俨��Դ���������俨ʧ��'
					rollback
					return
			End
			update AutoDistributeSequence 
			set SequenceID=@cardSerial
			where ProvinceID=@ProvinceID and AreaID=@OriginalAreaCode

			select @CustID = SequenceID+1 from ParSequenceNumber with(updlock)where SequenceType = '0101'
			update ParSequenceNumber set SequenceID = @CustID where SequenceType = '0101'
			if ( @@error <> 0)
			begin
					set @Result = -22500
					set @ErrMsg = '�Զ�����CustIDʧ��'
					rollback
					return
			end
			
			if exists (select 1 from PreservedCardRule where ProvinceID=@ProvinceID and AreaID=@OriginalAreaCode and charindex(PreservedNumber,@cardSerial)>0)
			Begin       
				continue
			End
			--set @password = Convert(varchar(6),convert(decimal,RAND()*1000000))
			
			if(len(@cardSerial)=5 )
			begin
				--ȡ�ÿ���
				set @InnerCardID = Convert(varchar,@cardPre)+convert(varchar,@cardSerial)+'000'
                set @UserAccount = @BTAreaID+@retainStr+convert(varchar,@cardSerial)

				if exists (select 1 from CustInfo where UserAccount=@UserAccount)
				begin
					continue
				end

				--�������Ƿ��Ѿ�����				
				Set @ExeSql = ' select 1 from BestToneCard_'+@ProvinceID+'_'+@OriginalAreaCode + ' where CardID=''' + @UserAccount + ''''
				Exec(@ExeSql)
				if(@@RowCount>0)
				begin
					continue
				end			
				--Set @CustID=@UserAccount
				set @OCustID = @CustID
				set @OUserAccount = @UserAccount
				break
			end
			else --������ɵ����кŲ���8λ����������
				continue
		end
	End
	
	if(len(@UserAccount) < 9 or len(@UserAccount)>16)
	Begin
		set @Result = -30008
		set @ErrMsg = '���ɿ���ʧ��'
		rollback
		return
	End

	--У��CustID,UserAccount
	if exists (select 1 from CustInfo where CustID = @CustID or UserAccount=@UserAccount)
	Begin
		set @Result = -30001
		set @ErrMsg = '�ͻ�id���û��ʺ��Ѵ���' 
		rollback
		return
	End
	

	-- У��CustUserInfo��
	if exists (select 1 from CustUserInfo  where CustID = @CustID or UserAccount=@UserAccount or InnerCardID = @InnerCardID)
	begin
		set @Result = -30001
		set @ErrMsg = '�ͻ�id���û��ʺ���CustUserInfo���Ѵ���' 
		rollback
		return
	end

	-- У��֤������@CertificateType
	if ( @CertificateType <> '' and @CertificateType is not null)
	begin
		if exists (select 1 from CustInfo where CertificateType = @CertificateType and CertificateCode = @CertificateCode)
		begin
			set @Result = -22500
			set @ErrMsg = '��ǰ֤�������Ѿ�ע�ᣬ������ע��'
			Rollback
			return
		end
	end
	

	if exists (select 1 from CustInfo where RealName = @RealName)
	begin
	   set @Valid1=1
	end
	if exists (select 1 from CustExtend where CustContactTel = @MobilePhone)
	begin
		set @Valid2=1
	end
	
	if( @Valid1=1 and @Valid2=1)
	begin
		set @Result = -22500
		set @ErrMsg = '��ǰ�����ֻ����������ݿ����Ѿ�����'
		Rollback
		return
	end	

	--����ͻ���Ϣ��	
	insert into CustInfo( CustID,UserAccount,CustType,EncryptedPassword,CertificateType,CertificateCode,RealName,
		ProvinceID,AreaID,CustLevel,RegistrationSource,RegistrationType,RegistrationDate,Status,DealTime )
		values (@CustID,@UserAccount,@CustType,@EncryedPassword,@CertificateType,@CertificateCode,@RealName,
		@ProvinceID,@OriginalAreaCode,@CustLevel,@RegSrc,@RegType,getdate(),'00', getdate())

	if(@@Error<>0)
	Begin
		set @Result = -22500
		set @ErrMsg = '����CustInfoʱ����'
		Rollback
		return
	End
	-- ����ͻ��û���Ϣ��
	------------------------------------------------------------------------------------------------------------------------
	if not exists (select 1 from CustUserInfo where UserAccount=@UserAccount)
	begin
		insert into CustUserInfo(CustID,SPID,InnerCardID,UserAccount,RegistrationSource,RegistrationType,RegistrationDate,Status,DealTime)
							values(@CustID,@SPID,@InnerCardID,@UserAccount,@RegSrc,@RegType,getdate(),'00',getdate())
		if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '����CustUserInfoʱ����'
			Rollback
			return
		End
	end
	else
	begin
		set @Result = -30001
		set @ErrMsg = 'UserAccount�� CustUserInfo�����Ѿ�����'
		Rollback
		return
	end

	-- ������չ��Ϣ
	------------------------------------------------------------------------------------------------------------------------------------------
	if exists (select 1 from CustExtend where CustID = @CustID)
	begin
		set @Result = -22500
		set @ErrMsg = '�޷�����ͻ���չ��Ϣ��CustID�Ѿ�����'
		Rollback
		return
	end
	
	insert into CustExtend(CustID,UserAccount,Sex,BirthDay,EduLevel,Favorites,InComeLevel,Email,CustContactTel,DealTime)
	      values(@CustID,@UserAccount,@Sex,@BirthDay,@EduLevel,@Fav,@IncomLevel,@Email,@MobilePhone,getdate())
	      
	if(@@Error<>0)
	Begin
		set @Result = -22500
		set @ErrMsg = '����CustInfoʱ����'
		Rollback
		return
	End
	

	
			
	-- ����绰�������Ϣ ���۰�绰�Ƿ�ɹ����Կͻ�ע�ᣨ�������������ٻ���
	-----------------------------------------------------------------------------------------------------------------------------------------------
	--Declare @BoundErrMsg varchar(1024)
	Declare @BoundDate datetime
	--set @BoundErrMsg = ''
	--set @BoundDate = getdate()
	
	if not exists ( select 1 From CustInfo Where CustID = @CustID and UserAccount = @UserAccount)
	Begin
		set @Result = 1
		set @ErrMsg = ' ����󶨵绰ʧ�ܣ��޴��ʺ�'
		commit
		return 
	End
	
	select 1 from BoundPhone where CustID = @CustID
	if(@@RowCount>=5)
	Begin
		set @Result = 1
		set @ErrMsg = ' ���û������󶨵绰��������'
		commit
		return
	End
	
	---  bound mobilephone------------------
	IF(@MobilePhone <> '' and @MobilePhone is not null)
	BEGIN
	if exists( select 1 from BoundPhone where BoundPhoneNumber = @MobilePhone)
	Begin
		select @BoundDate = DealTime from BoundPhone where BoundPhoneNumber = @MobilePhone
		if( @MobileUserDate > @BoundDate)
		begin
			update BoundPhone set CustID=@CustID,UserAccount=@UserAccount,DealTime=getdate() where BoundPhoneNumber = @MobilePhone
			if(@@Error<>0)
				Begin
					set @Result = 1
					set @ErrMsg = @ErrMsg+ ' ���°��ֻ��������'				
				End
		end	
		else
		begin
			set @Result = 1
			set @ErrMsg = @ErrMsg+' �ֻ������ѱ�ʹ�� '	
		end		
	End
	else
	Begin
		insert into BoundPhone (CustID,UserAccount,BoundPhoneNumber,DealTime) values(@CustID,@UserAccount,@MobilePhone,getdate())
		if(@@Error<>0)
				Begin
					set @Result = 1
					set @ErrMsg = @ErrMsg+ ' ���ֻ��������'				
				End
	End
	END  -- for BEGIN
	--- End bound mobilephone
	
	--- Bound @TelCode1-----
	IF(@TelCode1 <> '' and @TelCode1 is not null)
	BEGIN
	select 1 from BoundPhone where CustID = @CustID
	if(@@RowCount>=5)
	Begin
		set @Result = 1
		set @ErrMsg = @ErrMsg+ ' ���û������󶨵绰��������,���ܰ󶨵绰1'
		commit
		return
	End
	
	if exists (select 1 from BoundPhone where BoundPhoneNumber = @TelCode1)
	Begin
		select @BoundDate = DealTime from BoundPhone where BoundPhoneNumber = @TelCode1
		if( @TelCode1Date > @BoundDate)
		begin
			update BoundPhone set CustID=@CustID,UserAccount=@UserAccount,DealTime=getdate() where BoundPhoneNumber = @TelCode1
			if(@@Error<>0)
				Begin
					set @Result = 1
					set @ErrMsg = @ErrMsg+'  ���°󶨵绰����1����'				
				End
		end	
		else
		begin
			set @Result = 1
			set @ErrMsg = @ErrMsg+' �绰����1�ѱ�ʹ�� '	
		end				
	End
	else
	Begin
		insert into BoundPhone (CustID,UserAccount,BoundPhoneNumber,DealTime) values(@CustID,@UserAccount,@TelCode1,getdate())
		if(@@Error<>0)
				Begin
					set @Result = 1
					set @ErrMsg = @ErrMsg+ ' �󶨵绰����1����'				
				End
	
	End
	END
	--- End Bound @TelCode1-----------------------

	--- Bound TelCode2-----------------------------
	IF (@TelCode2 <>'' AND @TelCode2 is not null)
	BEGIN
	select 1 from BoundPhone where CustID = @CustID
	if(@@RowCount>=5)
	Begin
		set @Result = 1
		set @ErrMsg = @ErrMsg+ ' ���û������󶨵绰��������,���ܰ󶨵绰2'
		commit
		return
	End
	
	if exists (select 1 from BoundPhone where BoundPhoneNumber = @TelCode2)
	Begin
		select @BoundDate = DealTime from BoundPhone where BoundPhoneNumber = @TelCode2
		if( @TelCode2Date > @BoundDate)
		begin
			update BoundPhone set CustID=@CustID,UserAccount=@UserAccount,DealTime=getdate() where BoundPhoneNumber = @TelCode2
			if(@@Error<>0)
				Begin
					set @Result = 1
					set @ErrMsg = @ErrMsg+' ���°󶨵绰����2����'				
				End
		end	
		else
		begin
			set @Result = 1
			set @ErrMsg = @ErrMsg+' �绰����2�ѱ�ʹ�� '	
		end				
	End
	else
	Begin
		insert into BoundPhone (CustID,UserAccount,BoundPhoneNumber,DealTime) values(@CustID,@UserAccount,@TelCode2,getdate())
		if(@@Error<>0)
				Begin
					set @Result = 1
					set @ErrMsg = @ErrMsg+ ' �󶨵绰����2����'				
				End
	
	End
	END
	---End Bound TelCode2 ------------------------


	Commit  -- End For Tran
END








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

