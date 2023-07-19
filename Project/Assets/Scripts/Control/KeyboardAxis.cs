using UnityEngine;

namespace TowerDefense.Control
{
    /// <summary>
    /// Ось, управляемая с клавиатуры.
    /// </summary>
    public class KeyboardAxis : ControlAxis
    {
        /// <summary>
        /// Кнопка для "Полный вперёд".
        /// </summary>
        [SerializeField] private KeyCode _onwardKey;
        
        /// <summary>
        /// Кнопка для "Полный назад".
        /// </summary>
        [SerializeField] private KeyCode _backwardKey;

        /// <summary>
        /// Update запускается каждый кадр.
        /// </summary>
        private void Update()
        {
            //переключение оси в зависимости от нажатия на кнопки
            if (Input.GetKey(_onwardKey)) _value = 1f;
            else if (Input.GetKey(_backwardKey)) _value = -1f;
            else _value = 0f;
        }
    }
}
