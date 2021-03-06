set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go













/*
 * 存储过程dbo.up_Customer_V3_Interface_PhoneSel
 *
 * 功能描述: 查询手机是否可以绑定
 *
 * Author: zhaorui
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-5
 * Remark:
 *
 */

ALTER proc [dbo].[up_Customer_V3_Interface_PhoneSel]
(
	@CustID varchar(16),
	@Phone varchar(20),
--	@SourceSPID varchar(8),
	@Result int out,
	@ErrMsg varchar(256) out
)
as
	DECLARE @CustType varchar(2)

begin
	if (@CustID='')
	 begin
		set @CustType=43
		if  exists(select 1 from custphone with(nolock) where phone=@phone and CustType=@CustType and phoneclass!=1)
		begin
			set @Result=-30003
			set @ErrMsg='该电话已被其它客户绑定'
			return
		end

		else

		begin
			set @Result=0
			return
		end
	 end
else 
	begin
	 if exists (select 1 from custinfo where custID=@CustID)
		begin
			select custtype=@CustType from custinfo where CustID=@CustID
			if exists (select 1 from custphone with(nolock) where phone=@phone and CustID=@CustID and CustType='43' and phoneclass!=1)	
				begin
					set @Result=-30003
					set @ErrMsg='该电话已被其它客户绑定'
					return
				end
			else
				set @Result=0
				return	
		end
	 else
		begin
			set @Result=-3000
			set @ErrMsg='用户不存在'
		end
	end
end


