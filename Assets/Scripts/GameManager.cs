using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public LevelManager levelScript;

    private int level = 1;
    private float turnDelay = 0.1f;
    private List<FallingObject> fallingObjects;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        fallingObjects = new List<FallingObject>();
        levelScript = GetComponent<LevelManager>();
        InitGame();
    }

    void InitGame()
    {
        fallingObjects.Clear();

        List<Vector3> presetGems = new List<Vector3>();
        List<Vector3> presetRocks = new List<Vector3>();

        presetRocks.Add(new Vector3(1, 1, 1f));
        presetRocks.Add(new Vector3(2, 2, 1f));
        presetRocks.Add(new Vector3(9, 9, 1f));

        levelScript.SetupGems(presetGems);
        levelScript.SetupRocks(presetRocks);
        levelScript.SetupScene(level);

    }

    // Update is called once per frame
    void Update ()
    {
        StartCoroutine(MoveFallObjects());
    }

    IEnumerator MoveFallObjects()
    {
        yield return new WaitForSeconds(turnDelay);

        if (fallingObjects.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < fallingObjects.Count; i++)
        {
            fallingObjects[i].FallSetup();
        }

        for (int i = 0; i < fallingObjects.Count; i++)
        {
            fallingObjects[i].Move();
        }
    }
}
