using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class LevelSequenceController : MonoSingleton<LevelSequenceController>
    {
        /// <summary>
        /// Имя сцены с главным меню.
        /// </summary>
        public static string MainMenuSceneName = "main_menu";

        /// <summary>
        /// Текущий эпизод.
        /// </summary>
        public Episode CurrentEpisode { get; private set; }

        /// <summary>
        /// Номер текущего уровня.
        /// </summary>
        public int CurrentLevel { get; private set; }

        /// <summary>
        /// Результат прохождения последнего уровня.
        /// </summary>
        public bool LastLevelResult { get; private set; }

        /// <summary>
        /// Статистика пройденного уровня.
        /// </summary>
        public PlayerStatistics LevelStatistics { get; private set; }

        /// <summary>
        /// Имя игрока.
        /// </summary>
        public string PlayerName { get; set; } = "Пилот";

        /// <summary>
        /// Статистика эпизода.
        /// </summary>
        public PlayerStatistics TotalStatistics { get; private set; }

        public List<PlayerStatistics> Records { get; private set; }

        /// <summary>
        /// Космический корабль игрока.
        /// </summary>
        public static Walker PlayerWalker { get; set; }

        protected override void Awake()
        {
            base.Awake();
            Records = new List<PlayerStatistics>();
        }

        public void EpisodeStart(Episode e)
        {
            CurrentEpisode = e;
            CurrentLevel = 0;

            //обнулить игровую статистику перед началом эпизода
            LevelStatistics = new PlayerStatistics();
            TotalStatistics = new PlayerStatistics { PlayerName = this.PlayerName };

            SceneManager.LoadScene(e.Levles[CurrentLevel]);
        }

        public void LevelRestart()
        {
            SceneManager.LoadScene(CurrentEpisode.Levles[CurrentLevel]);
        }

        public void FinishCurrentLevel(bool success)
        {
            LastLevelResult = success;
            CalculateStatistics(success);
            if (success)
            {
                TotalStatistics.Score += LevelStatistics.Score + LevelStatistics.TimeBounus;
                TotalStatistics.Time += LevelStatistics.Time;
                TotalStatistics.Kills += LevelStatistics.Kills;
            }
            if(CurrentLevel + 1 < CurrentEpisode.Levles.Length)
                ResultPanelController.Instance.ShowLevelStatistics(LevelStatistics, success);
            else
            {
                if(success)
                    ResultPanelController.Instance.ShowEpisodeStatistics(TotalStatistics, RecordTest());
                else
                    ResultPanelController.Instance.ShowLevelStatistics(LevelStatistics, success);
            }
        }

        public void LevelAdvance()
        {
            LevelStatistics.Reset();
            CurrentLevel++;
            if(CurrentLevel < CurrentEpisode.Levles.Length)
                SceneManager.LoadScene(CurrentEpisode.Levles[CurrentLevel]);
            else
            {
                if (RecordTest())
                {
                    while (Records.Count > 2)
                    {
                        Records.RemoveAt(Records.Count - 1);
                    }
                    Records.Add(TotalStatistics);
                    Records.Sort();
                }
                SceneManager.LoadScene(MainMenuSceneName);
            }
        }

        /// <summary>
        /// Проверить, не является ли результат прохождения эпизода рекордом.
        /// </summary>
        private bool RecordTest()
        {
            //если рекордов меньше трёх, рекорд есть всегда
            if (Records.Count < 3) return true;

            //проверить, не является ли результат прохождения эпизода рекордом
            bool isRecord = false;
            foreach (PlayerStatistics statistics in Records)
            {
                if (TotalStatistics.CompareTo(statistics) > 0)
                {
                    isRecord = true;
                    break;
                }
            }

            return isRecord;
        }

        /// <summary>
        /// Подсчёт статитстики.
        /// </summary>
        private void CalculateStatistics(bool success)
        {
            if (LevelController.Instance.IsMaze)
            {
                //подсчёт очков для лабиринта.
                //очки игрока
                LevelStatistics.Score = 0;
                //бонус времени за прохождение уровня - на каждый 1% сэкономленного времени 10% призовых очков, только если уровень успешно пройден
                LevelStatistics.TimeBounus = success && LevelController.Instance.LevelTime > 0
                    ? (int)(LevelController.Instance.LevelTime / LevelController.Instance.ReferenceTime * 1000)
                    : 0;
                //убийства игрока
                LevelStatistics.Kills = 0;
                //время прохождения уровня
                LevelStatistics.Time = LevelController.Instance.LevelTime;
                return;
            }

            //стандартный подсчёт очков
            //очки игрока
            LevelStatistics.Score = Player.Instance.Score;
            //бонус времени за прохождение уровня - на каждый 1% сэкономленного времени 10% призовых очков, только если уровень успешно пройден
            LevelStatistics.TimeBounus = success && LevelController.Instance.LevelTime > 0
                ? (int)(LevelController.Instance.LevelTime / LevelController.Instance.ReferenceTime * (float)LevelStatistics.Score * 10)
                : 0;
            //убийства игрока
            LevelStatistics.Kills = Player.Instance.Kills;
            //время прохождения уровня
            LevelStatistics.Time = LevelController.Instance.LevelTime;
        }
    }
}
