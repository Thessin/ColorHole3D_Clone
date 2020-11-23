using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class StageData
{
    public int Level;
    public int Stage;
    public int PointsNeeded;
}

[CreateAssetMenu(fileName = "PointsPerLevel", menuName = "ColorHole3D/PointsPerLevelScriptableObject")]
public class PointsPerLevelSO : ScriptableObject
{
    [SerializeField]
    private List<StageData> stageDatas;

    public int GetPointsNeeded(int level, int stage)
    {
        int pointNeeded = 0;

        try
        {
            pointNeeded = stageDatas.Find((x) => x.Level == level && x.Stage == stage).PointsNeeded;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error occured while getting points needed for current stage with message = " + e.Message);
        }

        return pointNeeded;
    }
}
