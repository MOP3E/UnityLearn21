using TowerDefense.Control;
using UnityEngine;

namespace TowerDefense
{
    /// <summary>
    /// Контроллер управления пешеходом.
    /// </summary>
    public abstract class Controller : MonoBehaviour
    {
        /// <summary>
        /// Ось движения по вертикали.
        /// </summary>
        public virtual ControlAxis VerticalAxis { get; set; }

        /// <summary>
        /// Ось движения по горизонтали.
        /// </summary>
        public virtual ControlAxis HorisontalAxis { get; set; }

        /// <summary>
        /// Кнопка стрельбы из основного оружия.
        /// </summary>
        public virtual ControlAxis PrimaryButton { get; set; }

        /// <summary>
        /// Кнопка стрельбы из вторичного оружия.
        /// </summary>
        public virtual ControlAxis SecondaryButton { get; set; }

        /// <summary>
        /// Угол поворота.
        /// </summary>
        public virtual Quaternion Rotation { get; set; }
    }
}
