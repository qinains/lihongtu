SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





/*
 * 存储过程dbo.up_BT_V2_Interface_QueryCustInfo
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-5-20
 * Remark:
 *
 */

 
 ALTER   Procedure [dbo].[up_BT_V2_Interface_QueryCustInfo]
(
	@CustID varchar(16),
	@UserAccount varchar(16)
)
as
		--基本信息
	select a.UserAccount, a.CustID,a.UserType,a.[Password],a.CertificateType,a.CertificateCode,
		a.RealName,a.UProvinceID,a.AreaCode ,
		a.CustLevel,a.Status, a.EnterpriseID, a.RegistrationDate,b.Sex, b.BirthDay, b.EduLevel, b.Favorite,b.InComeLevel,b.Email, b.CustContactTel,
		c.PaymentAccountType,c.PaymentAccount,c.PaymentAccountPassword,d.AuthenName, a.outerID outerID
		 from 
	(select j.UserAccount, i.CustID,i.CustType UserType,i.EncryptedPassword [Password],IsNull(i.CertificateType,'') CertificateType, IsNull(i.CertificateCode,'') CertificateCode,
		i.RealName RealName,i.ProvinceID UProvinceID,
		case left(i.AreaID,1) 
			when '0' then Right(i.AreaID,2)
			else i.AreaID
		end 
		AreaCode ,
		i.CustLevel CustLevel,i.Status Status, IsNull(EnterpriseID,'') EnterpriseID, j.RegistrationDate, j.outerID
	   from CustInfo i, CustUserInfo j where i.CustID = @CustID and i.UserAccount=@UserAccount and i.CustID=j.CustID) a,
	(select UserAccount, CustID,IsNull(Sex,'2') Sex ,IsNull(BirthDay,'') BirthDay ,IsNull(EduLevel,'') EduLevel,Favorites Favorite,InComeLevel,Email,IsNull(CustContactTel,'') CustContactTel
	   from CustExtend where CustID = @CustID and UserAccount=@UserAccount) b,
	(select UserAccount, CustID,AccountType PaymentAccountType, AccountNumber PaymentAccount, AccountPassword PaymentAccountPassword
	   from PaymentAccount where CustID = @CustID and UserAccount=@UserAccount) c,
		(select custID, AuthenName from UserAuthenStyle where CustID =@CustID and AuthenType='1' and status='0') d
	where a.CustID *= b.CustID and a.CustID *= c.CustID and a.CustID *= d.CustID
	

	




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

