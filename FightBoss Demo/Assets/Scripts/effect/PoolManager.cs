using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    private static Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>( );

    public static GameObject GetObject(string name , Vector3 pos , Quaternion direction) {
        GameObject obj = null;

        if(pool.ContainsKey(name) && pool[name].Count > 0) {
            obj = pool[name][0];
            obj.transform.position = pos;
            obj.transform.rotation = direction;
            pool[name].RemoveAt( 0 );
        } else {
            obj = GameObject.Instantiate( Resources.Load<GameObject>( name ) , pos , direction);
            obj.name = name;
        }
        obj.SetActive( true );
        return obj;
    }

    public static GameObject GetObject(string name, Transform parent) {
        GameObject obj = null;

        if (pool.ContainsKey( name ) && pool[name].Count > 0) {
            obj = pool[name][0];
            obj.transform.SetParent( parent );
            pool[name].RemoveAt( 0 );
        } else {
            obj = GameObject.Instantiate( Resources.Load<GameObject>( name ), parent);
            obj.name = name;
        }
        obj.SetActive( true );
        return obj;
    }

    public static void PushObject(string name, GameObject obj) {
        if(pool.ContainsKey( name )) {
            pool[name].Add( obj );
        } else {
            pool.Add( name, new List<GameObject>( ) { obj } );
        }
        obj.SetActive( false );
    }
}
