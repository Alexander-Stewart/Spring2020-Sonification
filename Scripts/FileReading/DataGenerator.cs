using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DataGenerator : MonoBehaviour {

    public string path = "Assets/Resources/DataFile.txt";
    string path2 = "Assets/Resources/GradientFile.txt";

    float randomDensityValue = 0f;

    // setting up the spherical harmonics formula semi-randomly
   


    // Use this for initialization
    void Start () {
        int l = 2;
        //int l = UnityEngine.Random.Range(0, 3);
        Debug.Log("This is l: " + l);
        GenerateFile(l);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private float HarmonicGenerator(int l, int m, float xCor, float yCor, float zCor)
    {
        float returnHarmonic;
        float updateRandomDV;

        //creating random value for the density value at that point
        updateRandomDV = UnityEngine.Random.Range(randomDensityValue - 2f, randomDensityValue + 3f);
        if(updateRandomDV > 5f)
        {
            updateRandomDV = 5f;
        } else if(updateRandomDV < -5f)
        {
            updateRandomDV = -5f;
        }

        randomDensityValue = updateRandomDV;
        returnHarmonic = Mathf.Sqrt(15f / (4f * Mathf.PI)) * xCor * yCor;
        return returnHarmonic * randomDensityValue;
        //        switch (l)
        //        {
        //            case 0:
        //                returnHarmonic = Mathf.Sqrt(1f / (4f * Mathf.PI));
        //                return returnHarmonic * randomDensityValue;
        //            case 1:
        //                if(m == -1)
        //                {
        //                    returnHarmonic = Mathf.Sqrt(3f / (4f * Mathf.PI)) * xCor;
        //                    return returnHarmonic * randomDensityValue;
        //                } else if (m == 0)
        //                {
        //                    returnHarmonic = Mathf.Sqrt(3f / (4f * Mathf.PI)) * zCor;
        //                    return returnHarmonic * randomDensityValue;
        //                } else
        //                    returnHarmonic = Mathf.Sqrt(3f / (4f * Mathf.PI)) * yCor;
        //                    return returnHarmonic * randomDensityValue;
        //               }
        //            case 2:
        //                if (m == -2)
        //                {
        //                    returnHarmonic = Mathf.Sqrt(15f / (4f * Mathf.PI)) * xCor * yCor;
        //                    return returnHarmonic * randomDensityValue;
        //                } else if(m == -1)
        //                {
        //                    returnHarmonic = Mathf.Sqrt(15f / (4f * Mathf.PI)) * yCor * zCor;
        //                    return returnHarmonic * randomDensityValue;
        //                } else if(m == 0)
        //                {
        //                   returnHarmonic = Mathf.Sqrt(5f / (16f * Mathf.PI)) * (3f * Mathf.Pow(zCor, 2) - 1f);
        //                    return returnHarmonic * randomDensityValue;
        //                } else if(m == 1)
        //                {
        //                    returnHarmonic = Mathf.Sqrt(15f / (8f * Mathf.PI)) * xCor * zCor;
        //                    return returnHarmonic * randomDensityValue;
        //                } else
        //                {
        //                    returnHarmonic = Mathf.Sqrt(15f / (32f * Mathf.PI)) * (Mathf.Pow(xCor,2) - Mathf.Pow(yCor,2));
        //                    return returnHarmonic * randomDensityValue;
        //                }
        //            default:
        //                returnHarmonic = Mathf.Sqrt(15f / (32f * Mathf.PI)) * (Mathf.Pow(xCor, 2) - Mathf.Pow(yCor, 2));
        //                return returnHarmonic * randomDensityValue;
        //        }
    }

    private void GenerateFile(int l)
    {
        // creating stream Writer
        StreamWriter writer = new StreamWriter(path);

        int m = 0;
        //int m = UnityEngine.Random.Range(-l, (l + 1));
        Debug.Log("This is m: " + m);
        for(float x = -50; x <= 50; x++)
        {
            for(float y = -50; y <= 50; y++)
            {
                for(float z = -50; z <= 50; z++)
                {
                    float harmonicVal = HarmonicGenerator(l, m, x, y, z);
                    writer.WriteLine((float)Math.Round(x,1) + " " + 
                        (float)Math.Round(y, 1) + " " + (float)Math.Round(z, 1) + " " + (x+y+z));
                }
            }
        }
        writer.Flush();
        writer.Close();
    }
}
