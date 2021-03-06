set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go




ALTER proc [dbo].[up_Customer_OV3_Interface_SelEmailAuDel]
(
@UserName varchar(16),
@Email varchar(100),
@AuthenCode varchar(6),
@SqlResult int out,
@ErrMsg varchar(256) out
)
as

BEGIN

if EXISTS(select 1 from SendEmailRecord where  Email=@Email and AuthenCode=@AuthenCode)
BEGIN
SET @SqlResult=0
BEGIN TRANSACTION DelRecordInRecordHistory
DECLARE @errorSun INT --定义错误计数器
DECLARE @SequenceIDHistory int
DECLARE @CustIDHistory  varchar(16)
DECLARE @ProvinceIDHistory varchar(2)
DECLARE @OPTypeHistory varchar(1)
DECLARE @MessageHistory varchar(800)
DECLARE @AuthenCodeHistory varchar(6)
DECLARE @ResultHistory int
DECLARE @EmailHistory varchar(100)
DECLARE @DealTimeHistory datetime
DECLARE @DescriptionHistory varchar(40)
DECLARE @NotifyCountHistory int 
DECLARE @CustID varchar(16)


SET @errorSun=0 --没错为0

set @CustID=(select CustID from CustInfo where username=@username and Email=@Email)

set @SequenceIDHistory=(select SequenceID from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode)
set @CustIDHistory=(select custid from custinfo where username=@username and email=@email)
set @ProvinceIDHistory=(select ProvinceID from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode)
set @OPTypeHistory=(select OPType from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode)
set @MessageHistory=(select Message from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode)
set @AuthenCodeHistory=@AuthenCode
set @ResultHistory=(select Result from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode)
set @EmailHistory=@Email
set @DealTimeHistory=(select DealTime from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode)
set @DescriptionHistory=(select Description from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode)
set @NotifyCountHistory=(select NotifyCount from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode)





INSERT INTO SendEmailRecordHistory(SequenceID,CustID,ProvinceID,OPType,Message,AuthenCode,Result,Email,DealTime,Description,NotifyCount)
values(@SequenceIDHistory,@CustIDHistory,@ProvinceIDHistory,@OPTypeHistory,@MessageHistory,@AuthenCodeHistory,@ResultHistory,@EmailHistory,@DealTimeHistory,@DescriptionHistory,@NotifyCountHistory)

SET @errorSun=@errorSun+@@ERROR --累计是否有错
DELETE SendEmailRecord where SequenceID=@SequenceIDHistory

SET @errorSun=@errorSun+@@ERROR --累计是否有错

IF @errorSun<>0 BEGIN PRINT '有错误，回滚'

ROLLBACK TRANSACTION--事务回滚语句

END ELSE BEGIN PRINT '成功，提交'

COMMIT TRANSACTION--事务提交语句
end

return
END




BEGIN
SET @SqlResult=-30008
Set @ErrMsg='帐号无效'
return
END
END


