using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTargetDetector : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    private void OnTriggerEnter(Collider obj) {
        if (obj.gameObject.CompareTag( "Enemy" )) {
            target = obj.gameObject;
        }
    }

    private void OnTriggerExit(Collider obj) {
        if(target == obj.gameObject) {
            target = null;
        }
    }

    public GameObject GetTarget() {
        return this.target;
    }
}
