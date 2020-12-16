using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputController pi;
    public Animator anim;

    private bool canDash;
    private bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        pi = GetComponent<InputController>( );
        anim = GetComponent<Animator>( );
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canDash && Input.GetKeyDown( KeyCode.Space )) { 
            anim.SetTrigger( "dash" );
        }

        if (Input.GetMouseButtonDown( 0 )) {
            anim.SetTrigger( "attack" );
        }

        if (Input.GetKey( KeyCode.LeftShift )) {
            anim.SetBool( "run", true );
        } else {
            anim.SetBool( "run", false );
        }

        anim.SetFloat( "x_Input", pi.Dvec.x );
        anim.SetFloat( "y_Input", pi.Dvec.z );
        anim.SetFloat( "magnitude", pi.Dmag );

    }

    public void OnEnterDash() {
        canDash = false;
    }

    public void OnExitDash() {
        canDash = true;
    }
}
