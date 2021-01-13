using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器のコリジョン判定を処理するクラス
/// </summary>
public class WeaponManager : MonoBehaviour
{
    PlayerData pd;

    [SerializeField]
    private BoxCollider[] weaponColliders;

    [SerializeField]
    private string hitMessages;

    [SerializeField]
    private bool weaponEnable = false;
    private bool lastWeaponEnable = false;

    public bool WeaponEnable {
        get { return weaponEnable; }
        set { weaponEnable = value; }
    }

    private void Awake() {
        weaponColliders = GetComponents<BoxCollider>( );
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="target"></param>
    private void OnTriggerEnter(Collider target) {
        target.gameObject.SendMessage( hitMessages );

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
