set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go








ALTER proc [dbo].[up_Customer_V3_Interface_SelSendEmailMassage]
(
@CustID varchar(16),
@Email varchar(100),
@AuthenCode varchar(6),
@SqlResult int out,
@ErrMsg varchar(256) out
)
as

BEGIN
if (@CustID='')
begin
if EXISTS(select top(1) 1 from SendEmailRecord where  Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
begin
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




SET @errorSun=0 --没错为0
set @SequenceIDHistory=(select top(1) SequenceID from SendEmailRecord where  Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
set @CustIDHistory=@CustID
set @ProvinceIDHistory=(select top(1) ProvinceID from SendEmailRecord where Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
set @OPTypeHistory=(select top(1) OPType from SendEmailRecord where  Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
set @MessageHistory=(select top(1) Message from SendEmailRecord where  Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
set @AuthenCodeHistory=@AuthenCode
set @ResultHistory=(select top(1) Result from SendEmailRecord where   Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
set @EmailHistory=@Email
set @DealTimeHistory=(select top(1) DealTime from SendEmailRecord where  Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
set @DescriptionHistory=(select top(1) Description from SendEmailRecord  where  Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
set @NotifyCountHistory=(select top(1) NotifyCount from SendEmailRecord where  Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)




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
SET @SqlResult=0
return
END

BEGIN
SET @SqlResult=-30008
SET @ErrMsg='帐号无效'
return
End
END


else
begin
if EXISTS(select top(1) 1 from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
BEGIN
BEGIN TRANSACTION DelRecordInRecordHistory
SET @errorSun=0 --没错为0
set @SequenceIDHistory=(select top(1) SequenceID from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
set @CustIDHistory=@CustID
set @ProvinceIDHistory=(select top(1) ProvinceID from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
set @OPTypeHistory=(select top(1) OPType from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
set @MessageHistory=(select top(1) Message from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
set @AuthenCodeHistory=@AuthenCode
set @ResultHistory=(select top(1) Result from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
set @EmailHistory=@Email
set @DealTimeHistory=(select top(1) DealTime from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
set @DescriptionHistory=(select top(1) Description from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)
set @NotifyCountHistory=(select top(1) NotifyCount from SendEmailRecord where CustID=@CustID and Email=@Email and AuthenCode=@AuthenCode order by DealTime desc)


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
SET @SqlResult=0
return
END


END

begin
SET @SqlResult=-30000
SET @ErrMsg='帐号无效'
return


end
END



