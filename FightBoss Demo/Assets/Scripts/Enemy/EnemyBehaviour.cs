using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : EnemyAttribut
{
    // components
    Animator anim;
    Transform playerTrans;
    Transform targetDirector;

    // parameters
    private float distanceFromPlayer;
    private float rotationSpeed = 240f;

    // アニメーターインフォを用いて、アニメーションの状態を把握できます
    AnimatorStateInfo m_CurrentStateInfo;
    AnimatorStateInfo m_LastStateInfo;
    int m_ActionNum;
    /// <summary>
    /// 0は短距離、1は中距離、2は長距離
    /// </summary>
    int m_DistNum;
    /// <summary>
    /// プレイヤー向きまでの角度
    /// </summary>
    float m_AngleDiff;
    bool m_IsInSight;

    WaitForSeconds m_ActionTime;
    Coroutine m_ActionCorotine;

    readonly int m_HashActionNum = Animator.StringToHash( "ActionNum" );
    readonly int m_HashDistNum = Animator.StringToHash( "DistNum" );
    readonly int m_HashAngleDiff = Animator.StringToHash( "AngleDiff" );
    readonly int m_HashIsInSight = Animator.StringToHash( "IsInSight" );
    readonly int m_HashBroken = Animator.StringToHash( "Broken" );
    readonly int m_HashAttack = Animator.StringToHash( "Attack" );
    readonly int m_HashDead = Animator.StringToHash( "Dead" );
    readonly int m_HashHit = Animator.StringToHash( "Hit" );
    readonly int m_HashStateTime = Animator.StringToHash( "StateTime" );

    void Start()
    {
        anim = GetComponent<Animator>( );
        playerTrans = GameObject.FindGameObjectWithTag( "PlayerDirector" ).transform;
        targetDirector = GameObject.FindGameObjectWithTag( "EnemyDirector" ).transform;

        currtHP = 100;
        currtSTA = 100;
        enemyState = EnemyState.Action;
    }

    /// <summary>
    /// 各フレームでアニメーターインフォを更新します
    /// </summary>
    void CacheAnimatorStatus() {
        m_CurrentStateInfo = anim.GetCurrentAnimatorStateInfo( 0 );
        anim.SetFloat( m_HashStateTime, Mathf.Repeat( m_CurrentStateInfo.normalizedTime, 1f ) );
    }

    /// <summary>
    /// デッド状態・ヒット状態に遷移するかをチェックします。
    /// </summary>
    void CheckState() {
        if (currtHP <= 0) {
            currtHP = 0;
            enemyState = EnemyState.Dead;
            return;
        }else if(currtSTA <= 0) {
            currtSTA = 0;
            enemyState = EnemyState.Hit;
            return;
        }
    }


    void CheckDistance() {
        distanceFromPlayer = Vector3.Distance( transform.position, playerTrans.position );
        print( distanceFromPlayer );
        if( distanceFromPlayer > 10f) {  //　長距離
            m_DistNum = 2;
        }else if(distanceFromPlayer > 5.5f) {　　//　中距離
            m_DistNum = 1;
        } else {  // 　短距離
            m_DistNum = 0;
        }
        distInfo = (Dist)m_ActionNum;
        anim.SetInteger( m_HashDistNum, m_DistNum );
    }
    
    void CalculateAngle() {
        float angleCurrent = Mathf.Atan2( transform.forward.x, transform.right.x ) * Mathf.Rad2Deg;
        float targetAngle = Mathf.Atan2( targetDirector.forward.x, targetDirector.right.x ) * Mathf.Rad2Deg;

        m_AngleDiff = Mathf.DeltaAngle( angleCurrent, targetAngle );
        anim.SetFloat( m_HashAngleDiff, m_AngleDiff );
        m_IsInSight = (Mathf.Abs( m_AngleDiff ) <= 40f) ? true : false;
    }

    void EnemyTurning() {
        targetDirector.forward = playerTrans.position - transform.position;
        transform.rotation = Quaternion.RotateTowards( transform.rotation, targetDirector.rotation , rotationSpeed * Time.deltaTime );
    }

    void ChooseAction() {
        if (m_CurrentStateInfo.IsTag( "Attack" ))
            return;
        if ( m_IsInSight ) {
            switch (distInfo) {
                case Dist.ShortDist:
                    m_ActionNum = Random.Range( 0, 4 );
                    break;
                case Dist.MediumDist:
                    m_ActionNum = Random.Range( 0, 3 );
                    break;
                case Dist.LongDist:
                    m_ActionNum = 0;
                    break;
            }
        } else {
            switch (distInfo) {
                case Dist.ShortDist:
                    m_ActionNum = Random.Range( 0, 4 );
                    break;
                case Dist.MediumDist:
                    m_ActionNum = Random.Range( 0, 2 );
                    break;
                case Dist.LongDist:
                    m_ActionNum = 0;
                    break;
            }
        }
        anim.SetInteger( m_HashActionNum, m_ActionNum );
    }


    // Update is called once per frame
    void Update() {
        CacheAnimatorStatus( );
        CheckDistance( );
        CalculateAngle( );
        CheckState( );
        EnemyTurning( );

        switch (enemyState) {
            case EnemyState.Dead:
                anim.SetTrigger( m_HashDead );
                break;
            case EnemyState.Hit:
                anim.SetTrigger( m_HashHit );
                break;
            case EnemyState.Action:
                ChooseAction( );
                break;
            case EnemyState.Idle:
                break;
        }
    }
}
