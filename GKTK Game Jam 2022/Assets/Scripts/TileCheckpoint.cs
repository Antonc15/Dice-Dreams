using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCheckpoint : MonoBehaviour
{
    // Serialized Fields \\
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] private float volume = 1f;
    [SerializeField] private int selectSide = 1;

    private LevelParts lvlParts;
    private DiceRotateSetter rotSetter;

    private void Start()
    {
        lvlParts = FindObjectOfType<LevelParts>();
        rotSetter = FindObjectOfType<DiceRotateSetter>();
    }

    public void ReachCheckpoint()
    {
        SoundHandler.Instance.MakeSound(checkpointSound, transform.position, volume, 1f, 1f);
        lvlParts.ReachedCheckpoint(transform.position);
        rotSetter.SetRot(selectSide);
    }
}
