using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シングルトンを用いた、データ処理に使うクラス
public class PlayerData : ActorAttribute
{
    PlayerController pc;
    public bool isDead = false;

    bool isRecoverSTA = true;
    Coroutine StaminaRecoverCooldownTimer;

    IEnumerator StaminaRecoverWait() {
        isRecoverSTA = false;
        yield return new WaitForSeconds(1f);
        isRecoverSTA = true;
    }

    private void Start() {
        MaxHP = 500;
        MaxSTA = 200;


        pc = GetComponentInChildren<PlayerController>( );
        InitUI( );
    }

    private void Update() {
        StaminaUpdate( );
        BarUpdate( );
        print( pc.m_Animator );
    }

    public override void DoDamage(float damage , Vector3 hitPoint) {
        if (isDead)
            return;

        if ( !pc.CheckInvincible( )) {
            PoolManager.GetObject( "skillAttack", hitPoint );
            HP = Mathf.Clamp( HP - damage, 0, MaxHP );
            if (HP <= 0) {
                isDead = true;
                pc.SetDead( );
                return;
            }
            pc.SetHit( hitPoint );
        }
    }

    public void EnterDash() {
        STA = STA - 40;
        if (StaminaRecoverCooldownTimer != null)
            StopCoroutine( StaminaRecoverCooldownTimer );
        StaminaRecoverCooldownTimer = StartCoroutine( StaminaRecoverWait( ) );
    }

    public void EnterATK() {
        STA = STA - 20;
        if (StaminaRecoverCooldownTimer != null)
            StopCoroutine( StaminaRecoverCooldownTimer );
        StaminaRecoverCooldownTimer = StartCoroutine( StaminaRecoverWait( ) );
    }

    private void StaminaUpdate() {
        if (STA < MaxSTA && isRecoverSTA) {
            STA++;
        }
    }
}
