set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go









ALTER proc [dbo].[up_Customer_V3_Interface_SelPhoneUPPwd]
(
@Num varchar(20),
@PwdType int,
@Phone varchar(20),
@sqlResult int out,
@ErrMsg varchar(256) out
)
as
DECLARE @CustID varchar(16)
BEGIN
	if exists (select 1 from custphone where phone=@phone and phoneclass!='1' and phonetype='2')
	 begin
	    set @CustID=(select custid from custphone where phone=@phone and phoneclass!='1' and phonetype='2')
		if (@PwdType=1)
		 begin
			update custinfo set voicepwd=@Num where CustID=@CustID
			set @sqlResult=0
			return 
		 end
		else
		 begin
			update custinfo set webpwd=@Num where CustID=@CustID
			set @sqlResult=0
			return 
		 end
	 end
	else
	 begin
		set @sqlResult=-30008
		set @ErrMsg='您输入的认证手机号并没有被绑定'
	 end
END

