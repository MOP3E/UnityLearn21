using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TowerDefense
{
    /// <summary>
    /// Типы турелей.
    /// </summary>
    public enum TurretType
    {
        Primary,
        Secondary
    }

    /// <summary>
    /// Параметры турели.
    /// </summary>
    [CreateAssetMenu]
    public sealed class TurretProperties : ScriptableObject
    {
        /// <summary>
        /// Тип турели.
        /// </summary>
        [SerializeField] private TurretType _type;

        /// <summary>
        /// Тип турели.
        /// </summary>
        public TurretType Type => _type;

        /// <summary>
        /// Шаблон снаряда.
        /// </summary>
        [SerializeField] private Projectile _projectilePrefab;

        /// <summary>
        /// Шаблон снаряда.
        /// </summary>
        public Projectile ProjectilePrefab => _projectilePrefab;

        /// <summary>
        /// Скорострельность турели, выстр./мин.
        /// </summary>
        [SerializeField] private float _fireRate;

        /// <summary>
        /// Скорострельность турели, выстр./мин.
        /// </summary>
        public float FireRate => _fireRate;

        /// <summary>
        /// Потребление энергии на выстрел турели.
        /// </summary>
        [SerializeField] private int _energyUsage;

        /// <summary>
        /// Потребление энергии на выстрел турели.
        /// </summary>
        public int EnergyUsage => _energyUsage;

        /// <summary>
        /// Потребление патронов на выстрел турели.
        /// </summary>
        [SerializeField] private int _ammoUsage;

        /// <summary>
        /// Потребление патронов на выстрел турели.
        /// </summary>
        public int AmmoUsage => _ammoUsage;

        /// <summary>
        /// Звуковой эффект выстрела из турели.
        /// </summary>
        [SerializeField] private AudioClip _fireSfx;

        /// <summary>
        /// Звуковой эффект выстрела из турели.
        /// </summary>
        public AudioClip FireSfx => _fireSfx;
    }
}
