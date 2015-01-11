
/****** 对象:  StoredProcedure [dbo].[up_BT_V3_Interface_GetPassword]    脚本日期: 10/16/2009 11:04:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


 
 create Procedure [dbo].[up_BT_V3_Interface_GetPassword]
(
	@CustID varchar(16),
	@Result int out,
	@Pwd varchar(100) out,
	@ErrMsg varchar(100) out
)
as
	set @Pwd = ''
	Set @ErrMsg = ''
	Set @Result = -22500
	select @Pwd=VoicePwd  from CustInfo where custID = @CustID

	if(@@Rowcount != 1)
	Begin
		Set @ErrMsg = '无此客户号'
		Set @REsult = -21500
		return
	End

	set @Result=0