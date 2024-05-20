using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    void Start()
    {
        startButton.onClick.AddListener(GameManager.i.StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
