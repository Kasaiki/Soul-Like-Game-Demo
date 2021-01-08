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
    int lastActionNum;
    int actionNum;

    // Animator Info
    protected AnimatorStateInfo m_CurrentStateInfo;

    WaitForSeconds m_ActionTime;
    Coroutine m_ActionCorotine;

    // Animator parameters Hash 
    readonly int m_HashActionNum = Animator.StringToHash( "ActionNum" );
    readonly int m_HashDirection = Animator.StringToHash( "Direction" );
    readonly int m_HashJumpSpeed = Animator.StringToHash( "JumpSpeed" );
    readonly int m_HashIsWalk = Animator.StringToHash( "IsWalk" );
    readonly int m_HashBroken = Animator.StringToHash( "Broken" );
    readonly int m_HashAttack = Animator.StringToHash( "Attack" );
    readonly int m_HashDead = Animator.StringToHash( "Dead" );
    readonly int m_HashHit = Animator.StringToHash( "Hit" );
    readonly int m_HashStateTime = Animator.StringToHash( "StateTime" );

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>( );
        playerTrans = GameObject.FindGameObjectWithTag( "Player" ).transform;
        targetDirector = GameObject.FindGameObjectWithTag( "EnemyDirector" ).transform;

        currtHP = 100;
        currtSTA = 100;
        enemyState = EnemyState.Idle;
    }

    void CacheAnimatorStatus() {
        m_CurrentStateInfo = anim.GetCurrentAnimatorStateInfo( 0 );
        anim.SetFloat( m_HashStateTime, Mathf.Repeat( m_CurrentStateInfo.normalizedTime, 1f ) );
    }

    void CheckState() {
        if (currtHP <= 0) {
            currtHP = 0;
            enemyState = EnemyState.Dead;
            return;
        }else if(currtSTA <= 0) {
            currtSTA = 0;
            enemyState = EnemyState.Broken;
            return;
        }
        //if ( hitted ) {
        //  enemyState = EnemyState.Hit;

        //}
    }

    void DeadBehaviour() {
        // play You Win animation and back to menu
    }

    void HitBehaviour() {
        // 
    }

    void AttackBehaviour() {
        if (distanceFromPlayer >= 7.0f) { // long distance

        } else if (distanceFromPlayer >= 2.0f) {
            lastActionNum = actionNum;
            actionNum = Random.Range( 0, System.Enum.GetNames( typeof( MediumDistAction ) ).Length );
        } else {
            lastActionNum = actionNum;
            actionNum = Random.Range( 0, System.Enum.GetNames( typeof( ShortDistAction ) ).Length );
        }
    }

    void MoveBehaviour() {
        //
    }

    void IdleBehaviour() {

    }

    void CheckDistance() {
        distanceFromPlayer = Vector3.Distance( transform.position, playerTrans.position );
    }
    

    void EnemyTurning() {
        targetDirector.forward = playerTrans.position - transform.position;
        transform.rotation = Quaternion.RotateTowards( transform.rotation, targetDirector.rotation , rotationSpeed * Time.deltaTime );
    }

    // Update is called once per frame
    void Update() {
        CacheAnimatorStatus( );
        CheckDistance( );
        CheckState( );
        EnemyTurning( );

        switch (enemyState) {
            case EnemyState.Dead:
                DeadBehaviour( );
                anim.SetTrigger( m_HashDead );
                break;
            //case EnemyState.Broken:
            //    anim.SetBool( m_HashBroken , true );
            //    break;
            case EnemyState.Hit:
                HitBehaviour( );
                anim.SetTrigger( m_HashHit );
                break;
            case EnemyState.Attack:
                AttackBehaviour( );
                anim.SetTrigger( m_HashAttack );
                break;
            case EnemyState.Move:
                MoveBehaviour( );
                anim.SetBool( m_HashIsWalk , true );
                break;
            case EnemyState.Idle:
                IdleBehaviour( );
                anim.SetBool( m_HashIsWalk , false );
                break;
        }
    }
}
