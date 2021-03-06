alter   Procedure [dbo].[up_V5_BestTone_CustStatusChange]
(
	@ProvinceID	varchar(2),
    @CRMID      varchar(20),
    @CustType   varchar(2),
    @Status     varchar(2),
    @Result     int out,
    @ErrMsg     varchar(256) out,
    @OutProvinceID varchar(2) out

) 
as 
DECLARE @CustID  varchar(16)


select @CustID = CustID,@OutProvinceID=ProvinceID from CustInfo
 where OuterID=@CRMID and SysID=(@ProvinceID	+'01') and  CustType = @CustType  
if(@@Rowcount<1)
		Begin
			Set @Result = -19999
			set @ErrMsg = '数据不存在2'
			return
		End


select 1 from CustUserInfo where CustID=@CustID
		if(@@Rowcount<1)
		Begin
			Set @Result = -19999
			set @ErrMsg = '数据不存在1'
			return
		End


 begin tran
    update CustInfo set Status=@Status,DealTime=getdate()
    where CustID=@CustID
    If( @@error <> 0)
	begin
        Set @Result = -19999
		set @ErrMsg = '修改状态失败1'
		rollback
		return 												
	end
    update CustUserInfo set Status=@Status ,DealTime=getdate()
    where CustID=@CustID

    If( @@error <> 0)
	begin
		Set @Result = -19999
		set @ErrMsg = '修改状态失败2'
		rollback
		return 														
	end

    commit
    Set @Result = 0
	set @ErrMsg = ''