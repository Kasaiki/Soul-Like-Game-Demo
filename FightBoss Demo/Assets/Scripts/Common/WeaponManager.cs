using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器のコリジョン判定を処理するクラス
/// </summary>
abstract public class WeaponManager : MonoBehaviour
{
    #region パラメータ
    [SerializeField]
    protected BoxCollider[] weaponColliders;
    [SerializeField]
    public bool weaponEnable = false;
    public bool lastWeaponEnable = false;

    #endregion
}
