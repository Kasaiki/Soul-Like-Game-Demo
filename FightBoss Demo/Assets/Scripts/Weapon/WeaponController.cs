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

    // Animation Event
    void StartAttackEvent() {
        print( "Start Attack");
        // play sound
        weaponManager.WeaponEnable = true;
    }

    void EndAttackEvent() {
        print( "End Attack" );
        // stop play sound
        weaponManager.WeaponEnable = false;
    }
}
