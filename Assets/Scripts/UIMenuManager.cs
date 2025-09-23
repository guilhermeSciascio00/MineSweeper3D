using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class UIMenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] List<TextMeshProUGUI> _elapsedTimeTexts;
    [SerializeField] float _elapsedTime;
    bool _timeFlag = false;

    [Header("References")]
    [SerializeField] GameInputManager _gameInptManager;
    [SerializeField] GameObject _gameMenuRef;
    [SerializeField] GameObject _gameOverScreen;
    [SerializeField] GameObject _gameWonScreen;

    private void OnEnable()
    {
        _gameMenuRef.SetActive(false);
        _gameOverScreen.SetActive(false);
        _gameWonScreen.SetActive(false);

        EventManager.OnFirstTileRevealed += GameStart;
        EventManager.OnGameWon += GameWonMenu;
        EventManager.OnGameOver += GameOverMenu;
    }

    private void OnDisable()
    {
        EventManager.OnFirstTileRevealed -= GameStart;
        EventManager.OnGameWon -= GameWonMenu;
        EventManager.OnGameOver -= GameOverMenu;
    }

    private void GameStart(Tile obj)
    {
        _timeFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeFlag)
        {
            _elapsedTime += Time.deltaTime;
            UpdateTimeText();
        }
        
        ShowGameMenu();

    }
    
    void UpdateTimeText()
    {
        TimeSpan t = TimeSpan.FromSeconds(_elapsedTime);

        foreach(TextMeshProUGUI text in _elapsedTimeTexts)
        {
            text.text = $"{t.Minutes:D2}:{t.Seconds:D2}";
        }

    }

    void GameWonMenu()
    {
        _timeFlag = false;
        _gameWonScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    void GameOverMenu()
    {
        _timeFlag = false;
        _gameOverScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }



    void ShowGameMenu()
    {
        if (_gameInptManager.IsPauseButtonPressed())
        {
            _gameMenuRef.SetActive(!_gameMenuRef.activeSelf);
            PauseGame();
        }
        
    }

    void PauseGame()
    {
        if (_gameInptManager.IsPauseButtonPressed() && Time.timeScale >= 1f)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }
        else if(_gameInptManager.IsPauseButtonPressed() && Time.timeScale <= 0f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
    }
}
