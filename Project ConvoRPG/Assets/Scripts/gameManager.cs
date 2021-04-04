using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    [Header("Player Variables")]
    public int lives = 3;
    public float stress = 0.5f;

    [Header("Enemy Prefabs")]
    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        //if there is an instance of the Game Manager, then delete the current game object (prevents gameManager being instanced multiple times)
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            stress = instance.stress;
            lives = instance.lives;
            GameObject.Destroy(instance.gameObject);
            instance = this;
        }
        Object.DontDestroyOnLoad(gameObject);
    }

    //public function that sets which enemy prefab 
    public void SetEnemyPrefab(GameObject input) 
    {
        enemyPrefab = input;
    }

    public void battleTrigger() 
    {
        SceneManager.LoadSceneAsync("Battle Screen");
    }
}
