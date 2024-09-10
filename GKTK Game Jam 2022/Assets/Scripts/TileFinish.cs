using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFinish : MonoBehaviour
{

    // Serialized Fields \\
    [SerializeField] private int nextLevel = 1;
    [SerializeField] private AudioClip levelClearSound;
    [SerializeField] private float volume = 1f;

    // Private Fields \\
    private LevelManager lvlManager;
    private DiceMenu dMenu;

    private void Start()
    {
        lvlManager = FindObjectOfType<LevelManager>();
        dMenu = FindObjectOfType<DiceMenu>();
    }

    public void FinishLevel()
    {
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);

        lvlManager.LoadLevel(nextLevel, pos);

        GameManager.Instance.ClearUndoList();

        if (levelClearSound)
        {
            SoundHandler.Instance.MakeSound(levelClearSound, transform.position, volume, 1f, 1f);
        }

        if (nextLevel > 6)
        {
            dMenu.LoadMenu();
            return;
        }
        else
        {
            lvlManager.LoadLevel(nextLevel, pos);
        }
    }
}
