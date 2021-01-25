using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class ActorAttribute : MonoBehaviour, IDamageEnable
{
    #region 属性
    [SerializeField]
    protected float _HP;
    [SerializeField]
    protected float MaxHP;
    [SerializeField]
    protected float _STA;
    [SerializeField]
    protected float MaxSTA; // stamina
    [SerializeField]
    protected float ATK;
    [SerializeField]
    protected float DEF;
    #endregion

    #region 属性を取得するためのメソッド
    public float HP {
        get { return _HP; }
        set { _HP = value; }
    }

    public float STA {
        get { return _STA; }
        set { _STA = value; }
    }

    public float getMaxSTA {
        get { return MaxSTA; }
    }

    public float getMaxHP {
        get { return MaxHP; }
    }

    public float getATK {
        get { return ATK; }
    }

    public float getDEF {
        get { return DEF; }
    }
    #endregion

    #region UI関連
    [SerializeField]
    protected Slider HPBar;
    [SerializeField]
    protected Slider STABar;

    /// <summary>
    /// HPバーとSTAバーの最大値と最小値を設定します
    /// </summary>
    public void InitUI() {
        HPBar.minValue = 0;
        HPBar.maxValue = MaxHP;
        HP = MaxHP;

        STABar.minValue = 0;
        STABar.maxValue = MaxSTA;
        STA = MaxSTA;

        HPBar.value = HP;
        HPBar.value = STA;
        BarUpdate( );
    }

    /// <summary>
    /// HPとSTAのバーを更新します
    /// </summary>
    protected void BarUpdate() {
        HPBar.value = HP;
        STABar.value = STA;
    }
    #endregion

    /// <summary>
    /// ヒット時の処理
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="hitPosition"></param>
    public abstract void DoDamage(float damage, Vector3 hitPosition);

}
