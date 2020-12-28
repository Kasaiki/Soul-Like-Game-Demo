using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Variables
    [Header( "==== Mouse Setting ====" )]
    public bool mouseEnable = true;
    public float mouseSensitivityX = 10.0f;
    public float mouseSensitivityY = 5.0f;

    // The inputment setting
    [Header("==== Key Setting ====")]
    private KeyCode keyA = KeyCode.LeftShift;
    private KeyCode keyB = KeyCode.Space;
    public KeyCode keyC;
    public KeyCode keyD;

    // 1. pressing signal
    public bool run;
    // 2. trigger signal
    public bool dodge;
    // 3. double trigger


    [Header("==== Input Signals ====")]
    // Camera rotation
    public float Jup;
    public float Jright;

    // Battle-mode movement input
    // Original Signals
    public float Dup;
    public float Dright;
    // Fixed Signals
    public float Dup2;
    public float Dright2;

    // Non-battle-mode movement input
    public float Dmag;
    public Vector3 Dvec;

    [Header( "==== Ohters ====" )]
    public bool inputEnabled = true;
    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;
    private Vector2 tempDAxis;

    /// <summary>
    /// normalize the input of x&y axis
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private Vector2 FixInput(Vector2 input) {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt( 1 - (input.y * input.y / 2.0f) );
        output.y = input.y * Mathf.Sqrt( 1 - (input.x * input.x / 2.0f) );
        return output;
    }

    // Update is called once per frame
    void Update()
    {
        // control the camera by mouse
        if (mouseEnable) {
            Jup = -Input.GetAxis( "Mouse Y" ) * mouseSensitivityY;
            Jright = Input.GetAxis( "Mouse X" ) * mouseSensitivityX;
        }

        /* movement input signals */
        targetDup = Input.GetAxisRaw( "Vertical" );
        targetDright = Input.GetAxisRaw( "Horizontal" );

        if(inputEnabled == false) {
            targetDup = 0;
            targetDright = 0;
        }

        /* other input signals */
        run = Input.GetKey( keyA );
        dodge = Input.GetKeyDown( keyB );

        // make movement signals varify smoothly
        Dup = Mathf.SmoothDamp( Dup, targetDup, ref velocityDup, 0.1f );
        Dright = Mathf.SmoothDamp( Dright, targetDright, ref velocityDright, 0.1f );

        /* normalize input */
        tempDAxis = FixInput( new Vector2( Dright, Dup ) );

        /* the normalized inputs */
        Dup2 = tempDAxis.y; // vertical
        Dright2 = tempDAxis.x; // horizontal

        /* the Non-battle-mode animator input */
        Dmag = Mathf.Sqrt( (Dup2 * Dup2) + (Dright2 * Dright2) ); // magnitude
        Dvec = Dright2 * this.transform.right + Dup2 * this.transform.forward; // direction

    }
}
