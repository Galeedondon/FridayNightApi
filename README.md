### 主要資料夾／檔案結構如下： 
Controllers/ — Web API 控制器 (endpoint)

Services/ — 服務層 (business logic)

Repositories/ — 資料存取層 (Repository pattern)

Models/ — Domain / 資料模型 (event, user, blog post …)

Data/ — 資料上下文 (DB context) 或種子資料 (seed)，設定資料庫

Program.cs + appsettings.json — 啟動與設定檔


### 三層式架構

將 Controller (API endpoint) / Service (商業邏輯) / Repository (資料存取) 分層，有助於責任分離 — 易於維護、測試、未來擴充。

前端 (Blazor)／後端 (API) 的分離，使 UI 可以專注於互動與顯示，後端專注於資料與邏輯 — modular。

使用者透過前端發送 HTTP request，所有資料操作都透過後端集中管理，安全且易於串接 (也方便日後改用不同前端或 native app)。


### 流程

```mermaid
graph TD

  U[使用者 在 UI 點擊  新增事件] --> UI1[前端顯示表單輸入 事件內容、時間、提醒設定 ...]
  UI1 --> Req1[前端 傳送 POST /events API 請求 包含事件資料]
  Req1 --> Ctrl1[後端 Controller: EventsController → CreateEvent]
  Ctrl1 --> Svc1[Service: 驗證資料合法性 / 商業邏輯 如是否衝突 ]
  Svc1 --> Repo1[Repository: 將事件寫入 DataContext / Database]
  Repo1 --> DB[(SQL/其他 DB 儲存)]
  DB --> Repo1
  Repo1 --> Svc1
  Svc1 --> Ctrl1
  Ctrl1 --> Res1[回傳 成功 / 失敗 JSON 結果 + 新事件資料]
  Res1 --> UI2[前端 接收 Response，更新日曆 UI顯示 新事件]
```
