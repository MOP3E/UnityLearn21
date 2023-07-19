using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    /// <summary>
    /// Синглетон для MonoBehaviour
    /// </summary>
    /// <typeparam name="T">Тип объекта, наследуемого от MonoBehaviour.</typeparam>
    [DisallowMultipleComponent]
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// Сделать объект неразрушаемым при перезагрузке сцены.
        /// </summary>
        [Header("Singleton")]
        [SerializeField] private bool _doNotDestroyOnLoad;

        /// <summary>
        /// Экземпляр синглетона. Если флаг _doNotDestroyOnLoad не установлен может быть null.
        /// </summary>
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            //проверить, не существует ли уже экземпляр синглетона
            if (Instance != null)
            {
                Debug.LogWarning($"MonoSingleton: объект {typeof(T).Name} уже существует, новый экземпляр объекта будет уничтожен.");
                Destroy(this);
                return;
            }
            //создать экземпляр сигнлетона
            Instance = this as T;
            //запретить уничтожение при перезагрузке сцены
            if (_doNotDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
    }
}
