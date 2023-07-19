using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu]
    public class Episode : ScriptableObject
    {
        /// <summary>
        /// Наименование эпизода.
        /// </summary>
        [SerializeField] private string _name;

        /// <summary>
        /// Наименование эпизода.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Наименования уровней.
        /// </summary>
        [SerializeField] private string[] _lelvels;

        /// <summary>
        /// Наименования уровней.
        /// </summary>
        public string[] Levles => _lelvels;

        /// <summary>
        /// Спрайт предпосмотра эпизода.
        /// </summary>
        [SerializeField] private Sprite _previewImage;

        /// <summary>
        /// Спрайт предпосмотра эпизода.
        /// </summary>
        public Sprite PreviewImage => _previewImage;
    }
}
