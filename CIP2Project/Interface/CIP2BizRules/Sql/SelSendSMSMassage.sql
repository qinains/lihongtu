set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go













ALTER proc [dbo].[up_Customer_V3_Interface_SelSendSMSMassage]
(
@CustID varchar(16),
@Phone varchar(20),
@AuthenCode varchar(6),
@SqlResult int out,
@ErrMsg varchar(256) out
)
as
BEGIN

if (@CustID='')
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
DECLARE @PhoneHistory varchar(20)
DECLARE @DealTimeHistory datetime
DECLARE @DescriptionHistory varchar(40)
DECLARE @NotifyCountHistory int 

SET @errorSun=0 --没错为0
set @SequenceIDHistory=(select top(1) SequenceID from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)
set @CustIDHistory=@CustID
set @ProvinceIDHistory=(select top(1) ProvinceID from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)
set @OPTypeHistory=(select top(1) OPType from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)
set @MessageHistory=(select top(1) Message from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)
set @AuthenCodeHistory=@AuthenCode
set @ResultHistory=(select top(1) Result from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)
set @PhoneHistory=@Phone
set @DealTimeHistory=(select top(1) DealTime from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)
set @DescriptionHistory=(select top(1) Description from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)
set @NotifyCountHistory=(select top(1) NotifyCount from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)

INSERT INTO SendSMSRecordHistory(SequenceID,CustID,ProvinceID,OPType,Message,AuthenCode,Result,Phone,DealTime,Description,NotifyCount )
values(@SequenceIDHistory,@CustIDHistory,@ProvinceIDHistory,@OPTypeHistory,@MessageHistory,@AuthenCodeHistory,@ResultHistory,@PhoneHistory,@DealTimeHistory,@DescriptionHistory,@NotifyCountHistory)

SET @errorSun=@errorSun+@@ERROR --累计是否有错
DELETE SendSMSRecord where SequenceID=@SequenceIDHistory

SET @errorSun=@errorSun+@@ERROR --累计是否有错

IF @errorSun<>0 BEGIN PRINT '有错误，回滚'

ROLLBACK TRANSACTION--事务回滚语句

END ELSE BEGIN PRINT '成功，提交'

COMMIT TRANSACTION--事务提交语句
set @sqlResult=0
return
end

end

else
begin
if EXISTS(select top(1) * from  SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode  order by DealTime desc )
BEGIN
BEGIN TRANSACTION DelRecordInRecordHistory
SET @errorSun=0 --没错为0
set @SequenceIDHistory=(select top(1) SequenceID from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)
set @CustIDHistory=@CustID
set @ProvinceIDHistory=(select top(1) ProvinceID from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)
set @OPTypeHistory=(select top(1) OPType from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)
set @MessageHistory=(select top(1) Message from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)
set @AuthenCodeHistory=@AuthenCode
set @ResultHistory=(select top(1) Result from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)
set @PhoneHistory=@Phone
set @DealTimeHistory=(select top(1) DealTime from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)
set @DescriptionHistory=(select top(1) Description from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)
set @NotifyCountHistory=(select top(1) NotifyCount from SendSMSRecord where CustID=@CustID and Phone=@Phone and AuthenCode=@AuthenCode order by DealTime desc)


INSERT INTO SendSMSRecordHistory(SequenceID,CustID,ProvinceID,OPType,Message,AuthenCode,Result,Phone,DealTime,Description,NotifyCount)
values(@SequenceIDHistory,@CustIDHistory,@ProvinceIDHistory,@OPTypeHistory,@MessageHistory,@AuthenCodeHistory,@ResultHistory,@PhoneHistory,@DealTimeHistory,@DescriptionHistory,@NotifyCountHistory)

SET @errorSun=@errorSun+@@ERROR --累计是否有错
DELETE SendSMSRecord where SequenceID=@SequenceIDHistory

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
SET @SqlResult=-30008
SET @ErrMsg='帐号无效'
return


end
END




