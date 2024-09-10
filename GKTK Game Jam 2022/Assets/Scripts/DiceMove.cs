using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceMove : MonoBehaviour
{
    // Serialized Fields \\
    [SerializeField] private float speed = 500;
    [SerializeField] private float transformSpeed = 500;
    [SerializeField] private float bounceHeight = 1f;
    [SerializeField] private float leapHeight = -0.5f;
    [SerializeField] private float fallDistance = 25f;
    [SerializeField] private float fallSpeed = 12;

    [Header("Audio")]
    [SerializeField] private AudioClip[] moveSounds;
    [SerializeField] private AudioClip fallSound;
    [SerializeField] private float moveVolume = 1f;
    [SerializeField] private float fallVolume = 1f;
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    // Private Fields \\
    private int currentSide = 1;

    private Vector3 forwardRotationPoint;
    private Vector3 backRotationPoint;
    private Vector3 leftRotationPoint;
    private Vector3 rightRotationPoint;

    private Vector3 lastStartPos = Vector3.zero;
    private Vector3 lastRotEuler = Vector3.up;

    private bool isMoving = false;
    private bool isFalling = false;

    public bool isTransforming = false;

    private float fallVelocity = 0;

    // Public Fields \\
    public bool isLevelLoading = false;

    // Private Methods \\

    private void Start()
    {
        forwardRotationPoint = new Vector3(0, -leapHeight, leapHeight);
        backRotationPoint = new Vector3(0, -leapHeight, -leapHeight);
        leftRotationPoint = new Vector3(-leapHeight, -leapHeight, 0);
        rightRotationPoint = new Vector3(leapHeight, -leapHeight, 0);

        ChangeSide();
    }

    private void Update()
    {
        if(isFalling)
        {
            Fall();
        }
        else
        {
            fallVelocity = 0;
        }

        if (isMoving || isFalling || isLevelLoading || isTransforming) { return; }

        if (Input.GetKey("up") || Input.GetKey(KeyCode.W))
        {
            StartCoroutine(Roll(forwardRotationPoint, currentSide, 1, false));
        }
        else if (Input.GetKey("down") || Input.GetKey(KeyCode.S))
        {
            StartCoroutine(Roll(backRotationPoint, currentSide, 1, false));
        }
        else if (Input.GetKey("left") || Input.GetKey(KeyCode.A))
        {
            StartCoroutine(Roll(leftRotationPoint, currentSide, 1, false));
        }
        else if (Input.GetKey("right") || Input.GetKey(KeyCode.D))
        {
            StartCoroutine(Roll(rightRotationPoint, currentSide, 1, false));
        }
    }

    private bool CheckIfFalling()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            isFalling = false;

            TileRespond tile = hit.transform.GetComponent<TileRespond>();

            if (tile)
            {
                tile.Step();
            }
        }
        else
        {
            isFalling = true;
        }

        return isFalling;
    }

    private bool CheckForPlates()
    {
        bool platesFound = false;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            TileFinish tFinish = hit.transform.GetComponent<TileFinish>();
            TileCheckpoint tCheckpoint = hit.transform.GetComponent<TileCheckpoint>();

            if (tFinish)
            {
                tFinish.FinishLevel();
                platesFound = true;
            }

            if (tCheckpoint)
            {
                tCheckpoint.ReachCheckpoint();
                platesFound = true;
            }
        }

        return platesFound;
    }

    public void ChangeSide()
    {
        float[] angles = {0f,0f,0f,0f,0f,0f};

        angles[0] = Vector3.Angle(-transform.up, Vector3.up); // One Side
        angles[1] = Vector3.Angle(transform.right, Vector3.up); // Two Side
        angles[2] = Vector3.Angle(-transform.forward, Vector3.up); // Three Side
        angles[3] = Vector3.Angle(transform.forward, Vector3.up); // Four Side
        angles[4] = Vector3.Angle(-transform.right, Vector3.up); // Five Side
        angles[5] = Vector3.Angle(transform.up, Vector3.up); // Six Side

        float minAngle = 360;
        int closestCharacter = 0;

        for (int i = 0; i < 6; i++)
        {
            if(angles[i] < minAngle)
            {
                minAngle = angles[i];
                closestCharacter = i;
            }
        }

        currentSide = closestCharacter + 1;
    }

    private void Fall()
    {
        fallVelocity += Time.deltaTime * fallSpeed / 60;
        transform.position += Vector3.down * fallVelocity;

        if(transform.position.y < -fallDistance)
        {
            transform.position = lastStartPos;
            transform.rotation = Quaternion.Euler(lastRotEuler);

            SoundHandler.Instance.MakeSound(fallSound, transform.position, fallVolume, minPitch, maxPitch);

            ChangeSide();

            isFalling = false;
        }
    }

    private IEnumerator TransformIntoThree()
    {
        float dTime = 0;
        isTransforming = true;

        Quaternion targetRot = Quaternion.Euler(-transform.eulerAngles);

        while (transform.rotation != targetRot)
        {
            dTime += Time.deltaTime;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, Time.deltaTime * transformSpeed);

            float sin = Mathf.Clamp(bounceHeight * Mathf.Sin(dTime * 10), 0.0f, bounceHeight);
            transform.position = new Vector3(transform.position.x, sin, transform.position.z);

            yield return null;
        }

        SoundHandler.Instance.MakeSound(moveSounds[Random.Range(0, moveSounds.Length)], transform.position, moveVolume, minPitch, maxPitch);

        ChangeSide();

        isTransforming = false;
    }

    private IEnumerator TransformIntoFour()
    {
        float dTime = 0;
        isTransforming = true;

        Quaternion targetRot = Quaternion.Euler(-transform.eulerAngles);

        while (transform.rotation != targetRot)
        {
            dTime += Time.deltaTime;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, Time.deltaTime * transformSpeed);

            float sin = Mathf.Clamp(bounceHeight * Mathf.Sin(dTime * 10), 0.0f, bounceHeight);
            transform.position = new Vector3(transform.position.x, sin, transform.position.z);

            yield return null;
        }

        SoundHandler.Instance.MakeSound(moveSounds[Random.Range(0, moveSounds.Length)], transform.position, moveVolume, minPitch, maxPitch);

        ChangeSide();

        isTransforming = false;
    }

    // Public Methods \\

    public IEnumerator Roll(Vector3 rotationPoint, int amount, float multiplier, bool isUndo)
    {
        lastStartPos = transform.position;
        lastRotEuler = transform.eulerAngles;
        
        isMoving = true;
        int iterations = amount;

        while (iterations > 0)
        {
            Vector3 point = transform.position + rotationPoint;

            Vector3 axis = Vector3.Cross(Vector3.up, rotationPoint).normalized;
            float angle = 90;

            while (angle > 0)
            {
                float a = Time.deltaTime * speed * multiplier;
                transform.RotateAround(point, axis, a);
                angle -= a;
                yield return null;
            }

            // Round Rotation to an Int
            transform.RotateAround(point, axis, angle);
            Quaternion rot = transform.rotation;
            transform.rotation = Quaternion.Euler(Mathf.RoundToInt(rot.eulerAngles.x), Mathf.RoundToInt(rot.eulerAngles.y), Mathf.RoundToInt(rot.eulerAngles.z));

            // Round Position to an Int
            Vector3 pos = transform.position;
            transform.position = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));

            ChangeSide();

            if (!CheckIfFalling())
            {
                SoundHandler.Instance.MakeSound(moveSounds[Random.Range(0, moveSounds.Length)], transform.position, moveVolume, minPitch, maxPitch);
            }
            else
            {
                iterations = 1;
            }

            iterations--;
        }

        if (amount == 4 && !isUndo)
        {
            if(!isFalling && !CheckForPlates())
            {
                StartCoroutine(TransformIntoThree());
            }
        }
        else if (amount == 4 && isUndo)
        {
            if (!isFalling && !CheckForPlates())
            {
                StartCoroutine(TransformIntoFour());
            }
        }

        if (!isUndo && !isFalling && !isTransforming)
        {
            GameManager.Instance.LogMove(rotationPoint, amount, currentSide);
            CheckForPlates();
        }

        isMoving = false;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public bool IsFalling()
    {
        return isFalling;
    }
}