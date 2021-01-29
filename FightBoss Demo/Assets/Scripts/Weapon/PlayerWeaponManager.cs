using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : WeaponManager
{
    PlayerData playerData;

    private void Start() {
        weaponColliders = GetComponents<BoxCollider>( );
        playerData = GetComponentInParent<PlayerData>( );
    }

    private void OnTriggerEnter(Collider target) {
        Vector3 location = this.transform.position;
        Vector3 hitPoint = target.ClosestPointOnBounds( location );

        PoolManager.GetObject( "skillAttack" ,hitPoint);
        var damageInterface = target.gameObject.GetComponent<IDamageEnable>( );
        target.gameObject.GetComponent<HitStopEnable>( ).HitStop( );
        damageInterface.DoDamage( playerData.getATK , hitPoint );
    }

    private void LateUpdate() {
        if (weaponEnable != lastWeaponEnable) {
            foreach (var collider in weaponColliders) {
                collider.enabled = weaponEnable;
            }
        }
        lastWeaponEnable = weaponEnable;
    }
}
