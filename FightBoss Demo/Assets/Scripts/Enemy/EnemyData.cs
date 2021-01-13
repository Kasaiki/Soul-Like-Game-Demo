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

    /// <summary>
    /// 敵のダメージ処理
    /// </summary>
    /// <param name="damage"></param>
    public override void DoDamage(float damage) {
        print( "Enemy is hit" );
        HP = Mathf.Clamp( HP - damage, 0, MaxHP );
        STA = Mathf.Clamp( STA - damage, 0, MaxSTA );
        BarUpdate( );
    }
}
