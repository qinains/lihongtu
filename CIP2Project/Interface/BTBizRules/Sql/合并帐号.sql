USE [BestTone]
GO
/****** 对象:  StoredProcedure [dbo].[up_BT_V2_Interface_IncorporateUserAccount]    脚本日期: 10/10/2008 14:35:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*
 * 存储过程dbo.up_BT_V2_Interface_IncorporateUserAccount
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-7-3
 * Remark:
 *
 */
 
Alter Procedure [dbo].[up_BT_V2_Interface_IncorporateUserAccount]
(
	@DeleteUserAccountRecords Text,
	@IncorporatedCustID varchar(16),
	@IncorporatedUserAccount varchar(16),
	@Result int output,
	@ErrMsg varchar(256) output
)
as
	DECLARE  @UserAccountDoc int

	Declare @CurrentTime dateTime
	Set @CurrentTime = getDate()
	
	Set @Result = -19999
	Set @ErrMsg = ''

	-- 创建临时表

	Declare @UserAccountRecord Table
	(
		CustID varchar(16),
		UserAccount varchar(16)
	)
	

	if( @DeleteUserAccountRecords is not null )
	Begin
		-- 解析请求数据
		-- 解析绑定电话列表关系
		EXECUTE sp_xml_preparedocument @UserAccountDoc OUTPUT, @DeleteUserAccountRecords


		Insert Into @UserAccountRecord( CustID, UserAccount )
		SELECT CustID,UserAccount
			FROM OpenXML( @UserAccountDoc, '/ROOT/DeleteUserAccountRecord' , 2 ) 
			With( CustID Varchar(16), UserAccount Varchar(16) )
	End
	
	--检查帐号是否有效

	if not exists( select 1 from custUserInfo where CustID=@IncorporatedCustID and UserAccount=@IncorporatedUserAccount and status ='00' )	
	Begin
		Set @Result = -30008
		Set @ErrMsg = @IncorporatedCustID+'帐号无效，可能不存在或状态不正常，'		
		return
	End
	
	Declare @DeleteCount int
	Declare @ExistsCount int
	--需要删除的帐户
	select @DeleteCount=Count(1) from @UserAccountRecord
	--已存在的帐户
	select @ExistsCount=Count(1) 
	from CustUserInfo 
	where CustID in (select CustID from @UserAccountRecord ) and UserAccount in (select UserAccount from @UserAccountRecord ) and Status='00'
	
	if( @DeleteCount != @ExistsCount)
	Begin
		Set @Result = -30008
		Set @ErrMsg = @IncorporatedCustID+'DeleteUserAccountRecords中的帐号无效，可能不存在或状态不正常，'		
		return
	End
	
	
	--计算信用，用户等级，信用等级 取最高级别

	Declare @ComsumeLevel varchar(2)
	Declare @CreditLevel varchar(2)
	Declare @Credit bigint
	select @ComsumeLevel=max(ComsumeLevel),@CreditLevel=max(CreditLevel),@Credit=max(Credit)
	from CreditEmuinfo 
	where CustID in (select CustID from @UserAccountRecord ) and UserAccount in (select UserAccount from @UserAccountRecord ) or (CustID=@IncorporatedCustID and UserAccount=@IncorporatedUserAccount )

--------------------------------------------------------------------------------------------------------------------
	begin Tran  --开启事务


	--合并绑定电话-------------------------------------------------------------
	Declare @tmpBoundPhone Table
	(
		BoundPhoneNumber varchar(20),
		DealTime datetime,
		CustPersonType char(1)
	)
	--取出最近绑定的5条电话号码

	insert into @tmpBoundPhone
		select top 5 BoundPhoneNumber, DealTime,CustPersonType
		from BoundPhone 
		Where CustID in (select CustID from @UserAccountRecord ) or CustID=@IncorporatedCustID
		order by dealTime desc
	--删除所有绑定电话

	delete from BoundPhone where CustID in (select CustID from @UserAccountRecord ) or CustID=@IncorporatedCustID
	--插入绑定电话
	insert into BoundPhone ( CustID,UserAccount,BoundPhoneNumber, DealTime,CustPersonType )
		select @IncorporatedCustID,@IncorporatedUserAccount,BoundPhoneNumber, DealTime,CustPersonType from @tmpBoundPhone
	

	--合并联系地址 ------------------------------------------------------------
	update ContactInfo
	set CustID = @IncorporatedCustID, UserAccount = @IncorporatedUserAccount
	where CustID in (select CustID from @UserAccountRecord )
	if( @@Error<>0)
	Begin
		Set @Result = -22500
		Set @ErrMsg = '合并联系地址失败'
		Rollback
		return
	End

	--插入历史表


		
	Insert Into CustInfoHistory  (CustID,UserAccount,CustType,EncryptedPassword,CertificateType,CertificateCode,
		RealName,ProvinceID,AreaID,CustLevel,EnterpriseID,RegistrationSource,RegistrationType,RegistrationDate,
		Status,DealTime,IncorporateCustID,IncorporateUserAccount,Reason,HistoryDescription)
	select CustID,UserAccount,CustType,EncryptedPassword,CertificateType,CertificateCode,
		RealName,ProvinceID,AreaID,CustLevel,EnterpriseID,RegistrationSource,RegistrationType,RegistrationDate,
		Status,@CurrentTime,@IncorporatedCustID,@IncorporatedUserAccount,'1','账号合并' from CustInfo where CustID in (select CustID from @UserAccountRecord)
	if( @@Error<>0)
	Begin
		Set @Result = -22500
		Set @ErrMsg = '插入客户历史表失败'
		Rollback
		return
	End
	
	Insert Into CustExtendHistory (CustID,UserAccount,Sex,BirthDay,EduLevel,Favorites,InComeLevel,Email,CustContactTel,
		DealTime,Reason,HistoryDescription)
	select CustID,UserAccount,Sex,BirthDay,EduLevel,Favorites,InComeLevel,Email,CustContactTel,
		@CurrentTime,'1','账号合并' from CustExtend where CustID in (select CustID from @UserAccountRecord)
	if( @@Error<>0)
	Begin
		Set @Result = -22500
		Set @ErrMsg = '插入客户扩展历史表失败'
		Rollback
		return
	End
	
	Insert Into CustUserInfoHistory (CustID,SPID,InnerCardID,UserAccount,CustPersonType,RegistrationSource,RegistrationType,
		RegistrationDate,Status,ISHaveCard,DealTime,Description,Reason,HistoryDescription)
	select CustID,SPID,InnerCardID,UserAccount,CustPersonType,RegistrationSource,RegistrationType,
		RegistrationDate,Status,ISHaveCard,@CurrentTime,Description,'1','账号合并'
		from CustUserInfo where CustID in (select CustID from @UserAccountRecord)
	if( @@Error<>0)
	Begin
		Set @Result = -22500
		Set @ErrMsg = '合插入客户用户历史表失败'
		Rollback
		return
	End
	--从现有表中删除

	delete from CustInfo where CustID in (select CustID from @UserAccountRecord)
	if( @@Error<>0)
	Begin
		Set @Result = -22500
		Set @ErrMsg = '删除客户表失败'
		Rollback
		return
	End
	delete from CustExtend where CustID in (select CustID from @UserAccountRecord)
	if( @@Error<>0)
	Begin
		Set @Result = -22500
		Set @ErrMsg = '删除客户扩展表失败'
		Rollback
		return
	End
	delete from CustUserInfo where CustID in (select CustID from @UserAccountRecord)
	if( @@Error<>0)
	Begin
		Set @Result = -22500
		Set @ErrMsg = '删除客户用户表失败'
		Rollback
		return
	End
	
	--删除客户认证方式
	delete from UserAuthenStyle where CustID in (select CustID from @UserAccountRecord)
	

/*
	--更新客户表状态 ----------------------------------------------------------
	Update CustInfo
	Set Status = '04'
	Where CustID in (select CustID from @UserAccountRecord ) and UserAccount in (select UserAccount from @UserAccountRecord ) 
	if( @@Error<>0)
	Begin
		Set @Result = -22500
		Set @ErrMsg = '更新客户表失败'
		Rollback
		return
	End

	--更新客户用户表状态 ----------------------------------------------------------
	Update CustUserInfo
	Set Status = '06'
	Where CustID in (select CustID from @UserAccountRecord ) and UserAccount in (select UserAccount from @UserAccountRecord ) 
	if( @@Error<>0)
	Begin
		Set @Result = -22500
		Set @ErrMsg = '更新客户用户表失败'
		Rollback
		return
	End
*/	
	
	
	COMMIT

	Set @Result = 0

Set QUOTED_IDENTIFIER Off
