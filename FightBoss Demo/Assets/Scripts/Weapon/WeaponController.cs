using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器コリジョン
/// </summary>
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
        // play sound
        weaponManager.WeaponEnable = true;
    }

    // Animation Eventから呼び出します
    void EndAttackEvent() {
        // stop play sound
        weaponManager.WeaponEnable = false;
    }
}
