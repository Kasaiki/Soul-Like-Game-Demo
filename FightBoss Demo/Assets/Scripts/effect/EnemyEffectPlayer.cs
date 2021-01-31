using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectPlayer : MonoBehaviour
{
    void PlayEffect(string effectName) {
        print( "PlayEffect:" + effectName );
        PoolManager.GetObject( effectName, transform );
    }

    void PlaySkillEffect(string effectName) {
        PoolManager.GetObject( effectName, transform.position + transform.up * 1.3f, transform.rotation );
    }
}

