using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シングルトンを用いた、データ処理に使うクラス
public class PlayerData : ActorAttribute
{
    PlayerController pc;
    public bool isDead = false;

    private void Start() {
        MaxHP = 500;
        MaxSTA = 200;
        pc = GetComponentInChildren<PlayerController>( );
        InitUI( );
    }


    public override void DoDamage(float damage , Vector3 hitPosition) {
        if (isDead)
            return;

        HP = Mathf.Clamp( HP - damage, 0, MaxHP );
        if (HP <= 0) {
            BarUpdate( );
            isDead = true;
            pc.SetDead( );
            return;
        }

        pc.SetHit( hitPosition );
        BarUpdate( );
    }
}
