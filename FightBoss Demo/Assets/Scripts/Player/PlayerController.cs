using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// このクラスでアニメーションに関する処理を行う
public class PlayerController : MonoBehaviour
{
    // Components
    [SerializeField]
    public InputController ic;
    [SerializeField]
    public PlayerData playerData;
    [SerializeField]
    public Animator m_Animator;
    [SerializeField]
    public Rigidbody rig;
    [SerializeField]
    public Transform targetDirector;
    [SerializeField]
    public GameObject model;
    [SerializeField]
    public WeaponManager weaponManager;

    // Animator parameters
    public bool canDash = true;
    public bool canAttack = true;

    // Player propoties
    [SerializeField]
    private float currentRotateSpeed = 1080f;
    [SerializeField]
    private float maxRotateSpeed = 1080f;
    [SerializeField]
    private float minRotateSpeed = 90f;

    // Animator parameters Hash 
    readonly int m_HashForwardSpeed = Animator.StringToHash( "ForwardSpeed" );
    readonly int m_HashGrounded = Animator.StringToHash( "Grounded" );
    readonly int m_HashAttack = Animator.StringToHash( "Attack" );
    readonly int m_HashHeaveAttack = Animator.StringToHash( "HeaveAttack" );
    readonly int m_HashHit = Animator.StringToHash( "Hit" );
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
        playerData = GetComponentInParent<PlayerData>( );
        ic = GetComponentInParent<InputController>( );
        rig = GetComponentInParent<Rigidbody>( );
        m_Animator = GetComponent<Animator>( );
        targetDirector = GameObject.FindGameObjectWithTag( "PlayerDirector" ).transform;
        weaponManager = GetComponentInChildren<PlayerWeaponManager>( );
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
        rig.velocity = Vector3.zero;
    }

    void FaceToDirect( Vector3 faceToDir ) {
        transform.forward = faceToDir;
    }

    void ChangeRotationSpeed() {
        if (m_CurrentStateInfo.IsTag( "Attacking" ) || m_CurrentStateInfo.IsTag("Hit")) {
            currentRotateSpeed = minRotateSpeed;
        } else {
            currentRotateSpeed = maxRotateSpeed;
        }
    }

    public void SetHit(Vector3 hitPosition) {
        if (!m_CurrentStateInfo.IsTag( "Hit" ))
            m_Animator.SetTrigger( m_HashHit );
        hitPosition = new Vector3( hitPosition.x, 0, hitPosition.z );
        FaceToDirect( hitPosition - transform.position );
    }

    void SetDash() {
        if (m_CurrentStateInfo.IsName( "Dash" )) {
            if(ic.Dash && m_Animator.GetFloat(m_HashStateTime) > 0.6) {
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

    public void SetDead() {
        m_Animator.SetTrigger( m_HashDeath );
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

    public bool CheckInvincible() {
        if (m_CurrentStateInfo.IsTag( "Hit" ) || m_CurrentStateInfo.IsTag( "Dodge" ) || m_CurrentStateInfo.IsTag( "Dead" ))
            return true;
        else
            return false;
    }

    void FixedUpdate()
    {
        if (playerData.isDead) {
            rig.velocity = Vector3.zero;
            return;
        }
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
