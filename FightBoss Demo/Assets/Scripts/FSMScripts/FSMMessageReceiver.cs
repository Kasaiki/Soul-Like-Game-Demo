using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMMessageReceiver : MonoBehaviour
{
    private PlayerController pc;
    private PlayerData pd;
    private Animator anim;

    private int temp;

    private void Start() {
        pc = GetComponentInParent<PlayerController>( );
        pd = GetComponentInParent<PlayerData>( );
        anim = GetComponent<Animator>( );
    }

    // attached on DashAttack / OnEnterMove
    void OnExitDashAttack() {
        anim.SetFloat( "ForwardSpeed", 0f );
    }

    void OnEnterHit() {
        anim.ResetTrigger( "Hit" );
    }

    void OnEnterDash() {
        pd.EnterDash( );
    }
}
