# Swagger API 三層式架構

## 1. Controller 層
- 處理 HTTP 請求，調用 Service 層的方法。
- 例如，`TodoController` 會調用 `TodoService` 中的方法來獲取、創建或更新 Todo 項目。

## 2. Service 層
- 包含業務邏輯，負責處理來自 Controller 的請求。
- 例如，`TodoService` 實現 `ITodoService` 接口，提供業務相關的方法，如 `AddTodo`、`GetAllTodos` 等。
- `TodoService` 會調用 `IRepository` 接口中的方法來執行數據操作。

## 3. Repository 層
- 負責與數據庫進行交互，執行實際的數據操作。