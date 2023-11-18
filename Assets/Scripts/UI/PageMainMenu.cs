using CommonEnums;
using UnityEngine.UI;

public class PageMainMenu : Page
{
    public Button PlayButton;
    public override void Hide()
    {
        this.gameObject.SetActive(false);
        PlayButton.onClick.RemoveAllListeners();
    }
    public override void Show()
    {
        this.gameObject.SetActive(true);
        PlayButton.onClick.AddListener(PlayButtonClicked);
    }
    private void PlayButtonClicked()
    {
        GameManager.Instance.UpdateGameState(GameState.SelectLevel);
    }
}
