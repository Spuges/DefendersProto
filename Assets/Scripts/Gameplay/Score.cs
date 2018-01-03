using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int ScoreAmount = 50;

    public void GiveScore()
    {
        ScoreCounter.I.GiveScore(ScoreAmount);
    }
}
