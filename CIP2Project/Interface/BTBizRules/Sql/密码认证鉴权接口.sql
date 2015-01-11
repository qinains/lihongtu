set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go





/*
 * 存储过程dbo.up_Customer_OV3_Interface_CheckUserPassword
 *
 * 功能描述: 
 *			 
 *
 * Author: lihongtu
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-10
 * Remark:
 *
 */

 
 ALTER Procedure [dbo].[up_Customer_OV3_Interface_CheckUserPassword]
(
	@ProvinceID varchar(2),
	@SPID varchar(8),
	@UserAccount varchar(16),
	@CustID varchar(16),
	@PhoneNum varchar(20),
	@EncryptedPassword varchar(50),
	@Result int output,
	@ErrMsg varchar(40) output
)
as
	set @Result = -19999
	set @ErrMsg = ''
	
	declare @tmpCustID varchar(16)
	set @tmpCustID = ''

	

	if(@CustID != '')
		begin
			set @tmpCustID=@CustID
		end
	else if(@CustID = '' and @PhoneNum != '')
		begin
			select @tmpCustID=CustID from CustPhone where Phone=@PhoneNum
		end
	else if(@CustID = '' and @PhoneNum = '' and @UserAccount != '')
		begin
			if(len(@UserAccount)=16)
			begin
				set @UserAccount = substring(@UserAccount,5,9)
			end
			select @tmpCustID=CustID from CustTourCard where CardID=@UserAccount
		end

	Declare @sqlStr nvarchar(4000)
	declare @WebPwd varchar(50)
	declare @VoicePwd varchar(50)
	set @WebPwd = ''
	set @VoicePwd = ''

	set @sqlStr = 'select top 1 @WebPwd=WebPwd,@VoicePwd=VoicePwd  from CustInfo where  CustID='''+@tmpCustID+''' '

	exec sp_executesql @sqlStr,N' @WebPwd varchar(50) output,@VoicePwd varchar(50) output', @WebPwd output,@VoicePwd output

	if(@@RowCount != 1)
	Begin
		set @Result = -20504
		set @ErrMsg = '用户信息不存在,无此帐号'
		return
	End
	
	if(@WebPwd!=@EncryptedPassword and @VoicePwd!=@EncryptedPassword)
	Begin
		set @Result = -20504
		set @ErrMsg = '密码不正确'
		return
	End
	else
	Begin
		set @Result = 0
		set @ErrMsg = '密码正确'
		return
	End

--		select * from 
--	(select --UserAccount, 
--		CustID,CustType UserType,WebPwd [Password],CertificateType,CertificateCode,Email,Sex,
--		RealName RealName,ProvinceID UProvinceID,
--		case left(AreaID,1) 
--			when '0' then Right(AreaID,2)
--		end 
--		AreaCode,
--		CustLevel CustLevel,Status Status
--			from CustInfo where CustID = @tmpCustID 
--			--and UserAccount=@tmpUserAccount
--		) a
--		right outer join 
--	(select  CustID,BirthDay ,EduLevel EduLevel,Favorite,InComeLevel 
--	   from CustExtendInfo where CustID = @tmpCustID --and UserAccount=@UserAccount
--       ) b
--        on a.CustID = b.CustID 
--      right outer join 
--	(select CustID, PaymentAccount
--	   from PaymentAccount where CustID = @tmpCustID ) c
--	  on  a.CustID = c.CustID

	select top 1 * from(select CustType,CustID,ProvinceID,AreaID,Status,RealName,CertificateCode,
    CertificateType,Sex,CustLevel,Email from CustInfo with(nolock) where CustID=@tmpCustID) a
	left join
	(select CustID,Birthday,EduLevel,Favorite,IncomeLevel from CustExtendInfo with(nolock) where 
	CustID=@tmpCustID) b on a.CustID=b.CustID
	left join
	(select CustID,Phone from CustPhone with(nolock) where CustID=@tmpCustID) c on a.CustID=c.CustID
	left join
    (select CustID,CardID,CardType from CustTourCard with(nolock) where CustID=@tmpCustID) d on a.CustID=d.CustID
	order by CardType

	--绑定电话
	select Phone from  CustPhone with(nolock) where CustID = @tmpCustID 
	
	--联系信息
	select CustInfo.RealName,CustPhone.Phone,AddressInfo.Address,AddressInfo.ZipCode,AddressInfo.TYpe
	 from AddressInfo with(nolock),CustInfo with(nolock),CustPhone with(nolock)
	 where AddressInfo.CustID=@tmpCustID and AddressInfo.CustID=CustInfo.CustID and AddressInfo.CustID=CustPhone.CustID
	
	--认证方式
	select AuthenName,AuthenType from CustAuthenInfo with(nolock)
	where CustID = @tmpCustID



	Set @Result =0
Set QUOTED_IDENTIFIER Off






