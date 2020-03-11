using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Data", menuName = "Data")]
public class Data : ScriptableObject
{
    [System.Serializable]
    public class DataPoints
    {

        public Dictionary<Vector3, float> densData = new Dictionary<Vector3, float>();

        public Dictionary<Vector3, Vector3> gradientData = new Dictionary<Vector3, Vector3>();

    }

    [SerializeField]
    private DataPoints dataPoints = new DataPoints();

    public void AddPoint(Vector3 point, float val)
    {
        dataPoints.densData.Add(point, val);
    }

    public void AddGradient(Vector3 point, Vector3 gradient)
    {
        dataPoints.gradientData.Add(point, gradient);
    }

    public float GetPoint(Vector3 point)
    {
        return dataPoints.densData[point];
    }

    public Vector3 GetGradient(Vector3 point)
    {
        return dataPoints.gradientData[point];
    }

    public bool CheckGradient(Vector3 point)
    {
        return dataPoints.gradientData.ContainsKey(point);
    }

    public bool CheckPoint(Vector3 point)
    {
        return dataPoints.densData.ContainsKey(point);
    }

    public void PrintValue()
    {
        Debug.Log(dataPoints.densData[new Vector3(67, 123, 193)]);
    }
}