# SurvivorsPractice

以 **「生存 + 自動攻擊」** 為核心的 2D 動作遊戲練習專案：玩家以走位與升級選擇為主，在怪物潮中撐過關卡時間；並包含 Run 結算、永久強化（MetaProgress）等系統方向。產品需求與詞彙說明可參考倉庫內 `.cursor/rules/project/vampire_survivors_outline.md`。

專案進行中，尚未完成。

## 環境需求
 - **Unity**: 6000.3

## 技術棧（主要套件）
付費套件:
 - **Editor 擴充**: Sirenix Odin Inspector
 - **Tween動畫**: DoTween Pro

免費套件:
- **DI**：VContainer  
- **訊息 / 事件**：MessagePipe（含 VContainer 整合）  
- **非同步**：UniTask  
- **狀態機**：UnityHFSM  
- **數值屬性**：rpg-stats（Kryzarel）  
- **工具庫**：ZString、ZLogger、ZLinq 
- **NuGet**：NuGetForUnity（若專案有額外 .NET 套件需求）

## 快速開始

1. 以 Unity Hub 開啟本倉庫根目錄（內含 `Assets`、`Packages`、`ProjectSettings`）。
2. 等待套件與腳本編譯完成。
3. 開啟主要遊玩場景：`Assets/GamePlay/Content/Scenes/Mvp1.unity`（另有 `SampleScene.unity` 可作參考）。
4. 按下 Play 執行（實際入口依場景內 Bootstrapper / Installer 設定為準）。


完整清單見 `Packages/manifest.json`。

## 專案結構（程式）

主要遊戲邏輯集中在 `Assets/GamePlay/Scripts/`：

| 路徑 | 說明 |
|------|------|
| `Application/` | Run、DI（`Mvp1Installer`、`RootInstaller`、`Mvp1Bootstrapper` 等） |
| `Actor/` | 玩家、敵人、角色、Build |
| `Combat/` | 戰鬥管線、HP、護甲、抗性、無敵 |
| `Equipment/` | 武器定義與視覺 |
| `Item/` | 被動道具、寶箱、經驗寶石等 |
| `MetaProgress/` | 永久進度資料與檔案儲存 |
| `Service/` | 工廠、池化、Meta 服務等 |
| `Stage/` | 關卡執行期（`StageRuntime`） |
| `SpatialHash2D/` | 2D 空間雜湊與代理註冊 |
| `Targeting/` | 目標選取策略（最近、圓形、矩形、扇形等） |
| `Movement/` | 移動步驟管線（追逐、分離等） |
| `Status/` | 狀態容器、Buff / PowerUp |
| `Tests/EditMode/` | Edit Mode 測試（例如 `SpatialHashGrid2DTests`） |

## 授權與歸屬

若未另行標示，程式與資產歸屬以專案維護者／授權檔案為準；第三方套件各自遵循其授權條款。
