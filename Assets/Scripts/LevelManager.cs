using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    private float offset = 0.5f;
    public int columns = 10;
    public int rows = 10;

    private Count emptyCount;
    public Count emptyPercentage = new Count(10, 10);

    public GameObject background;
    public GameObject[] outerWallTiles;
    public GameObject[] rocks;
    public GameObject[] gems;

    private Transform levelHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    private List<Vector3> presetRocks = new List<Vector3>();
    private List<Vector3> presetGems = new List<Vector3>();

    // Update is called once per frame
    void Awake()
    {

    }

    public void SetupScene(int level)
    {
        InitializeList();
        LevelSetup(level);
        LayoutGems();
        LayoutRocks();
        RemoveRandomPositions(emptyCount);
    }

    void InitializeList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns + 1; x++)
        {
            for (int y = 1; y < rows + 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void LevelSetup(int level)
    {
        int tileAmount = columns * rows;
        int percentile = Mathf.RoundToInt(tileAmount / 100);
        float levelFactor = Mathf.Log(level + 10);

        emptyCount = new Count(emptyPercentage.minimum * percentile, emptyPercentage.maximum * percentile);

        levelHolder = new GameObject("Level").transform;

        for (int x = 0; x < columns + 2; x++)
        {
            for (int y = 0; y < rows + 2; y++)
            {
                GameObject toInstantiate = background;
                if (x == 0 || x == columns + 1 || y == 0 || y == rows + 1)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(levelHolder);
            }
        }
    }

    public void SetupGems(List<Vector3> presets)
    {
        presetGems = presets;
    }

    void LayoutGems()
    {
        foreach (Vector3 gem in presetGems)
        {
            LayoutObject(gems, gem);
        }
    }

    public void SetupRocks(List<Vector3> presets)
    {
        presetRocks = presets;
    }

    void LayoutRocks()
    {
        foreach (Vector3 rock in presetRocks)
        {
            LayoutObject(rocks, rock);
        }
    }

    void RemoveRandomPositions(Count removeCount)
    {
        int removables = GetRandom(removeCount);
        for (int i = 0; i < removables; i++)
        {
            gridPositions.RemoveAt(Random.Range(0, gridPositions.Count));
        }
    }

    void LayoutObject(GameObject[] tileArray, Vector3 position)
    {
        GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
        position.x += offset;
        position.y += offset;
        Instantiate(tileChoice, position, Quaternion.identity);
    }

    int GetRandom(Count limits)
    {
        return Random.Range(limits.minimum, limits.maximum + 1);
    }


}