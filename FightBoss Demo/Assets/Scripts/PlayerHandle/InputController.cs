using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Variables
    [Header( "==== Mouse Setting ====" )]
    public bool mouseEnable = true;
    private float mouseSensitivityX = 12.0f;
    private float mouseSensitivityY = 8.0f;

    // The inputment setting
    [Header("==== Key Setting ====")]
    private KeyCode keyA = KeyCode.LeftShift;
    private KeyCode keyB = KeyCode.Space;
    public KeyCode keyC;
    public KeyCode keyD;

    // pressing signal
    private bool m_Run;
    private bool m_Dodge;
    private bool m_Attack;
    private bool m_HeaveAttack;

    [Header("==== Input Signals ====")]
    // Camera rotation
    public float Jup;
    public float Jright;

    // Original Signals
    private float Dup;
    private float Dright;

    // Fixed Signals
    public float Dup2;
    public float Dright2;

    // magnitude and direction
    public float Dmag;
    public Vector3 Dvec;

    [Header( "==== Ohters ====" )]
    public bool inputEnabled = true;
    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    // coroutine define
    WaitForSeconds m_AttackInputWait;
    WaitForSeconds m_HeaveAttackInputWait;
    WaitForSeconds m_DodgeInputWait;
    Coroutine m_AttackWaitCoroutine;
    Coroutine m_HeaveAttackWaitCoroutine;
    Coroutine m_DodgeWaitCoroutine;
    const float k_AttackInputDuration = 0.03f;
    const float k_HeaveAttackInputDuration = 0.03f;
    const float k_DodgeInputDuration = 0.03f;

    private void Awake() {
        m_AttackInputWait = new WaitForSeconds( k_AttackInputDuration );
        m_HeaveAttackInputWait = new WaitForSeconds( k_HeaveAttackInputDuration );
        m_DodgeInputWait = new WaitForSeconds( k_DodgeInputDuration );
    }

    public bool Run {
        get { return m_Run && inputEnabled; }
    }

    public bool Dodge {
        get { return m_Dodge && inputEnabled; }
    }

    public bool Attack {
        get { return m_Attack && inputEnabled; }
    }

    public bool HeaveAttack {
        get { return m_HeaveAttack && inputEnabled; }
    }

    // Update is called once per frame
    void Update()
    {
        /* control the camera by mouse */
        if (mouseEnable) {
            Jup = -Input.GetAxis( "Mouse Y" ) * mouseSensitivityY;
            Jright = Input.GetAxis( "Mouse X" ) * mouseSensitivityX;
        }

        /* movement input signals */
        targetDup = Input.GetAxisRaw( "Vertical" );
        targetDright = Input.GetAxisRaw( "Horizontal" );
        if (inputEnabled == false) {
            targetDup = 0;
            targetDright = 0;
        }

        /* control the signal by coroutine */
        if (Input.GetButtonDown( "Fire1" )) { // Attack
            if (m_AttackWaitCoroutine != null)
                StopCoroutine( m_AttackWaitCoroutine );
            m_AttackWaitCoroutine = StartCoroutine( AttackWait( ) );
        }

        if (Input.GetButtonDown( "Fire2" )) { // HeaveAttack
            if (m_AttackWaitCoroutine != null)
                StopCoroutine( m_AttackWaitCoroutine );
            m_AttackWaitCoroutine = StartCoroutine( HeaveAttackWait( ) );
        }

        if (Input.GetKeyDown( keyB )) { // Dodge
            if (m_DodgeWaitCoroutine != null)
                StopCoroutine( m_DodgeWaitCoroutine );
            m_DodgeWaitCoroutine = StartCoroutine( DodgeWait( ) );
        }

        /* other input signals */
        m_Run = Input.GetKey( keyA );

        /* normalize movement input */
        DampMovementInput( );
        NormalizeMovementInput( new Vector2( Dright, Dup ) );

        /* calculate magnitude and direction */
        CalculateMovement( );
    }

    IEnumerator AttackWait() {
        m_Attack = true;
        yield return m_AttackInputWait;
        m_Attack = false;
    }

    IEnumerator HeaveAttackWait() {
        m_HeaveAttack = true;
        yield return m_AttackInputWait;
        m_HeaveAttack = false;
    }

    IEnumerator DodgeWait() {
        m_Dodge = true;
        yield return m_DodgeInputWait;
        m_Dodge = false;
    }

    private void DampMovementInput() {
        Dup = Mathf.SmoothDamp( Dup, targetDup, ref velocityDup, 0.1f );
        Dright = Mathf.SmoothDamp( Dright, targetDright, ref velocityDright, 0.1f );
    }

    private void NormalizeMovementInput(Vector2 input) {
        Vector2 output = Vector2.zero;

        Dright2 = input.x * Mathf.Sqrt( 1 - (input.y * input.y / 2.0f) );
        Dup2 = input.y * Mathf.Sqrt( 1 - (input.x * input.x / 2.0f) );
    }

    private void CalculateMovement() {
        Dmag = Mathf.Sqrt( (Dup2 * Dup2) + (Dright2 * Dright2) ); // magnitude
        Dvec = Dright2 * this.transform.right + Dup2 * this.transform.forward; // direction
    }
}
