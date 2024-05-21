using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelScriptable lvlScriptable;

    private void Start()
    {
        BuildLevel();
    }

    private void BuildLevel()
    {
        var startPosition = new Vector3(0,0,6f);

        var startingPlatform = Instantiate(lvlScriptable.startingPlatformPrefab, Vector3.zero, Quaternion.identity);
        startingPlatform.transform.parent = transform;
        
        for (var i = 0; i < lvlScriptable.platformAmount; i++)
        {
            var pos = startPosition + new Vector3(0, 0, i * 2);

            var platform = Instantiate(lvlScriptable.platformPrefab, pos, Quaternion.identity);
            
            platform.transform.parent = transform;
            GameManager.i.movingPlatforms.Add(platform.GetComponent<MovingPlatform>());
        }
        
        var finishPlatform = Instantiate(lvlScriptable.finishPlatformPrefab, new Vector3(0,0,GameManager.i.movingPlatforms[^1].transform.localPosition.z + 6), Quaternion.identity);
        finishPlatform.transform.parent = transform;
    }
}

