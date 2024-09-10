using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMixer : MonoBehaviour
{
    // Serialized Fields \\
    [Header("Settings")]
    [SerializeField] private float maxVolume = 0.7f;
    [SerializeField] private float fadeInSpeed = 0.1f;
    [SerializeField] private float fadeOutSpeed = 0.3f;
    [SerializeField] private AudioSource[] layers;

    [Header("General")]
    public int currentLevel = 1;


    private void Update()
    {
        for (int i = 0; i < currentLevel; i++)
        {
            if(i >= layers.Length) { break; }

            if(layers[i].volume < maxVolume)
            {
                layers[i].volume = Mathf.MoveTowards(layers[i].volume, maxVolume, Time.deltaTime * fadeInSpeed);
            }
        }

        int clampedCurrentLevel = Mathf.Clamp(currentLevel, 0, layers.Length);

        for (int i = layers.Length - 1; i >= clampedCurrentLevel; i--)
        {
            if (layers[i].volume > 0)
            {
                layers[i].volume = Mathf.MoveTowards(layers[i].volume, 0, Time.deltaTime * fadeOutSpeed);
            }
        }
    }
}
