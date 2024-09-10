using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayWhen : MonoBehaviour
{
    [SerializeField] private int levelUnlocked = 6;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        if(GameManager.Instance.GetUnlockedLevels() <= levelUnlocked)
        {
            source.Play();
        }
    }
}
