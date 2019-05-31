using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{

    public Material mapMaterial;

    static MapGenerator mapGenerator;
    int mapWidth;
    int mapHeight;

    MapData mapData;
    MeshData meshData;

    static WorldObjectGenerator worldObjectGenerator;

    void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        worldObjectGenerator = FindObjectOfType<WorldObjectGenerator>();

        mapWidth = mapGenerator.mapWidth;
        mapHeight = mapGenerator.mapHeight;
        mapData = mapGenerator.GenerateMapData(Vector2.zero);
        meshData = mapGenerator.GenerateMeshData();

        Debug.Log("Color" + mapData.colorMap[0]);

        TerrainChunk terrainChunk = new TerrainChunk(mapWidth, mapHeight, transform, mapMaterial, meshData, mapData);

        worldObjectGenerator.GenerateWorldObjects(mapData);
    }

    public class TerrainChunk
    {

        GameObject plane;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        public TerrainChunk(int mapWidth, int mapHeight, Transform parent, Material material, MeshData meshData, MapData mapData)
        {
            plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.name = "Terrain";
            plane.tag = "Terrain";
            plane.layer = 10;

            meshRenderer = plane.GetComponent<MeshRenderer>();
            meshFilter = plane.GetComponent<MeshFilter>();
            meshRenderer.material = material;

            Mesh mesh = meshData.CreateMesh();
            meshFilter.mesh = mesh;

            Texture2D texture = TextureGenerator.TextureFromColorMap(mapData.colorMap, mapWidth, mapHeight);
            meshRenderer.material.mainTexture = texture;

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            plane.AddComponent<MeshCollider>();

            plane.transform.parent = parent;
        }

    }

}