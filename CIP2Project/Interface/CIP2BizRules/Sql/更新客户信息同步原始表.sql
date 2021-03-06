set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go








/*
 * 存储过程dbo.up_Customer_V3_Interface_UpdateCustInfoNotify
 *
 * 功能描述: 更新原始表,插入历史表
 *
 * Author: liuchunli
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-24
 * Remark:
 *
 */

ALTER PROCEDURE [dbo].[up_Customer_V3_Interface_UpdateCustInfoNotify]
(
	@SequenceID bigint,
	@CustID	Varchar(16),
	@ProvinceID	varchar(2),
	@CustType	varchar(2),
	@OPType	varchar(1),
	@ToSPID	varchar(8),
	@DealType int,
	@PaymentPwd varchar(50),
	@Result	int,
	@Description	varchar(256)
)
AS
BEGIN

	declare @NotifyCount int
	set @NotifyCount = 0

	IF @Result<>0
		BEGIN
			UPDATE CustInfoNotify
			SET Result = @Result, NotifyCount = NotifyCount + 1, Description = @Description
			WHERE SequenceID = @SequenceID			
		END
	ELSE
		BEGIN
			select @NotifyCount = NotifyCount + 1 from CustInfoNotify where SequenceID = @SequenceID

			DELETE FROM CustInfoNotify 
			WHERE SequenceID = @SequenceID

			INSERT INTO CustInfoNotifyHistory
			(
				SequenceID,
				CustID,
				ProvinceID,
				CustType,
				OPType,
				ToSPID,
				DealType,
				PaymentPwd,
				DealTime,
				Description,
				NotifyCount
			)
			VALUES
			(
				@SequenceID,
				@CustID,
				@ProvinceID,
				@CustType,
				@OPType,
				@ToSPID,
				@DealType,
				@PaymentPwd,
				getdate(),
				@Description,
				@NotifyCount
			)
		END
END











