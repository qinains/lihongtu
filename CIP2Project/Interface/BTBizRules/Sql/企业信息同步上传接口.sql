if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V2_Interface_EnterpriseInfoUpLoad]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V2_Interface_EnterpriseInfoUpLoad]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
 * 存储过程dbo.up_BT_V2_Interface_EnterpriseInfoUpLoad
 *
 * 功能描述: 
 *			 
 *
 * Author: Li Ye
 * Company: Linkage Technology CO., LTD.
 * Create: 2008-01-02
 * Remark: 政企卡 ProvinceId = '35' areacode = '35'
 * -----------------------------------------------------------------------------------------
 * insert into AutoDistributeSequence(areaid,provinceid,Sequenceid) values('035','35',80000)
 * ADD Table BestToneCard_35_035
 */

Create Procedure [dbo].[up_BT_V2_Interface_EnterpriseInfoUpLoad]
(
 @EnterpriseID    varchar(30),
 @EnterpriseName  varchar(50),
 @EnterpriseType  varchar(2),
 @CustID          varchar(16),
 @UserAccount     varchar(16),
 @EncryedPassword varchar(50),
 @OCustID         varchar(16) out,
 @OUserAccount    varchar(16) out,
 @Result          int         out,
 @ErrMsg          varchar(1024) out
)
AS

BEGIN

set @OCustID = ''
set @OUserAccount = ''
set @Result = 0
set @ErrMsg = ''

begin tran
	if exists (select 1 from EnterpriseInfo where EnterpriseID = @EnterpriseID)
	Begin
		select @OCustID = CustID , @OUserAccount = UserAccount from EnterpriseInfo where EnterpriseID = @EnterpriseID
		if not exists ( select 1 from EnterpriseInfo where EnterpriseID = @EnterpriseID and EnterpriseName = @EnterpriseName and EnterpriseType = @EnterpriseType)
			begin
				update EnterpriseInfo set  EnterpriseName = @EnterpriseName ,EnterpriseType = @EnterpriseType where EnterpriseID = @EnterpriseID
                if (@@error <> 0 )
					begin
                       set @Result = -20504
					   set @ErrMsg = '更新企业信息失败'
					   rollback
					   return
					end
				update CustInfo set RealName = @EnterpriseName where CustID = @OCustID and UserAccount = @OUserAccount
                if (@@error <> 0 )
					begin
                       set @Result = -20504
					   set @ErrMsg = '更新客户信息失败'
					   rollback
					   return
					end
			end
	end
    else
   
    begin ---------------------------------------------------------------------------------------------------------------------------------
		
		declare @ProvinceID varchar(2)
        declare @OriginalAreaCode varchar(3)
		declare @BTAreaID         varchar(3)
        declare @InnerCardID      varchar(16)
		declare @cardSerial	int  --卡的 i~m位		
		declare @cardPre	varchar(8)
		declare @i	int
		declare @retainStr char(1)
		Declare @ExeSql nvarchar(4000)

		set @ProvinceID = '35'
		set @OriginalAreaCode = '035'
        set @BTAreaID = '350'
		set @InnerCardID = ''
		set @i = 0
		set @retainStr = '1'     -- H 位置 1 
		-- 0~4 间的任一位	
        set @ExeSql = ''			
		set @cardPre = '86'+'0'+'0'+@BTAreaID+@retainStr
		
		while(1=1)
		begin
			select @cardSerial=SequenceID+1 from AutoDistributeSequence with(updlock) where ProvinceID=@ProvinceID and AreaID=@OriginalAreaCode
			if (@cardSerial =100000)
			Begin
					set @Result = -22500
					set @ErrMsg = '自动分配卡资源已满，分配卡失败'
					rollback
					return
			End
			update AutoDistributeSequence 
			set SequenceID=@cardSerial
			where ProvinceID=@ProvinceID and AreaID=@OriginalAreaCode

			select @OCustID = SequenceID+1 from ParSequenceNumber with(updlock)where SequenceType = '0101'
			update ParSequenceNumber set SequenceID = @OCustID where SequenceType = '0101'
			if ( @@error <> 0)
			begin
					set @Result = -22500
					set @ErrMsg = '自动分配CustID失败'
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
				--取得卡号
				set @InnerCardID = Convert(varchar,@cardPre)+convert(varchar,@cardSerial)+'000'
                set @OUserAccount = @BTAreaID+@retainStr+convert(varchar,@cardSerial)

				if exists (select 1 from CustInfo where UserAccount=@OUserAccount)
				begin
					continue
				end

				--卡表里是否已经存在				
				Set @ExeSql = ' select 1 from BestToneCard_'+@ProvinceID+'_'+@OriginalAreaCode + ' where CardID=''' + @OUserAccount + ''''
				Exec(@ExeSql)
				if(@@RowCount>0)
				begin
					continue
				end			
				break
			end
			else --如果生成的序列号不是8位，重新生成
				continue
		--end
		End
	
		if(len(@OUserAccount) < 9 or len(@OUserAccount)>16)
		Begin
			set @Result = -30008
			set @ErrMsg = '生成卡号失败'
			rollback
			return
		End

		--校验CustID,UserAccount
		if exists (select 1 from CustInfo where CustID = @OCustID or UserAccount=@OUserAccount)
		Begin
			set @Result = -30001
			set @ErrMsg = '客户id或用户帐号已存在' 
			rollback
			return
		End
		-- 校验CustUserInfo表
		if exists (select 1 from CustUserInfo  where CustID = @OCustID or UserAccount=@OUserAccount or InnerCardID = @InnerCardID)
		begin
			set @Result = -30001
			set @ErrMsg = '客户id或用户帐号在CustUserInfo中已存在' 
			rollback
			return
		end


		declare @CustType varchar(2)
		declare @CustLevel varchar(1)
		declare @RegSrc    varchar(2)	
		declare @RegType   varchar(1)
		declare @SPID      varchar(8)
		declare @CertificateType char(2)
		declare @CertificateCode varchar(20)

		set @CustType = '03'
		set @CustLevel = '1'
		set @RegSrc = '04'
		set @RegType = '1'
		set @SPID = '35000000'
		set @CertificateType = ''
		set @CertificateCode = ''


		--插入客户信息表	
		insert into CustInfo( CustID,UserAccount,CustType,EncryptedPassword,CertificateType,CertificateCode,RealName,
			ProvinceID,AreaID,CustLevel,RegistrationSource,RegistrationType,RegistrationDate,Status,DealTime , EnterpriseID )
			values (@OCustID,@OUserAccount,@CustType,@EncryedPassword,@CertificateType,@CertificateCode,@EnterpriseName,
			@ProvinceID,@OriginalAreaCode,@CustLevel,@RegSrc,@RegType,getdate(),'00', getdate(),@EnterpriseID)

		if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '插入CustInfo时错误'
			Rollback
			return
		End
		-- 插入客户用户信息表
		------------------------------------------------------------------------------------------------------------------------
		if not exists (select 1 from CustUserInfo where UserAccount=@UserAccount)
		begin
			insert into CustUserInfo(CustID,SPID,InnerCardID,UserAccount,RegistrationSource,RegistrationType,RegistrationDate,Status,DealTime)
								values(@OCustID,@SPID,@InnerCardID,@OUserAccount,@RegSrc,@RegType,getdate(),'00',getdate())
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入CustUserInfo时错误'
				Rollback
				return
			End
		end
		else
		begin
			set @Result = -30001
			set @ErrMsg = 'UserAccount在 CustUserInfo表中已经存在'
			Rollback
			return
		end
		
		--- 插入企业信息表 EnterpriseInfo
        insert into EnterpriseInfo (EnterpriseID,EnterpriseName,EnterpriseType,CustID,UserAccount)
                          values (@EnterpriseID,@EnterpriseName,@EnterpriseType,@OCustID,@OUserAccount)
		if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入CustUserInfo时错误'
				Rollback
				return
			End
     

    end -- End For ELSE --------------------------------------------------------------------------------------------------------------------

 commit
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

