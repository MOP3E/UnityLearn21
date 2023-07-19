using UnityEngine;

namespace TowerDefense.Control
{
    /// <summary>
    /// Ось, объединяющая сигналы от нескольких осей.
    /// </summary>
    public class CompoziteAxis : ControlAxis
    {
        /// <summary>
        /// Массив осей, сигналы от которых нужно объединить. Чем позднее стоит ось в массиве, тем приоритетнее её ненулевой сигнал.
        /// </summary>
        [SerializeField] private ControlAxis[] _axis;

        /// <summary>
        /// Update запускается каждый кадр.
        /// </summary>
        private void Update()
        {
            if(_axis == null) return;
            float result = 0;
            foreach (ControlAxis axis in _axis)
            {
                if (axis.Value != 0)
                {
                    result = axis.Value;
                    break;
                }
            }

            _value = result;
        }
    }
}
