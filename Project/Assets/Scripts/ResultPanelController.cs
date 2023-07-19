using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class ResultPanelController : MonoSingleton<ResultPanelController>
    {
        /// <summary>
        /// Поле заголовка окна результатов.
        /// </summary>
        [SerializeField] private Text _caption;

        /// <summary>
        /// Поле времени окна результатов.
        /// </summary>
        [SerializeField] private Text _time;

        /// <summary>
        /// Поле числа убийств окна результатов.
        /// </summary>
        [SerializeField] private Text _numKills;

        /// <summary>
        /// Поле очков окна результатов.
        /// </summary>
        [SerializeField] private Text _score;

        /// <summary>
        /// Поле добавки очков за время окна результатов.
        /// </summary>
        [SerializeField] private Text _timeBonus;

        /// <summary>
        /// Поле полных очков окна результатов.
        /// </summary>
        [SerializeField] private Text _totalScore;

        /// <summary>
        /// Текст на кнопке окна результатов.
        /// </summary>
        [SerializeField] private Text _buttonText;

        /// <summary>
        /// Объект с сообщением о временном бонусе.
        /// </summary>
        [SerializeField] private GameObject _bonus;

        /// <summary>
        /// Объект с сообщением о рекорде.
        /// </summary>
        [SerializeField] private GameObject _record;

        /// <summary>
        /// Уровень пройден успешно.
        /// </summary>
        private bool _success;

        /// <summary>
        /// Start запускается перед первым кадром.
        /// </summary>
        private void Start()
        {
            //отключить окно статистики
            gameObject.SetActive(false);
        }

        public void ShowLevelStatistics(PlayerStatistics statistics, bool success)
        {

            _success = success;
            
            //остановить время
            Time.timeScale = 0;
            
            //настройка заголовка окна и надписи на кнопке
            _caption.text = success ? "Уровень пройден" : "Поражение";
            _buttonText.text = success ? "Дальше" : "Заново";

            //вывод статистики
            int time = (int)statistics.Time;
            int seconds = time % 60;
            int minutes = (time / 60) % 60;
            int hours = time / 3600;
            _time.text = $"{hours:00}:{minutes:00}:{seconds:00}";
            _numKills.text = statistics.Kills.ToString();
            _score.text = statistics.Score.ToString();
            _timeBonus.text = statistics.TimeBounus.ToString();
            _totalScore.text = (statistics.Score + statistics.TimeBounus).ToString();

            //отобразить окно статистики
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Показать статистику эпизода.
        /// </summary>
        public void ShowEpisodeStatistics(PlayerStatistics statistics, bool record)
        {
            //статистика эпизода показывается только после успешного прохождения последнего уровня эпизода
            _success = true;

            //остановить время
            Time.timeScale = 0;

            //настройка заголовка окна и надписи на кнопке
            _caption.text = "Эпизод пройден";
            _buttonText.text = "В меню";

            //вывод статистики
            int time = (int)statistics.Time;
            int seconds = time % 60;
            int minutes = (time / 60) % 60;
            int hours = time / 3600;
            _time.text = $"{hours:00}:{minutes:00}:{seconds:00}";
            _numKills.text = statistics.Kills.ToString();
            _score.text = statistics.Score.ToString();
            _bonus.SetActive(false);
            _record.SetActive(record);

            //отобразить окно статистики
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Нажатие на кнопку окна результатов.
        /// </summary>
        public void OnButonPressed()
        {
            //отключить окно статистики
            gameObject.SetActive(false);
            //запустить время
            Time.timeScale = 1;
            if (_success)
            {
                //если успешно - перейти на следующий уровень
                LevelSequenceController.Instance.LevelAdvance();
            }
            else
            {
                LevelSequenceController.Instance.LevelRestart();
            }
        }
    }
}
