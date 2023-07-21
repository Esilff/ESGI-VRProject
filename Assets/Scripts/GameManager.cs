using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance of the GameManager
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject ballDispenser;
    [SerializeField]
    private GameObject pinParent;
    [SerializeField]
    private GameObject[] pins;

    public GameObject currentBall;

    [Header("Game Variables")]
    [SerializeField]
    private int currentFrame = 1;
    [SerializeField]
    private int maxFrames = 10;
    [SerializeField]
    private int currentTry = 1;
    [SerializeField]
    private int maxTry = 2;

    // Score variables
    [SerializeField]
    private int playerScore = 0;

    private bool isGameActive = false;
    private bool isFrameActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        pins = new GameObject[pinParent.transform.childCount];

        for (int i = 0; i < pinParent.transform.childCount; i++)
        {
            pins[i] = pinParent.transform.GetChild(i).gameObject;
        }
        ResetGame();
    }

    private void ResetGame()
    {
        currentFrame = 1;
        isGameActive = false;

        playerScore = 0;

        StartNewFrame();
    }

    private void StartNewFrame()
    {
        if (currentFrame > maxFrames)
        {
            EndGame();
            return;
        }

        isFrameActive = true;
        Debug.Log("Frame " + currentFrame);

        // Reset pins and ball for a new turn
        ResetPins();
        ResetBall();

        // Add UI elements to display the current frame and player here

        // Wait for the player to throw the ball
    }

    private void ResetPins()
    {
        foreach (GameObject pin in pins)
        {
            PinKnockdownDetector pKD = pin.GetComponent<PinKnockdownDetector>();

            // Reset the pin to the upright position
            pin.SetActive(true);
            pin.transform.localPosition = pKD.startPosition;
            pin.transform.GetChild(0).rotation = pKD.startRotation;

            Rigidbody pinRigidbody = pin.transform.GetChild(0).GetComponent<Rigidbody>();
            pinRigidbody.velocity = Vector3.zero;
            pinRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void SetPins()
    {
        foreach (GameObject pin in pins)
        {
            PinKnockdownDetector pKD = pin.GetComponent<PinKnockdownDetector>();

            // Set only the upright pins back
            pin.SetActive(pKD.IsUpright());
            pin.transform.localPosition = pKD.startPosition;
            pin.transform.GetChild(0).rotation = pKD.startRotation;

            Rigidbody pinRigidbody = pin.transform.GetChild(0).GetComponent<Rigidbody>();
            pinRigidbody.velocity = Vector3.zero;
            pinRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void ResetBall()
    {
        // Reset the position and velocity of the bowling ball
        currentBall.transform.position = ballDispenser.transform.GetChild(1).position;
        currentBall.GetComponent<Rigidbody>().velocity = Vector3.forward * 0.1f;
    }

    public void OnBallThrown()
    {
        // Detect pin knockdowns and calculate the score for this turn
        int knockedDownCount = CalculateKnockedDownPins();
        playerScore += knockedDownCount;

        // Move to the next player or next frame if necessary
        NextTurn();
    }

    private int CalculateKnockedDownPins()
    {
        int knockedDownCount = 0;

        foreach (GameObject pin in pins)
        {
            PinKnockdownDetector pKD = pin.GetComponent<PinKnockdownDetector>();

            // Check if the pin is knocked down based on its current rotation
            if (!pKD.IsUpright())
            {
                knockedDownCount++;
            }
        }
        return knockedDownCount;
    }

    private void NextTurn()
    {
        if (currentTry <= maxTry && CalculateKnockedDownPins() < 10)
        {
            currentTry++;

            ResetBall();
            SetPins();
        }
        else
        {
            currentFrame++;
            currentTry = 1;

            StartNewFrame();
        }
    }

    private void EndGame()
    {
        // Display the final scores and any other end-game logic
        Debug.Log("Game Over");
    }
}

