using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;
    private Transform cameraMain;
    private InputController ic;
    private LockOnTargetDetector detector;
    public GameObject target = null;

    /* paramaters */
    private float distance;
    private float maxDistance = 3f;
    private float dampRate;
    private float cameraHeight = 1.7f;
    private Vector3 cameraOffset = new Vector3( 0, 0, 4f );
    private Vector3 cameraTargetDir;

    private Vector3 cameraDampVelocity;


    private void Start() {
        player = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<Transform>( );
        ic = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<InputController>( );
        cameraMain = Camera.main.transform;
        cameraTargetDir = cameraMain.eulerAngles;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if( target == null) {
            /* Unlockon Mode */
            // calculate target vertical and horizontal rotation
            cameraTargetDir.x = Mathf.Clamp( Mathf.Lerp( cameraTargetDir.x, cameraTargetDir.x + ic.cameraVerticalSignal, 0.2f ), -50, 30 );
            cameraTargetDir.y = Mathf.Lerp( cameraTargetDir.y, cameraTargetDir.y + ic.cameraHorizontalSignal, 0.2f );
            transform.rotation = Quaternion.Euler( cameraTargetDir );

            // update the camera rotation
            cameraMain.rotation = transform.rotation;
        } else {

            /* Lockon Mode */
            cameraTargetDir = target.transform.position - player.position;
            transform.forward = cameraTargetDir;

            // update the camera rotation
            cameraMain.LookAt( target.transform.position );
        }
        ResetRotationSignal( );


        // calculate offset
        transform.position = player.position - transform.rotation * cameraOffset + Vector3.up * cameraHeight;

        // update the camera position smoothly
        UpdateCameraPos( );
    }

    void UpdateCameraPos() {
        distance = Vector3.Distance( cameraMain.position, this.transform.position );
        dampRate = 1 - Mathf.Min(  distance / maxDistance, 0.8f);
        cameraMain.transform.position = Vector3.SmoothDamp( cameraMain.transform.position, transform.position, ref cameraDampVelocity, dampRate * 0.5f );
    }

    private void ResetRotationSignal() {
        ic.cameraHorizontalSignal = 0;
        ic.cameraVerticalSignal = 0;
    }
}