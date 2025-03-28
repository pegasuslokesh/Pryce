GO
/****** Object:  StoredProcedure [dbo].[sp_Inv_SalesInvoiceHeader_SelectRow]    Script Date: 07/23/2015 12:04:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================================================
-- Entity Name:	sp_Inv_SalesInvoiceHeader_SelectRow
-- Author:	Lokesh Rawal
-- Create date:	7/13/2013 11:04:08 PM
-- Description:	This stored procedure is intended for selecting a specific row from Inv_SalesInvoiceHeader table
-- ==========================================================================================
ALTER Procedure [dbo].[sp_Inv_SalesInvoiceHeader_SelectRow]
	@Company_Id int,
	@Brand_Id int,
	@Location_Id int,
	@Trans_Id int,
	@Invoice_No nvarchar(255),
	@SIFromTransType nvarchar(1),
	@SIFromTransNo nvarchar(50),
	@Emp_Id nvarchar(Max),
	@Optype int
As
Begin

if(@Optype=1)
Begin
  SELECT [Company_Id]
      ,[Brand_Id]
      ,[Location_Id]
      ,[Trans_Id]
      ,[Invoice_No]
      ,[Invoice_Date]
      ,[PaymentModeId]
      ,[Currency_Id]
      ,[SIFromTransType]
      ,[SIFromTransNo]
      ,[SalesPerson_Id]
      ,[PosNo]
      ,[Remark]
      ,[Account_No]
      ,[Invoice_Costing]
      ,[Shift]
      ,[Post]
      ,[Tender]
      ,[Amount]
      ,[TotalQuantity]
      ,[TotalAmount]
      ,[NetTaxP]
      ,[NetTaxV]
      ,[NetAmount]
      ,[NetDiscountP]
      ,[NetDiscountV]
      ,[GrandTotal]
      ,[Supplier_Id]
      ,[Invoice_Ref_No]
      ,[Invoice_Merchant_Id]
      ,[Ref_Order_Number]
      ,[Condition1]
      ,[Condition2]
      ,[Condition3]
      ,[Condition4]
      ,[Condition5]
      ,[Field1]
      ,[Field2]
      ,[Field3]
      ,[Field4]
      ,[Field5]
      ,[Field6]
      ,[Field7]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,[IsActive]
  FROM Inv_SalesInvoiceHeader
  Where Company_Id = @Company_Id and Brand_Id=@Brand_Id and Location_Id=@Location_Id order by Trans_Id desc	
End
	
ELSE IF (@Optype=2)
Begin
  SELECT [Company_Id]
      ,[Brand_Id]
      ,[Location_Id]
      ,[Trans_Id]
      ,[Invoice_No]
      ,[Invoice_Date]
      ,[PaymentModeId]
      ,[Currency_Id]
      ,[SIFromTransType]
      ,[SIFromTransNo]
      ,[SalesPerson_Id]
      ,[PosNo]
      ,[Remark]
      ,[Account_No]
      ,[Invoice_Costing]
      ,[Shift]
      ,[Post]
      ,[Tender]
      ,[Amount]
      ,[TotalQuantity]
      ,[TotalAmount]
      ,[NetTaxP]
      ,[NetTaxV]
      ,[NetAmount]
      ,[NetDiscountP]
      ,[NetDiscountV]
      ,[GrandTotal]
      ,[Supplier_Id]
       ,[Invoice_Ref_No]
      ,[Invoice_Merchant_Id]
      ,[Ref_Order_Number]
      ,[Condition1]
      ,[Condition2]
      ,[Condition3]
      ,[Condition4]
      ,[Condition5]
      ,[Field1]
      ,[Field2]
      ,[Field3]
      ,[Field4]
      ,[Field5]
      ,[Field6]
      ,[Field7]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,[IsActive],
      (select Emp_Name from Set_EmployeeMaster where Emp_Id=Inv_SalesInvoiceHeader.SalesPerson_Id) as EmployeeName,
          (Select name from Ems_ContactMaster where Trans_Id =Inv_SalesInvoiceHeader.Supplier_Id) as CustomerName,
          case when Inv_SalesInvoiceHeader.SIFromTransType='S' then 'By Sales Order' end as Transtype
  FROM Inv_SalesInvoiceHeader
  Where Company_Id = @Company_Id and Brand_Id=@Brand_Id and Location_Id=@Location_Id and IsActive='True' order by Trans_Id desc
End	
	
ELSE IF (@Optype=3)
Begin
  SELECT [Company_Id]
      ,[Brand_Id]
      ,[Location_Id]
      ,[Trans_Id]
      ,[Invoice_No]
      ,[Invoice_Date]
      ,[PaymentModeId]
      ,[Currency_Id]
      ,[SIFromTransType]
      ,[SIFromTransNo]
      ,[SalesPerson_Id]
      ,[PosNo]
      ,[Remark]
      ,[Account_No]
      ,[Invoice_Costing]
      ,[Shift]
      ,[Post]
      ,[Tender]
      ,[Amount]
      ,[TotalQuantity]
      ,[TotalAmount]
      ,[NetTaxP]
      ,[NetTaxV]
      ,[NetAmount]
      ,[NetDiscountP]
      ,[NetDiscountV]
      ,[GrandTotal]
      ,[Supplier_Id]
       ,[Invoice_Ref_No]
      ,[Invoice_Merchant_Id]
      ,[Ref_Order_Number]
      ,[Condition1]
      ,[Condition2]
      ,[Condition3]
      ,[Condition4]
      ,[Condition5]
      ,[Field1]
      ,[Field2]
      ,[Field3]
      ,[Field4]
      ,[Field5]
      ,[Field6]
      ,[Field7]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,[IsActive],
        (select Emp_Name from Set_EmployeeMaster where Emp_Id=Inv_SalesInvoiceHeader.SalesPerson_Id) as EmployeeName,
          (Select name from Ems_ContactMaster where Trans_Id =Inv_SalesInvoiceHeader.Supplier_Id) as CustomerName
  FROM Inv_SalesInvoiceHeader
  Where Company_Id = @Company_Id and Brand_Id=@Brand_Id and Location_Id=@Location_Id and IsActive='False' order by Trans_Id desc
End	
	
ELSE IF (@Optype=4)
Begin
  SELECT [Company_Id]
      ,[Brand_Id]
      ,[Location_Id]
      ,[Trans_Id]
      ,[Invoice_No]
      ,[Invoice_Date]
      ,[PaymentModeId]
      ,[Currency_Id]
      ,[SIFromTransType]
      ,[SIFromTransNo]
      ,[SalesPerson_Id]
      ,[PosNo]
      ,[Remark]
      ,[Account_No]
      ,[Invoice_Costing]
      ,[Shift]
      ,[Post]
      ,[Tender]
      ,[Amount]
      ,[TotalQuantity]
      ,[TotalAmount]
      ,[NetTaxP]
      ,[NetTaxV]
      ,[NetAmount]
      ,[NetDiscountP]
      ,[NetDiscountV]
      ,[GrandTotal]
      ,[Supplier_Id]
       ,[Invoice_Ref_No]
      ,[Invoice_Merchant_Id]
      ,[Ref_Order_Number]
      ,[Condition1]
      ,[Condition2]
      ,[Condition3]
      ,[Condition4]
      ,[Condition5]
      ,[Field1]
      ,[Field2]
      ,[Field3]
      ,[Field4]
      ,[Field5]
      ,[Field6]
      ,[Field7]
    ,CAST(Inv_SalesInvoiceHeader.row_lock_Id as bigint) as Row_Lock_Id 
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,[IsActive],
       (select Emp_Name from Set_EmployeeMaster where Emp_Id=Inv_SalesInvoiceHeader.SalesPerson_Id) as EmployeeName,
          (Select name from Ems_ContactMaster where Trans_Id =Inv_SalesInvoiceHeader.Supplier_Id) as CustomerName,
           (Select name from Ems_ContactMaster where Trans_Id =Inv_SalesInvoiceHeader.Field2) as ShipCustomerName
  FROM Inv_SalesInvoiceHeader
  Where Company_Id = @Company_Id and Brand_Id=@Brand_Id and Location_Id=@Location_Id and IsActive='True' and Trans_Id=@Trans_Id order by Trans_Id desc
End	
	
ELSE IF (@Optype=5)
Begin
  SELECT [Company_Id]
      ,[Brand_Id]
      ,[Location_Id]
      ,[Trans_Id]
      ,[Invoice_No]
      ,[Invoice_Date]
      ,[PaymentModeId]
      ,[Currency_Id]
      ,[SIFromTransType]
      ,[SIFromTransNo]
      ,[SalesPerson_Id]
      ,[PosNo]
      ,[Remark]
      ,[Account_No]
      ,[Invoice_Costing]
      ,[Shift]
      ,[Post]
      ,[Tender]
      ,[Amount]
      ,[TotalQuantity]
      ,[TotalAmount]
      ,[NetTaxP]
      ,[NetTaxV]
      ,[NetAmount]
      ,[NetDiscountP]
      ,[NetDiscountV]
      ,[GrandTotal]
      ,[Supplier_Id]
       ,[Invoice_Ref_No]
      ,[Invoice_Merchant_Id]
      ,[Ref_Order_Number]
      ,[Condition1]
      ,[Condition2]
      ,[Condition3]
      ,[Condition4]
      ,[Condition5]
      ,[Field1]
      ,[Field2]
      ,[Field3]
      ,[Field4]
      ,[Field5]
      ,[Field6]
      ,[Field7]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,[IsActive]
  FROM Inv_SalesInvoiceHeader
  Where Company_Id = @Company_Id and Brand_Id=@Brand_Id and Location_Id=@Location_Id and IsActive='True' and Invoice_No=@Invoice_No order by Trans_Id desc
End

ELSE IF (@Optype=6)
Begin
  SELECT [Company_Id]
      ,[Brand_Id]
      ,[Location_Id]
      ,[Trans_Id]
      ,[Invoice_No]
      ,[Invoice_Date]
      ,[PaymentModeId]
      ,[Currency_Id]
      ,[SIFromTransType]
      ,[SIFromTransNo]
      ,[SalesPerson_Id]
      ,[PosNo]
      ,[Remark]
      ,[Account_No]
      ,[Invoice_Costing]
      ,[Shift]
      ,[Post]
      ,[Tender]
      ,[Amount]
      ,[TotalQuantity]
      ,[TotalAmount]
      ,[NetTaxP]
      ,[NetTaxV]
      ,[NetAmount]
      ,[NetDiscountP]
      ,[NetDiscountV]
      ,[GrandTotal]
      ,[Supplier_Id]
       ,[Invoice_Ref_No]
      ,[Invoice_Merchant_Id]
      ,[Ref_Order_Number]
      ,[Condition1]
      ,[Condition2]
      ,[Condition3]
      ,[Condition4]
      ,[Condition5]
      ,[Field1]
      ,[Field2]
      ,[Field3]
      ,[Field4]
      ,[Field5]
      ,[Field6]
      ,[Field7]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,[IsActive]
  FROM Inv_SalesInvoiceHeader
  Where Company_Id = @Company_Id and Brand_Id=@Brand_Id and Location_Id=@Location_Id and IsActive='True' and SIFromTransType=@SIFromTransType and SIFromTransNo=@SIFromTransNo order by Trans_Id desc
End
		
ELSE IF (@Optype=7)
Begin
   SELECT MAX(Trans_Id) FROM Inv_SalesInvoiceHeader
  Where Company_Id = @Company_Id and Brand_Id=@Brand_Id and Location_Id=@Location_Id
End	
End


Go
INSERT INTO [Att_AttendanceLog]([Company_Id],[Device_Id],[Emp_Id],[Event_Date],[Event_Time],[Func_Code],[Type],[Verified_Type],[IsActive],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate])
VALUES ('2','0','59','2015-07-15 00:00:00.000','2015-07-15 15:00:00.000','0','In','By Script','True','Lokesh','2015-07-15 00:00:00.000','Lokesh','2015-07-15 00:00:00.000')

Go
if not exists (SELECT * FROM   INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Att_AttendanceLog'AND COLUMN_NAME = 'Employee_Detail')
begin
alter table Att_AttendanceLog add Employee_Detail nvarchar(max)
end
Go
Update Att_AttendanceLog set Employee_Detail='Employee Detail Not Exist' 




