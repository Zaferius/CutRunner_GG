using DG.Tweening;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelScriptable lvlScriptable;
    
    private void OnEnable()
    {
        Actions.OnFinishPlatform += BuildFinishPlatform;
        
    }

    private void OnDisable()
    {
        Actions.OnFinishPlatform -= BuildFinishPlatform;
       
    }

    private void Start()
    {
        BuildLevel();
        GameManager.i.playPlatform = lvlScriptable.platformPrefab;
    }

    private void BuildLevel()
    {
        var startPosition = new Vector3(0,0,6f);

        var startingPlatform = Instantiate(lvlScriptable.startingPlatformPrefab, Vector3.zero, Quaternion.identity);
        startingPlatform.transform.parent = transform;
        
    }

    private void BuildFinishPlatform()
    {
        /*var finishPlatform = Instantiate(lvlScriptable.finishPlatformPrefab
            , new Vector3(0,0,GameManager.i.movingPlatforms[^1].transform.localPosition.z + 6)
            , Quaternion.identity);
        
        finishPlatform.transform.parent = transform;
        var finishPlatformDefaultScale = finishPlatform.transform.localScale;
        finishPlatform.transform.localScale = Vector3.zero;
        finishPlatform.transform.DOScale(finishPlatformDefaultScale, 0.3f).SetEase(Ease.OutBack);*/
    }
}

