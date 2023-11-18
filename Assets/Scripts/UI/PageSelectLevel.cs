using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageSelectLevel : Page
{
    public GameObject ContentParent;
    public GameObject ContentPrefab;
    List<LevelScrollViewItem> LevelButtons;
    public override void Hide()
    {
        this.gameObject.SetActive(false);
        foreach (LevelScrollViewItem item in LevelButtons) 
        {
            item.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    public override void Show()
    {
        if(LevelButtons == null)
            LevelButtons = new List<LevelScrollViewItem>();
        this .gameObject.SetActive(true);
        List<LevelData> ListRef = GameManager.Instance.LevelManagerRef.CurrentData.LevelList;
        for(int i = 0; i < ListRef.Count;i++)
        {
            if(i > (LevelButtons.Count - 1))
            {
                LevelScrollViewItem TempItem = GameObject.Instantiate(ContentPrefab).GetComponent<LevelScrollViewItem>();
                TempItem.gameObject.transform.SetParent(ContentParent.transform, false);
                LevelButtons.Add(TempItem);
            }
            if (ListRef[i].IsLocked)
            {
                LevelButtons[i].ButtonImage.color = Color.red;
                LevelButtons[i].ButtonText.text = "Level " + (i + 1) + " [Locked]";
            }
            else if(ListRef[i].IsCompleted)
            {
                int Index = i;
                LevelButtons[i].ButtonImage.color = Color.green;
                LevelButtons[i].ButtonText.text = "Level " + (i + 1) + " [Completed]";
                LevelButtons[i].gameObject.GetComponent<Button>().onClick.AddListener(() => SelectLevelButtonClicked(Index));
            }
            else
            {
                int Index = i;
                LevelButtons[i].ButtonImage.color = Color.yellow;
                LevelButtons[i].ButtonText.text = "Level " + (i + 1);
                LevelButtons[i].gameObject.GetComponent<Button>().onClick.AddListener(() => SelectLevelButtonClicked(Index));
            }
        }
    }

    private void SelectLevelButtonClicked(int index)
    {
        GameManager.Instance.LoadLevel(index);
    }
}
