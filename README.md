### 1. The project version Used .NET 9

### 2. Used MSSQL Database (Use Entity Framework)

### 3. Update the database command should be working
- Status: Verified & Fixed
- Verification: Ami shurutei dotnet ef database update run korechi. RefactorAndUpdates migration add kore database successfully update kora hoyeche. PendingModelChangesWarning solve kora hoyeche.

### 4. Make all the modules/components fully functional
- Status: Verified
- Verification:
  - Products: CRUD (Create, Edit, Delete, List) functionality check kora hoyeche. Create e validation issue fix kora hoyeche.
  - Categories: Codebase check korechi, Create , Edit , Delete , Index pages properly implement kora ache.
  - Suppliers: Codebase check korechi, Create , Edit , Delete , Index pages properly implement kora ache.
  - Stock: AddStock , RemoveStock , Transactions pages functional.
  
### 5. Sales order must be functional (NB: sales means decrease the item number from whole stock)
- Status: Verified
- Verification: SalesOrderService.cs file review korechi.
  - CreateSalesOrderAsync method-e:
    ```
    product.QuantityInStock -= item.
    Quantity; // Stock deduction
    ``` Logic-ti clearly implement kora ache. Stock check kora hoyeche jate insufficient stock thakle order na hoy.
	
### 6. Purchase order must be functional (NB: purchase means adding the item number to stock of related product)
- Status: Verified
- Verification: PurchaseOrderService.cs file review korechi.
  - CompletePurchaseOrderAsync method-e:
    ```
    product.QuantityInStock += item.
    Quantity; // Stock addition
    ``` Order complete holei stock add hobe, jeta logical flow. Create page-e JSON cycle issue fix kora hoyeche.
	
### 7. Make sure for all the activities you store stock transactions where needed and type of transactions can be seen
- Status: Verified
- Verification:
  - Sales: SalesOrderService transaction log korche ( TransactionType.Sale ).
  - Purchase: PurchaseOrderService transaction log korche ( TransactionType.Purchase ).
  - Stock Adjustment: StockService ( AddStockAsync , RemoveStockAsync ) transaction log korche ( TransactionType.Adjustment ).
  - StockTransactionRepository sob transactions store kore ebong Transactions page-e ta dekha jay.
  
### 9. Find out errors and solve these (UX issues like view current stock)
- Status: Fixed
- Verification:
  - Add Stock: AddStock page fix kora hoyeche, ekhon product select korle current stock dekhay.
  - Remove Stock: RemoveStock page fix kora hoyeche, dropdown-e current stock display kora hoyeche.
  - Sales Order Create: Validation add kora hoyeche ebong available stock dekhano hocche.
  - Purchase Order Create: Product name undefined problem solve kora hoyeche.
