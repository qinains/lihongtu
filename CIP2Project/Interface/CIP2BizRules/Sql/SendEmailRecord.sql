set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go




ALTER proc [dbo].[up_Customer_V3_Interface_SendEmailRecord]
(
@CustID varchar(16),
@Message text,
@AuthenCode varchar(6),
@Result int ,
@Email varchar(100),
@DealTime datetime,
@Description varchar(40),
@NotifyCount int,
@OPType varchar(1),
@sqlResult int out,
@ErrMsg varchar(256) out
)
as
DECLARE  @ProvinceID varchar(2)
Begin
if (@CustID='')
begin
set @ProvinceID=''
insert into SendEmailRecord(CustID,ProvinceID,OPType,Message,AuthenCode,Result,Email,DealTime,Description,NotifyCount) 
values(@CustID,@ProvinceID,@OPType,@Message,@AuthenCode,@Result,@Email,@DealTime,@Description,@NotifyCount) 

if exists(select 1 from SendEmailRecord where Email=@Email and DealTime=@DealTime)
begin
set @sqlResult=0
return
end

else

begin
set @sqlResult=-30000
set @ErrMsg='邮箱验证码插入失败'
return
end
end

else
begin
begin
select @ProvinceID = ProvinceID from custinfo where CustId=@CustId
end
insert into SendEmailRecord(CustID,ProvinceID,OPType,Message,AuthenCode,Result,Email,DealTime,Description,NotifyCount) 
values(@CustID,@ProvinceID,@OPType,@Message,@AuthenCode,@Result,@Email,@DealTime,@Description,@NotifyCount) 

if exists(select 1 from SendEmailRecord where CustID=@CustID and Email=@Email and DealTime=@DealTime)
begin
set @sqlResult=0
return
end

else

begin
set @sqlResult=-30000
set @ErrMsg='邮箱验证码插入失败'
return
end

end


END

