using CommonEnums;
using TMPro;
using UIDataClass;
using UnityEngine.UI;

public class PageEndLevel : Page
{
    public TextMeshProUGUI TitleText;
    public Button NextLevelButton;
    public Button RestartButton;
    public Button MainMenuButton;
    public override void Hide()
    {
        this.gameObject.SetActive(false);
        NextLevelButton.onClick.RemoveAllListeners();
        RestartButton.onClick.RemoveAllListeners();
        MainMenuButton.onClick.RemoveAllListeners();
    }
    public override void Show()
    {
        this.gameObject.SetActive(true);
        MainMenuButton.onClick.AddListener(MainMenuButtonClicked);
        RestartButton.onClick.AddListener(RestartButtonClicked);
        NextLevelButton.onClick.AddListener(NextLevelButtonClicked);
    }

    private void NextLevelButtonClicked()
    {
        GameManager.Instance.UpdateGameState(GameState.Menu);
        GameManager.Instance.LoadNextLevel();
    }

    private void RestartButtonClicked()
    {
        GameManager.Instance.UpdateGameState(GameState.Menu);
        GameManager.Instance.UpdateGameState(GameState.InGame);
    }

    private void MainMenuButtonClicked()
    {
        GameManager.Instance.UpdateGameState(GameState.Menu);
    }
    public void UpdateData(EndLevelData Data)
    {
        if (Data != null) 
        {
            if(Data.IsWon)
            {
                TitleText.text = "WON";
            }
            else 
            {
                TitleText.text = "LOST";
            }
            NextLevelButton.gameObject.SetActive(Data.IsNextLevelButtonActive);
            RestartButton.gameObject.SetActive(Data.IsRestartButtonActive);
            
        }
    }
}
