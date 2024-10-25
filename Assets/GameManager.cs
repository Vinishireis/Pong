using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    private enum Difficulty { Easy, Normal, Hard }

    [Header("UI")]
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private ScoreManager scoreManager;
    [Header("Game Entities")]
    [SerializeField] private Ball ball;
    [SerializeField] private Paddle leftPaddle;
    [SerializeField] private Paddle rightPaddle;
    [SerializeField] private Goal leftGoal;
    [SerializeField] private Goal rightGoal;
    [Header("Gameplay")]
    [SerializeField] private float gameSpeed = 1;
    [SerializeField] private int winningScore = 11;

    private bool gameInProgress = false;
    private Difficulty difficulty;
    private UniversalAdditionalCameraData mainCameraData;

    private void OnValidate()
    {
        gameSpeed = Mathf.Max(0.001f, gameSpeed);
        winningScore = Mathf.Max(1, winningScore);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        mainCameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
        pauseMenu.OnDifficultyChanged += OnDifficultyChanged;
        pauseMenu.gameObject.SetActive(true);
        leftGoal.OnScoring += OnLeftGoalScoring;
        rightGoal.OnScoring += OnRightGoalScoring;
        Pause();
    }

    private void Update()
    {
        if (gameInProgress && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.Displayed = !pauseMenu.Displayed;
            Time.timeScale = pauseMenu.Displayed ? 0 : gameSpeed;
        }
    }

    public void PlayerVsCPU()
    {
        CleanUp();
        leftPaddle.gameObject.AddComponent<PlayerInput>();
        SetPaddleCPU(rightPaddle, difficulty);
        Unpause();
    }

    public void PlayerVsPlayer()
    {
        CleanUp();
        leftPaddle.gameObject.AddComponent<PlayerOneInput>();
        rightPaddle.gameObject.AddComponent<PlayerTwoInput>();
        Unpause();
    }

    public void CPUVsCPU()
    {
        CleanUp();
        SetPaddleCPU(leftPaddle, difficulty);
        SetPaddleCPU(rightPaddle, difficulty);
        Unpause();
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void CleanUp()
    {
        scoreManager.ResetScores();
        ball.ResetPos();
        leftPaddle.ResetPos();
        rightPaddle.ResetPos();
        DestroyImmediate(leftPaddle.GetComponent<Controller>());
        DestroyImmediate(rightPaddle.GetComponent<Controller>());
    }

    private void SetPaddleCPU(Paddle paddle, Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                paddle.gameObject.AddComponent<EasyCPU>();
                break;
            case Difficulty.Normal:
                paddle.gameObject.AddComponent<NormalCPU>();
                break;
            case Difficulty.Hard:
                paddle.gameObject.AddComponent<HardCPU>();
                break;
        }
    }

    private void Pause()
    {
        pauseMenu.Displayed = true;
        Time.timeScale = 0;
    }

    private void Unpause()
    {
        pauseMenu.Displayed = false;
        Time.timeScale = gameSpeed;
        gameInProgress = true;
    }

    private void OnDifficultyChanged(int index)
    {
        difficulty = (Difficulty)index;

        if (gameInProgress)
        {
            CPUController CPUController;
            CPUController = leftPaddle.GetComponent<CPUController>();
            if (CPUController != null)
            {
                DestroyImmediate(CPUController);
                SetPaddleCPU(leftPaddle, difficulty);
            }
            CPUController = rightPaddle.GetComponent<CPUController>();
            if (CPUController != null)
            {
                DestroyImmediate(CPUController);
                SetPaddleCPU(rightPaddle, difficulty);
            }
        }
    }

    private void OnLeftGoalScoring()
    {
        scoreManager.RightScore++;
        if (scoreManager.RightScore == winningScore)
        {
            gameInProgress = false;
            Pause();
        }
    }

    private void OnRightGoalScoring()
    {
        scoreManager.LeftScore++;
        if (scoreManager.LeftScore == winningScore)
        {
            gameInProgress = false;
            Pause();
        }
    }
}
