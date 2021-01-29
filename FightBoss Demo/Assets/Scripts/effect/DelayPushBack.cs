using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayPushBack : MonoBehaviour
{
    public float delayTime;

    private void OnEnable() {
        Invoke( "PushThisObject", delayTime );
    }

    private void PushThisObject() {
        PoolManager.PushObject( gameObject.name, gameObject );
    }
}
