using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Control;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TowerDefense
{
    public enum AiTowerBehaviour
    {
        /// <summary>
        /// Не стрелять.
        /// </summary>
        None,

        /// <summary>
        /// Стрелять всегда, когда есть возможность.
        /// </summary>
        AutoFire,

        /// <summary>
        /// Стрелять только если выбрана цель.
        /// </summary>
        TargetFire,
    }
}
