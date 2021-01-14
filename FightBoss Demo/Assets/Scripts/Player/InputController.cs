using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    #region パラメータ宣言
    // Variables
    [Header( "==== Mouse Setting ====" )]
    public bool mouseEnable = true;
    [SerializeField]
    private float mouseSensitivityX = 8.0f;
    [SerializeField]
    private float mouseSensitivityY = 6.0f;

    // Key signals
    [SerializeField]
    private bool m_Dash;
    [SerializeField]
    private bool m_Attack;
    [SerializeField]
    private bool m_HeaveAttack;
    [SerializeField]
    private bool m_isMove;
    [SerializeField]
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

    public bool Dash {
        get { return m_Dash && inputEnabled; }
    }

    public bool Attack {
        get { return m_Attack && inputEnabled; }
    }

    public bool HeaveAttack {
        get { return m_HeaveAttack && inputEnabled; }
    }

    public bool IsMove {
        get { return m_isMove && inputEnabled; }
    }

    public bool IsRun {
        get { return m_isRun && m_isMove && inputEnabled; }
    }
    #endregion

    #region コルーチン宣言
    // coroutine define
    WaitForSeconds m_AttackInputWait;
    WaitForSeconds m_HeaveAttackInputWait;
    WaitForSeconds m_DashInputWait;
    Coroutine m_AttackWaitCoroutine;
    Coroutine m_HeaveAttackWaitCoroutine;
    Coroutine m_DashWaitCoroutine;

    const float InputWaitTime = 0.08f;
    #endregion

    #region 初期化
    private void Awake() {
        m_AttackInputWait = new WaitForSeconds( InputWaitTime );
        m_HeaveAttackInputWait = new WaitForSeconds( InputWaitTime );
        m_DashInputWait = new WaitForSeconds( InputWaitTime );
    }

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        inputEnabled = true;
        cameraLockOn = true;
        cameraVerticalSignal = 0;
        cameraHorizontalSignal = 0;
    }
    #endregion

    #region コルーチン実現
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
    #endregion


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

        /* control the signals by coroutines */
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
            if (m_DashWaitCoroutine != null)
                StopCoroutine( m_DashWaitCoroutine );
            m_DashWaitCoroutine = StartCoroutine( DashWait( ) );
        }

        /* other input signals */
        if (Input.GetKey( KeyCode.LeftShift )) {
            m_isRun = true;
        } else {
            m_isRun = false;
        }

        if(Input.GetKeyDown( KeyCode.LeftControl )) {
            cameraLockOn = !cameraLockOn;
        }

        /* normalize movement input */
        DampMovementInput( );
        NormalizeMovementInput( new Vector2( moveHorizontalSignal, moveVerticalSignal ) );

        /* calculate magnitude and direction */
        CalculateMovement( );
    }
}
