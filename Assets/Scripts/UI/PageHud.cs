using TMPro;

public class PageHud : Page
{
    public TextMeshProUGUI TimerText;
    public override void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public override void Show()
    {
        this.gameObject.SetActive(true);
    }
    void Update()
    {
        if(GameManager.Instance.CheckState(CommonEnums.GameState.InGame))
        {
            TimerText.text = "Remaining Time : " + GameManager.Instance.GetRemainingTime().ToString("F2");
        }
    }
}
