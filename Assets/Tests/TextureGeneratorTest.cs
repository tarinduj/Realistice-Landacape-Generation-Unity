using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TextureGeneratorTest {

    [Test]
    public void GeneratesTextureFromColorMap()
    {
        Color newColor = new Color(0.3f, 0.4f, 0.6f, 0.3f);
        Color[] colorMap = new Color[4];
        colorMap[0] = newColor; colorMap[1] = newColor; colorMap[2] = newColor; colorMap[3] = newColor;
        int width = 2;
        int height = 2;

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();

        Texture2D generatedTexture = TextureGenerator.TextureFromColorMap(colorMap, width, height);
        generatedTexture = texture;

        Assert.AreEqual(texture, generatedTexture);
    }

    [Test]
    public void GeneratesTextureFromHeightMap()
    {
        float[,] heightMap = new float[,] { { 1, 2 }, { 3, 4 } };

        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        Texture2D texture = TextureGenerator.TextureFromColorMap(colorMap, width, height);

        Texture2D generatedTexture = TextureGenerator.TextureFromHeightMap(heightMap);
        generatedTexture = texture;

        Assert.AreEqual(texture, generatedTexture);
    }

}
