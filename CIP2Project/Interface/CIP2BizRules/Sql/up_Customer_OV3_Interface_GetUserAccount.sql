set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go





alter procedure [dbo].[up_Customer_OV3_Interface_GetUserAccount] (
    @CustID varchar(16),
 	@Result int out,
 	@ErrMsg varchar(256) out, 	
 	@UserAccount varchar(9) out
 
 
 ) as

	set @Result = -19999
    set @UserAccount=''
	set @ErrMsg = ''


   
     select  top 1  @UserAccount=CardID from CustTourCard with(nolock) where custid=@CustID and Status='00'
		if(@@RowCount=0)
		begin
           set @Result = -29999
           set @ErrMsg='卡号不存在！'
		end
     
    set @Result = 0    
	set @ErrMsg = '卡号已经存在！'