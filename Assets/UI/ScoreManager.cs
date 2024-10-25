using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI leftScoreText;
    [SerializeField] private TextMeshProUGUI rightScoreText;

    private int leftScore = 0;
    private int rightScore = 0;

    public int LeftScore
    {
        get { return leftScore; }
        set { leftScoreText.text = string.Format("{0:D2}", leftScore = value); }
    }

    public int RightScore
    {
        get { return rightScore; }
        set { rightScoreText.text = string.Format("{0:D2}", rightScore = value); }
    }

    public void ResetScores()
    {
        LeftScore = RightScore = 0;
    }
}
