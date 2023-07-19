using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TowerDefense
{
    /// <summary>
    /// Круговая зона.
    /// </summary>
    public class CircleArea : MonoBehaviour
    {
        /// <summary>
        /// Радиус круговой зоны.
        /// </summary>
        [SerializeField] private float _radius;

        /// <summary>
        /// Получить случайную позицию внутри зоны.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetRandomInsideArea()
        {
            return transform.position + UnityEngine.Random.insideUnitSphere * _radius;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Цвет зоны в редакторе.
        /// </summary>
        private Color _gizmoColor = new Color(0, 1, 0, .3f);

        /// <summary>
        /// Отобразить зону в редкторе.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Handles.color = _gizmoColor;
            Handles.DrawSolidDisc(transform.position, transform.forward, _radius);
        }
#endif

    }
}
