using UnityEngine;

[CreateAssetMenu]
public class LevelScriptable : ScriptableObject
{
    public int platformAmount;
    public GameObject startingPlatformPrefab;
    public GameObject platformPrefab;
    public GameObject finishPlatformPrefab;
}
