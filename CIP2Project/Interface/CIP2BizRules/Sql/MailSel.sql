set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go





ALTER proc [dbo].[up_Customer_V3_Interface_MailSel]
(
@CustID varchar(16),
--@CustType varchar(16),
@Email varchar(20),
@SourceSPID varchar(8),
@SqlResult int out,
@ErrMsg varchar(256) out
)
as
DECLARE @CustType varchar(16)
begin

if (@CustID='')
begin
if  exists(select 1 from custinfo where  custtype='01' and Email=@Email and emailclass='2' and SourceSPID=@SourceSPID)
begin
set @SqlResult=-30000
set @ErrMsg='该邮箱已经被其他用户设置为认证邮箱'

return
end


ELSE
begin
set @SqlResult=0
return
end

end

else
begin

if exists (select 1 from custinfo where custid=@custid )
begin
select custtype=@CustType from custinfo where CustID=@CustID
if  exists(select 1 from custinfo where Email=@Email and emailclass='2' )
begin
set @SqlResult=-30000
set @ErrMsg='该邮箱已经被其他用户设置为认证邮箱'
return
end
else
begin
set @SqlResult=0
return
end
end
end
End





