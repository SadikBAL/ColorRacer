using CommonEnums;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public UIManager UIManagerRef;
    public LevelManager LevelManagerRef;
    public Fabrica FabricaRef;
    public LevelLocationDatas LevelLocationDatas;
    private GameState CurrentState = GameState.None;
    private GameObject[] LeftBackList = new GameObject[3] { null, null, null };
    private GameObject[] MiddleBackList = new GameObject[3] { null, null, null };
    private GameObject[] RightBackList = new GameObject[3] { null, null, null };
    private GameObject[,] BlockList = new GameObject[3,9];
    private GameObject Player = null;
    private LevelConfig LevelConfigRef = null;
    private float CurrentTime = 0.0f;
    void Awake()
    {
        // Check if instance already exists
        if (Instance == null)
        {
            // If not, set instance to this
            Instance = this;
        }
        // If instance already exists and it's not this:
        else if (Instance != this)
        {
            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        // Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateGameState(GameState.Menu);
    }
    // Update is called once per frame
    void Update()
    {
        if(CheckState(GameState.InGame))
        {
            BacgroundUpdate();
            TimerControl();
        }
        
    }
    public void UpdateGameState(GameState State)
    {

        if (State == CurrentState)
            return;
        CurrentState = State;
        switch (State) 
        {
            case GameState.None:
                break;
            case GameState.InGame:
                UIManagerRef.UpdateState(UIState.InGame);
                InitLevel();
                break;
            case GameState.EndGame:
                UIManagerRef.UpdateState(UIState.EndLevel);
                break;
            case GameState.Menu:
                ClearLevel();
                UIManagerRef.UpdateState(UIState.MainMenu);
                break;
            case GameState.SelectLevel:
                UIManagerRef.UpdateState(UIState.SelectLevel);
                break;
        }
    }
    public void GameOver()
    {
        UpdateGameState(GameState.EndGame);
        if (UIManagerRef.EndLevel is PageEndLevel)
        {
            PageEndLevel child = (PageEndLevel)UIManagerRef.EndLevel;
            child.UpdateData(new UIDataClass.EndLevelData { IsWon = false, IsNextLevelButtonActive = false, IsRestartButtonActive = true });
        }
    }
    public void GameWon()
    {
        UpdateGameState(GameState.EndGame);
        LevelManagerRef.LevelPassed(LevelConfigRef);
        if (UIManagerRef.EndLevel is PageEndLevel)
        {
            PageEndLevel child = (PageEndLevel)UIManagerRef.EndLevel;
            child.UpdateData(new UIDataClass.EndLevelData { IsWon = true, IsNextLevelButtonActive = true, IsRestartButtonActive = false });
        }
    }
    //First Create for Player Bacground and Blocks.
    private void InitLevel()
    {
        //Bacground Init
        for (int i = 0; i < 3; i++)
        {
            LeftBackList[i] = FabricaRef.GetRandomBackground();
            LeftBackList[i].transform.position = LevelLocationDatas.LeftBackStartLocation.position + new Vector3(0, LevelLocationDatas.BackHeight * i, 0);

            MiddleBackList[i] = FabricaRef.GetRandomBackground();
            MiddleBackList[i].transform.position = LevelLocationDatas.MidBackStartLocation.position + new Vector3(0, LevelLocationDatas.BackHeight * i, 0);

            RightBackList[i] = FabricaRef.GetRandomBackground();
            RightBackList[i].transform.position = LevelLocationDatas.RightBackStartLocation.position + new Vector3(0, LevelLocationDatas.BackHeight * i, 0);
        }
        Player = FabricaRef.GetPlayer();
        Player.transform.position = LevelLocationDatas.PlayerStartLocation.position;
        //Block Init
        for (int i = 0;i < 3;i++)
        {
            int Counter = 0;
            for(int j= 0; j< 9;j++)
            {
                if (j % 3 == 0)
                    Counter = 0;
                if(Counter < 2)
                {
                    BlockList[i, j] = FabricaRef.GetRandomBlock(100 - (int)LevelConfigRef.BlockFrequency);
                    if (BlockList[i, j] != null)
                    {
                        Counter++;

                        BlockList[i, j].gameObject.transform.position = LevelLocationDatas.MidBackStartLocation.position + new Vector3(-2 + ((j % 3) * 2), (LevelLocationDatas.BackHeight * i) -3 + (j / 3) * 3 , 0);
                    }
                }
                
               
            }
        }
    }
    //Clear Blocks Player and Background Lists
    private void ClearLevel()
    {
        //Clear Back Lists
        for (int i = 0; i < 3; i++)
        {
            if (LeftBackList[i])
            {
                LeftBackList[i].GetComponent<PooledPrefab>().Pool.ReturnObject(LeftBackList[i]);
                LeftBackList[i] = null;
            }
            if (MiddleBackList[i])
            {
                MiddleBackList[i].GetComponent<PooledPrefab>().Pool.ReturnObject(MiddleBackList[i]);
                MiddleBackList[i] = null;
            }
            if (RightBackList[i])
            {
                RightBackList[i].GetComponent<PooledPrefab>().Pool.ReturnObject(RightBackList[i]);
                RightBackList[i] = null;
            }
        }
        //Clear Player
        if(Player)
        {
            Player.GetComponent<PooledPrefab>()?.Pool.ReturnObject(Player);
            Player = null;
        }
        //Clear Block List
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (BlockList[i, j] != null)
                {
                    BlockList[i, j].gameObject.GetComponent<PooledPrefab>()?.Pool.ReturnObject(BlockList[i, j]);
                    BlockList[i, j] = null;
                }
            }
        }

        CurrentTime = 0.0f;
    }
    //Slide Bacground and Blocks.
    private void BacgroundUpdate()
    {
        //Update BacgList and RegenaritonLoop
        for (int i = 0; i < 3; i++)
        {
            if (LeftBackList[i] != null)
            {
                LeftBackList[i].transform.position = new Vector2(LeftBackList[i].transform.position.x, LeftBackList[i].transform.position.y - Time.deltaTime * LevelConfigRef.SlideSpeed);
            }
              
            if (RightBackList[i] != null)
            {
                RightBackList[i].transform.position = new Vector2(RightBackList[i].transform.position.x, RightBackList[i].transform.position.y - Time.deltaTime * LevelConfigRef.SlideSpeed);
            }
               
            if (MiddleBackList[i] != null)
            {
                MiddleBackList[i].transform.position = new Vector2(MiddleBackList[i].transform.position.x, MiddleBackList[i].transform.position.y - Time.deltaTime * LevelConfigRef.SlideSpeed);
            }
        }
        for (int i = 0; i < 3; i++)
        {
            if (LeftBackList[i] != null)
            {
                if (LeftBackList[i].transform.position.y <= LevelLocationDatas.EndLocation.position.y)
                {
                    LeftBackList[i].GetComponent<PooledPrefab>().Pool.ReturnObject(LeftBackList[i]);
                    LeftBackList[i] = FabricaRef.GetRandomBackground();
                    //LeftBackList[i].transform.position = LevelLocationDatas.LeftBackStartLocation.position + new Vector3(0, LevelLocationDatas.BackHeight, 0);
                    LeftBackList[i].transform.position = LeftBackList[(i + 2) % 3].transform.position + new Vector3(0.0f, (float)LevelLocationDatas.BackHeight, 0.0f);
                }
            }

            if (RightBackList[i] != null)
            {
                if (RightBackList[i].transform.position.y <= LevelLocationDatas.EndLocation.position.y)
                {
                    RightBackList[i].GetComponent<PooledPrefab>().Pool.ReturnObject(RightBackList[i]);
                    RightBackList[i] = FabricaRef.GetRandomBackground();
                    //RightBackList[i].transform.position = LevelLocationDatas.RightBackStartLocation.position + new Vector3(0, LevelLocationDatas.BackHeight, 0);
                    RightBackList[i].transform.position = RightBackList[(i + 2) % 3].transform.position + new Vector3(0.0f, (float)LevelLocationDatas.BackHeight, 0.0f);
                }
            }

            if (MiddleBackList[i] != null)
            {
                if (MiddleBackList[i].transform.position.y <= LevelLocationDatas.EndLocation.position.y)
                {
                    MiddleBackList[i].GetComponent<PooledPrefab>().Pool.ReturnObject(MiddleBackList[i]);
                    MiddleBackList[i] = FabricaRef.GetRandomBackground();
                    //MiddleBackList[i].transform.position = LevelLocationDatas.MidBackStartLocation.position + new Vector3(0, LevelLocationDatas.BackHeight, 0);
                    MiddleBackList[i].transform.position = MiddleBackList[(i + 2) % 3].transform.position + new Vector3(0.0f, (float)LevelLocationDatas.BackHeight, 0.0f);
                    break;
                }
            }
        }
        //Update Block and RegenaritonLoop
        for (int i = 0; i < 3; i ++)
        {
            for(int j = 0; j < 9; j ++) 
            { 
                if(BlockList[i, j] != null)
                {
                    BlockList[i, j].transform.position = new Vector2(BlockList[i, j].transform.position.x, BlockList[i, j].transform.position.y - Time.deltaTime * LevelConfigRef.SlideSpeed);
                    if (BlockList[i, j].transform.position.y <= LevelLocationDatas.EndLocation.position.y)
                    {
                        int Counter = 0;
                        for (int k = 0; k < 9; k++)
                        {
                            if (BlockList[i, k] != null)
                            {
                                BlockList[i, k].gameObject.GetComponent<PooledPrefab>()?.Pool.ReturnObject(BlockList[i, k]);
                                BlockList[i, k] = null;
                            }
                            if (k % 3 == 0)
                                Counter = 0;
                            if (Counter < 2)
                            {
                                BlockList[i, k] = FabricaRef.GetRandomBlock(100 - (int)LevelConfigRef.BlockFrequency);
                                if (BlockList[i, k] != null)
                                {
                                    Counter++;

                                    BlockList[i, k].gameObject.transform.position = LevelLocationDatas.MidBackStartLocation.position + new Vector3(-2 + ((k % 3) * 2), (LevelLocationDatas.BackHeight) - 3 + (k / 3) * 3 - (Time.deltaTime * LevelConfigRef.SlideSpeed), 0);
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
    //Calculate remaning time.
    private void TimerControl()
    {
        CurrentTime += Time.deltaTime;
        if(GetRemainingTime() <= 0)
        {
            GameWon();
        }
        
    }
    public bool CheckState(GameState state)
    {
        return CurrentState == state;
    }
    public void LoadLevel(int index)
    {
        if(index < 0 || index >= LevelManagerRef.ConfigList.Count)
        {
            Debug.Log("Level not found !");
            return;
        }
        LevelConfigRef = LevelManagerRef.ConfigList[index];
        UpdateGameState(GameState.InGame);
    }
    public void LoadNextLevel()
    {
        LoadLevel(LevelManagerRef.FindNextLevel(LevelConfigRef));
    }
    public float GetRemainingTime()
    {
        if(LevelConfigRef)
        {
            return Mathf.Max(LevelConfigRef.TargetTime - CurrentTime,0.0f);
        }
        return 0;
    }
    //Get player scale speed for PlayerController.
    public float GetPlayerScaleSpeed()
    {
        if(LevelConfigRef) 
        {
            return LevelConfigRef.PlayerScaleSpeed;
        }
        return 0;
    }

  
}
