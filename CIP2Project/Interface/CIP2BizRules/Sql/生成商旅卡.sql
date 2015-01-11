set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



/*
 * �洢����up_Customer_OV3_Interface_UserRegistryV2
 *
 * ��������: ���ɿ���
 *			 
 *
 * Author: tongbo
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-11
 * Remark:
 *
 */

ALTER Procedure [dbo].[up_Customer_OV3_Interface_UserRegistryV2]
(
@CustID varchar(16),
@parCardID varchar(16),
@UProvinceID varchar(2),
@AreaCode varchar(3),
@CardType int,
@CustLevel  varchar(1),
@CardRegSource varchar(2),
@CardRegType varchar(1),

@sUserAccount varchar(9) out,
@UserAccount varchar(16) out,
@Result int out,
@ErrMsg varchar(256) out
)
as
begin
Declare @OriginalAreaCode varchar(3) 
declare @retainStr char(1) 
declare @cardSerial bigint 
declare @cardID varchar(6) 

declare @isbool char(1)--�ж�areaid�Ƿ�2λ
declare @ExtendAreaID varchar(3)
declare @areid varchar(3)
declare @cardPre varchar(8) 
Declare @ExeSql nvarchar(4000)   --�������Ƿ��Ѿ�����      

Set @OriginalAreaCode = right(('0' + @AreaCode),3) 

 if not exists(select 1 from area  with(nolock) where areaid=@OriginalAreaCode and provinceid=@UProvinceID)  
 Begin
	set @Result = -30001      
	set @ErrMsg = '���������'+@OriginalAreaCode
	return 
 End  
 -----------------------------------------------
 set  @isbool='0'   
 --���ɿ���ʱ��3λ���ĵ�����Ҫ�ں��油0      
 if(len(@AreaCode)=3 and left(@AreaCode,1)='0') 
  begin  
    set @areid= @AreaCode+'0'  
    set @AreaCode=right(@AreaCode,2)+'0' 
    set   @isbool='1'   
  end     
     

 if(@CustLevel='0')
 begin
	set @CustLevel='3'
 end

  set @retainStr='1'
-----------------------------------------------

if(@parCardID is not null and @parCardID<>'')
begin

    set @sUserAccount= @parCardID
    set @UserAccount ='86'+'00'+@parCardID+ '000'

    select 1 from CustTourCard with(nolock) where CardID=@sUserAccount and InnerCardID=@UserAccount
    if(@@RowCount>0) 
	begin   
		set @Result = -30001      
	    set @ErrMsg = '���Ѿ�����1��'
	    return  
	end

   
    Set @ExeSql = ' select 1 from BestToneCard_'+@UProvinceID+'_'+@OriginalAreaCode + '  with(nolock) where InnerCardID=''' + @UserAccount + ''' and CardID=''' + @sUserAccount + ''''    
    Exec(@ExeSql)      
    if(@@RowCount>0)      
    begin      
        set @Result = -30001      
	    set @ErrMsg = '���Ѿ�����2��'
	    return     
    end   

     insert into CustTourCard (CustID,CardID,CardType,CardProvinceID,CardAreaID,InnerCardID,CardRegSource,CardRegType,CardRegDate,IsHaveCard,SourceSPID,Dealtime,Status) values
             (@CustID,@sUserAccount,@CardType,@UProvinceID,@OriginalAreaCode,@UserAccount,@CardRegSource,@CardRegType,getdate(),1,'3500001',getdate(),'00')
	   if ( @@error <> 0)
	   begin
			 set @Result = -22500       
			 set @ErrMsg = '�ƿ�ʧ�ܣ�'      
	          
			 return      
	   end 
		set @Result = 0      
		set @ErrMsg = ''  
	return 
end  

----------------------------------------------------

while(1=1)
begin

 select @cardSerial=SequenceID+1,@ExtendAreaID=ExtendAreaID from Autodistribute with(nolock) where ProvinceID=@UProvinceID and AreaID=@OriginalAreaCode and  htype=@CardType and status='0'

 if (@cardSerial > 99999)      
   Begin   
     if(@isbool='0'   )
     begin      
		 set @Result = -22500      
		 set @ErrMsg = '��ǰ���������Զ����俨��Դ���������俨ʧ��'
		 return      
     end   

     begin tran  

     update Autodistribute set status='1' where ProvinceID=@UProvinceID and AreaID=@OriginalAreaCode and htype=@CardType and status='0'
     if ( @@error <> 0)
      begin
         set @Result = -22500      
		 set @ErrMsg = '��������ʧ��1��'  
         rollback   
      end 

     if(@ExtendAreaID is null or @ExtendAreaID='')
        set @ExtendAreaID= @areid+'0'       
    
     set @ExtendAreaID=convert(int,@ExtendAreaID)+1


     insert into  Autodistribute(Hflag,AreaID,ExtendAreaID,ProvinceID,SequenceID,Htype,Status)
        values('0',@OriginalAreaCode,@ExtendAreaID,@UProvinceID,0,convert(varchar,@CardType),'0')
     if ( @@error <> 0)
       begin
         set @Result = -22500      
		 set @ErrMsg = '��������ʧ��2��'

         rollback   
       end 

     commit

     set @cardSerial=1

   End 



 set @cardPre = '86'+'00'+@CustLevel+@ExtendAreaID+@retainStr 

 
   begin tran 
   update Autodistribute set SequenceID=@cardSerial where ProvinceID=@UProvinceID and AreaID=@OriginalAreaCode and  htype=@CardType and status='0'
    if ( @@error <> 0)
       begin
         set @Result = -22500      
		 set @ErrMsg = '���¿�������ʧ�ܣ�'  
         rollback   
       end 
---------------
   set @cardID=Convert(varchar,@cardSerial)

   while(len(@cardID)<5)
   begin
     set @cardID='0'+@cardID
   end 

--   860120011000
--20011000
    --ȡ�ÿ���      
    set @UserAccount=Convert(varchar,@cardPre)+@cardID+'000'      
    set @sUserAccount= SubString(@UserAccount,5,9)     

--print  '86'
--print '0'
--print @CustLevel
--print @ExtendAreaID
--print @retainStr  
--print @cardID
--print '000'    
--print @UserAccount
--print @sUserAccount


    select 1 from CustTourCard  with(nolock) where CardID=@sUserAccount and InnerCardID=@UserAccount
    if(@@RowCount>0) 
	begin   
		 continue
	end


    
    Set @ExeSql = ' select 1 from BestToneCard_'+@UProvinceID+'_'+@OriginalAreaCode + ' with(nolock) where InnerCardID=''' + @UserAccount + ''' and CardID=''' + @sUserAccount + ''''    
    Exec(@ExeSql)      
    if(@@RowCount>0)      
    begin      
     continue      
    end      
   insert into CustTourCard (CustID,CardID,CardType,CardProvinceID,CardAreaID,InnerCardID,CardRegSource,CardRegType,CardRegDate,IsHaveCard,SourceSPID,Dealtime,Status) values
             (@CustID,@sUserAccount,@CardType,@UProvinceID,@OriginalAreaCode,@UserAccount,@CardRegSource,@CardRegType,getdate(),1,'3500001',getdate(),'00')
   if ( @@error <> 0)
   begin
         set @Result = -22500       
		 set @ErrMsg = '�ƿ�ʧ�ܣ�'      
          rollback
		 return      
   end 
commit

   set @Result = 0       
   set @ErrMsg = ''  
 
   return
  end
--end

end

