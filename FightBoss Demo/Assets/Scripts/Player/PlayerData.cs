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
    /// HP処理を行うファンクション
    /// </summary>
    /// <param name="value"></param>
    public void PlayerIsHitted() {
        print( "player is hit" );
        HP = Mathf.Clamp( HP - 70, 0, MaxHP );
        BarUpdate( );
    }
}
