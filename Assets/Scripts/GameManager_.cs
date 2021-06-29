using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager_ : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject cameraObj;
    [SerializeField] UIManager uiManager;
    public SoundManager soundManager;

    public int Score => _score;
    public int Lives => _lives;
    private int _score = 0;
    private int _lives = 3;

    public static GameManager_ Instance
    {
        get
        {
            if (instance != null)
            return instance;
            throw new MissingReferenceException();
        }
    }
    private static GameManager_ instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        print("asd");
        if (_score + amount < int.MaxValue && amount > 0)
        {
            _score += amount;
            uiManager.UpdateScore(_score);
        }
    }

    public void AddLives(int amount)
    {
        if (_lives + amount <= 9)
        {
            _lives += amount;
            uiManager.UpdateLives(_lives);
        } else if (_lives + amount < 0)
        {
            Time.timeScale = 0;
        }
    }


}
