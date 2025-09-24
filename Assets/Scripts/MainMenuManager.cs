using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    //TODO Refactor this code tomorrow.

    [SerializeField] private GameObject _tutorialText;
    [SerializeField] private GameObject _tutorialText2;
    [SerializeField] private GameObject _nextTutorialBtn;
    [SerializeField] private GameObject _previousTutorialBtn;


    private void Start()
    {
        _tutorialText.SetActive(false);
        _nextTutorialBtn.SetActive(false);
    }

    public void TurnTutorialText()
    {
        _tutorialText.SetActive(!_tutorialText.activeSelf);
    }

    public void NextTutorialText()
    {

        _tutorialText.SetActive(false);
        _tutorialText2.SetActive(true);
    }

    public void PreviousTutorialText()
    {
        _tutorialText.SetActive(true);
        _tutorialText2.SetActive(false);
    }

    public void TurnNextTutorialBtnOn()
    {
        _nextTutorialBtn.SetActive(!_nextTutorialBtn.activeSelf);
        _previousTutorialBtn.SetActive(false);
        _tutorialText2.SetActive(false);
    }


    public void TurnPreviousButtonOn()
    {
        _nextTutorialBtn.SetActive(false);
        _previousTutorialBtn.SetActive(true);
    }

    public void TurnNextBtnOn()
    {
        _nextTutorialBtn.SetActive(true);
        _previousTutorialBtn.SetActive(false);
    }
}
