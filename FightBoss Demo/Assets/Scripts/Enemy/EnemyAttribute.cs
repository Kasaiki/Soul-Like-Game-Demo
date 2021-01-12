using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttribut : MonoBehaviour
{
    protected int currtHP, maxHP;
    protected int currtSTA, maxSTA;
    protected int ATK;

    // states
    protected EnemyState enemyState;
    protected Dist distInfo;

    protected enum EnemyState
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
