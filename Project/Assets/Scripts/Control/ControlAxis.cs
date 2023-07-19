namespace TowerDefense.Control
{
    /// <summary>
    /// Ось упраления.
    /// </summary>
    public abstract class ControlAxis : Entity
    {
        /// <summary>
        /// Нормализованное значение оси.
        /// </summary>
        protected float _value;

        /// <summary>
        /// Нормализованное значение оси.
        /// </summary>
        public float Value => _value;

        protected ControlAxis()
        {
            _value = 0;
        }

        // Автоматическое привдение типа при чтении значения оси.
        public static implicit operator float(ControlAxis a) => a._value;
    }
}
