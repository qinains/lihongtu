create Procedure [dbo].[up_BT_V5_Interface_PwdQuestionAuth]
(
	@CustID varchar(16),
	@QuestionID int,
    @Answer varchar(100),
    @Result int out,
    @ErrorDescription varchar(256) out
)
as
    if not exists( select 1 from Answer where CustID=@custID and QuestionID=@QuestionID and Answer=@Answer)
    begin
      set @Result=-19999
      set @ErrorDescription='此用户的密码答案不正确！'
      return 
    end
    set @Result=-0
    set @ErrorDescription=''

set ANSI_NULLS Off
set QUOTED_IDENTIFIER Off
go

