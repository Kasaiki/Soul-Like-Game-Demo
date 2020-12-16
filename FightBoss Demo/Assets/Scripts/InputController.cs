using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Variables
    [Header( "==== Mouse Setting ====" )]
    public bool mouseEnable = false;
    public float mouseSensitivityX = 3.0f;
    public float mouseSensitivityY = 1.0f;

    [Header("==== Output Signals ====")]
    public float Dup;
    public float Dright;

    public float Jup;
    public float Jright;

    public float Dmag;
    public Vector3 Dvec;


    [Header( "==== Ohters ====" )]
    public bool inputEnabled = true;
    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;


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
            Jup = Input.GetAxis( "Mouse Y" ) * mouseSensitivityY;
            Jright = Input.GetAxis( "Mouse X" ) * mouseSensitivityX;
        }

        // movement signals input
        targetDup = Input.GetAxisRaw( "Vertical" );
        targetDright = Input.GetAxisRaw( "Horizontal" );

        if(inputEnabled == false) {
            targetDup = 0;
            targetDright = 0;
        }

        // make movement signals varify smoothly
        Dup = Mathf.SmoothDamp( Dup, targetDup, ref velocityDup, 0.1f );
        Dright = Mathf.SmoothDamp( Dright, targetDright, ref velocityDright, 0.1f );

        // fix the "square" inputment to  "circle"
        Vector2 tempDAxis = FixInput( new Vector2( Dright, Dup ) );
        float Dup2 = tempDAxis.y;
        float Dright2 = tempDAxis.x;

        // calculte the magnitude and direction
        Dmag = Mathf.Sqrt( (Dup2 * Dup2) + (Dright2 * Dright2) );
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;
    }
}
