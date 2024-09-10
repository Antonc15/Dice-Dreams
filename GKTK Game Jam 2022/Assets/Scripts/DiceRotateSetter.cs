using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRotateSetter : MonoBehaviour
{
    private DiceMove dMove;

    private void Start()
    {
        dMove = FindObjectOfType<DiceMove>();
    }

    public void SetRot(int side)
    {
        switch (side)
        {
            case 1:
                transform.eulerAngles = new Vector3(0, 0, 180);
                dMove.ChangeSide();
                return;
            case 2:
                transform.eulerAngles = new Vector3(0, 0, 90);
                dMove.ChangeSide();
                return;
            case 3:
                transform.eulerAngles = new Vector3(90, 0, 0);
                dMove.ChangeSide();
                return;
            case 4:
                transform.eulerAngles = new Vector3(-90, 0, 0);
                dMove.ChangeSide();
                return;
            case 5:
                transform.eulerAngles = new Vector3(0, 0, -90);
                dMove.ChangeSide();
                return;
            case 6:
                transform.eulerAngles = new Vector3(0, 0, 0);
                dMove.ChangeSide();
                return;
        }
    }
}
