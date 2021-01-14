using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopEnable : MonoBehaviour
{
    public float hitStopTime;

    Coroutine HitStop_Coroutine;
    float _timeScale;

    private void Start() {
        _timeScale = Time.timeScale;
    }

    public void HitStop() {
        if (HitStop_Coroutine != null) {
            StopCoroutine( HitStop_Coroutine );
            Time.timeScale = _timeScale;
        }
        HitStop_Coroutine = StartCoroutine( HitStopWait( ) );
    }

    IEnumerator HitStopWait() {
        print( "Hitstop" );
        Time.timeScale = Time.timeScale / 10f;
        yield return new WaitForSecondsRealtime( hitStopTime );
        Time.timeScale = _timeScale;
    }
}
