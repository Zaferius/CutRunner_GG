using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    public static Level i;
    
    public LevelScriptable lvlScriptable;
    [SerializeField] private Transform fogPlane;
    [SerializeField] private GameObject levelPropCube;

    private void OnEnable()
    {
        Actions.OnGameWin += GameWin;
    }

    private void OnDisable()
    {
        Actions.OnGameWin -= GameWin;
    }
    
    private void Awake()
    {
        if (i)
        {
            Destroy(this);
        }
        else
        {
            i = this;
        }
    }

    private void Start()
    {
        BuildLevel();
        GameManager.i.playPlatform = lvlScriptable.platformPrefab;
    }

    private void BuildLevel()
    {
        var startingPlatform = Instantiate(lvlScriptable.startingPlatformPrefab, Vector3.zero, Quaternion.identity);
        startingPlatform.transform.parent = transform;

        var environmentObj = transform.GetChild(0);
        
        for (var j = 0; j < lvlScriptable.platformAmount * 4; j++)
        {
            var cube = Instantiate(levelPropCube, new Vector3(Random.Range(-7,7),Random.Range(-7f,-9f), 3 * j), Quaternion.identity);
            cube.transform.parent = environmentObj;
        }
        
    }

    private void GameWin()
    {
        fogPlane.transform.DOLocalMoveY(3.5f, 2f).SetEase(Ease.OutQuad);
    }
    
    
}

