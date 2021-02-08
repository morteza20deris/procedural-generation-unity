using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap }
    public DrawMode drawMode;
    public int mapHeight;
    public int mapWidth;
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
        float[,] generatedMap = Noise.GenerateNoiseMap(mapHeight, mapWidth, noiseScale, Octaves, Persistance, Lacunarity, Seed, Offset);

        Color[] colorMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentheight = generatedMap[x, y];
                for (int i = 0; i < Regiens.Length; i++)
                {
                    if (currentheight <= Regiens[i].Height)
                    {
                        colorMap[y * mapWidth + x] = Regiens[i].color;
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
            display.DrawTexture(Texturegenerator.TexturefromColorMap(colorMap,mapWidth,mapHeight));

        }

    }

    void OnValidate()
    {
        if (mapHeight < 1)
        {
            mapHeight = 1;
        }
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
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
