using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SpawnOnDestroy : MonoBehaviour {
    public GameObject[] toSpawn;
    [SerializeField] bool addMyRotationInSpawn = false;
    void OnDestroy () {
        if (SceneManager.GetActiveScene ().isLoaded == true) //prevents them from spawning while loading a scene
        {
            bool help = false;
#if (UNITY_EDITOR) 
            if (EditorApplication.isPlayingOrWillChangePlaymode == false && EditorApplication.isPlaying) // prevents spawning when exiting playmode
            {
                help = true;
            }
#endif
            if (help == true) // prevents spawning when exiting playmode
            {
                //dont question it lol
            } else {
                for (int i = 0; i < toSpawn.Length; i++) {
                    GameObject g = Instantiate (toSpawn[i], transform.position, transform.rotation);
                    if (addMyRotationInSpawn == true) {
                        g.transform.rotation *= toSpawn[i].transform.rotation;
                    }
                }
            }
        }
    }
}