using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : EnemyAttribute
{
    // components
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform playerTrans;
    [SerializeField]
    private Transform targetDirector;
    [SerializeField]
    private EnemyData enemyData;
    [SerializeField]
    private WeaponManager weaponManager;

    // parameters
    [SerializeField]
    private float distanceFromPlayer;
    [SerializeField]
    private float longDist = 10f;
    [SerializeField]
    private float mediumDist = 4f;
    [SerializeField]
    private float rotationSpeed = 300f;

    // アニメーターインフォを用いて、アニメーションの状態を把握できます
    AnimatorStateInfo m_CurrentStateInfo;
    AnimatorStateInfo m_NextStateInfo;

    [SerializeField]
    int m_ActionNum;
    /// <summary>
    /// 0は短距離、1は中距離、2は長距離
    /// </summary>
    [SerializeField]
    int m_DistNum;
    /// <summary>
    /// プレイヤー向きまでの角度
    /// </summary>
    [SerializeField]
    float m_AngleDiff;
    /// <summary>
    /// アニメーションの進捗
    /// </summary>
    [SerializeField]
    float m_StateTime;
    [SerializeField]
    bool m_IsInSight;

    readonly int m_HashActionNum = Animator.StringToHash( "ActionNum" );
    readonly int m_HashDistNum = Animator.StringToHash( "DistNum" );
    readonly int m_HashAngleDiff = Animator.StringToHash( "AngleDiff" );
    readonly int m_HashForwardSpeed = Animator.StringToHash( "ForwardSpeed" );
    readonly int m_HashIsInSight = Animator.StringToHash( "IsInSight" );
    readonly int m_HashDead = Animator.StringToHash( "Dead" );
    readonly int m_HashHit = Animator.StringToHash( "Hit" );
    readonly int m_HashStateTime = Animator.StringToHash( "StateTime" );
    

    void Start()
    {
        anim = GetComponent<Animator>( );
        enemyData = GetComponent<EnemyData>( );
        playerTrans = GameObject.FindGameObjectWithTag( "PlayerDirector" ).transform;
        targetDirector = GameObject.FindGameObjectWithTag( "EnemyDirector" ).transform;
        weaponManager = GetComponentInChildren<EnemyWeaponManager>( );
        state = State.Action;
    }

    // Animation Eventから呼び出します
    void StartAttackEvent() {
        // play sound
        weaponManager.weaponEnable = true;
    }

    // Animation Eventから呼び出します
    void EndAttackEvent() {
        // stop play sound
        weaponManager.weaponEnable = false;
    }

    /// <summary>
    /// 各フレームでアニメーターインフォを更新します
    /// </summary>
    void CacheAnimatorStatus() {
        m_CurrentStateInfo = anim.GetCurrentAnimatorStateInfo( 0 );
        m_StateTime = Mathf.Repeat( m_CurrentStateInfo.normalizedTime, 1f );
        anim.SetFloat( m_HashStateTime, m_StateTime );
    }

    /// <summary>
    /// デッド状態・ヒット状態に遷移するかをチェックします。
    /// </summary>
    void CheckState() {
        if ( enemyData.HP <= 0) {
            state = State.Dead;
            anim.ResetTrigger( m_HashHit );
            return;
        }else if( enemyData.STA <= 0) {
            enemyData.STA = enemyData.getMaxSTA;
            state = State.Hit;
            return;
        }
    }

    /// <summary>
    /// 距離を求めます
    /// </summary>
    void CheckDistance() {
        distanceFromPlayer = Vector3.Distance( transform.position, playerTrans.position );
        if( distanceFromPlayer > longDist) {  //　長距離
            anim.SetFloat( m_HashForwardSpeed, 1 );
            m_DistNum = 2;
        }else if(distanceFromPlayer > mediumDist) {　　//　中距離
            anim.SetFloat( m_HashForwardSpeed, 1 );
            m_DistNum = 1;
        } else {  // 　短距離
            anim.SetFloat( m_HashForwardSpeed, 0.25f );
            m_DistNum = 0;
        }
        distInfo = (Dist)m_DistNum;

        anim.SetInteger( m_HashDistNum, m_DistNum );
    }
    
    /// <summary>
    /// プレイヤーが視界内に存在するかを判定します
    /// </summary>
    void CalculateAngle() {
        float angleCurrent = Mathf.Atan2( transform.forward.x, transform.right.x ) * Mathf.Rad2Deg;
        float targetAngle = Mathf.Atan2( targetDirector.forward.x, targetDirector.right.x ) * Mathf.Rad2Deg;

        m_AngleDiff = Mathf.DeltaAngle( angleCurrent, targetAngle );
        anim.SetFloat( m_HashAngleDiff, m_AngleDiff );

        m_IsInSight = (Mathf.Abs( m_AngleDiff ) <= 40f) ? true : false;
        anim.SetBool( m_HashIsInSight, m_IsInSight );
    }


    /// <summary>
    /// 攻撃ステートの最初25％の時間内は回転することができます
    /// </summary>
    void EnemyTurning() {
        targetDirector.forward = playerTrans.position - transform.position;
        if (m_CurrentStateInfo.IsTag( "Hit" ))
            return;
        if ( (m_CurrentStateInfo.IsTag("Attack") && m_StateTime <= 0.25) || !m_CurrentStateInfo.IsTag( "Attack" ) ) {
            transform.rotation = Quaternion.RotateTowards( transform.rotation, targetDirector.rotation, rotationSpeed * Time.deltaTime );
        }

    }

    void HitBehaviour() {
        if (m_CurrentStateInfo.IsTag( "Hit" ) || m_NextStateInfo.IsTag("Hit") ){
            state = State.Action;
            return;
        } else {
            anim.SetTrigger( m_HashHit );
        }
    }

    /// <summary>
    /// 距離により、次のアクションが決まります。
    /// </summary>
    void ChooseAction() {
        if (m_CurrentStateInfo.IsTag( "Attack" ))
            return;
        if ( m_IsInSight ) {
            switch (distInfo) {
                case Dist.ShortDist:
                    m_ActionNum = Random.Range( 0, 4 );
                    break;
                case Dist.MediumDist:
                    m_ActionNum = Random.Range( 0, 5 );
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
                    m_ActionNum = Random.Range( 0, 4 );
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
        if (state == State.Dead)
            return;
        CacheAnimatorStatus( );
        CheckDistance( );
        CalculateAngle( );
        CheckState( );
        EnemyTurning( );
        switch (state) {
            case State.Dead:
                anim.SetTrigger( m_HashDead );
                break;
            case State.Hit:
                HitBehaviour( );
                break;
            case State.Action:
                ChooseAction( );
                break;
            case State.Idle:
                break;
        }
    }
}
