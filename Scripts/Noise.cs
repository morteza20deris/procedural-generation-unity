using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapHeight, int mapWidth, float noiseScale, int octaves, float persistance, float lacunarity, int seed, Vector2 offset)
    {
        float[,] genMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octavesOffset = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octavesOffset[i] = new Vector2(offsetX, offsetY);
        }

        if (noiseScale <= 0)
        {
            noiseScale = 0.0001f;
        }

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;



        float maxNoiseheight = float.MinValue;
        float minNoiseheight = float.MaxValue;



        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {

                    float sampleX = (x - halfWidth) / noiseScale * frequency + octavesOffset[i].x;
                    float sampleY = (y - halfHeight) / noiseScale * frequency + octavesOffset[i].y;

                    float perlinNoise = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinNoise * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseheight)
                {
                    maxNoiseheight = noiseHeight;
                }
                else if (noiseHeight < minNoiseheight)
                {
                    minNoiseheight = noiseHeight;
                }

                genMap[x, y] = noiseHeight;

            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                genMap[x, y] = Mathf.InverseLerp(minNoiseheight, maxNoiseheight, genMap[x, y]);
            }
        }

        return genMap;
    }
}
