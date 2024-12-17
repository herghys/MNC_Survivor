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
        public GameUIManager UIManager;

        public UnityEvent OnOneMinutePassed;
        public UnityEvent OnCountDownDone;

        bool _isPlayerDead;

        public override void DoOnAwake()
        {
            base.DoOnAwake();
            GameManager ??= FindFirstObjectByType<GameManager>();
        }

        private void OnEnable()
        {
            GameManager.OnGameEnded += OnGameEnded;
        }



        private void OnDisable()
        {
            GameManager.OnGameEnded -= OnGameEnded;

        }

        private void OnGameEnded(EndGameState arg)
        {
            StopAllCoroutines();

            if (arg == EndGameState.Lose)
                _isPlayerDead = true;

            else
                _isPlayerDead= false;
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

                GameUIManager.Instance.UpdateTimer(elapsedMillisecond / totalMilliSecond, FormatElapsedTime(elapsedMillisecond));
                yield return null;
            }

            GameManager.OnTimerEnded?.Invoke();
        }

        public string FormatElapsedTime(float elapsedMilliseconds)
        {
            // Calculate minutes, seconds, and milliseconds
            int minutes = Mathf.FloorToInt(elapsedMilliseconds / 60000); // 60000 milliseconds in 1 minute
            int seconds = Mathf.FloorToInt((elapsedMilliseconds / 1000) % 60); // Get seconds from milliseconds
            int milliseconds = Mathf.FloorToInt(elapsedMilliseconds % 1000); // Get remaining milliseconds

            // Return the formatted string as MM:SS.MMM
            return string.Format("{0:D2}:{1:D2}.{2:D3}", minutes, seconds, milliseconds);
        }
    }
}
