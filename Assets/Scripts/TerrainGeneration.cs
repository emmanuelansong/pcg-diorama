using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;


//using UnityEngine.Rendering.PostProcessing;
//using UnityEngine.Rendering.Universal;

public class TerrainGeneration : MonoBehaviour
{
    //The frequency of the noise
    public float frequency = 1f;
    //The lacunarity of the noise
    public float lacu = 1f;
    //The number of octaves
    public int octaves = 1;
    //The persistance of the noise
    public float persist = 1f;

    //public PostProcessVolume ppv;
    public static TerrainGeneration tg;

    public bool coldWeather;

    public float layer0Transparency = 1f;
    public float layer1Transparency = 1f;
    public float layer1Lenience = 1f;

    public float layer2HeightLenience;
    public float layer2HeightAngleLenience;

    public GameObject cube;

    private void Awake()
    {

        //generate in editor
        generate();

        
    }

    public void SplatMap()
    {
        //algorithm to sort alphamaps of terrain
        TerrainData terrainData = Terrain.activeTerrain.terrainData;
        float[,,] splatmapData = new float[terrainData.alphamapHeight, terrainData.alphamapWidth, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                // Normalise x/y coordinates
                float y_01 = (float)y / terrainData.alphamapHeight;
                float x_01 = (float)x / terrainData.alphamapWidth;

                // get normals at given points
                Vector3 normal = terrainData.GetInterpolatedNormal(y_01, x_01);

                float steepness = terrainData.GetSteepness(y_01, x_01);

                float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapResolution), Mathf.RoundToInt(x_01 * terrainData.heightmapResolution));

                // Setup an array to record the mix of texture weights at this point
                float[] splatWeights = new float[terrainData.alphamapLayers];

                // Texture[0] has constant influence
                splatWeights[0] = layer0Transparency;

                //
                splatWeights[1] = Mathf.Clamp01((float)Math.Cosh(layer1Transparency / height) - Mathf.Clamp01(steepness * steepness / (200 / layer1Lenience)));

                //
                splatWeights[2] = Mathf.Clamp01(height / Mathf.Pow(layer2HeightLenience, 2) * (float)Math.Cosh(normal.z / layer2HeightAngleLenience));

                // Texture[3] only on surfaces facing positive Z axis 
                //splatWeights[3] = Mathf.Clamp01(normal.z);

                // Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
                float z = splatWeights.Sum();

                // Loop through each terrain texture
                for (int i = 0; i < terrainData.alphamapLayers; i++)
                {
                    // Normalize so that sum of all texture weights = 1
                    splatWeights[i] /= z;

                    // assigned to splats
                    splatmapData[x, y, i] = splatWeights[i];
                }
            }
            terrainData.SetAlphamaps(0, 0, splatmapData);
        }
    }
    void Start()
    {
        var lightSource = GameObject.Find("Directional Light").GetComponent<Light>();

        //lightSource.intensity = Random.Range(0, 2);
        //lightSource.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 180),0, 0));
    }


    //terrain generation using libnoise
    public void generate()
    {
        var terrainData = Terrain.activeTerrain.terrainData;

        //different generators
        var constGenerator = new Const(1f);

        //perlin generator
        var perlinGenerator2 = new RidgedMultifractal(frequency * 2, lacu, octaves, UnityEngine.Random.Range(0,
        0xffffff), QualityMode.High);
        var generator = new RidgedMultifractal(frequency, lacu, octaves, UnityEngine.Random.Range(0,
        0xffffff), QualityMode.High);

        var terrace = new Terrace(false, perlinGenerator2);
        terrace.Generate(UnityEngine.Random.Range(10, 50));
        var mixedGenerator2 = new Add(new Multiply(generator, constGenerator), new Multiply(generator, constGenerator));
        var finalGen = new Add(new Multiply(mixedGenerator2, terrace), new Multiply(mixedGenerator2, constGenerator));

        //Created a 2D noise generator for the terrain heightmap, using the generator
        var noise = new Noise2D(terrainData.heightmapResolution, finalGen);



        //Generate a plane from [0, 1] on x, [0, 1] on y
        noise.GeneratePlanar(0, 1, 0, 1);


        //Get the data in an array so we can use it to set the heights
        var data = noise.GetData(true, 0, 0, true);
        //.. and actually set the heights
        terrainData.SetHeights(0, 0, data);

        SplatMap();

    }
    public Vector3 RandomTerrainPosition(Terrain terrain)
    {
        //Get the terrain size in all 3 dimensions
        Vector3 terrainSize = terrain.terrainData.size;

        //Choose a uniformly random x and z to sample y
        float rX = UnityEngine.Random.Range(0, terrainSize.x);
        float rZ = UnityEngine.Random.Range(0, terrainSize.z);

        //Sample y at this point and put into an offset vec3
        Vector3 sample = new Vector3(rX, 0, rZ);
        sample.y = terrain.SampleHeight(sample);

        //Just return terrain pos + sample offset
        return terrain.GetPosition() + sample;
    }
    
}