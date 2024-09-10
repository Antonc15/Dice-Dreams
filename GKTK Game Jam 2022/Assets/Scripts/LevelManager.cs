using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Serialized Fields \\
    [SerializeField] private GameObject[] levels;
    [SerializeField] private GameObject placedLevel;

    public int currentLevelNumb = 1;

    // Private Fields \\
    private MusicMixer mMixer;
    private DiceRotateSetter rotSetter;

    public void Start()
    {
        mMixer = FindObjectOfType<MusicMixer>();
        rotSetter = FindObjectOfType<DiceRotateSetter>();
    }

    public void LoadLevel(int levelNumber, Vector3 position)
    {
        GameManager.Instance.UnlockLevel(levelNumber);

        if(levelNumber < 7)
        {
            GameObject currentLevel = Instantiate(levels[levelNumber - 1], position, Quaternion.identity, transform);

            currentLevelNumb = levelNumber;
            mMixer.currentLevel = levelNumber;

            rotSetter.SetRot(levelNumber);

            UnloadLevel();
            StartCoroutine(SceneLoading(2f));

            placedLevel = currentLevel;
        }
    }

    public void UnloadLevel()
    {
        if (placedLevel)
        {
            Destroy(placedLevel);
        }
    }

    private IEnumerator SceneLoading(float time)
    {
        DiceMove player = FindObjectOfType<DiceMove>();

        if (player)
        {
            player.isLevelLoading = true;

            yield return new WaitForSeconds(time);

            player.isLevelLoading = false;
        }
    }
}
