set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go








ALTER proc [dbo].[up_Customer_V3_Interface_FindPwdByEmail]
(
@UserName varchar(30),
@Email varchar(100),
@SqlResult int out,
@Pwd varchar(128) out,
@ErrMsg varchar(256) out
)
as
BEGIN

 
if exists(select 1 from custinfo where username=@UserName and Email=@Email)
begin
set @SqlResult=0
end



else
begin
set @SqlResult=-30008
set @ErrMsg='帐号无效'
end

END



