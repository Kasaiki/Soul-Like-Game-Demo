using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// このクラスでアニメーションに関する処理を行う
public class PlayerController : MonoBehaviour
{
    // Components
    public InputController ic;
    public Animator m_Animator;
    public Rigidbody rig;
    public Transform targetDirector;
    public GameObject model;

    // Animator parameters
    public bool canDash = true;
    public bool canAttack = true;

    // Player propoties
    private float currentRotateSpeed = 1080f;
    private float maxRotateSpeed = 1080f;
    private float minRotateSpeed = 120f;

    // Animator parameters Hash 
    readonly int m_HashForwardSpeed = Animator.StringToHash( "ForwardSpeed" );
    readonly int m_HashGrounded = Animator.StringToHash( "Grounded" );
    readonly int m_HashAttack = Animator.StringToHash( "Attack" );
    readonly int m_HashHeaveAttack = Animator.StringToHash( "HeaveAttack" );
    readonly int m_HashHurt = Animator.StringToHash( "Hurt" );
    readonly int m_HashDeath = Animator.StringToHash( "Death" );
    readonly int m_HashIsMove = Animator.StringToHash( "IsMove" );
    readonly int m_HashDash = Animator.StringToHash( "Dash" );
    readonly int m_HashStateTime = Animator.StringToHash( "StateTime" );

    readonly int m_Curve_DashVelocity = Animator.StringToHash( "DashVelocity" );

    // Animator State Name Hash
    readonly int m_State_Move = Animator.StringToHash( "Move" );

    //　Animator state info
    protected AnimatorStateInfo m_CurrentStateInfo;
    protected AnimatorStateInfo m_LastStateInfo;
    protected float m_AngleDifferent;

    void Start()
    {
        ic = GetComponent<InputController>( );
        rig = GetComponentInParent<Rigidbody>( );
        m_Animator = GetComponentInChildren<Animator>( );
        targetDirector = GameObject.FindGameObjectWithTag( "PlayerDirector" ).transform;

    }

    // smoothly rotate
    void TurningCharacter() {
        if (ic.IsMove) {
            targetDirector.forward = ic.targetDirection;
            model.transform.rotation = Quaternion.RotateTowards( model.transform.rotation, targetDirector.rotation, currentRotateSpeed * Time.fixedDeltaTime );
        }
    }

    void MoveCharacter() {
        float currForwardSpeed = m_Animator.GetFloat( m_HashForwardSpeed );
        if (ic.IsRun && m_CurrentStateInfo.IsName("Moving")) {
            if (m_LastStateInfo.IsName( "Dash" )) {
                m_Animator.SetFloat( m_HashForwardSpeed, 0.4f );
            } else {
                m_Animator.SetFloat( m_HashForwardSpeed, Mathf.MoveTowards( currForwardSpeed, 1f, 3f * Time.fixedDeltaTime ) );
            }
        } else {
            m_Animator.SetFloat( m_HashForwardSpeed, Mathf.Lerp( currForwardSpeed, 0f, 10f * Time.fixedDeltaTime ) );
        }
        m_Animator.SetBool( m_HashIsMove, ic.IsMove );
    }

    void ChangeRotationSpeed() {
        if (m_CurrentStateInfo.IsTag( "Attacking" )) {
            currentRotateSpeed = minRotateSpeed;
        } else {
            currentRotateSpeed = maxRotateSpeed;
        }
    }

    void SetDash() {
        if (m_CurrentStateInfo.IsName( "Dash" )) {
            if(ic.Dash && m_Animator.GetFloat(m_HashStateTime) > 0.8) {
                m_Animator.SetTrigger( m_HashDash );
            }
            float velocity = m_Animator.GetFloat( m_Curve_DashVelocity );
            rig.transform.position += velocity * m_Animator.transform.forward;
        } else {
            if (ic.Dash) {
                m_Animator.SetTrigger( m_HashDash );
            }
        }
    }

    void SetAttack() {
        if (ic.Attack) {
            m_Animator.SetTrigger( m_HashAttack );
        }
    }

    void SetHeaveAttck() {
        if (ic.HeaveAttack) {
            m_Animator.SetTrigger( m_HashHeaveAttack );
        }
    }

    void ResetAllTriggers() {
        m_Animator.ResetTrigger( m_HashAttack );
        m_Animator.ResetTrigger( m_HashDash );
        m_Animator.ResetTrigger( m_HashHeaveAttack );
    }

    void CacheAnimatorState() {
        m_CurrentStateInfo = m_Animator.GetCurrentAnimatorStateInfo( 0 );
        m_Animator.SetFloat( m_HashStateTime, Mathf.Repeat( m_CurrentStateInfo.normalizedTime, 1f ) );
    }

    void FixedUpdate()
    {
        CacheAnimatorState( );
        ChangeRotationSpeed( );

        ResetAllTriggers( );

        SetDash( );
        SetAttack( );
        SetHeaveAttck( );

        TurningCharacter( );
        MoveCharacter( );

        m_LastStateInfo = m_CurrentStateInfo;
    }
}
