# Vampire Survivors：Weapons（武器系統）學習筆記

資料來源：[Vampire Survivors Wiki：Weapons](https://vampire.survivors.wiki/w/Weapons)  
用途：清楚理解「武器系統規則」與可用於做 Survivors-like 的抽象模型（非逐一列出所有武器數值表）。

---

## 清楚摘要（你要先記住的 7 件事）

1. **武器是主要輸出／控場模組**：多數造成傷害；少數提供 debuff（對敵）或 buff（對己）。
2. **一般情況最多同時帶 6 種武器**：來自升級選項的「武器槽位」上限；但也能用「關卡生成（Stage items）」的方式拿到更多。
3. **多數武器是唯一的**：同一局通常不能重複取得同一種武器（即便被消耗也不能再拿一把一樣的）。
4. **武器是否會在升級畫面出現，受稀有度（Rarity/weight）影響**：它是「出現權重」而非單純品質標籤，且武器與被動會在同一池子裡競爭出現。
5. **升級（Upgrading）會讓已持有武器變強，滿等後不再出現在升級選項**。
6. **升級有額外戰術價值**：每次升級會「重置冷卻並立刻觸發一次」，長 CD 武器特別吃香。
7. **進化（Evolution）通常是「置換」而不是「加成」**：多數進化會移除基礎武器、換成進化武器，並讓進化武器冷卻從 0 開始。

---

## 1) 取得（Obtaining）與池子（Pool）

### 1.1 起始武器

- 每個角色通常有一把起始武器（維基提到 Antonio 進入 Mad Forest 的起始武器是 Whip）。
- 起始武器將強烈影響前期節奏與你偏好的 build 分支。

### 1.2 升級選項的來源

- 角色升級時，會從「可用武器＋可用被動」的池子抽出數個選項。
- 多數武器需要先解鎖，才會加入可抽池。
- **Rarity 決定 weight**：越高權重越常見，越低權重越罕見。

### 1.3 6 武器上限（但可突破）

- 玩家通常可從升級中選擇最多 **6 種不同武器**。
- 但可透過撿拾「隨關卡生成」的武器／道具來突破（如 Stage items 機制）。

---

## 2) 升級（Upgrading）：規則與手感細節

### 2.1 已持有武器會再出現，選了就升級

- 升級畫面有一定機率出現你**已擁有的武器**。
- 選取已擁有武器 → 該武器升到更高等級，並獲得該武器專屬的數值增益。

### 2.2 滿等後從升級選項移除

- 當武器達到 Max Level 後，不會再出現在升級選項中。
- 這是避免「抽到已滿等的無效選項」的關鍵 UI／體驗規則。

### 2.3 升級會重置冷卻並立刻觸發

- 每次升級會：
  - **Reset Cooldown**
  - **Instantly activate once（立刻發動一次）**
- 實戰含義：升級長 CD 武器等於立刻多打一輪爆發，能救場也能加速清怪。

---

## 3) 進化（Evolution）：條件、置換、冷卻

### 3.1 常見進化條件（典型規則）

- **基礎武器滿等**
- **取得對應被動道具**
- **拾取寶箱（Treasure Chest）觸發進化**

> 也存在變體：有些只需要武器滿等即可進化；也不是每把武器都有進化。

### 3.2 進化通常是「移除基礎武器 → 取得進化武器」

- 維基明確指出：進化多數是將基礎武器移除、換成進化武器（少數例外）。
- 因此：
  - 基礎武器的原有效果會消失（不是疊加）
  - 進化後武器是新的行為／數值集合

### 3.3 進化後冷卻從 0 開始（長 CD 武器很有感）

- 進化後武器的 cooldown 從零開始計時。
- 維基舉例指出像 Santa Water 這類武器會很明顯。

### 3.4 透過特殊道具獲得進化，也可能會拆掉基礎武器

- 維基提到：即便是透過「繞過正常條件」的方式（例如 Super Candybox II Turbo）獲得進化，仍會移除基礎武器。

### 3.5 角色特例：變身時自動進化起始武器

- 某些角色（維基列出 Mortaccio、Yatta Cavallo、Bianca Ramba、O'Sole Meeo）在滿足 relic 與條件後，變身時會自動進化起始武器。

---

## 4) 武器數值（Stats）速查：欄位意義與常見對應

武器有多種數值欄位，構成其運作方式。下面是維基列出的核心欄位（加上「實作／平衡的理解」與常見對應玩家屬性）。

| 欄位 | 意義 | 常見對應／影響 |
|---|---|---|
| Max Level | 可升級到的最高等級 | 決定「成長段落」長度 |
| Rarity | 在池子中的權重（weight） | 決定出現頻率 |
| Base Damage | 單次命中傷害基底 | 受 Might 影響 |
| Area | 判定／效果的範圍基底 | 受 Area 影響 |
| Speed | 投射物速度基底 | 受 Speed 影響 |
| Amount | 每次使用的投射物數量 | 受 Amount 影響 |
| Duration | 效果持續時間 | 受 Duration 影響 |
| Pierce | 單顆投射物可命中幾個敵人 | 影響貫穿與清怪效率 |
| Cooldown | 再次使用所需時間；可能在持續結束後才開始算 | 受 Cooldown 影響 |
| Projectile Interval | 同一輪發射內「追加子彈」的間隔，可與下一輪重疊 | 影響射擊密度與手感 |
| Hitbox Delay | 同一顆投射物對同一敵的再命中延遲（多個延遲同步刷新） | 控制多段命中與 DPS 上限 |
| Knockback | 擊退倍率 | 與敵人的擊退承受、移速相關 |
| Pool Limit | 場上可同時存在的投射物上限 | 壓制極端堆疊 |
| Chance | 觸發特殊效果的機率 | 受 Luck 影響（維基註：Crit 與 Chance 技術上可分） |
| Crit Multi | 暴擊傷害倍率 | 影響爆發上限 |
| Block by Walls | 是否會被牆／障礙擋住 | 與地圖設計強連動 |

### 4.1 「傷害飄字」與實際傷害（重要）

- 維基註記：開啟 Damage Numbers 時看到的傷害數字，會在縮放後基礎傷害附近做 **±5** 的視覺隨機，但**實際造成的傷害不會因此浮動**。
- 實作含義：建議把「戰鬥結算」與「顯示飄字」分離，避免除錯誤判。

---

## 5) 類型（Types）：你在系統設計時要分清的三層

維基將武器概略分為：

- **Base Weapons（基礎武器）**：通常由升級選擇取得，且多數最初鎖在成就解鎖後。
- **Evolutions / Unions（進化／合體）**：由基礎武器在滿足條件後置換／合成而來。
- **Special weapons（特殊武器）**：不一定遵循一般池子規則（例如一次性選武器的 Candybox 類型）。

延伸閱讀：
- [Weapons/Combos](https://vampire.survivors.wiki/w/Weapons/Combos)：整理「哪些被動、哪些玩家屬性」會影響哪些武器
- [Weapons/Overview Stats](https://vampire.survivors.wiki/w/Weapons/Overview_Stats)：各武器基礎數值總表

---

## 6) 用於實作 Survivors-like 的抽象模型（對照程式結構的建議）

把維基描述抽象成你專案裡好落地的幾個概念（不綁任何特定引擎）：

1. **WeaponDefinition（靜態資料）**
   - id、名稱、Rarity、MaxLevel
   - base stats（damage/area/speed/amount/duration/…）
   - evolution rules（需要的被動、是否需要寶箱、進化結果 id）

2. **WeaponInstance（動態狀態）**
   - currentLevel
   - cooldownTimer
   - 是否已進化（或當前武器 id 已切換）

3. **LevelUpPool（抽選池）**
   - 可用武器／被動清單（解鎖條件）
   - 權重（Rarity → weight）
   - 滿等移除規則
   - 已持有物出現的機率／權重規則

4. **OnUpgrade（手感規則）**
   - weapon.LevelUp()
   - weapon.ResetCooldown()
   - weapon.ActivateOnce()

5. **Evolve（置換規則）**
   - Remove(baseWeaponInstance)
   - Add(evolvedWeaponInstance)
   - evolvedWeaponInstance.cooldown = 0

---

## 7) 快速自我測驗（用來確認你真的懂）

1. 為什麼「Rarity」是系統設計核心，而不只是稀有裝備的標籤？
2. 你能說清楚「Upgrading」與「Evolution」最大的差別是「疊加」還是「置換」嗎？
3. 為什麼「升級會立刻觸發一次」會顯著影響長 CD 武器的平衡？
4. 什麼是 Projectile Interval？它和 Cooldown 有什麼不同？
5. 為什麼 Hitbox Delay 的存在能避免某些武器在大型敵人身上爆炸式疊 DPS？

