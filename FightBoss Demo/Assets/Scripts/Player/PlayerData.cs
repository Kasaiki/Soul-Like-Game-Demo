using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シングルトンを用いた、データ処理に使うクラス
public class PlayerData : ActorAttribute
{
    private void Start() {
        MaxHP = 500;
        MaxSTA = 200;

        InitUI( );
    }

    // Update is called once per frame
    private void Update()
    {
        BarUpdate( );
    }

    /// <summary>
    /// 被ダメージされるときのHP処理を行うインターフェースファンクション
    /// </summary>
    /// <param name="value"></param>
    public override void DoDamage(float damage) {
        print( "player is hit" );
        HP = Mathf.Clamp( HP - damage, 0, MaxHP );
        BarUpdate( );
    }
}
