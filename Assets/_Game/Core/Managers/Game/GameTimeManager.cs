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
        public float lastMinuteMillisecondCheckTime = 0f;
        public float totalMilliSecond;

        public GameManager GameManager;

        public UnityEvent OnOneMinutePassed;
        public UnityEvent OnCountDownDone;

        bool _isPlayerDead;

        public override void DoOnAwake()
        {
            base.DoOnAwake();
            GameManager??= FindFirstObjectByType<GameManager>();
        }

        private void OnEnable()
        {
            GameManager.OnGameEnded += OnGameEnded;
        }

      

        private void OnDisable()
        {
            GameManager.OnGameEnded -= OnGameEnded;

        }

        private void OnGameEnded(bool lose)
        {
            StopAllCoroutines();
            _isPlayerDead = lose;
        }

        public void StartGameTimer()
        {
            totalMilliSecond = (float)TimeSpan.FromMinutes(TotalGameMinutes).TotalMilliseconds;
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

        }

        public void StartLoop()
        {
            TimerLoop().Run();

        }

        private IEnumerator TimerLoop()
        {
            while (elapsedMillisecond < totalMilliSecond)
            {
                if (GameManager.IsPlayerDead)
                    break;

                if (GameManager.IsPaused)
                {
                    yield return null;
                    continue;
                }

                elapsedMillisecond += Time.deltaTime * 1000f;

                if (elapsedMillisecond - lastMinuteMillisecondCheckTime >= 6000f)
                {
                    OnOneMinutePassed?.Invoke(); // Call this method when a minute has passed
                    lastMinuteMillisecondCheckTime = elapsedMillisecond; // Update last minute check time
                }

                GameUIManager.Instance.UpdateTimer(elapsedMillisecond / totalMilliSecond, "Time");
                yield return null;
            }

            GameManager.OnTimerEnded?.Invoke();
        }
    }
}
