using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DiceMenu : MonoBehaviour
{
    [SerializeField] private int selectedLevel = 1;
    [SerializeField] private AudioSource menuMusic;
    [SerializeField] private TextMeshProUGUI[] titleCards;
    [SerializeField] private float titleCardFadeSpeed = 1f;
    [SerializeField] private GameObject[] cross;
    [SerializeField] private float musicMaxVolume = 0.8f;
    [SerializeField] private float musicInSpeed = 0.2f;
    [SerializeField] private float musicOutSpeed = 0.4f;
    [SerializeField] private string UIScene = "UIScene";

    [Header("Audio")]
    [SerializeField] private AudioClip levelSelect;
    [SerializeField] private AudioClip diceMove;
    [SerializeField] private AudioClip exitGame;
    [SerializeField] private AudioClip levelSelectDeny;
    [SerializeField] private float volume = 1f;
    [SerializeField] private float minPitch = 1f;
    [SerializeField] private float maxPitch = 1f;

    private bool inMenu = true;
    private bool isRotating = false;

    private LevelManager lvlManager;
    private DiceMove dMove;
    private CameraFollow camFollow;
    private MusicMixer mMixer;

    private void Start()
    {
        lvlManager = FindObjectOfType<LevelManager>();
        camFollow = FindObjectOfType<CameraFollow>();
        mMixer = FindObjectOfType<MusicMixer>();

        dMove = GetComponent<DiceMove>();

        GetSelectedSide();
        DisplayCrosses();
    }

    private void Update()
    {
        MenuThemeVolume();
        FadeTitleText();

        if (inMenu)
        {
            RotateDiceInput();

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
            {
                if (!isRotating)
                {
                    if(selectedLevel <= GameManager.Instance.GetUnlockedLevels())
                    {
                        LoadLevel();
                    }
                    else
                    {
                        SoundHandler.Instance.MakeSound(levelSelectDeny, transform.position, 0.4f, minPitch, maxPitch);
                        CrossRejectAnim();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKey(KeyCode.X) && Input.GetKeyUp(KeyCode.Backspace))
            {
                PlayerPrefs.DeleteAll();
                Application.Quit();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !dMove.IsMoving() && !dMove.IsFalling())
            {
                SoundHandler.Instance.MakeSound(exitGame, transform.position, volume, minPitch, maxPitch);
                LoadMenu();
            }
        }
    }

    private void FadeTitleText()
    {

        for (int i = 0; i < titleCards.Length; i++)
        {
            Color tempColor = titleCards[i].color;

            if (!inMenu)
            {
                tempColor.a = Mathf.MoveTowards(tempColor.a, 0, Time.deltaTime * titleCardFadeSpeed);
            }
            else
            {
                tempColor.a = Mathf.MoveTowards(tempColor.a, 1, Time.deltaTime * titleCardFadeSpeed);
            }

            titleCards[i].color = tempColor;
        }
    }

    private void RotateDiceInput()
    {
        if (isRotating) { return; }

        if (Input.GetKey("up") || Input.GetKey(KeyCode.W))
        {
            StartCoroutine(RotateDice(new Vector3(0, -0.5f, 0.5f)));
        }
        else if (Input.GetKey("down") || Input.GetKey(KeyCode.S))
        {
            StartCoroutine(RotateDice(new Vector3(0, -0.5f, -0.5f)));
        }
        else if (Input.GetKey("left") || Input.GetKey(KeyCode.A))
        {
            StartCoroutine(RotateDice(new Vector3(-0.5f, -0.5f, 0)));
        }
        else if (Input.GetKey("right") || Input.GetKey(KeyCode.D))
        {
            StartCoroutine(RotateDice(new Vector3(0.5f, -0.5f, 0)));
        }
    }

    private void MenuThemeVolume()
    {
        if (inMenu)
        {
            menuMusic.volume = Mathf.MoveTowards(menuMusic.volume, musicMaxVolume, musicInSpeed * Time.deltaTime);
        }
        else
        {
            menuMusic.volume = Mathf.MoveTowards(menuMusic.volume, 0, musicOutSpeed * Time.deltaTime);
        }
    }

    private void LoadLevel()
    {
        HideCrosses();

        SceneManager.LoadSceneAsync(UIScene, LoadSceneMode.Additive);

        SoundHandler.Instance.MakeSound(levelSelect, transform.position, volume, minPitch, maxPitch);

        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);

        lvlManager.LoadLevel(selectedLevel, pos);
        dMove.enabled = true;
        camFollow.inMenu = false;

        inMenu = false;
    }

    public void LoadMenu()
    {
        DisplayCrosses();

        camFollow.transform.position = Vector3.zero + (camFollow.transform.position - transform.position);
        transform.position = Vector3.zero;

        SceneManager.UnloadSceneAsync(UIScene);

        lvlManager.UnloadLevel();
        mMixer.currentLevel = 0;

        dMove.enabled = false;
        camFollow.inMenu = true;
        GameManager.Instance.ClearUndoList();

        inMenu = true;
    }

    private void GetSelectedSide()
    {
        float[] angles = { 0f, 0f, 0f, 0f, 0f, 0f };

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
            if (angles[i] < minAngle)
            {
                minAngle = angles[i];
                closestCharacter = i;
            }
        }

        selectedLevel = closestCharacter + 1;
    }

    private void DisplayCrosses()
    {
        int amount = GameManager.Instance.GetUnlockedLevels() - 1;

        for (int i = cross.Length - 1; i >= amount; i--)
        {
            cross[i].SetActive(true);
        }
    }

    private void HideCrosses()
    {
        for (int i = 0; i < cross.Length; i++)
        {
            cross[i].SetActive(false);
        }
    }

    private void CrossRejectAnim()
    {
        cross[selectedLevel - 2].GetComponent<Animator>().Play("Cross_Reject");
    }

    private IEnumerator RotateDice(Vector3 rotatePoint)
    {
        isRotating = true;

        SoundHandler.Instance.MakeSound(diceMove, transform.position, volume, minPitch, maxPitch);

        Vector3 pos = transform.position;
        Vector3 point = transform.position + rotatePoint;

        Vector3 axis = Vector3.Cross(Vector3.up, rotatePoint).normalized;
        float angle = 90;

        while (angle > 0)
        {
            float a = Time.deltaTime * 500f;
            transform.RotateAround(point, axis, a);
            angle -= a;

            transform.position = pos;

            yield return null;
        }

        // Round Rotation to an Int
        transform.RotateAround(point, axis, angle);
        Quaternion rot = transform.rotation;
        transform.rotation = Quaternion.Euler(Mathf.RoundToInt(rot.eulerAngles.x), Mathf.RoundToInt(rot.eulerAngles.y), Mathf.RoundToInt(rot.eulerAngles.z));

        // Round Position to an Int
        Vector3 intPos = transform.position;
        transform.position = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));

        GetSelectedSide();

        isRotating = false;
    }
}
