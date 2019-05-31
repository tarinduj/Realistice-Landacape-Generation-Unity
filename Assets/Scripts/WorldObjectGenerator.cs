using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectGenerator : MonoBehaviour {

    //Arrays to hold objects
    public VegetationRegions[] vegetationRegions;

    private int stoneChanceAmount = 5; //1 in 5 chance

    //Spawning grid values/  variables to control world size
    private Vector3 currentPosition;
    private Vector3 worldObjectStartPosition;

    int mapWidth;  // width col x
    int mapHeight; //hright row z

    float colIncrementAmount;
    float rowIncrementAmount;

    //how far the randomized locations should be
    float colRandomAmount;
    float rowRandomAmount;

    //values to control spawn loop through grid
    public int cols;
    public int rows;

    public int repeatPasses;
    int currentPass;

    public float sphereRadius;

    //layer masks
    public LayerMask terrainLayer;
    public LayerMask worldObjectLayer;

    static MapGenerator mapGenerator;
    float[,] heightMap;

    public GameObject environment;

    // Use this for initialization
    public void GenerateWorldObjects(MapData mapData)
    {
        mapGenerator = FindObjectOfType<MapGenerator>();

        heightMap = mapData.heightMap;

        //StartCoroutine("SpawnWorld");
        SpawnWorld(heightMap);
    }

    public void SpawnWorld(float[,] heightMap)
    {
        mapWidth = mapGenerator.mapWidth;
        mapHeight = mapGenerator.mapHeight;

        colIncrementAmount = (mapWidth * 1f)  / cols;
        rowIncrementAmount = (mapHeight * 1f) / rows;

        colRandomAmount = colIncrementAmount / 2f;
        rowRandomAmount = rowIncrementAmount / 2f;
        
        float startPositionX = - (mapWidth /2f) + (colIncrementAmount / 2f);
        float startPositionY = 0;
        float startPositionZ = - (mapHeight / 2f) + (rowIncrementAmount / 2f);

        worldObjectStartPosition = new Vector3(startPositionX, startPositionY, startPositionZ );

        for (int rp = 0; rp <= repeatPasses; rp++)
        {
            currentPosition = worldObjectStartPosition;

            for (int x = 1; x <= cols; x++)
            {
                for (int z = 1; z <= rows; z++)
                {
                    int intX = (int)currentPosition.x - (int)startPositionX;
                    int intZ = (int)currentPosition.z - (int)startPositionZ;

                    /*
                    Debug.Log("currentPosition X: " + currentPosition.x);
                    Debug.Log("intcurrentPosition X: " + (int)currentPosition.x);
                    Debug.Log("currentPosition Z: " + currentPosition.z);
                    Debug.Log("intcurrentPosition Z: " + (int)currentPosition.z);
                    Debug.Log("intX: " + intZ);
                    Debug.Log("height: " + heightMap[intX, intZ]);
                    */

                    for (int i = 0; i < vegetationRegions.Length; i++)
                    {
                        if (heightMap[intX, intZ] > 0.0)    // value here can create separate grooves of trees
                        {
                            int spawnChance = Random.Range(1, stoneChanceAmount + 1);

                            if (spawnChance == 1)
                            {
                                GameObject[] stones = vegetationRegions[i].stones;
                                if (stones.Length > 0)
                                {
                                    GameObject newSpawn = stones[Random.Range(0, stones.Length)];
                                    SpawnHere(currentPosition, newSpawn, sphereRadius, i);
                                }
                            }
                            else
                            {
                                GameObject[] trees = vegetationRegions[i].trees;
                                if (trees.Length > 0)
                                {
                                    GameObject newSpawn = trees[Random.Range(0, trees.Length)];
                                    SpawnHere(currentPosition, newSpawn, sphereRadius, i);
                                }
                            }
                        }
                    }

                    currentPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + rowIncrementAmount);
                }

                currentPosition = new Vector3(currentPosition.x + colIncrementAmount, currentPosition.y, worldObjectStartPosition.z);
            }
        }

        WorldGenerationDone();
    }

    void SpawnHere(Vector3 newSpawnPosition, GameObject objectToSpawn, float radiusOfSphere, int region)
    {
        float randPositionX = newSpawnPosition.x + Random.Range(-colRandomAmount, colRandomAmount + 1);
        float randPositionY = newSpawnPosition.y;
        float randPositionZ = newSpawnPosition.z + Random.Range(-rowRandomAmount, rowRandomAmount + 1);

        Vector3 randPosition = new Vector3(randPositionX, randPositionY, randPositionZ);

        Vector3 rayPosition = new Vector3(randPosition.x, randPosition.y + mapGenerator.meshHeightMultiplier, randPosition.z);

        RaycastHit hit;

        if (Physics.Raycast(rayPosition, -Vector3.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.point.y > vegetationRegions[region].lowerBound & hit.point.y < vegetationRegions[region].upperBound)///////////////////////////////////////////////
            {
                randPosition = new Vector3(randPosition.x, hit.point.y, randPosition.z);

                Collider[] objectsHit = Physics.OverlapSphere(randPosition, radiusOfSphere, worldObjectLayer);

                if (objectsHit.Length == 0)
                {
                    GameObject worldObject = (GameObject)Instantiate(objectToSpawn, randPosition, Quaternion.identity);

                    worldObject.transform.position = new Vector3(worldObject.transform.position.x, worldObject.transform.position.y + (worldObject.GetComponent<Renderer>().bounds.extents.y * 0.7f), worldObject.transform.position.z);

                    worldObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);

                    worldObject.transform.parent = environment.transform;

                }
            }

        }
    }

    void WorldGenerationDone()
    {
        print("World has been generated!!");
    }
}

[System.Serializable]
public struct VegetationRegions
{
    public string name;
    public float upperBound;
    public float lowerBound;
    public GameObject[] trees;
    public GameObject[] stones;
}