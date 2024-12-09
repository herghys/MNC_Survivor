using System;
using System.Collections;
using System.Collections.Generic;

using HerghysStudio.Survivor.Utility.Coroutines;
using HerghysStudio.Survivor.Utility.Singletons;

using UnityEngine;
using UnityEngine.Events;

namespace HerghysStudio.Survivor
{
    public class GameTimeManager : NonPersistentSingleton<GameTimeManager>
    {
        public int TotalGameMinutes = 3;
        public float elapsedMillisecond = 0f;
        public float lastMinuteMilliseondCheckTime = 0f;

        public GameManager GameManager;

        public UnityEvent OnOneMinutePassed;

        public override void DoOnAwake()
        {
            base.DoOnAwake();
            GameManager??= FindFirstObjectByType<GameManager>();
        }

        public void StartGameTimer()
        {
            elapsedMillisecond = 0;
            StartGameCountdown().Run();
        }

        private IEnumerator StartGameCountdown()
        {
            var countDownSecond = 3;
            while (countDownSecond > 0)
            {
                countDownSecond--;
                yield return new WaitForSeconds(1);
            }

            GameManager.OnStartCountdownEnded();
            var totalMillisecond = TimeSpan.FromMinutes(TotalGameMinutes).Milliseconds;

            TimerLoop(totalMillisecond).Run();
        }

        private IEnumerator TimerLoop(int totalMillisecond)
        {
            while (elapsedMillisecond < totalMillisecond)
            {
                if (GameManager.Instance.IsPaused)
                {
                    yield return null;
                    continue;
                }

                elapsedMillisecond += Time.deltaTime * 1000f;

                if (elapsedMillisecond - lastMinuteMilliseondCheckTime >= 6000f)
                {
                    OnOneMinutePassed?.Invoke(); // Call this method when a minute has passed
                    lastMinuteMilliseondCheckTime = elapsedMillisecond; // Update last minute check time
                }

                yield return null;
            }

            GameManager.OnTimerEnded();
        }
    }
}
