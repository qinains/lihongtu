USE [CIP2]
GO
/****** 对象:  StoredProcedure [dbo].[up_Customer_V3_Interface_PhoneSet]    脚本日期: 10/10/2009 15:30:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO












/*
 * 存储过程dbo.up_Customer_V3_Interface_PhoneSet
 *
 * 功能描述: 电话设置
 *
 * Author: liuchunli
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-3
 * Remark:
 *
 */

ALTER PROCEDURE [dbo].[up_Customer_V3_Interface_PhoneSet]
(
	@SPID varchar(8),
	@CustID varchar(16),
	@PhoneNum varchar(30),
	@PhoneClass varchar(1),
	@PhoneType Varchar(1),
	@Result int out,
	@ErrMsg varchar(256) out
)
AS
	set @Result = -22500
	set @ErrMsg = ''

	DECLARE @ProvinceID Varchar(2)
	SET @ProvinceID = ''
	DECLARE @CustType Varchar(2)
	SET @CustType = ''
	DECLARE @PhoneClass_custPhone varchar(1)
	SET @PhoneClass_custPhone = ''
	DECLARE @SourceSPID varchar(8)
	SET @SourceSPID = ''

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

	BEGIN
		SELECT @ProvinceID = ProvinceID,@CustType = CustType ,@SourceSPID = SourceSPID
		FROM custinfo with(nolock)
		WHERE CustID = @CustID
	END

	if exists( select 1 from CustPhone with(nolock) where Phone = @PhoneNum and CustType = @CustType and PhoneClass != '1' and @PhoneClass != '1')
	Begin
		set @Result = -1
		set @ErrMsg = '该电话已被其它客户绑定'
		return
	End
	ELSE
	Begin

		if exists( select 1 from CustPhone with(nolock) where CustID =@CustID and Phone = @PhoneNum and CustType = @CustType and PhoneClass = '1' and @PhoneClass != '1')
		Begin
			delete CustPhone where CustID =@CustID and Phone = @PhoneNum
		End

		INSERT INTO CustPhone
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
		VALUES
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













