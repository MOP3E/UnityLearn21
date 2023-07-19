namespace TowerDefense.Control
{
    /// <summary>
    /// Ось, которой может управлять ИИ.
    /// </summary>
    public class AiAxis : ControlAxis
    {
        /// <summary>
        /// Нормализованное значение оси, которое может изменять ИИ.
        /// </summary>
        public float AiValue
        {
            get => _value;
            set => _value = value;
        }
    }
}
