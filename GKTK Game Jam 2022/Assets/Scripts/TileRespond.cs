using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TileRespond : MonoBehaviour
{
    [SerializeField] private MeshRenderer rend;
    [SerializeField] private Material glowMat;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;

        transform.GetChild(0).localScale = Vector3.zero;

        StartCoroutine(SpawnTile(1f));
    }

    public void Step()
    {
        anim.Play("Tile_Step");
    }

    private IEnumerator SpawnTile(float maxTime)
    {
        yield return new WaitForSeconds(Random.Range(0f, maxTime));

        anim.enabled = true;
    }
}
