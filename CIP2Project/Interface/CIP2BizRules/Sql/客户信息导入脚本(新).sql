--�ͻ���Ϣ����ű�
------------------------------------------------------------------------------------------------------------
--���ݵ���ɹ����ղ���ִ��
--��һ��ִ��---start
declare @temTableRecordCount bigint   --��¼��ʱ���¼����
declare @parSequenceNumber bigint     --Ŀǰϵͳ���CustID
select @temTableRecordCount=count(1) from ��ʱ���ݱ�  --tmp1Ϊ������ʱ��
select @temTableRecordCount as '��ʱ���ݱ��¼����'
select @parSequenceNumber=SequenceID from parSequenceNumber where sequenceType = '0101'
select @parSequenceNumber as 'δ����ǰϵͳ���CustID'
set @parSequenceNumber = @parSequenceNumber + @temTableRecordCount
update parSequenceNumber set SequenceID=@parSequenceNumber where sequenceType = '0101'
select @parSequenceNumber as '���Ӻ�ϵͳ���CustID'
select * from parSequenceNumber  --�鿴parSequenceNumber��SequenceID���Ƿ��ѱ��޸�
--��һ��ִ��--end
------------------------------------------------------------------------------------------------------------

--�ڶ���ִ��--start
--�޸���ʱ���ݱ�ṹ(�õ�һ���ġ����Ӻ�ϵͳ���CustIDֵ���滻'��ʼֵ')
ALTER   TABLE   ��ʱ���ݱ�  
  add   [CustID]   [bigint]   IDENTITY   (25920591,   1)   NOT   NULL  
GO
--�ڶ���ִ��--end

------------------------------------------------------------------------------------------------------------

--������ִ��--start
--����У��
select * from ��ʱ���ݱ� where len(custAccount)>16 or len(CustType)>2
or len(CertificateType)>1 or len(RealName)>100 or datalength(CertificateCode)>30
or len(Sex)>1 or len(contactTel)>13 or len(areaID)>3 or len(custLevel)>1
--������ڴ�������1.�������ݴ�������ݴ������������ImportData_����_����_DateError��
--1.
select * into ImportData_gs_20090915_DateError
from ��ʱ���ݱ� where len(custAccount)>16 or len(CustType)>2
or len(CertificateType)>1 or len(RealName)>100 or len(zipcode)>6 or datalength(CertificateCode)>30
or len(Sex)>1 or len(contactTel)>13 or len(areaID)>3 or len(custLevel)>1
--2.����������ݼ�ԭ��
select * from  ��ʱ���ݱ�  where  len(RealName)>30 --41
select * from  ��ʱ���ݱ�  where  len(custAccount)>16 
select * from  ��ʱ���ݱ�  where  custType not in (select CustType from ParCustType)
select * from  ��ʱ���ݱ�  where  len(CertificateType)>1 --14
select * from  ��ʱ���ݱ�  where  datalength(CertificateCode)>30
select * from  ��ʱ���ݱ�  where  sex not in ('0','1','2') --63
select * from  ��ʱ���ݱ�  where  len(contactTel)>13 --17
select * from  ��ʱ���ݱ�  where  len(areaID)>3 --
select * from  ��ʱ���ݱ�  where  custLevel not in ('0','1','2','3','4')--55
select * from    where  len(zipcode)>6--6
--3.ɾ����ʱ���ݱ����ݴ����¼
delete from ��ʱ���ݱ�
where outerID in (select outerID from ImportData_gs_20090915_DateError)

--������ִ��--end

------------------------------------------------------------------------------------------------------------
--���Ĳ�ִ��--start(�ظ����ݴ������������--ImportData_����_����_RepeatUserAccount) 
--����ظ������ݲ������ظ����ݴ����
select * into ImportData_gs_20090915_RepeatUserAccount from ��ʱ���ݱ�
where outerID in (select outerID from custinfo)
--���������ݣ�ִ�е��岽��û��ִ�е�����
--���Ĳ�ִ��--end

------------------------------------------------------------------------------------------------------------
--���岽ִ��--start
--�����ʱ���ݱ��ظ����ݼ�¼
delete from ��ʱ���ݱ�
where outerID in (select outerID from ImportData_gs_20090915_RepeatUserAccount)
--���岽ִ��--end

------------------------------------------------------------------------------------------------------------
--������ִ��--start
--�޸���ʱ���ݱ��ֶγ���
alter Table ��ʱ���ݱ� alter column outerID varchar(30)
alter Table ��ʱ���ݱ� alter column custType varchar(2)
alter Table ��ʱ���ݱ� alter column custAccount varchar(16)
alter Table ��ʱ���ݱ� alter column realName varchar(100)
alter Table ��ʱ���ݱ� alter column custLevel varchar(1)
alter Table ��ʱ���ݱ� alter column areaID varchar(3)
alter Table ��ʱ���ݱ� alter column address varchar(256)
alter Table ��ʱ���ݱ� alter column zipCode varchar(6)
alter Table ��ʱ���ݱ� alter column sex varchar(1)
alter Table ��ʱ���ݱ� alter column certificateType varchar(1)
alter Table ��ʱ���ݱ� alter column CertificateCode varchar(30)
alter Table ��ʱ���ݱ� alter column contactTel varchar(20)
alter Table ��ʱ���ݱ� alter column email varchar(256)
--������ִ��--end

------------------------------------------------------------------------------------------------------------

--���߲�ִ��--start
--��ʽ�������
declare @UProvinceID varchar(2)
declare @SPID  varchar(8)
Set @UProvinceID = '14'  ---------------------����ʱ�밴����޸�------------------
Set @SPID = @UProvinceID + '999999'
declare	@RegistrationSource char(2)
declare	@RegistrationType char(1)
set @RegistrationSource = '07'
set @RegistrationType = '2'
declare @AddressType varchar(2)
set @AddressType= '00' --�˵���ַ
declare @status varchar(2)
set @status = '00'  --�ͻ�״̬
declare @EmailClass int
declare @PhoneClass varchar(1)
declare @PhoneType varchar(1)
set @EmailClass = 1 --�ʼ�����
set @PhoneClass = '1' --�绰����
set @PhoneType = '4'  --�绰����
declare	@RegDate DateTime
set @RegDate = getDate()

--1.����ͻ���Ϣ��
insert into CIP2..CustInfo(CustID,ProvinceID,AreaID,CustType,CertificateType,CertificateCode,RealName,
CustLevel,Sex,RegistrationSource,Status,Email,EmailClass,SourceSPID,OuterID,DealTime,CreateTime)
select CustID,@UProvinceID,areaID,custType,certificateType,certificateCode,realName,
custLevel,sex,@RegistrationSource,@status,email,@EmailClass,@SPID,outerID,@RegDate,@RegDate  
from ��ʱ���ݱ�


--2.����ͻ���ַ��Ϣ��
insert into CIP2..AddressInfo(CustID,AddressProvince,AddressArea,Address,ZipCode,Type,DealTime) 
select CustID,@UProvinceID,areaID,address,zipCode,@AddressType,@RegDate  
from ��ʱ���ݱ�

--3.����ͻ��绰��Ϣ��
insert into CIP2..CustPhone(CustID,ProvinceID,CustType,Phone,PhoneType,PhoneClass,SourceSPID,Dealtime)
select CustID,@UProvinceID,custType,contactTel,@PhoneType,@PhoneClass,@SPID,@RegDate 
from ��ʱ���ݱ�

--���߲�ִ��--end

------------------------------------------------------------------------------------------------------------
--�ڰ˲�ִ��---start
--������Ӧ��ϵ(��Ӧ��ϵ����������--ImportData_����_����_Relation)
select CustID, OuterID, AreaID into ImportData_gs_20090323_Relation 
from CIP2..custInfo with(nolock) 
where Dealtime>'2009-05-09 04:00:01' and SourceSPID = @SPID   ----����ʱ��Ϊ��������ʱ��
--�ڰ˲�ִ��--end


--�����Ա�
update ��ʱ���ݱ� set sex = '2' where  sex not in ('0','1','2')