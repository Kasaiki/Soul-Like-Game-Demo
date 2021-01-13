using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTargetDetector : MonoBehaviour
{

    public int currTargetIndex = 0;

    public ArrayList targetsList = new ArrayList( );

    private void OnTriggerEnter(Collider obj) {
        targetsList.Add( obj.gameObject.transform );
    }

    private void OnTriggerExit(Collider obj) {
        targetsList.Remove( (Object)obj.gameObject.transform );
    }

    public Transform GetTargetTrans() {
        if (targetsList.Count > 0)
            return (Transform)targetsList[currTargetIndex];
        return null;
    }


}
