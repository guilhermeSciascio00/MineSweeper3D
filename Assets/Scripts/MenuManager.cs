using System;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI _elapsedTimeText;
    [SerializeField] float _elapsedTime;
    bool _timeFlag = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.OnFirstTileRevealed += GameStart;
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
        
    }
    
    void UpdateTimeText()
    {
        TimeSpan t = TimeSpan.FromSeconds(_elapsedTime);
        _elapsedTimeText.text = $"{t.Minutes:D2}:{t.Seconds:D2}";
    }
}
