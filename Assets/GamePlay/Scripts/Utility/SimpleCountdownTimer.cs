using UnityEngine;

namespace GamePlay.Scripts.Utility
{
    /// <summary>
    /// 簡單倒數計時：<see cref="Start"/> 設定秒數後，每幀呼叫 <see cref="Tick"/> 扣時間，<see cref="Remaining"/> 歸零表示結束。
    /// </summary>
    public struct SimpleCountdownTimer
    {
        float duration;
        float remaining;

        public float Duration => duration;
        public float Remaining => remaining;
        public bool IsRunning => remaining > 0f;

        /// <summary>已過時間（0 ～ Duration）。</summary>
        public float Elapsed => Mathf.Clamp(duration - remaining, 0f, duration);

        /// <summary>進度 0（剛開始）～ 1（結束）。</summary>
        public float NormalizedProgress => duration > 0f ? Elapsed / duration : 1f;

        public void Start(float seconds)
        {
            duration = Mathf.Max(0f, seconds);
            remaining = duration;
        }

        public void Restart()
        {
            remaining = duration;
        }

        /// <summary>立即停止（剩餘時間歸零）。</summary>
        public void Stop()
        {
            remaining = 0f;
        }

        /// <summary>扣除時間；回傳此幀是否剛好從「進行中」變成結束。</summary>
        public bool Tick(float deltaTime)
        {
            if (remaining <= 0f)
            {
                return false;
            }

            var wasRunning = true;
            remaining = Mathf.Max(0f, remaining - deltaTime);
            return wasRunning && remaining <= 0f;
        }
    }
}
