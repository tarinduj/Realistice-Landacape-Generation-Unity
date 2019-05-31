using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MapGeneratorTests {

    [Test]
    public void GeneratesNoiseMap() {

        var mapGenerator = new MapGenerator();

        int mapWidth = mapGenerator.mapWidth;
        int mapHeight = mapGenerator.mapHeight;
        int seed = mapGenerator.seed;
        float noiseScale = mapGenerator.noiseScale;
        int octaves = mapGenerator.octaves;
        float persistance = mapGenerator.persistance;
        float lacunarity = mapGenerator.lacunarity;
        Vector2 offset = mapGenerator.offset;

        var noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        var mapData = mapGenerator.GenerateMapData(Vector2.zero);
        var generatedNoiseMap = mapData.heightMap;

        Assert.AreEqual(noiseMap, generatedNoiseMap);

        GameObject.Destroy(mapGenerator);
    }

    [Test]
    public void GeneratesColorMap()
    {

        var mapGenerator = new MapGenerator();

        int mapWidth = mapGenerator.mapWidth;
        int mapHeight = mapGenerator.mapHeight;
        int seed = mapGenerator.seed;
        float noiseScale = mapGenerator.noiseScale;
        int octaves = mapGenerator.octaves;
        float persistance = mapGenerator.persistance;
        float lacunarity = mapGenerator.lacunarity;
        Vector2 offset = mapGenerator.offset;
        TerrainType[] regions = mapGenerator.regions;

        var noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapHeight * mapWidth];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight < regions[i].height)
                    {
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        var mapData = mapGenerator.GenerateMapData(Vector2.zero);
        var generatedColorMap = mapData.colorMap;

        Assert.AreEqual(colorMap, generatedColorMap);

        GameObject.Destroy(mapGenerator);
    }

    [Test]
    public void GeneratesMeshData()
    {
        var mapGenerator = new MapGenerator();

        int mapWidth = mapGenerator.mapWidth;
        int mapHeight = mapGenerator.mapHeight;
        int seed = mapGenerator.seed;
        float noiseScale = mapGenerator.noiseScale;
        int octaves = mapGenerator.octaves;
        float persistance = mapGenerator.persistance;
        float lacunarity = mapGenerator.lacunarity;
        Vector2 offset = mapGenerator.offset;
        float meshHeightMultiplier = mapGenerator.meshHeightMultiplier;
        AnimationCurve meshHeightCurve = mapGenerator.meshHeightCurve;

        var heightMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        var meshData = MeshGenerator.GenerateTerrainMesh(heightMap, meshHeightMultiplier, meshHeightCurve);

        var generatedMeshData = mapGenerator.GenerateMeshData();

        Assert.AreEqual(meshData.vertices, generatedMeshData.vertices);
        Assert.AreEqual(meshData.triangles, generatedMeshData.triangles);
        Assert.AreEqual(meshData.uvs, generatedMeshData.uvs);

        GameObject.Destroy(mapGenerator);
    }
}
