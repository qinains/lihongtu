set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go






ALTER proc [dbo].[up_Customer_V3_Interface_InsertSendEmail]
(
@UserName varchar(30),
@Email varchar(100),
@OPType varchar(1),
@Message text,
@AuthenCode varchar(6),
@Result int,
@DealTime datetime,
@Description varchar(40),
@NotifyCount int,
@SqlResult int out,
@ErrMsg varchar(256) out
)
as
DECLARE @CustID varchar(16)
DECLARE @ProvinceID varchar(16)

BEGIN
if exists (select 1 from custinfo where username=@UserName and email=@Email)
begin
select @CustID=CustID,@ProvinceID=ProvinceID from CustInfo where username=@UserName and email=@Email
end
begin
insert into SendEmailRecord (CustID,ProvinceID,OPType,Message,AuthenCode,Result,Email,DealTime,Description,NotifyCount)
values(@CustID,@ProvinceID,@OPType,@OPType,@AuthenCode,@Result,@Email,@DealTime,@Description,@NotifyCount)
set @SqlResult=0
return 
end


begin
set @SqlResult=-30008
set @ErrMsg='帐号无效'
return 
end


END



