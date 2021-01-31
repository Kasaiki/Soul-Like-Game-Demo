using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyAttribute : MonoBehaviour
{
    // states
    [SerializeField]
    protected State state;
    protected Dist distInfo;

    protected enum State
    {
        Dead,
        Hit,
        Action,
        Idle,
    }

    protected enum Dist
    {
        ShortDist,
        MediumDist,
        LongDist,
    }
}
