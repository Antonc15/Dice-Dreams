using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Sprite[] diceSides;
    [SerializeField] private Image Icon;

    private LevelManager lvlManager;
    private int lastLevel = 0;

    private void Start()
    {
        lvlManager = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        if(lastLevel != lvlManager.currentLevelNumb)
        {
            lastLevel = lvlManager.currentLevelNumb;

            Icon.sprite = diceSides[lvlManager.currentLevelNumb - 1];
        }
    }
}
