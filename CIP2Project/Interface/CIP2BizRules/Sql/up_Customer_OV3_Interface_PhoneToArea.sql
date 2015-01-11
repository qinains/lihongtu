set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



create procedure [dbo].[up_Customer_OV3_Interface_PhoneToArea] (
    @ProvinceID varchar(2),
    @Phone varchar(20),
    @AreaID varchar(4) out,
    @strPhone varchar(20) out
 ) as
set @AreaID=''
 select  @AreaID=areaid from area with(nolock) where provinceID=@ProvinceID and  areaid=left(@Phone,3)
 if(@@ROWCount>0)
 begin
  set @strPhone=right(@Phone,len(@Phone)-3)
  return
 end
 select  @AreaID=areaid from area with(nolock) where provinceID=@ProvinceID and  areaid=right(left(@Phone,4),3)
 if(@@ROWCount>0)
 begin
  set @AreaID='0'+@AreaID
  set @strPhone=right(@Phone,len(@Phone)-4)
  return
 end
 set @strPhone=@Phone

	
	
Set QUOTED_IDENTIFIER Off

