using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    #region Singleton
    private static SoundHandler _instance;

    public static SoundHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundHandler>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public void MakeSound(AudioClip clip, Vector3 pos, float volume, float minPitch, float maxPitch)
    {
        GameObject soundObject = new GameObject(string.Format("{0}", clip.name));
        soundObject.transform.SetParent(transform);
        soundObject.transform.position = pos;

        AudioSource source = soundObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = Random.Range(minPitch, maxPitch);
        source.Play();

        Destroy(soundObject, clip.length / source.pitch);
    }
}
