# 7000 NET CORE 8.0考試項目

### 觀念題

1. 請說明DI與IoC
   - IoC 是一種設計原則，在一般的程式設計中，如果要使用物件要自己主動 new 一個，而「控制反轉」則是將這種控制權移轉給外部容器，只要開出清單，外部系統會在我需要時直接拿給我
   - DI 是實現 IoC 的方式，假設煮飯時，食材供應商會直接把食材送到廚房，不需要去尋找供應商，供應商會自動送來食材，這就是依賴注入。
  
2. 解釋Transient、Scoped、Singleton三個Service生命週期使用時機
   - Transient：每次請求時，都會建立一個全新的實例，使用於Email發送服務
   - Scoped：在同一個 http請求中會共用一個實例，使用於資料庫操作
   - Singleton：在程式執行期間，大家都共會同一個實例，使用於讀取設定檔
3. 請說明public、private、protected區別
   - public：沒有存取限制，任何人都可以直接使用
   - private：只有自身的class可以存取，外部的class 沒辦法使用
   - protected：只有自身的class 和 繼承的class可存取 
4. 請說明 static、readonly、const區別
   - Static：所有類別的instance都共享，且不需要執行new 就可以直接存取
   - readonly：只能在建構子內或宣告時賦值，建構子執行完畢後就不能修改內容了
   - const： 宣告時就要賦值，宣告後不能再更改內容，只能是基本型別或字串 
5. 請說明Array與List差異
   - 它們之間的主要差異是「長度是否固定」和「記憶體管理」
   - Array 長度固定且使用連續記憶體空間，存取資料速度較快
   - List 長度可變且不需要使用連續記憶體空間，可以動態調整長度
6. 說明IQueryable 與 IEnumerable差別
   - IEnumerable：從資料庫中撈取資料，存至記憶體中，並在記憶體中篩選。
   - IQueryable：在IDE中寫好的 IQueryable只是查詢狀態，此時還沒執行資料庫查詢，在使用ToList()的function時，才會執行SQL指令，取得查詢結果
