using UnityEngine;

namespace TowerDefense
{
    /// <summary>
    /// Базовый класс всех интерактивных игровых объектов на сцене.
    /// </summary>
    public abstract class Entity : MonoBehaviour
    {
        /// <summary>
        /// Название объекта пользователя.
        /// </summary>
        [SerializeField] public string nickname;

        ///// <summary>
        ///// Название объекта пользователя.
        ///// </summary>
        //public string Nickname => _nickname;
        
        public override string ToString() => nickname;
    }
}
