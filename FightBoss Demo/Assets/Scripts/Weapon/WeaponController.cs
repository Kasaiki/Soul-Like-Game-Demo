using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponManager weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        weaponManager = GetComponentInChildren<WeaponManager>( );
    }

    // Animation Eventから呼び出します
    void StartAttackEvent() {
        print( "Start Attack");
        // play sound
        weaponManager.WeaponEnable = true;
    }

    // Animation Eventから呼び出します
    void EndAttackEvent() {
        print( "End Attack" );
        // stop play sound
        weaponManager.WeaponEnable = false;
    }
}
