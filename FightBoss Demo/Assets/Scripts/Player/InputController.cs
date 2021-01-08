using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Variables
    [Header( "==== Mouse Setting ====" )]
    public bool mouseEnable = true;
    private float mouseSensitivityX = 8.0f;
    private float mouseSensitivityY = 6.0f;

    // Key signals
    private bool m_Dash;
    private bool m_Attack;
    private bool m_HeaveAttack;
    private bool m_isMove;
    private bool m_isRun;

    [Header( "==== Input Signals ====" )]
    // Camera rotation
    public float cameraVerticalSignal;
    public float cameraHorizontalSignal;

    // Original Signals
    private float moveVerticalSignal;
    private float moveHorizontalSignal;

    // Fixed Signals
    public float fixedMoveVerticalSignal;
    public float fixedMoveHorizontalSignal;

    // magnitude and direction
    public float targetMagnitude;
    public Vector3 targetDirection;

    [Header( "==== Ohters ====" )]
    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    public bool inputEnabled;
    public bool cameraLockOn;

    // coroutine define
    WaitForSeconds m_AttackInputWait;
    WaitForSeconds m_HeaveAttackInputWait;
    WaitForSeconds m_DashInputWait;
    Coroutine m_AttackWaitCoroutine;
    Coroutine m_HeaveAttackWaitCoroutine;
    Coroutine m_DashWaitCoroutine;
    const float k_InputWait = 0.08f;

    private void Awake() {
        m_AttackInputWait = new WaitForSeconds( k_InputWait );
        m_HeaveAttackInputWait = new WaitForSeconds( k_InputWait );
        m_DashInputWait = new WaitForSeconds( k_InputWait );
    }

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        inputEnabled = true;
        cameraLockOn = false;
        cameraVerticalSignal = 0;
        cameraHorizontalSignal = 0;
    }

    public bool Dash {
        get { return m_Dash && inputEnabled; }
    }

    public bool Attack {
        get { return m_Attack && inputEnabled; }
    }

    public bool HeaveAttack {
        get { return m_HeaveAttack  && inputEnabled; }
    }

    public bool IsMove {
        get { return m_isMove && inputEnabled ; }
    }

    public bool IsRun {
        get { return m_isRun && m_isMove && inputEnabled; }
    }

    IEnumerator AttackWait() {
        m_Attack = true;
        yield return m_AttackInputWait;
        m_Attack = false;
    }

    IEnumerator HeaveAttackWait() {
        m_HeaveAttack = true;
        yield return m_HeaveAttackInputWait;
        m_HeaveAttack = false;
    }

    IEnumerator DashWait() {
        m_Dash = true;
        yield return m_DashInputWait;
        m_Dash = false;
    }

    private void DampMovementInput() {
        moveVerticalSignal = Mathf.SmoothDamp( moveVerticalSignal, targetDup, ref velocityDup, 0.1f );
        moveHorizontalSignal = Mathf.SmoothDamp( moveHorizontalSignal, targetDright, ref velocityDright, 0.1f );
    }

    private void NormalizeMovementInput(Vector2 input) {
        Vector2 output = Vector2.zero;

        fixedMoveHorizontalSignal = input.x * Mathf.Sqrt( 1 - (input.y * input.y / 2.0f) );
        fixedMoveVerticalSignal = input.y * Mathf.Sqrt( 1 - (input.x * input.x / 2.0f) );
    }

    private void CalculateMovement() {
        //targetMagnitude = Mathf.Sqrt( (fixedMoveVerticalSignal * fixedMoveVerticalSignal) + (fixedMoveHorizontalSignal * fixedMoveHorizontalSignal) );
        targetDirection = fixedMoveHorizontalSignal * Camera.main.transform.right + fixedMoveVerticalSignal * Camera.main.transform.forward;
        targetDirection.y = 0;
        targetDirection.Normalize( );
    }

    // Update is called once per frame
    void Update()
    {
        /* movement input signals */
        targetDup = Input.GetAxisRaw( "Vertical" );
        targetDright = Input.GetAxisRaw( "Horizontal" );
        if (inputEnabled == false) {
            targetDup = 0;
            targetDright = 0;
        }
        m_isMove = Mathf.Abs( targetDup ) + Mathf.Abs( targetDright ) > 0 ? true : false;

        /* camera control signals */
        if (mouseEnable) {
            cameraVerticalSignal += -Input.GetAxis( "Mouse Y" ) * mouseSensitivityY;
            cameraHorizontalSignal += Input.GetAxis( "Mouse X" ) * mouseSensitivityX + fixedMoveHorizontalSignal * targetMagnitude * 0.4f;
            //print( fixedMoveHorizontalSignal );
        }

        /* control the signal by coroutine */
        if (Input.GetButtonDown( "Fire1" )) { // Attack
            if (m_AttackWaitCoroutine != null)
                StopCoroutine( m_AttackWaitCoroutine );
            m_AttackWaitCoroutine = StartCoroutine( AttackWait(  ) );
        }

        if (Input.GetButtonDown( "Fire2" )) { // HeaveAttack
            if ( m_HeaveAttackWaitCoroutine!= null)
                StopCoroutine( m_HeaveAttackWaitCoroutine );
            m_HeaveAttackWaitCoroutine = StartCoroutine( HeaveAttackWait(  ) );
        }

        if (Input.GetKeyDown( KeyCode.LeftShift )) { // HeaveAttack
            if (m_HeaveAttackWaitCoroutine != null)
                StopCoroutine( m_HeaveAttackWaitCoroutine );
            m_HeaveAttackWaitCoroutine = StartCoroutine( DashWait( ) );
        }

        /* other input signals */
        if (Input.GetKey( KeyCode.LeftShift )) {
            m_isRun = true;
        } else {
            m_isRun = false;
        }

        if(Input.GetKeyDown( KeyCode.LeftControl )) {
            cameraLockOn = !cameraLockOn;
            print( " lock mode is :" + cameraLockOn );
        }

        /* normalize movement input */
        DampMovementInput( );
        NormalizeMovementInput( new Vector2( moveHorizontalSignal, moveVerticalSignal ) );

        /* calculate magnitude and direction */
        CalculateMovement( );
    }
}
