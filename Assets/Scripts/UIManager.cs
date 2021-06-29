using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text liveText;

    public void UpdateScore(int score)
    {
        scoreText.text = $"x {score.ToString().PadLeft(7, '0')}";
    }

    public void UpdateLives(int live)
    {
        liveText.text = $"x {live}";
    }
}
