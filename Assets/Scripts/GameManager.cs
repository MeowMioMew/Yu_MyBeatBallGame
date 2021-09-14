using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject paddlePrefab;
    public Text scoreText; 
    public Text lifeText;
    public Text levelText;
    public Text highscoreText;


    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelLvlComplete;
    public GameObject panelGameOver;

    public GameObject[] levels;
    public static GameManager Instance { get; private set; }

    public enum State {MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, GAMEOVER }
    State _state;
    GameObject _currentBall;
    GameObject _currentLvl;
    bool _isSwitching = true;


    private int _score;

    public int Score
    {
        get { return _score; }
        set { _score = value;
            scoreText.text = "SCORE:" + _score;
        }
           
    }

    private int _level;

    public int Level
    {
        get { return _level; }
        set { _level = value;
            levelText.text = "LEVEL: " + _level;
        }
    }

    private int _ball;

    public int Ball
    {
        get { return _ball; }
        set { _ball = value;
            lifeText.text = "BALLS: " + _ball;
        }
    }

    public void PlayClick()
    {
        SwitchState(State.INIT);
    }

    void Start()
    {
        Instance = this;
        SwitchState(State.MENU);
        //PlayerPrefs.DeleteKey("highscore");
    }

    public void SwitchState(State newState, float delay = 0) 
    {
        //EndState();
        //BeginState(newState);
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        EndState();
        BeginState(newState);
        _state = newState;
        _isSwitching = false;
    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                Cursor.visible = true;
                highscoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("highscore");
                panelMenu.SetActive(true);
                break;
            case State.INIT:
                
                Cursor.visible = false;
                panelPlay.SetActive(true);
                Score = 0;
                Level = 0;
                Ball = 3;
                  if (_currentLvl != null)
                  {
                      Destroy(_currentLvl);
                  } 
                Instantiate(paddlePrefab);
                SwitchState(State.LOADLEVEL);
                
                break;
            case State.PLAY:
                
                break;
            case State.LEVELCOMPLETED:
                Destroy(_currentBall);
                Destroy(_currentLvl);
                Level++;
                panelLvlComplete.SetActive(true);
                SwitchState(State.LOADLEVEL, 2f);
                break;
            case State.LOADLEVEL:
                if (Level >= levels.Length)
                {
                    SwitchState(State.GAMEOVER);
                }
                else 
                {
                    _currentLvl = Instantiate(levels[Level]);
                    SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
               if(Score > PlayerPrefs.GetInt("highscore"))
                {
                    PlayerPrefs.SetInt("highscore", Score);
                }  
                panelGameOver.SetActive(true);
                break;
        }
    }
    private void EndState()
    {
        switch (_state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                panelLvlComplete.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panelPlay.SetActive(false);
                panelGameOver.SetActive(false);
                break;
        }
    }

    void Update()
    {
        switch (_state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if (_currentBall == null)
                {
                    if (Ball > 0)
                    {
                        _currentBall = Instantiate(ballPrefab);
                    }
                    else
                    {
                        SwitchState(State.GAMEOVER);
                    }
                }

                if(_currentBall != null && _currentLvl.transform.childCount == 0 && ! _isSwitching)
                {
                    SwitchState(State.LEVELCOMPLETED);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                if (Input.anyKeyDown)
                {
                    SwitchState(State.MENU);
                }
                break;
        }
    }
}
