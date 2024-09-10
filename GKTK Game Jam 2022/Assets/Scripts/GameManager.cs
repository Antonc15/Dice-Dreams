using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    // Serialized Fields \\
    [SerializeField] private float multiplier = 2f;
    [SerializeField] private int unlockedLevels = 1;

    // Private Fields \\
    private List<Vector3> oppositeRotationPoints = new List<Vector3>();
    private List<Vector3> rotationPoints = new List<Vector3>();
    private List<int> moveAmounts = new List<int>();
    private List<int> finalSides = new List<int>();

    private DiceMove dice;

    private void Start()
    {
        dice = FindObjectOfType<DiceMove>();

        if (PlayerPrefs.HasKey("unlockedLevels"))
        {
            unlockedLevels = PlayerPrefs.GetInt("unlockedLevels");
        }
    }

    private void Update()
    {
        HideAndLockCursor();

        if (Input.GetKey(KeyCode.Backspace) && !dice.IsMoving() && !dice.isTransforming)
        {
             UndoMove(multiplier);
        }
    }

    private void HideAndLockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockLevel(int level)
    {
        if(unlockedLevels < level)
        {
            unlockedLevels = level;
            PlayerPrefs.SetInt("unlockedLevels", unlockedLevels);
        }
    }

    public int GetUnlockedLevels()
    {
        return unlockedLevels;
    }

    private void UndoMove(float multi)
    {
        if (!dice || dice.IsMoving() || dice.IsFalling() || oppositeRotationPoints.Count < 1) { return; } // If their is no player dice found cancel the method.

        int lastMove = oppositeRotationPoints.Count - 1;

        StartCoroutine(dice.Roll(oppositeRotationPoints[lastMove], moveAmounts[lastMove], multi, true));

        oppositeRotationPoints.RemoveAt(lastMove);
        rotationPoints.RemoveAt(lastMove);
        moveAmounts.RemoveAt(lastMove);
        finalSides.RemoveAt(lastMove);
    }

    public void LogMove(Vector3 rotationPoint, int moveAmount, int currentSide)
    {
        oppositeRotationPoints.Add(GetOppositeRotationPoint(rotationPoint));
        rotationPoints.Add(rotationPoint);

        moveAmounts.Add(moveAmount);
        finalSides.Add(currentSide);
    }

    public void ClearUndoList()
    {
        oppositeRotationPoints.Clear();
        rotationPoints.Clear();
        moveAmounts.Clear();
        finalSides.Clear();
    }

    private Vector3 GetOppositeRotationPoint(Vector3 rotationPoint)
    {
        if (rotationPoint == new Vector3(0, -0.5f, 0.5f)) // Forward Facing
        {
            return new Vector3(0, -0.5f, -0.5f);
        }
        else if (rotationPoint == new Vector3(-0.5f, -0.5f, 0)) // Left Facing
        {
            return new Vector3(0.5f, -0.5f, 0);
        }
        else if (rotationPoint == new Vector3(0.5f, -0.5f, 0)) // Right Facing
        {
            return new Vector3(-0.5f, -0.5f, 0);
        }
        else if (rotationPoint == new Vector3(0, -0.5f, -0.5f)) // Backwards Facing
        {
            return new Vector3(0, -0.5f, 0.5f);
        }
        else
        {
            return Vector3.zero;
        }
    }
}
