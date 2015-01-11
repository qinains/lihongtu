USE [besttone]
GO
/****** 对象:  StoredProcedure [dbo].[up_BT_V2_Interface_UserInfoQuery]    脚本日期: 01/09/2009 10:08:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





/*
 * 存储过程dbo.up_BT_V2_Interface_UserInfoQuery
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-5-20
 * Remark:
 *
 */

 
 ALTER  Procedure [dbo].[up_BT_V2_Interface_UserInfoQuery]
(
	@ProvinceID varchar(2),
	@SPID varchar(8),
	@UserAccount varchar(16),
	@CustID varchar(16),
	@PhoneNum varchar(20),
	@Password varchar(6),
	@EncryptedPassword varchar(50),
	@Result int output,
	@ErrMsg varchar(40) output
)
as
	set @Result = -19999
	set @ErrMsg = ''
	
	Declare @tmpCustID varchar(16)
	Declare @tmpUserAccount varchar(16)
	Set @tmpCustID=''
	Set @tmpUserAccount=''	
	
	--如果 @UserAccount 和 @Password 不为空，并且用户表里无此用户，则为激活码的校验
	if(@UserAccount!='' and @Password !='')
	Begin
		if not exists(Select 1 from CustInfo where UserAccount = @UserAccount) and not exists(select 1 from CustUserInfo where UserAccount = @UserAccount)
		Begin
				Declare @UProvinceID varchar(2)
				Declare @OriginalAreaCode varchar(3)
				set @OriginalAreaCode = left(@UserAccount,3)
				--北京，上海等地的地市码为 250，须转换为025
				if exists( select 1 from area where left(areaID,1) = '0' and areaID = '0'+left(@OriginalAreaCode,2))
					set @OriginalAreaCode = '0' + left(@OriginalAreaCode,2)
				select @UProvinceID = ProvinceID from Area where AreaID = @OriginalAreaCode
				
				
				--校验卡号
					Declare @ExeSql nvarchar(4000)
					Declare @BatchNumber int
					Declare @tmpStatus varChar(2)
					Declare @CardExpireTime DateTime
					Declare @CurrentTime dateTime
					Set @CurrentTime = getDate()
					Set @BatchNumber = 0
					Set @tmpStatus = 'kk'
					
					--判断卡是否有效
					Set @ExeSql = ' select @BatchNumber=BatchNumber, @tmpStatus=Status, @CardExpireTime=ExpireDate from BestToneCard_'+@UProvinceID+'_'+@OriginalAreaCode + ' where CardID=''' + @UserAccount + ''''

					exec sp_executesql @ExeSql,N' @BatchNumber int output, @tmpStatus varchar(2) output, @CardExpireTime dateTime output', @BatchNumber output,@tmpStatus output, @CardExpireTime output

					if(@tmpStatus = '01')
					Begin
						Set @Result = -30001
						set @ErrMsg = '此卡已使用'
						return
					End
					
					if( @CardExpireTime < @CurrentTime )
					Begin
						Set @Result = -21528
						set @ErrMsg = '此卡已过期'
						return
					End

					if(@tmpStatus = '02')
					Begin
						Set @Result = -21528
						set @ErrMsg = '此卡已冻结'
						return
					End
					
					if(@tmpStatus = '03')
					Begin
						Set @Result = -21528
						set @ErrMsg = '此卡已废弃'
						return
					End
					
					if(@BatchNumber =0)
					Begin
						Set @Result = -30008
						set @ErrMsg = '无此卡批次'
						return
					End
					
					declare @tmpBatchStatus varchar(2)
					set @tmpBatchStatus = 'kk'
					Select @tmpBatchStatus =BatchStatus from BestToneCardBatch where BatchNumber=@BatchNumber

					if(@tmpBatchStatus = 'kk')
					Begin
						Set @Result = -30008
						set @ErrMsg = '卡批次状态不对'
						return
					End
					
					if(@tmpBatchStatus = '00')
					Begin
						Set @Result = -30008
						set @ErrMsg = '该批次卡未激活，不能注册'
						return
					End
					
					if(@tmpBatchStatus = '02')
					Begin
						Set @Result = -30008
						set @ErrMsg = '该批次卡已冻结，不能注册'
						return
					End
					
					if(@tmpBatchStatus = '03')
					Begin
						Set @Result = -30008
						set @ErrMsg = '该批次卡已废弃，不能注册'
						return
					End
					
					if(@tmpBatchStatus = '04')
					Begin
						Set @Result = -30008
						set @ErrMsg = '该批次卡已删除，不能注册'
						return
					End


					--验证激活码
					Set @ExeSql = ' select 1 from BestToneCard_'+@UProvinceID+'_'+@OriginalAreaCode + ' where CardID=''' + @UserAccount + ''' and Password = ''' + @Password + ''''

					exec(@ExeSql)
					if(@@Rowcount !=1)
					Begin
						Set @Result = -30008
						set @ErrMsg = '激活码有误'
						return
					End
					
					--卡验证成功 则返回成功
					set @Result =0
					--赋特殊的返回值，表示激活码验证成功。
					set @ErrMsg = '000'
					return
		End
	
	End
		
	
	--根据关键信息查询出UserAccount,和CustID
	Declare @sqlStr nvarchar(4000)
		
	Declare @UserAccountColumn varchar(12)
	if( len(@UserAccount)<>16)
		Set @UserAccountColumn = 'UserAccount'
	else 
		Set @UserAccountColumn = 'InnerCardID'
		
	set @sqlStr = 'select top 1 @tmpCustID=CustID,@tmpUserAccount=UserAccount from CustUserInfo where 1=1 '
	--按CustPersonType排序 是为了在一个电话被两种不同卡绑定的情况下，优先取公众卡
	if( @UserAccount != '')
		set @sqlStr = @sqlStr+ ' and ' + @UserAccountColumn + '=''' +@UserAccount+ ''''
	if(@CustID != '')
		set @sqlStr = @sqlStr+ ' and CustID=''' + @CustID + ''''
	if( @PhoneNum != '')
		set @sqlStr = @sqlStr+ ' and UserAccount in (select UserAccount from BoundPhone where BoundPhoneNumber=''' + @PhoneNum + ''') order by CustPersonType'
	--select @sqlStr

	exec sp_executesql @sqlStr,N' @tmpCustID varchar(16) output, @tmpUserAccount varchar(16) output', @tmpCustID output, @tmpUserAccount output
--select @tmpCustID, @tmpUserAccount
	if(@@RowCount < 1)
	Begin
		set @Result = -20504
		set @ErrMsg = '无用户记录'
		return
	End

	--如果密码不为空则校验密码
	if(@tmpCustID!='' and @tmpUserAccount != '' and @EncryptedPassword != '')
	Begin
		declare @tmpEncryptedPassword varchar(50)
		set @tmpEncryptedPassword = ''
		select @tmpEncryptedPassword = EncryptedPassWord from CustInfo where CustID=@tmpCustID and UserAccount = @tmpUserAccount
		if(@@RowCount=0)
		Begin
			set @Result = -20504
			set @ErrMsg = '用户不存在'
			return
		End
		if(@tmpEncryptedPassword != @EncryptedPassword)
		Begin
			set @Result = -30007
			set @ErrMsg = '密码不正确'
			return
		End
		
	End
				
				
	--基本信息
	select * from
	(select UserAccount, CustID,CustType UserType,EncryptedPassword [Password],CertificateType,CertificateCode,
		RealName RealName,ProvinceID UProvinceID,
		case(@SPID)
			when '01010101' then  b.RegionCode--若是移百系统则转换为国标
			else
				case left(AreaID,1) 
					when '0' then Right(AreaID,2)
					else AreaID
				end
		end
		AreaCode,
		CustLevel CustLevel, EnterpriseID
	   from CustInfo a, RegionCodeArea b where CustID = @tmpCustID and a.AreaID=b.AreaCode ) a left join (select CustID, IsPost from CardSendStyle where CustID = @tmpCustID and UserAccount =@tmpUserAccount ) e on a.custID = e.CustID	,
	(select UserAccount, CustID,Sex ,BirthDay ,EduLevel EduLevel,Favorites Favorite,InComeLevel,Email,CustContactTel 
	   from CustExtend where CustID = @tmpCustID ) b,
	(select CustID,Status Status from CustUserInfo where CustID = @tmpCustID and UserAccount=@tmpUserAccount ) c,
	(select top 1 UserAccount, CustID,AccountType PaymentAccountType, AccountNumber PaymentAccount, AccountPassword PaymentAccountPassword
	   from PaymentAccount  ) d

	--left join a on a.custID = e.CustID	
--where a.CustID *= b.CustID and a.CustID *= c.CustID and a.CustID *= c.CustID and a.CustID *= e.CustID

--select * from  CardSendStyle 
		
	--绑定电话
	select BoundPhoneNumber Phone from  BoundPhone where CustID = @tmpCustID and UserAccount=@tmpUserAccount
	
	--联系信息
	select LinkMan,ContactTel,Address,ZipCode,TYpe
	 from ContactInfo 
	 where CustID = @tmpCustID and UserAccount=@tmpUserAccount
	 
	--评估信息
	select Credit,CreditLevel,Score,AvailableScore
	from CreditEmuinfo
	where CustID = @tmpCustID and UserAccount=@tmpUserAccount
	
	--订购信息
	select CustID,UserAccount,SubscribeStyle,ServiceID,ServiceName,StartTime,EndTime,TransactionID 
	from ServiceSubscriptionInfo
	where CustID = @tmpCustID and UserAccount=@tmpUserAccount
	
	--认证方式
	select CustID,AuthenName,AuthenType,SourceSPID,Status,Description from UserAuthenStyle 
	where CustID = @tmpCustID and status = '0'


	Set @Result =0
	
Set QUOTED_IDENTIFIER Off




