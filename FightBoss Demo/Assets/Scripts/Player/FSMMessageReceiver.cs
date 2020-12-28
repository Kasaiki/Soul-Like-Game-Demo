using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMMessageReceiver : MonoBehaviour
{
    private PlayerController pc;
    private Animator anim;

    private int temp;

    private void Start() {
        pc = GetComponentInParent<PlayerController>( );
        anim = GetComponent<Animator>( );
    }

    public void OnUpdateDodge() {
        pc.rotationSpeed = pc.rotationSpeed /2;
    }

    public void OnExitDodge() {
        pc.rotationSpeed = 0.1f;
    }

    /* Coroutine Area */
    IEnumerable freezeRotationSpeed() {
        yield return new WaitWhile( ()=>temp > 0 );
    }

}
