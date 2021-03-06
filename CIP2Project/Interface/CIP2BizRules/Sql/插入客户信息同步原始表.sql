set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go




/*
 * 存储过程dbo.up_Customer_V3_Interface_InsertCustInfoNotify
 *
 * 功能描述: 插入客户信息同步原始表
 *
 * Author: liuchunli
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-24
 * Remark:
 *
 */

ALTER PROCEDURE [dbo].[up_Customer_V3_Interface_InsertCustInfoNotify]
(
	@CustID	Varchar(16),
	@OPType	varchar(1),
	@ToSPID	varchar(8),
	@DealType int,
	@PaymentPwd varchar(50),
	@Result int out,
	@ErrMsg varchar(256) out
)
AS
	set @Result = -19999
	Set @ErrMsg = ''

	if not exists ( select 1 From CustInfo with(nolock) Where CustID = @CustID)
	Begin
		set @Result = -20504
		set @ErrMsg = '无此帐号'
		return
	End

	DECLARE @CustType varchar(2)
	SET @CustType = ''
	DECLARE @ProvinceID varchar(2)
	SET @ProvinceID = ''
	DECLARE @NotifyCount INT
	SET @NotifyCount = 0
    DECLARE @CResult int
	SET @CResult = -19999

	SELECT @CustType = CustType, @ProvinceID = ProvinceID FROM CustInfo with(nolock) WHERE CustID = @CustID
	
	INSERT INTO CustInfoNotify
	(
		CustID,
		ProvinceID,
		CustType,
		OPType,
		Result,
		ToSPID,
		DealType,
		PaymentPwd,
		DealTime,
		Description,
		NotifyCount
	)
	VALUES
	(
		@CustID,
		@ProvinceID,
		@CustType,
		@OPType,
		@CResult,
		@ToSPID,
		@DealType,
		@PaymentPwd,
		GETDATE(),
		@ErrMsg,
		@NotifyCount
	)
	if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '插入客户信息同步原始表时错误'
			Rollback
			return
		End

	set @Result = 0



