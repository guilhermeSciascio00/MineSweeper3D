using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialText;

    private void Start()
    {
        _tutorialText.SetActive(false);
    }

    public void TurnTutorialText()
    {
        _tutorialText.SetActive(!_tutorialText.activeSelf);
    }
}
