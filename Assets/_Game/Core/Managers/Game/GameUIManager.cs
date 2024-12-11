using System;
using System.Collections;
using System.Collections.Generic;

using HerghysStudio.Survivor.Utility.Singletons;

using UnityEngine;

namespace HerghysStudio.Survivor
{
    public class GameUIManager : NonPersistentSingleton<GameUIManager>
    {
        [Header("Counters")]
        [SerializeField] GameCounter coinCounter;
        [SerializeField] GameCounter enemyKilledCounter;
        [SerializeField] GameCounter enemyActiveCounter;

        [Header("Time Slider")]
        [SerializeField] GameTimerUI timerUI;

        [Header("Panels")]
        [SerializeField] GameObject winPanel;
        [SerializeField] GameObject losePanel;
        [SerializeField] GameObject pausePanel;

        /// <summary>
        /// Update Coin
        /// </summary>
        /// <param name="coin"></param>
        public void UpdateCoin(long coin)
            => coinCounter.UpdateText(coin);

        /// <summary>
        /// Update Enemy Killed Count
        /// </summary>
        /// <param name="count"></param>
        public void UpdateEnemyKilled(long count)
            => enemyKilledCounter.UpdateText(count);

        /// <summary>
        /// Update Active Enemy Count
        /// </summary>
        /// <param name="count"></param>
        public void UpdateEnemyActiveCounter(long count)
            => enemyActiveCounter.UpdateText(count);

        /// <summary>
        /// Update Game Timer UI
        /// </summary>
        /// <param name="fillValue"></param>
        /// <param name="text"></param>
        public void UpdateTimer (float fillValue, string text) 
            => timerUI.UpdateTime(fillValue, text);

        internal void PauseGame(bool isPaused)
        {
            pausePanel.SetActive(isPaused);
        }

        internal void LoseGame()
        {
            losePanel.SetActive(true);
        }

        internal void WinGame()
        {
            winPanel.SetActive(true);
        }
    }
}
