using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace TowerDefense
{
    /// <summary>
    /// Параметры турели.
    /// </summary>
    [CreateAssetMenu]
    public sealed class EnemyProperties : ScriptableObject
    {
        [Header("Внешний вид")]
        [SerializeField] public Color color = Color.white;
        [SerializeField] public Vector2 spriteScale = new Vector2(3, 3);
        [SerializeField] public RuntimeAnimatorController animator;

        [Header("Игровые параметры")]
        [SerializeField] public float moveSpeed = 1;
        [SerializeField] public int hitPoints = 15;
        [SerializeField] public int score = 15;
        [SerializeField] public float colliderRadius = .2f;
    }
}
