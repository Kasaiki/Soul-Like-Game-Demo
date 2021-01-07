using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Components
    public InputController ic;
    public Animator m_Animator;
    public Rigidbody rig;
    public GameObject model;

    // Animator parameters
    public bool canDash = true;
    public bool canAttack = true;

    // Player propoties
    public float rotationSpeed = 12f;

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

    /* Animator State Name Hash */
    readonly int m_State_Move = Animator.StringToHash( "Move" );

    /* Animator state info */
    protected AnimatorStateInfo m_CurrentStateInfo;
    protected float m_AngleDifferent;

    void Start()
    {
        ic = GetComponent<InputController>( );
        rig = GetComponentInParent<Rigidbody>( );
        m_Animator = GetComponentInChildren<Animator>( );

    }

    void FixedUpdate()
    {
        CacheAnimatorState( );
        ChangeRotationSpeed( );

        m_Animator.SetFloat( m_HashStateTime, Mathf.Repeat( m_Animator.GetCurrentAnimatorStateInfo( 0 ).normalizedTime, 1f ) );
        m_Animator.ResetTrigger( m_HashAttack );
        m_Animator.ResetTrigger( m_HashDash );
        m_Animator.ResetTrigger( m_HashHeaveAttack );

        SetDash( );
        SetAttack( );
        SetHeaveAttck( );

        TurningCharacter( );
        MoveCharacter( );

    }

    // smoothly rotate
    void TurningCharacter( ) {
        if (ic.IsMove) {
            Quaternion targetDir = Quaternion.LookRotation( ic.targetDirection, Vector3.up );
            model.transform.rotation = Quaternion.Slerp( model.transform.rotation, targetDir, rotationSpeed * Time.fixedDeltaTime );
            //model.transform.forward = Vector3.Slerp( model.transform.forward, ic.Dvec, rotationSpeed);
        }
    }

    void MoveCharacter( ) {
        float currForwardSpeed = m_Animator.GetFloat( m_HashForwardSpeed );
        if ( ic.IsRun ) {
            m_Animator.SetFloat( m_HashForwardSpeed, Mathf.Lerp( currForwardSpeed, 1f, 0.4f ) );
        } else {
            m_Animator.SetFloat( m_HashForwardSpeed, Mathf.Lerp( currForwardSpeed, 0f, 10f * Time.fixedDeltaTime ) );
        }
        m_Animator.SetBool( m_HashIsMove, ic.IsMove );

        if (m_CurrentStateInfo.IsName( "Dash" )) {
            float velocity = m_Animator.GetFloat( m_Curve_DashVelocity );
            rig.transform.position +=  velocity * m_Animator.transform.forward;
        }
    }

    void ChangeRotationSpeed() {
        print( rotationSpeed );
        if(m_CurrentStateInfo.IsTag( "Attacking" )) {
            rotationSpeed = 2f;
        } else {
            rotationSpeed = 12f;
        }
    }

    void SetDash() {
        if (ic.Dash) {
            m_Animator.SetTrigger( m_HashDash );
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

    void CacheAnimatorState() {
        m_CurrentStateInfo = m_Animator.GetCurrentAnimatorStateInfo( 0 );
    }
}
