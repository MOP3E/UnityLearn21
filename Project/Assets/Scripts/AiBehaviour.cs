using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Control;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TowerDefense
{
    public enum AiBehaviour
    {
        /// <summary>
        /// Ничего не делать.
        /// </summary>
        None,

        /*
        /// <summary>
        /// Патрулировать в пределах заданной зоны.
        /// </summary>
        ZonePatrol,
        */

        /// <summary>
        /// Патрулировать по заданному маршруту, вперёд по списку точек.
        /// </summary>
        RoutePatrolOnward,

        /// <summary>
        /// Патрулировать по заданному маршруту, назад по списку точек.
        /// </summary>
        RoutePatrolBackward,

        /// <summary>
        /// Патрулировать по заданному маршруту, вперёд по списку точек, не отвлекаться на цели и не стрелять.
        /// </summary>
        DummyRoutePatrolOnward,
    }
}
