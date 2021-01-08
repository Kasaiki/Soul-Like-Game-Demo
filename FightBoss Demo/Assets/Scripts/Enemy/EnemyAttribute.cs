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
    protected ShortDistAction shortDistAction;
    protected MediumDistAction mediumDistAction;

    protected enum EnemyState
    {
        Dead,
        Broken,
        Hit,
        Attack,
        Move,
        Idle,
    }

    protected enum ShortDistAction
    {
        TripleCombo,
        FloatingDoubleStrike,
        HeaveDoubleStrike,
    }

    protected enum MediumDistAction
    {
        TripleCombo,
        FloatingDoubleStrike,
        HeaveDoubleStrike,
    }
}
