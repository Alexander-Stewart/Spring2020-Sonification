using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DataReader : MonoBehaviour {

    
    public Data data;

#if EightSevenA
    string path = "Assets/Resources/87a_values.txt";
#else
    public string path = "Assets/Resources/";
#endif


    // Use this for initialization
    void Start () {
        readCSV();
        //readSingleValues();


        //getting gradients
        //gradientGenerator();

        // debugs
       // Debug.Log("This is the densData: " + densData);
        //Debug.Log("the origin for gradient data: " + gradientData[new Vector3(1f, -45f, 2f)]);
    }

    /**
    * Currently just returns the passed in vector, if wanted, could calculate partials and return a gradient for the density data.
    **/
    Vector3 createGradient(Vector3 vect, float dens) {
        // Vector3 gradient = new Vector3(partialX(curPos.x, curPos.y, curPos.z),
        //     partialY(curPos.x, curPos.y, curPos.z),
        //     partialZ(curPos.x, curPos.y, curPos.z));
        Debug.Log("Density value for " + vect + ": " + dens);
        return vect;

    }

   
    private void readCSV() {
        StreamReader reader = new StreamReader(path);
        string line;
        string[] values;
        Vector3 curPos;
        float dens;
        line = reader.ReadLine();
        while (!reader.EndOfStream)
        {
            values = line.Split(' ');

            // getting the curPos as a vector and dens value
            curPos = new Vector3(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
            dens = float.Parse(values[3]);
            //Vector3 gradient = createGradient();
            //Debug.Log("Density value for " + curPos + ": " + dens);


            // adding to hashtable
            data.AddPoint(curPos, dens);
            line = reader.ReadLine();
        }

        reader.Close();

        //AssetDatabase.CreateAsset(data, "Assets/Resources/Data/ArgonData.asset");
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void readSingleValues() {
        StreamReader reader = new StreamReader(path);
        string line;
        string[] values;
        Vector3 curPos;
        float dens;
        line = reader.ReadLine();
        while (!reader.EndOfStream)
        {
            for(int i = -256; i < 256; i++)
            {
                for(int j = -256; j < 256; j++)
                {
                    for(int k = -256; k < 256; k++)
                    {
                        // getting the curPos as a vector and dens value
                        curPos = new Vector3(i,j,k);
                        dens = float.Parse(line);
                        // Debug.Log("Density value for " + curPos + ": " + dens);


                        // adding to hashtable
                        data.AddPoint(curPos, dens);
                        line = reader.ReadLine();
                    }
                }
            }
        }
        reader.Close();

        //AssetDatabase.CreateAsset(data, "Assets/Resources/Data/ArgonData.asset");
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }



}
