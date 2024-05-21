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
        var startPosition = new Vector3(0,0,8.5f);

        var startingPlatform = Instantiate(lvlScriptable.startingPlatformPrefab, Vector3.zero, Quaternion.identity);
        startingPlatform.transform.parent = transform;
        
        for (var i = 0; i < lvlScriptable.platformAmount; i++)
        {
            var pos = startPosition + new Vector3(0, 0, i * 2);

            var platform = Instantiate(lvlScriptable.platformPrefab, pos, Quaternion.identity);
            
            platform.transform.parent = transform;
            GameManager.i.movingPlatforms.Add(platform.GetComponent<MovingPlatform>());
        }
    }
}

