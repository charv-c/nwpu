using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("游戏设置")]
    public int maxBoardPoints = 20; // 游戏板最大点数
    public bool enableSound = true; // 是否启用音效
    
    [Header("UI组件")]
    public Button rollDiceButton; // 掷骰子按钮
    public TextMeshProUGUI diceResultText; // 骰子结果文本
    
    [Header("游戏对象")]
    public GameObject playerChess; // 玩家棋子
    public GameObject diceObject; // 骰子对象
    public List<GameObject> boardPointObjects = new List<GameObject>(); // 游戏板点位对象
    
    // 私有变量
    private BoardManager boardManager;
    private Dice dice;
    private Chess playerChessComponent;
    
    // 单例模式
    public static GameManager Instance { get; private set; }
    
    void Awake()
    {
        // 单例模式设置
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        InitializeGame();
    }
    
    /// <summary>
    /// 初始化游戏
    /// </summary>
    void InitializeGame()
    {
        // 获取组件
        boardManager = GetComponent<BoardManager>();
        if (boardManager == null)
        {
            boardManager = gameObject.AddComponent<BoardManager>();
        }
        
        dice = diceObject?.GetComponent<Dice>();
        playerChessComponent = playerChess?.GetComponent<Chess>();
        
        // 设置BoardManager
        SetupBoardManager();
        
        // 设置UI
        SetupUI();
        
        // 设置事件监听
        SetupEventListeners();
    }
    
    /// <summary>
    /// 设置BoardManager
    /// </summary>
    void SetupBoardManager()
    {
        if (boardManager != null)
        {
            // 设置游戏板点位
            boardManager.boardPoints.Clear();
            foreach (GameObject pointObj in boardPointObjects)
            {
                if (pointObj != null)
                {
                    boardManager.boardPoints.Add(pointObj.transform);
                }
            }
            
            boardManager.totalBoardPoints = maxBoardPoints;
            
            // 设置UI组件
            boardManager.rollDiceButton = rollDiceButton;
            boardManager.diceResultText = diceResultText;
            
            // 设置玩家棋子引用
            if (playerChess != null)
            {
                boardManager.playerChess = playerChess.transform;
            }
            else
            {
                Debug.LogError("GameManager: playerChess未设置！");
            }
        }
    }
    
    /// <summary>
    /// 设置UI
    /// </summary>
    void SetupUI()
    {
        // 初始化文本显示
        if (diceResultText != null)
        {
            diceResultText.text = "骰子点数: -";
        }
    }
    
    /// <summary>
    /// 设置事件监听
    /// </summary>
    void SetupEventListeners()
    {
        // 监听玩家移动完成事件
        if (boardManager != null)
        {
            boardManager.OnPlayerMoveComplete += OnPlayerMoveComplete;
        }
        
        // 监听骰子掷完事件
        if (dice != null)
        {
            dice.OnDiceRollComplete += OnDiceRollComplete;
        }
    }
    
    /// <summary>
    /// 玩家移动完成回调
    /// </summary>
    /// <param name="position">到达的位置</param>
    void OnPlayerMoveComplete(int position)
    {
        // 检查是否完成一圈
        if (position == 0)
        {
            Debug.Log("恭喜！玩家完成了一圈！");
        }
    }
    
    /// <summary>
    /// 骰子掷完回调
    /// </summary>
    /// <param name="result">骰子结果</param>
    void OnDiceRollComplete(int result)
    {
        Debug.Log($"掷出了 {result} 点！");
    }
    
    /// <summary>
    /// 重置游戏
    /// </summary>
    public void ResetGame()
    {
        if (boardManager != null)
        {
            boardManager.ResetGame();
        }
        
        if (playerChessComponent != null)
        {
            playerChessComponent.ResetPosition();
        }
    }
    
    /// <summary>
    /// 设置音效开关
    /// </summary>
    /// <param name="enabled">是否启用</param>
    public void SetSoundEnabled(bool enabled)
    {
        enableSound = enabled;
        
        // 设置所有AudioSource
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.mute = !enabled;
        }
    }
    
    /// <summary>
    /// 获取当前游戏状态
    /// </summary>
    /// <returns>游戏状态信息</returns>
    public string GetGameStatus()
    {
        if (boardManager != null)
        {
            return $"玩家位置: {boardManager.GetCurrentPlayerPosition() + 1}, 是否移动中: {boardManager.IsPlayerMoving()}";
        }
        return "游戏未初始化";
    }
    
    void OnDestroy()
    {
        // 取消事件监听
        if (boardManager != null)
        {
            boardManager.OnPlayerMoveComplete -= OnPlayerMoveComplete;
        }
        
        if (dice != null)
        {
            dice.OnDiceRollComplete -= OnDiceRollComplete;
        }
    }
}
