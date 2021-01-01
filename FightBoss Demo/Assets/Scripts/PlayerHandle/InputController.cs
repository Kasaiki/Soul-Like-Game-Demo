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

    public bool Run {
        get { return m_Run && inputEnabled; }
    }

    public bool Dodge {
        get { return m_Dodge && inputEnabled; }
    }

    public bool Attack {
        get { return m_Attack && inputEnabled; }
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

        /* control the attack signal by coroutine */
        if (Input.GetButtonDown( "Fire1" )) {
            if (m_AttackWaitCoroutine != null)
                StopCoroutine( m_AttackWaitCoroutine );

            m_AttackWaitCoroutine = StartCoroutine( AttackWait( ) );
        }

        /* other input signals */
        m_Run = Input.GetKey( keyA );
        m_Dodge = Input.GetKeyDown( keyB );

        DampMovementInput( );

        /* normalize input */
        NormalizeMovementInput( new Vector2( Dright, Dup ) );

        /* calculate magnitude and direction */
        CalculateMovement( );
    }


    WaitForSeconds m_AttackInputWait;
    Coroutine m_AttackWaitCoroutine;

    const float k_AttackInputDuration = 0.03f;
    private void Awake() {
        m_AttackInputWait = new WaitForSeconds( k_AttackInputDuration );
    }

    IEnumerator AttackWait() {
        m_Attack = true;
        yield return m_AttackInputWait;
        m_Attack = false;
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
