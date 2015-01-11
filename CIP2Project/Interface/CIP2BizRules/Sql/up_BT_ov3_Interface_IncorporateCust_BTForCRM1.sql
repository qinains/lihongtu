USE [CIP2]
GO
/****** ����:  StoredProcedure [dbo].[up_BT_ov3_Interface_IncorporateCust_BTForCRM1]    �ű�����: 10/22/2009 10:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER procedure [dbo].[up_BT_ov3_Interface_IncorporateCust_BTForCRM1] (
     @ProvinceID varchar(2),	
    -- @IncorporatedCustType varchar(16),
 	@IncorporatedCRMID varchar(16),	
 
 	@SavedCRMID  varchar(16),
 	--@SavedCustType varchar(2),
   
 
     @IncorporatedCustID varchar(16) out ,	
     @SavedCustID  varchar(16) out,
    
     @Result int out,	
     @ErrorDescription varchar(256) out--,
    -- @CustID varchar(16) out
 ) as
declare @SysID varchar(8)
    declare @Status varchar(2)
    set @SysID= @ProvinceID+'999999'

    select @IncorporatedCustID=CustID,@Status=Status from CustInfo with(nolock) where SourceSPID=@SysID and OuterID=@IncorporatedCRMID --and CustType=@IncorporatedCustType
    if(@@RowCount = 0)
	Begin
		Set @Result = -20504
		Set @ErrorDescription = '���ϲ����󲻴��ڣ�'
		return
	End

   if(@Status<>'00')
   begin
     Set @Result = -20503
	 Set @ErrorDescription = '���ϲ�����״̬��������'
	 return
   end

    select @SavedCustID=CustID,@Status=Status from CustInfo with(nolock) where SourceSPID=@SysID and OuterID=@SavedCRMID --and CustType=@SavedCustType
    if(@@RowCount = 0)
	Begin
		Set @Result = -20504
		Set @ErrorDescription = '�ϲ����󲻴��ڣ�'
		return
	End

   if(@Status<>'00')
   begin
     Set @Result = -20503
	 Set @ErrorDescription = '������״̬��������'
	 return
   end

   set @Result =0
   set @ErrorDescription=''

