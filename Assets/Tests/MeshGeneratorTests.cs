using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MeshGeneratorTests {

    [Test]
    public void GeneratesVerticesOfMesh()
    {
        MapGenerator mapGenerator = new MapGenerator();

        float heightMultiplier = mapGenerator.meshHeightMultiplier;
        AnimationCurve heightCurve = mapGenerator.meshHeightCurve;
        float[,] heightMap = mapGenerator.GenerateMapData(Vector2.zero).heightMap;

        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        MeshData generatedMeshData = MeshGenerator.GenerateTerrainMesh(heightMap, heightMultiplier, heightCurve);

        Assert.AreEqual(meshData.vertices, generatedMeshData.vertices);

        GameObject.Destroy(mapGenerator);
    }

    [Test]
    public void GeneratesTrianglesOfMesh()
    {
        MapGenerator mapGenerator = new MapGenerator();

        float heightMultiplier = mapGenerator.meshHeightMultiplier;
        AnimationCurve heightCurve = mapGenerator.meshHeightCurve;
        float[,] heightMap = mapGenerator.GenerateMapData(Vector2.zero).heightMap;

        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        MeshData generatedMeshData = MeshGenerator.GenerateTerrainMesh(heightMap, heightMultiplier, heightCurve);

        Assert.AreEqual(meshData.triangles, generatedMeshData.triangles);

        GameObject.Destroy(mapGenerator);
    }

    [Test]
    public void GeneratesUVsOfMesh()
    {
        MapGenerator mapGenerator = new MapGenerator();

        float heightMultiplier = mapGenerator.meshHeightMultiplier;
        AnimationCurve heightCurve = mapGenerator.meshHeightCurve;
        float[,] heightMap = mapGenerator.GenerateMapData(Vector2.zero).heightMap;

        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        MeshData generatedMeshData = MeshGenerator.GenerateTerrainMesh(heightMap, heightMultiplier, heightCurve);

        Assert.AreEqual(meshData.uvs, generatedMeshData.uvs);

        GameObject.Destroy(mapGenerator);
    }
}
