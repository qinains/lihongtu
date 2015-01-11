USE [CIP2]
GO
/****** 对象:  StoredProcedure [dbo].[up_Customer_OV3_Interface_QueryCustInfo]    脚本日期: 10/22/2009 10:31:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER procedure [dbo].[up_Customer_OV3_Interface_QueryCustInfo] (
 	@CustID varchar(16),
 	@UserAccount varchar(16)
 ) as
--基本信息
select a.UserAccount, a.CustID,a.UserType,a.[Password],a.CertificateType,a.CertificateCode,
		a.RealName,a.UProvinceID,a.AreaCode ,
		a.CustLevel,a.Status, a.EnterpriseID, a.RegistrationDate,a.Sex, b.BirthDay, b.EduLevel, b.Favorite,b.InComeLevel,a.Email,-- d.CustContactTel,
		c.PaymentAccountType,c.PaymentAccount,c.PaymentAccountPassword,a.AuthenName
from 
(select '' UserAccount,c.custid,c.custtype UserType,c.webpwd [Password],c.certificatetype,c.certificateCode,c.realname,c.provinceid UProvinceID,c.areaID areaCode,
c.custlevel,c.status,'' EnterpriseID,c.CreateTime RegistrationDate,c.sex,c.email,c.UserName AuthenName,c.outerid
from Custinfo c with(nolock) where CustID = @CustID ) a  right outer join 
(select  CustID,IsNull(BirthDay,'') BirthDay ,IsNull(EduLevel,'') EduLevel,Favorite Favorite,InComeLevel
	   from CustExtendInfo with(nolock)  where CustID = @CustID ) b on a.CustID = b.CustID right outer join 
(select  @CustID CustID,''  PaymentAccountType,'' PaymentAccount,'' PaymentAccountPassword	   ) c  on a.CustID = c.CustID right outer join 
(select top 1 custid,phone CustContactTel from custphone   with(nolock)  where CustID = @CustID) d 	on a.CustID = d.CustID 
	
	--where a.CustID *= b.CustID and a.CustID *= c.CustID and a.CustID = d.CustID
