USE [CIP2]
GO
/****** ����:  StoredProcedure [dbo].[dbo.up_OV3_BestTone_CustStatusChange]    �ű�����: 10/22/2009 10:32:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   Procedure [dbo].[dbo.up_OV3_BestTone_CustStatusChange]
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

select @CustID = CustID from CustInfo where OuterID=@CRMID and SourceSPID=(@ProvinceID	+'01') and  CustType = @CustType  
if(@@Rowcount=0)
Begin
	Set @Result = -19999
	set @ErrMsg = '���û�������2'
	return
End


    update CustInfo set Status=@Status 
    where CustID=@CustID
    If( @@error <> 0)
	begin
        Set @Result = -19999
		set @ErrMsg = '�޸�״̬ʧ��'		
		return 												
	end
   
    Set @Result = 0
	set @ErrMsg = ''