using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NoiseTests {

    [Test]
    public void GeneratesNoiseMap() {
        var mapGenerator = new MapGenerator();

        int mapWidth = mapGenerator.mapWidth;
        int mapHeight = mapGenerator.mapHeight;
        int seed = mapGenerator.seed;
        float scale = mapGenerator.noiseScale;
        int octaves = mapGenerator.octaves;
        float persistance = mapGenerator.persistance;
        float lacunarity = mapGenerator.lacunarity;
        Vector2 offset = mapGenerator.offset;

        float[,] generateNoiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, scale, octaves, persistance, lacunarity, offset);

        float[,] noiseMap = new float[mapWidth, mapHeight];

        //prng - pseudo random number generator
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x; //higher values keep returning the same value
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;


        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        Assert.AreEqual(noiseMap, generateNoiseMap);
    }


}
