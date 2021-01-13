using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region コンポーネントと変数の宣言
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
    #endregion

    #region　初期化
    private void Start() {
        player = GameObject.FindGameObjectWithTag( "Player" );
        playerTrans = player.GetComponent<Transform>( );
        ic = player.GetComponent<InputController>( );
        detector = player.GetComponentInChildren<LockOnTargetDetector>( );
        cameraMain = Camera.main.transform;
        rayLayerNum = LayerMask.GetMask( "Ground" );
    }
    #endregion

    #region　Update
    // Update is called once per frame
    void FixedUpdate() {
        if (ic.cameraLockOn && detector.targetsList.Count > 0) {
            target = detector.GetTargetTrans( );
            // ロックオンモードでのカメラ目標方向の計算
            cameraTargetDir = Vector3.Normalize(target.position - playerTrans.position); // 方向ベクトルを算出します
            cameraTargetDir.y = 0; // Y座標をクリアします
            transform.forward = Vector3.Slerp( transform.forward, cameraTargetDir, 0.2f );

            // カメラに算出した結果を反応します
            cameraMain.LookAt(  target.transform.position );
        } else {
            //　非ロックオンモードでのカメラ回転量の計算
            cameraTargetDir =  cameraMain.rotation.eulerAngles; // ロックオンモードから切り替えてきたときの回転量をクリアします
            cameraTargetDir.x = NormalizeAngle( cameraTargetDir.x ); // 垂直方向での[0,360]の度数範囲を[-180, 180]に正規化します
            cameraTargetDir.x = Mathf.Clamp( Mathf.Lerp( cameraTargetDir.x ,  cameraTargetDir.x + ic.cameraVerticalSignal , 0.2f ), -30, 50 ); // 垂直方向の計算
            cameraTargetDir.y = Mathf.Lerp( cameraTargetDir.y, cameraTargetDir.y + ic.cameraHorizontalSignal, 0.2f ); // 水平方向の計算
            cameraTargetDir.z = 0;　//　Z方向をクリアします
            transform.rotation = Quaternion.Euler( cameraTargetDir ); 

            // カメラに回転量を反応します
            cameraMain.rotation = transform.rotation;
        }

        // 反応された回転をInputから消します。
        ResetRotationSignal( );

        // カメラ位置補正
        FixCameraPos( );

        // カメラとキャラクター間のオフセットを再計算
        transform.position = playerTrans.position - transform.rotation * cameraOffset + Vector3.up * cameraHeight;

        // 計算された位置をカメラに反応します
        UpdateCameraPos( );
    }
    #endregion

    #region ファンクションの実現
    void UpdateCameraPos() {
        distance = Vector3.Distance( cameraMain.position, this.transform.position );
        dampRate = 1 - Mathf.Min(  distance / maxDistance, 0.9f);
        // using damp
        cameraMain.position = Vector3.SmoothDamp( cameraMain.position, transform.position, ref cameraDampVelocity, dampRate * 0.3f );
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

    /// <summary>
    /// Rayを用いて、カメラを地面に落ち込めないように補正する関数になります。
    /// </summary>
    private void FixCameraPos() {
        if(Physics.Raycast( playerTrans.position + playerTrans.up * 1.5f , -cameraMain.forward , out fixedCameraPos, 5f , rayLayerNum )) {
            Debug.DrawLine( playerTrans.position + playerTrans.up * 1.5f , fixedCameraPos.point , Color.blue );
            transform.position = fixedCameraPos.point ;
            cameraOffset.z = fixedCameraPos.distance - 0.2f;
        }
    }
    #endregion
}