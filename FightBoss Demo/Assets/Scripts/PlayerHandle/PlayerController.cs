using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /* Components */
    public InputController ic;
    public Animator m_Animator;
    public GameObject model;

    /* Animator parameters */
    public bool canDash = true;
    public bool canAttack = true;

    /* Player propoties */
    //private Transform lockTarget = null;
    public float rotationSpeed = 10f;

    /* Temp parameters */
    private Vector3 targetDirection;

    /* Animator parameters Hash */
    readonly int m_HashForwardSpeed = Animator.StringToHash( "ForwardSpeed" );
    readonly int m_HashGrounded = Animator.StringToHash( "Grounded" );
    readonly int m_HashAttack = Animator.StringToHash( "Attack" );
    readonly int m_HashDodge = Animator.StringToHash( "Dodge" );
    readonly int m_HashHurt = Animator.StringToHash( "Hurt" );
    readonly int m_HashDeath = Animator.StringToHash( "Death" );
    readonly int m_HashStateTime = Animator.StringToHash( "StateTime" );

    /* Animator state info */
    protected AnimatorStateInfo m_CurrentStateInfo;
    protected AnimatorStateInfo m_NextStateInfo;
    protected bool m_IsAnimatorTransitioning;
    protected AnimatorStateInfo m_PreviousCurrentStateInfo;
    protected AnimatorStateInfo m_PreviousNextStateInfo;
    protected bool m_PreviousIsAnimatorTransitioning;

    void Start()
    {
        ic = GetComponent<InputController>( );
        m_Animator = GetComponentInChildren<Animator>( );
    }

    void FixedUpdate()
    {
        CacheAnimatorState( );

        m_Animator.SetFloat( m_HashStateTime, Mathf.Repeat( m_Animator.GetCurrentAnimatorStateInfo( 0 ).normalizedTime, 1f ) );
        m_Animator.ResetTrigger( m_HashAttack );

        SetDodge( );
        SetAttack( );

        TurningCharacter( );
        MoveCharacter( );

    }

    // smoothly rotate
    void TurningCharacter( ) {
        if (ic.Dmag > 0.2f) {
            Quaternion targetDir = Quaternion.LookRotation( ic.Dvec, Vector3.up );
            model.transform.rotation = Quaternion.Slerp( model.transform.rotation, targetDir, rotationSpeed * Time.fixedDeltaTime );
            //model.transform.forward = Vector3.Slerp( model.transform.forward, ic.Dvec, rotationSpeed);
        }
    }

    void MoveCharacter( ) {
        m_Animator.SetFloat( m_HashForwardSpeed , Mathf.Lerp( m_Animator.GetFloat( m_HashForwardSpeed ), ic.Dmag * ((ic.Run) ? 2.0f : 1.0f), 0.2f ) );
    }

    void SetDodge() {
        if (ic.Dodge) {
            m_Animator.SetTrigger( m_HashDodge );
        }
    }

    void SetAttack() {
        if (ic.Attack) {
            m_Animator.SetTrigger( m_HashAttack );
        }
    }

    void CacheAnimatorState() {
        m_PreviousCurrentStateInfo = m_CurrentStateInfo;
        m_PreviousNextStateInfo = m_NextStateInfo;
        m_PreviousIsAnimatorTransitioning = m_IsAnimatorTransitioning;

        m_CurrentStateInfo = m_Animator.GetCurrentAnimatorStateInfo( 0 );
        m_NextStateInfo = m_Animator.GetNextAnimatorStateInfo( 0 );
        m_IsAnimatorTransitioning = m_Animator.IsInTransition( 0 );
    }
}
