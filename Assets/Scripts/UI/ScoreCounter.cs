using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Helpers;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter I { get; set; }

    public Text ScoreText;
    public string ScoringFormat = "Score: {0}";

    private int curScore = 0;

    public string PlayerPrefsID = "PLAYER_SCORE";

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        ResetScore();
    }

    public void ResetScore()
    {
        curScore = 0;
        SetScore(0);
    }

    public void GiveScore(int count)
    {
        curScore += count;
        SetScore(curScore);
    }

    public void SetScore(int count)
    {
        if(ScoreText)
        {
            ScoreText.text = string.Format(ScoringFormat, count);
        }
    }
}
