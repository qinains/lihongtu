set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



ALTER proc [dbo].[up_Customer_OV3_Interface_UpdateWebPWD]
(
@Email varchar(100),
@AuthenCode varchar(6),
@NewPwd varchar(128),
@SqlResult int out,
@ErrMsg varchar(256) out
)
as
DECLARE @CustID varchar(16)
BEGIN
if exists(select 1 from SendEmailRecordHistory where Email=@Email and AuthenCode=@AuthenCode )
begin
set @CustID=(select CustID from CustInfo where Email=@Email)
update Custinfo set webpwd=@NewPwd where CustId=@CustId
set @SqlResult=0
return
end

begin
set @SqlResult=-30008
set @ErrMsg='帐号无效'
return
end
END


