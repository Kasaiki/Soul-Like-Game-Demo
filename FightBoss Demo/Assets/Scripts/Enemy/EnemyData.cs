using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UIの更新や、HPの計算を行うクラス
/// </summary>
public class EnemyData : ActorAttribute
{
    private void Start() {
        MaxHP = 5000;
        MaxSTA = 300;
        
        InitUI( );
    }

    private void Update() {
        BarUpdate( );
    }


}
