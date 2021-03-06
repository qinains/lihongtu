set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go









/*
 * 存储过程dbo.up_Customer_V3_Interface_QueryByPhone
 *
 * 功能描述: 获取主叫识别用户信息
 *
 * Author: liuchunli
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-7-30
 * Remark:
 *
 */

ALTER Procedure [dbo].[up_Customer_V3_Interface_QueryByPhone]
(
	@PhoneNum varchar(20),
	@Result int out,
	@ErrMsg varchar(40) out
)
as
	declare @CustID Varchar(16)
	declare @PhoneType Varchar(1)
	Set @Result = -19999
	Set @ErrMsg = ''

	if not exists(select 1 from CustPhone with(nolock) where Phone = @PhoneNum)
	begin
		set @Result = -30005
		set @ErrMsg = '该电话不存在'
		return
	end

	select custInfo_1.SourceSPID, custInfo_1.RealName,custInfo_1.Sex,custPhone_1.CustID,custPhone_1.CustType,custPhone_1.PhoneClass ,custPhone_1.Dealtime
	from custInfo custInfo_1 with(nolock),
	( select CustID, CustType,PhoneClass,Dealtime from CustPhone with(nolock) where Phone = @PhoneNum ) custPhone_1 
	where custInfo_1.CustID = custPhone_1.CustID
	order by custPhone_1.PhoneClass desc, custPhone_1.Dealtime desc
	
    if(@@Error<>0)
	Begin
		set @Result = -22500
		set @ErrMsg = '系统内部错误'	
	End
	else
	Begin
		set @Result = 0
	End






