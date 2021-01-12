using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public float ATK;

    private BoxCollider[] weaponColliders;

    private bool weaponEnable = false;
    private bool lastWeaponEnable = false;

    public bool WeaponEnable {
        get { return weaponEnable; }
        set {
            weaponEnable = value;
        }
    }

    private void Awake() {
        weaponColliders = GetComponents<BoxCollider>( );
    }

    private void OnTriggerEnter(Collider target) {
        Debug.Log( target );

        Vector3 location = this.transform.position;
        Vector3 hitPoint = target.ClosestPointOnBounds( location );

        //Debug.Log( hitPoint );
    }

    private void LateUpdate() {
        if(weaponEnable != lastWeaponEnable) {
            foreach (var collider in weaponColliders) {
                collider.enabled = weaponEnable;
            }
        }
        lastWeaponEnable = weaponEnable;
    }
}
