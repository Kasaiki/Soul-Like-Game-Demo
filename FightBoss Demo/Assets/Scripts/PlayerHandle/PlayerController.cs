using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /* Components */
    public InputController ic;
    public Animator anim;
    public GameObject model;

    /* player states */

    /* Animator parameters */
    public bool canDash = true;
    public bool canAttack = true;

    /* Player propoties */
    //private Transform lockTarget = null;
    public float rotationSpeed = 0.1f;

    /* Temp parameters */
    private Vector3 targetDirection;

    void Start()
    {
        ic = GetComponent<InputController>( );
        anim = GetComponentInChildren<Animator>( );
    }

    void Update()
    {
        /* dodge */
        if (ic.dodge) {
            anim.SetTrigger( "dodge" );
        }


        /* Attack 
        if (Input.GetMouseButtonDown( 0 )) {
            anim.SetTrigger( "attack" );
        }*/


        /* smoothly rotate and move*/
        if(ic.Dmag > 0.2f) {
            model.transform.forward = Vector3.Slerp( model.transform.forward, ic.Dvec, rotationSpeed);
        }
        anim.SetFloat( "forward", Mathf.Lerp( anim.GetFloat( "forward" ), ic.Dmag * ((ic.run) ? 2.0f : 1.0f), 0.1f ) );
    }
}
