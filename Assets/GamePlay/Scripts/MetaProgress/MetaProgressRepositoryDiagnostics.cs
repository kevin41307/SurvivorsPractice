using System;
using UnityEngine;

namespace GamePlay.Scripts.MetaProgress
{
    /// <summary>
    /// 基本存讀檔驗證：確保 round-trip 不會破壞資料。
    /// （不依賴 Unity Test Runner，先用 Editor menu 觸發。）
    /// </summary>
    public static class MetaProgressRepositoryDiagnostics
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/MetaProgress/Run Repository RoundTrip Check")]
        public static void RunRoundTripCheck()
        {
            try
            {
                var repo = new MetaProgressFileRepository();
                const int slotId = 999;

                var p = MetaProgressData.Create();
                p.SetGold(123);
                p.TrySetPowerUpLevel("DebugPowerUp", 2);

                repo.Save(slotId, p);
                var loaded = repo.LoadOrCreate(slotId);

                var ok =
                    loaded.Gold == 123 &&
                    loaded.GetPowerUpLevel("DebugPowerUp") == 2;

                Debug.Log(ok
                    ? "[MetaProgress] RoundTripCheck OK"
                    : "[MetaProgress] RoundTripCheck FAILED (data mismatch)");
            }
            catch (Exception e)
            {
                Debug.LogError($"[MetaProgress] RoundTripCheck exception: {e}");
            }
        }
#endif
    }
}

