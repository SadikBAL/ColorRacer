using CommonEnums;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private ColorType CurrentAreaColor;
    private ColorType CurrentPlayerColor;
    private int PlayerMoveSpeed = 10;
    private ColorType[] ColorCircle = new ColorType[3] { ColorType.Red,ColorType.Green,ColorType.Blue};
    private int ColorCircleIndex = 0;
    public Material PlayerMaterial = null;

    public void Init()
    {
        //Reset this datas on Init because of Pooling.
        CurrentAreaColor = ColorType.Unknown;
        CurrentPlayerColor = ColorType.Unknown;
        ColorCircleIndex = 0;
        UpdatePlayerColor(ColorCircleIndex);
    }
    void Update()
    {
        HandleInputs();
    }

    //Handle Player Control in GameState
    void HandleInputs()
    {
        if (!GameManager.Instance.CheckState(GameState.InGame))
        {
            return;
        }
        MovementControl();
        PlayerExplosionControl();


    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Block")
        {
            GameManager.Instance.GameOver();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        ColorType type = GetType(other.gameObject.tag);
        if (type != ColorType.Unknown)
        {
            CurrentAreaColor = type;
        }
    }
    private ColorType GetType(string tag)
    {
        switch (tag) 
        {
            case "Red":
                return ColorType.Red;
            case "Green":
                return ColorType.Green;
            case "Blue":
                return ColorType.Blue;
        }
        return ColorType.Unknown;
    }
    //Change player color with ColorCircle array.
    private void UpdatePlayerColor(int Index)
    {
        if(Index < 0)
        {
            ColorCircleIndex = Index + ColorCircle.Length;
        }
        else
        {
            ColorCircleIndex = Index % ColorCircle.Length;
        }
        CurrentPlayerColor = ColorCircle[ColorCircleIndex];
        if(PlayerMaterial)
        {
            switch (CurrentPlayerColor)
            {
                case ColorType.Red:
                    PlayerMaterial.color = Color.red;
                    break;
                case ColorType.Green:
                    PlayerMaterial.color = Color.green;
                    break;
                case ColorType.Blue:
                    PlayerMaterial.color = Color.blue;
                    break;
            }
        }

    }
    //Player movement control.
    private void MovementControl()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position = new Vector3(Mathf.Max(this.transform.position.x - Time.deltaTime * PlayerMoveSpeed, -2.5f), this.transform.position.y, this.transform.position.z);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position = new Vector3(Mathf.Min(this.transform.position.x + Time.deltaTime * PlayerMoveSpeed, 2.5f), this.transform.position.y, this.transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpdatePlayerColor(ColorCircleIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            UpdatePlayerColor(ColorCircleIndex - 1);
        }
    }
    //Player scale control for explosin EndGame control.
    private void PlayerExplosionControl()
    {
        if(CurrentAreaColor != ColorType.Unknown && CurrentAreaColor != CurrentPlayerColor)
        {
            this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x + Time.deltaTime * GameManager.Instance.GetPlayerScaleSpeed(), this.gameObject.transform.localScale.y + Time.deltaTime * GameManager.Instance.GetPlayerScaleSpeed(), this.gameObject.transform.localScale.z);
            if (this.gameObject.transform.localScale.x >= 1.5f)
            {
                this.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                StartCoroutine(GameManager.Instance.FabricaRef.BlowEffect(this.gameObject.transform.position));
                this.gameObject.GetComponent<PooledPrefab>().Pool.ReturnObject(this.gameObject);
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(Mathf.Max(this.gameObject.transform.localScale.x - Time.deltaTime * GameManager.Instance.GetPlayerScaleSpeed(), 0.5f) , Mathf.Max(this.gameObject.transform.localScale.y - Time.deltaTime * GameManager.Instance.GetPlayerScaleSpeed(), 0.5f), this.gameObject.transform.localScale.z);
        }
    }

}
