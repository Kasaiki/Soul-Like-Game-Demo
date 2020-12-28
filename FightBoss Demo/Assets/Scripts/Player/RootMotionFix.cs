using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionFix : MonoBehaviour
{
    public Animator playerAnim;
    public GameObject playerHandle;

    private Vector3 rootMotionDeltaPos;

    // Start is called before the first frame update
    void Start() {
        playerAnim = GetComponent<Animator>( );
    }

    private void FixedUpdate() {
        playerHandle.transform.position += rootMotionDeltaPos;
        rootMotionDeltaPos = Vector3.zero;
    }

    // Update is called once per frame
    void OnAnimatorMove() {
        rootMotionDeltaPos += playerAnim.deltaPosition;
    }
}
