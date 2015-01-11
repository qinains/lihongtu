USE [besttone]
GO
/****** ����:  StoredProcedure [dbo].[up_BT_OV2_Interface_UserInfoQuery]     ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





/*
 * �洢����dbo.up_BT_OV2_Interface_UserInfoQuery
 *
 * ��������: 
 *			 
 *
 * Author: zhoutao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-08-14
 * Remark:
 *
 */

 
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go







/*
 * �洢����dbo.up_Customer_OV3_Interface_UserInfoQuery
 *
 * ��������: 
 *			 
 *
 * Author: zhoutao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-14
 * Remark:
 *
 */

 
 ALTER Procedure [dbo].[up_Customer_OV3_Interface_UserInfoQuery]
(
	@ProvinceID varchar(2),
	@SPID varchar(8),
	@UserAccount varchar(16),  --��ӦCustTourCard��CardID
	@CustID varchar(16),
	@PhoneNum varchar(20),
	@Password varchar(6),
	@EncryptedPassword varchar(50),
	@Result int output,
	@ErrMsg varchar(40) output
)
as
	set @Result = -19999
	set @ErrMsg = ''
	
	declare @tmpCustID varchar(16)
	declare @tmpCustType varchar(2)
	set @tmpCustID = ''
	set @tmpCustType = ''

	if(@CustID <> '')
		begin
			set @tmpCustID=@CustID
		end
	else if(@CustID = '' and @PhoneNum <> '')
		begin
			select @tmpCustID=CustID from CustPhone where Phone=@PhoneNum
		end
	else if(@CustID = '' and @PhoneNum = '' and @UserAccount <> '')
		begin
			select @tmpCustID=CustID from CustTourCard where CardID=@UserAccount
		end

	if not exists ( select 1 From CustInfo Where CustID = @tmpCustID )
	Begin
		set @Result = -20504
		set @ErrMsg = '�û���Ϣ������,�޴��ʺ�'
		return
	End	

	select @tmpCustType=CustType from CustInfo where CustID = @tmpCustID
				
	--������Ϣ
	
	if(@tmpCustType = '41' or @tmpCustType = '42')
	begin 
		select top 1 * from(select CustType,CustID,ProvinceID,
				case(@SPID)
				when '01010101' then  e.RegionCode--�����ư�ϵͳ��ת��Ϊ����
				else
					case left(AreaID,1) 
						when '0' then Right(AreaID,2)
						else AreaID
					end
				end
		AreaID,Status,RealName,UserName,CertificateCode,
		CertificateType,Sex,CustLevel,Email from CustInfo with(nolock),RegionCodeArea e where CustID=@tmpCustID) a
		left join
		(select CustID,Birthday,EduLevel,Favorite,IncomeLevel from CustExtendInfo with(nolock) where 
		CustID=@tmpCustID) b on a.CustID=b.CustID
		left join
		(select CustID,Phone,PhoneClass from CustPhone with(nolock) where CustID=@tmpCustID ) c on a.CustID=c.CustID
		left join
		(select CustID,CardID,CardType from CustTourCard with(nolock) where CustID=@tmpCustID) d on a.CustID=d.CustID
		order by CardType,PhoneClass desc
	end
	else 
    begin
    	select top 1 * from(select CustType,CustID,ProvinceID,
				case(@SPID)
				when '01010101' then  e.RegionCode--�����ư�ϵͳ��ת��Ϊ����
				else
					case left(AreaID,1) 
						when '0' then Right(AreaID,2)
						else AreaID
					end
				end
		AreaID,Status,RealName,UserName,CertificateCode,
		CertificateType,Sex,CustLevel,Email from CustInfo with(nolock),RegionCodeArea e where CustID=@tmpCustID) a
		left join
		(select CustID,Birthday,EduLevel,Favorite,IncomeLevel from CustExtendInfo with(nolock) where 
		CustID=@tmpCustID) b on a.CustID=b.CustID
		left join
		(select CustID,Phone,PhoneClass from CustPhone with(nolock) where CustID=@tmpCustID ) c on a.CustID=c.CustID
		left join
		(select CustID,CardID,CardType from CustTourCard with(nolock) where CustID=@tmpCustID) d on a.CustID=d.CustID
		order by CardType,PhoneClass
	end

	--�󶨵绰
	select Phone from  CustPhone with(nolock) where CustID = @tmpCustID 
	
	--��ϵ��Ϣ
	select CustInfo.RealName,CustPhone.Phone,AddressInfo.Address,AddressInfo.ZipCode,AddressInfo.TYpe
	 from AddressInfo with(nolock),CustInfo with(nolock),CustPhone with(nolock)
	 where AddressInfo.CustID=@tmpCustID and AddressInfo.CustID=CustInfo.CustID and AddressInfo.CustID=CustPhone.CustID
	
	--��֤��ʽ
	select AuthenName,AuthenType from CustAuthenInfo with(nolock)
	where CustID = @tmpCustID


	Set @Result =0
	
Set QUOTED_IDENTIFIER Off





