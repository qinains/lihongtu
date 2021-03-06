set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go









ALTER proc [dbo].[up_Customer_V3_Interface_SendSMSRecord]
(
@CustID varchar(16),
@Message text,
@AuthenCode varchar(6),
@Result int ,
@Phone varchar(20),
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
insert into SendSMSRecord(CustID,ProvinceID,OPType,Message,AuthenCode,Result,Phone,DealTime,Description,NotifyCount) 
values(@CustID,@ProvinceID,@OPType,@Message,@AuthenCode,@Result,@Phone,@DealTime,@Description,@NotifyCount) 
if exists (select 1 from SendSMSRecord where Phone=@Phone and DealTime=@DealTime)
begin
set @sqlResult=0
return 
end
else
begin
set @sqlResult=-30000
set @ErrMsg='查询不到信息发送记录'
return
end
end

else
begin
select @ProvinceID = ProvinceID from custinfo where CustId=@CustId
begin 

insert into SendSMSRecord(CustID,ProvinceID,OPType,Message,AuthenCode,Result,Phone,DealTime,Description,NotifyCount) 
values(@CustID,@ProvinceID,@OPType,@Message,@AuthenCode,@Result,@Phone,@DealTime,@Description,@NotifyCount) 

if exists(select 1 from SendSMSRecord where custid=@CustID and DealTime=@DealTime)
begin
set @sqlResult=0
return
return
end
begin
set @sqlResult=-30000
set @ErrMsg='查询不到信息发送记录'
end

end
end
End




