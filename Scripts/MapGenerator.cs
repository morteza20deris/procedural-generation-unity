using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, Mesh }
    public DrawMode drawMode;
    const int mapChunkSize = 241;
    [Range(0, 6)]
    public int levelOfDetail;
    public float meshHeightMultiplier;
    public AnimationCurve meshHightCurve;
    public float noiseScale;
    public bool AutoUpdate;
    public int Octaves;
    [Range(0, 1)]
    public float Persistance;
    public float Lacunarity;
    public int Seed;
    public Vector2 Offset;
    public TerrainTypes[] Regiens;

    public void generateMap()
    {
        float[,] generatedMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, noiseScale, Octaves, Persistance, Lacunarity, Seed, Offset);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentheight = generatedMap[x, y];
                for (int i = 0; i < Regiens.Length; i++)
                {
                    if (currentheight <= Regiens[i].Height)
                    {
                        colorMap[y * mapChunkSize + x] = Regiens[i].color;
                        break;
                    }
                }
            }
        }
        DisplayMap display = FindObjectOfType<DisplayMap>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(Texturegenerator.TextureFromHeightMap(generatedMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(Texturegenerator.TexturefromColorMap(colorMap, mapChunkSize, mapChunkSize));

        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(generatedMap, meshHeightMultiplier, meshHightCurve, levelOfDetail), Texturegenerator.TexturefromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }

    }

    void OnValidate()
    {
        if (Octaves < 0)
        {
            Octaves = 0;
        }
        if (Lacunarity < 1)
        {
            Lacunarity = 1;
        }
    }
}
[System.Serializable]
public struct TerrainTypes
{
    public string Name;
    public float Height;
    public Color color;
}
