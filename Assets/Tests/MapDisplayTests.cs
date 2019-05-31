using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MapDisplayTests {

    [Test]
    public void DrawsTexture() {
        var plane = Resources.Load("Tests/Plane");
        var mapGenerator = new MapGenerator();
        var mapDisplay = new MapDisplay();

        var mapData = mapGenerator.GenerateMapData(Vector2.zero);

        var texture = TextureGenerator.TextureFromHeightMap(mapData.heightMap);

        var renderedTexture = texture;

        Assert.AreEqual(texture, renderedTexture);

        GameObject.Destroy(mapDisplay);
        GameObject.Destroy(mapGenerator);
    }

    [Test]
    public void DrawsMesh()
    {
        var plane = Resources.Load("Tests/Plane");
        var mapGenerator = new MapGenerator();
        var mapDisplay = new MapDisplay();

        var mapData = mapGenerator.GenerateMapData(Vector2.zero);
        var meshHeightMultiplier = mapGenerator.meshHeightMultiplier;
        var meshHeightCurve = mapGenerator.meshHeightCurve;
        var mapWidth = mapGenerator.mapWidth;
        var mapHeight = mapGenerator.mapHeight;

        var mesh = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve);
        var texture = TextureGenerator.TextureFromColorMap(mapData.colorMap, mapWidth, mapHeight);

        var renderedMesh = mesh;
        var renderedTexture = texture;

        Assert.AreEqual(texture, renderedTexture);
        Assert.AreEqual(mesh, renderedMesh);

        GameObject.Destroy(mapDisplay);
        GameObject.Destroy(mapGenerator);
    }


}
