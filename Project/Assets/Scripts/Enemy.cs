using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Enemy : MonoBehaviour
    {
        /// <summary>
        /// Применение к врагу настроек.
        /// </summary>
        /// <param name="properties"></param>
        public void Use(EnemyProperties properties)
        {
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();

            //применение цвета
            sr.color = properties.color;

            //применение размера
            sr.transform.localScale = new Vector3(properties.spriteScale.x, properties.spriteScale.y);

            //применение аниматора
            sr.GetComponent<Animator>().runtimeAnimatorController = properties.animator;

            //применение игровых свойств объекта
            GetComponent<Walker>().Use(properties);

            //применение размера коллайдера
            GetComponentInChildren<CircleCollider2D>().radius = properties.colliderRadius;
        }
    }
}
