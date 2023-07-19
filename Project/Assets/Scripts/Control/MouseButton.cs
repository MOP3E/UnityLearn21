using UnityEngine;

namespace TowerDefense.Control
{
    public enum MouseButtons
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

    /// <summary>
    /// Ось, управляемая одной кнопкой с клавиатуры.
    /// </summary>
    public class MouseButton : ControlAxis
    {
        /// <summary>
        /// Кнопка для "Полный вперёд".
        /// </summary>
        [SerializeField] private MouseButtons _button;

        /// <summary>
        /// Update запускается каждый кадр.
        /// </summary>
        private void Update()
        {
            //1 - кнопка нажата; 0 - кнопка не нажата
            _value = Input.GetMouseButton((int)_button) ? 1f : 0f;
        }
    }
}
