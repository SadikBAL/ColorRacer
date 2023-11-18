using CommonEnums;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    public Page MainMenu;
    public Page EndLevel;
    public Page SelectLevel;
    public Page Hud;
    private UIState CurrentState = UIState.None;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateState(UIState State)
    {
        if (CurrentState == State)
            return;
        switch (CurrentState)
        {
            case UIState.MainMenu:
                MainMenu.Hide();
                break;
            case UIState.EndLevel:
                EndLevel.Hide();
                break;
            case UIState.InGame:
                Hud.Hide();
                break;
            case UIState.SelectLevel:
                SelectLevel.Hide();
                break;
        }
        CurrentState = State;
        switch (CurrentState) 
        {
            case UIState.MainMenu:
                MainMenu.Show();
                break;
            case UIState.EndLevel:
                EndLevel.Show();
                break;
            case UIState.InGame:
                Hud.Show();
                break;
            case UIState.SelectLevel:
                SelectLevel.Show();
                break;
        }
    }
}
