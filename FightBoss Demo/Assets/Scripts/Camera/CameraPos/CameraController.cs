using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject playerHandle; // control the horizontal rotation
    private GameObject cameraHandle; // control the vertical rotation
    private Transform cameraMain;
    private InputController ic;
    private Animator playerAnim;

    /* paramaters */
    private float maxDistance = 0.1f;
    private Vector3 smoothPosition;
    private Vector3 cameraDampVelocity;
    private float tempEulerX;


    private void Start() {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        ic = playerHandle.GetComponent<InputController>( );
        playerAnim = playerHandle.GetComponentInChildren<Animator>( );
        cameraMain = Camera.main.transform;
    }

    // Update is called once per frame
    void FixedUpdate() {
        // lock the avator's rotation(1/2)
        Vector3 tempAnimEuler = playerAnim.transform.eulerAngles;

        // horizontal camera rotation
        playerHandle.transform.Rotate( Vector3.up, ic.cameraHorizontalSignal * 20f * Time.fixedDeltaTime );
        // vertical camera rotation
        tempEulerX += ic.cameraVerticalSignal * 10f * Time.fixedDeltaTime;
        tempEulerX = Mathf.Clamp( tempEulerX, -50, 30 );
        cameraHandle.transform.localEulerAngles = new Vector3( tempEulerX, 0, 0 );

        // lock the avator's rotation(2/2)
        playerAnim.transform.eulerAngles = tempAnimEuler;

        // update the camera position smoothly
        if (CheckMargin( )) {
            //cameraMain.position = transform.position;
            cameraMain.transform.position = Vector3.SmoothDamp( cameraMain.transform.position, transform.position, ref cameraDampVelocity, 0.35f );
        }
        cameraMain.eulerAngles = transform.eulerAngles;


    }

    bool CheckMargin() {
        // check the distance between camera and target
        return Vector3.Distance( cameraMain.position, this.transform.position ) > maxDistance;
    }
}