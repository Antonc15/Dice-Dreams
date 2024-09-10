using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParts : MonoBehaviour
{
    [SerializeField] private GameObject[] parts;

    private GameObject placedPart;
    int nextSecion = 0;

    private void Start()
    {
        PlacePart(transform.position);
    }

    private void PlacePart(Vector3 pos)
    {
        StartCoroutine(PartLoading(2f));

        placedPart = Instantiate(parts[nextSecion], pos, Quaternion.identity, transform);

        nextSecion++;
    }

    public void ReachedCheckpoint(Vector3 pos)
    {
        Destroy(placedPart);

        GameManager.Instance.ClearUndoList();

        Vector3 modPos = new Vector3(pos.x, 0, pos.z);

        PlacePart(modPos);
    }

    private IEnumerator PartLoading(float time)
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
