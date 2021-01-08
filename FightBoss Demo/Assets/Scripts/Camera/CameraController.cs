using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // component
    private GameObject player;
    private Transform playerTrans;
    private Transform cameraMain;
    private InputController ic;
    private LockOnTargetDetector detector;
    public Transform target = null;

    // Use the ray to detect whether there is an obstacle between the person and the camera, if there is an obstacle then need to correct the camera position
    RaycastHit fixedCameraPos;
    int rayLayerNum;

    // paramaters
    private float distance;
    private float maxDistance = 3f;
    private float dampRate;
    private float cameraHeight = 1.7f;
    private Vector3 cameraOffset = new Vector3( 0, 0, 5f );
    private Vector3 cameraTargetDir;
    private Vector3 cameraDampVelocity;


    private void Start() {
        player = GameObject.FindGameObjectWithTag( "Player" );
        playerTrans = player.GetComponent<Transform>( );
        ic = player.GetComponent<InputController>( );
        detector = player.GetComponentInChildren<LockOnTargetDetector>( );
        cameraMain = Camera.main.transform;
        rayLayerNum = LayerMask.GetMask( "Ground" );
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (ic.cameraLockOn && detector.targetsList.Count > 0) {
            target = detector.GetTargetTrans( );
            /* Lockon Mode */
            cameraTargetDir = Vector3.Normalize(target.position - playerTrans.position); // get a direction vector
            cameraTargetDir.y = 0; // Clear the Y-axis value to prevent the camera facing UP direction when the distance is too close
            transform.forward = Vector3.Slerp( transform.forward, cameraTargetDir, 0.2f );

            // update the camera rotation
            cameraMain.LookAt(  target.transform.position );
        } else {
            /* Unlock Mode */
            cameraTargetDir =  cameraMain.rotation.eulerAngles; // Update the current camera position to prevent misplaced camera positions when calling back from LockOn mode
            cameraTargetDir.x = NormalizeAngle( cameraTargetDir.x ); // Change x-axis rotation value range from [0,360] to [-180, 180]
            cameraTargetDir.x = Mathf.Clamp( Mathf.Lerp( cameraTargetDir.x ,  cameraTargetDir.x + ic.cameraVerticalSignal , 0.2f ), -30, 50 ); // Calculate X-axis rotation
            cameraTargetDir.y = Mathf.Lerp( cameraTargetDir.y, cameraTargetDir.y + ic.cameraHorizontalSignal, 0.2f ); // Calculate Y-axis rotation
            cameraTargetDir.z = 0;
            transform.rotation = Quaternion.Euler( cameraTargetDir ); 

            // update the camera rotation
            cameraMain.rotation = transform.rotation;
        }
        ResetRotationSignal( );

        // fix
        FixCameraPos( );

        // calculate offset
        transform.position = playerTrans.position - transform.rotation * cameraOffset + Vector3.up * cameraHeight;

        // update the camera position smoothly
        UpdateCameraPos( );
    }

    void UpdateCameraPos() {
        distance = Vector3.Distance( cameraMain.position, this.transform.position );
        dampRate = 1 - Mathf.Min(  distance / maxDistance, 0.9f);
        // using damp
        cameraMain.position = Vector3.SmoothDamp( cameraMain.position, transform.position, ref cameraDampVelocity, dampRate * 0.2f );
    }

    private void ResetRotationSignal() {
        ic.cameraHorizontalSignal = 0;
        ic.cameraVerticalSignal = 0;
    }

    private float NormalizeAngle( float value) {
        float angle = value - 180;
        if (angle > 0)
            return angle - 180;
        return angle + 180;
    }

    private void FixCameraPos() {
        if(Physics.Raycast( playerTrans.position + playerTrans.up * 1.5f , -cameraMain.forward , out fixedCameraPos, 5f , rayLayerNum )) {
            Debug.DrawLine( playerTrans.position + playerTrans.up * 1.5f , fixedCameraPos.point , Color.blue );
            transform.position = fixedCameraPos.point ;
            cameraOffset.z = fixedCameraPos.distance - 0.2f;
        }
    }
}