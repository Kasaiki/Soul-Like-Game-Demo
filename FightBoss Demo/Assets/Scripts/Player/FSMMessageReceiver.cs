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

}
