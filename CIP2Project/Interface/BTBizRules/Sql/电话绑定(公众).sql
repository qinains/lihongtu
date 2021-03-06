set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go













/*
 * 存储过程dbo.up_Customer_OV3_Interface_BindPhoneNum
 *
 * 功能描述: 处理电话号码绑定请求，可对单个主叫号码进行绑定(公众接口)
 *			 
 *
 * Author: liuchunli
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-12
 * Remark:
 *
 */
 
ALTER Procedure [dbo].[up_Customer_OV3_Interface_BindPhoneNum]
(
	@SPID varchar(8),
	@CustID varchar(16),
	@PhoneNum varchar(20),
	@Result int out,
	@ErrMsg varchar(256) out
)
as
	set @Result = -22500
	set @ErrMsg = ''
		
	if not exists ( select 1 From CustInfo with(nolock) Where CustID = @CustID)
	Begin
		set @Result = -20504
		set @ErrMsg = '无此帐号'
		return
	End

	select 1 from CustPhone with(nolock) where CustID = @CustID
	if(@@RowCount>=5)
	Begin
		set @Result = -30004
		set @ErrMsg = '该用户超过绑定电话个数限制'
		return
	End

	select 1 from CustPhone with(nolock) where Phone = @PhoneNum
	if(@@RowCount>=5)
	Begin
		set @Result = -30004
		set @ErrMsg = '该电话超过用户绑定个数限制'
		return
	End
	
	DECLARE @ProvinceID Varchar(2)
	SET @ProvinceID = ''
	DECLARE @CustType Varchar(2)
	SET @CustType = ''
	DECLARE @SourceSPID varchar(8)
	SET @SourceSPID = ''
	DECLARE @PhoneType Varchar(1)
	SET @PhoneType = '4'
	DECLARE @PhoneClass Varchar(1)
	SET @PhoneClass = '1'

	BEGIN
		SELECT @ProvinceID = ProvinceID,@CustType = CustType
		FROM custinfo with(nolock)
		WHERE CustID = @CustID
	END
	if exists( select 1 from CustPhone with(nolock) where Phone = @PhoneNum and CustID = @CustID)
		Begin
			set @Result = -1
			set @ErrMsg = '该电话不能被同一个用户重复绑定'
			return
		End
	else
		Begin
			insert into CustPhone
			(
				CustID,
				ProvinceID,
				CustType,
				Phone,
				PhoneType,
				PhoneClass,
				SourceSPID,
				Dealtime
			)
			values
			(
				@CustID,
				@ProvinceID,
				@CustType,
				@PhoneNum,
				@PhoneType,
				@PhoneClass,
				@SPID,
				getdate()
			)
		End
	
	if(@@RowCount = 1)
		set @Result = 0

Set QUOTED_IDENTIFIER Off
Set ANSI_NULLS Off











