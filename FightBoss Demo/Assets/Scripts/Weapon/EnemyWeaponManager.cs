using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponManager : WeaponManager
{
    EnemyData enemyData;

    private void Start() {
        weaponColliders = GetComponents<BoxCollider>( );
        enemyData = GetComponentInParent<EnemyData>( );
    }

    private void OnTriggerEnter(Collider target) {
        Vector3 location = this.transform.position;
        Vector3 hitPoint = target.ClosestPointOnBounds( location );

        var damageInterface = target.gameObject.GetComponent<IDamageEnable>( );
        target.gameObject.GetComponent<HitStopEnable>( ).HitStop( );
        damageInterface.DoDamage( enemyData.getATK , transform.position );

        //Debug.Log( hitPoint );
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
