using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject playerHandle; // control the horizontal rotation
    private GameObject cameraHandle; // control the vertical rotation
    private GameObject cameraMain;
    private InputController ic;

    private Animator playerAnim;
    private Vector3 cameraDampVelocity;
    private float tempEulerX;


    private void Start() {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        ic = playerHandle.GetComponent<InputController>( );
        playerAnim = playerHandle.GetComponentInChildren<Animator>( );
        cameraMain = Camera.main.gameObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // lock the avator's rotation(1/2)
        Vector3 tempAnimEuler = playerAnim.transform.eulerAngles;

        // vertical camera rotation
        playerHandle.transform.Rotate( Vector3.up, ic.Jright * 10f * Time.fixedDeltaTime );
        // horizontal camera rotation
        tempEulerX += ic.Jup * 10f * Time.fixedDeltaTime;
        tempEulerX = Mathf.Clamp( tempEulerX , -50, 30 );
        cameraHandle.transform.localEulerAngles = new Vector3( tempEulerX, 0, 0 );

        // lock the avator's rotation(2/2)
        playerAnim.transform.eulerAngles = tempAnimEuler;
        //cameraMain.transform.position = Vector3.SmoothDamp( cameraMain.transform.position, transform.position,ref cameraDampVelocity, 0.02f );

        // update the camera position
        cameraMain.transform.position = transform.position;
        cameraMain.transform.eulerAngles = transform.eulerAngles;
    }
}
