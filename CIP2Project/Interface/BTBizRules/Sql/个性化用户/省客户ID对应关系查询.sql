SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


ALTER   procedure  [dbo].[up_v5_BestTone_CustProvinceRelationQuery]
(
@Outerid varchar(16),
@ProvinceID varchar(2),

@CustID varchar(16) out ,
@SID varchar(16) out ,
@CustAccount varchar(16) out,
@Result int out,
@ErrorDescription varchar(256) out
)

AS
BEGIN

SET @Result = 0
SEt @ErrorDescription = '' 

select @CustID=c.CustID,@SID=u.outerid,@CustAccount=c.useraccount
from CustInfo c with(nolock) ,CustUserInfo u with(nolock) where u.outerid=@Outerid and c.custid=u.custid
 if(@@RowCount=0)
		begin
            Set @Result = -19999
			Set @ErrorDescription = '²»´æÔÚOuterid£¡'				
			return
		end
END




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO