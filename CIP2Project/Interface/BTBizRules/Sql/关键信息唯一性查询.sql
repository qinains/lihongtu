


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V2_Interface_BasicInfoQuery]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V2_Interface_BasicInfoQuery]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程dbo.up_BT_V2_Interface_BasicInfoQuery
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-7-5
 * Remark:
 *
 */
 
 Create Procedure dbo.up_BT_V2_Interface_BasicInfoQuery
(
	@ProvinceID varchar(2),
	@SPID varchar(8),
	@UserAccount varchar(16),
	@PhoneNum varchar(20),
	@CertificateCode varchar(20),
	@CertificateType varchar(2),
	@Result int out,
	@ErrMsg varchar(256) out
)
as
	set @Result = -22500
	set @ErrMsg = ''
	
	Declare @CurrentTime DateTime
	Set @CurrentTime = GetDate()
	
	--校验卡号
	if(@UserAccount != '')
	Begin
		Declare @ExeSql nvarchar(4000)
		Declare @BatchNumber int
		Declare @tmpStatus varChar(2)
		Declare @AreaCode varchar(3)
		Declare @CardExpireTime DateTime
		Set @AreaCode = SubString(@UserAccount,5,3)
		Set @BatchNumber = 0
		Set @tmpStatus = 'kk'
/*		if not Exists(select 1 from Area Where AreaID = @AreaCode)		
		Begin
			Set @Result = -31008
			set @ErrMsg = '帐号中的地市码不正确'
			return
		End
*/
		Set @ExeSql = ' select @BatchNumber=BatchNumber, @tmpStatus=Status, @CardExpireTime=ExpireDate from BestToneCard_'+@ProvinceID+'_'+@areaCode + ' where CardID=''' + @UserAccount + ''''

		exec sp_executesql @ExeSql,N' @BatchNumber int output, @tmpStatus varchar(2) output, @CardExpireTime dateTime output', @BatchNumber output,@tmpStatus output, @CardExpireTime output

		if(@tmpStatus = '01')
		Begin
			Set @Result = -21528
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
			Set @Result = -21528
			set @ErrMsg = '无此卡'
			return
		End
		
		declare @tmpBatchStatus varchar(2)
		set @tmpBatchStatus = 'kk'
		Select @tmpBatchStatus =BatchStatus from BestToneCardBatch where BatchNumber=@BatchNumber

		if(@tmpBatchStatus = 'kk')
		Begin
			Set @Result = -30008
			set @ErrMsg = '无此卡批次'
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
	End
	
	--检查电话号码是否已被绑定

	if(@PhoneNum != '' )
	begin
		if exists (select 1 from BoundPhone where BoundPhoneNumber = @PhoneNum)
		begin
			set @Result = -30003
			set @ErrMsg = '该电话已经被其他用户绑定，不允许重复绑定'
			return 
		end
	end
	
	
	
	Declare @tmpCustID varchar(16)
	Declare @tmpUserAccount varchar(16)
	Declare @tmpCertificateCode varchar(20)
	Declare @tmpCertificateType varchar(1)
	Set @tmpCustID=''
	Set @tmpUserAccount=''
	Set @tmpCertificateCode = ''
	Set @tmpCertificateType = ''
	
	--根据关键信息查询出UserAccount,和CustID
	Declare @sqlStr nvarchar(4000)
	
	set @sqlStr = 'select CustID, UserAccount,RealName,CertificateCode,CertificateType from CustInfo where 1=1 '
	
	if( @UserAccount != '')
		set @sqlStr = @sqlStr+ ' and UserAccount=''' +@UserAccount+ ''''
	if( @CertificateCode != '' and @CertificateType != '')
		set @sqlStr = @sqlStr+ ' and CertificateCode=''' + @CertificateCode + ''''+ ' and CertificateType=''' + @CertificateType + ''''
	if( @PhoneNum != '')
		set @sqlStr = @sqlStr+ ' and UserAccount in (select UserAccount from BoundPhone where BoundPhoneNumber=''' + @PhoneNum + ''')'
	
	exec sp_executesql @sqlStr


/*
	--检查证件号码是否已被注册

	if( @CertificateCode <>'' and @CertificateType <> '')
	begin
		if exists(select 1 from CustInfo where CertificateCode=@CertificateCode and CertificateType=@CertificateType  )
		begin
			set @Result = -30002
			set @ErrMsg = '该证件已被注册。'
			return 
		end
	end
*/
	set @Result = 0
			


Set QUOTED_IDENTIFIER Off
go
Set ANSI_NULLS Off
go