using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager gm;
    public static GameManager getGM {
        get { return gm; }
    }

    private void Awake() {
        gm = this;
    }

    public void AddHP(int count) {
        // do something
    }

    public void GameOver() {
        // do something
        SceneManager.LoadScene( "Title" );
    }
}
