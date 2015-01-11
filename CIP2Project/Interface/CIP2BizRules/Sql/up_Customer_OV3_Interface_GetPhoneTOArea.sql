set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go





create procedure [dbo].[up_Customer_OV3_Interface_GetPhoneTOArea] (
 	@Phone varchar(20),
 	
 	@Result int out,
	@ErrMsg varchar(256) out,
    @ProvinceID varchar(2) out,
    @Areaid varchar(3) out
 ) as

	set @Result = -19999
    set @Areaid=''
	set @ErrMsg = ''
	set @ProvinceID = ''
    

   
   select top 1 @AreaID=areaid,@ProvinceID=ProvinceID from  phonearea with(nolock) where phone=left(@Phone,7) and status='00'
  if(@@RowCount = 0)
  begin
         Set @Result = -20504
	     Set @ErrMsg = '没有该号码的地区信息！'
		 return
  end 
  set @Result =0
 

 
