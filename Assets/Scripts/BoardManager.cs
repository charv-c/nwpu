using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoardManager : MonoBehaviour
{
    [Header("游戏板设置")]
    public List<Transform> boardPoints = new List<Transform>(); // 游戏板点位
    public int totalBoardPoints = 20; // 总点位数量
    
    [Header("UI组件")]
    public Button rollDiceButton; // 掷骰子按钮
    public TextMeshProUGUI diceResultText; // 骰子结果文本
    
    [Header("游戏对象")]
    public Transform playerChess; // 玩家棋子
    
    // 游戏状态
    private bool isPlayerMoving = false;
    private int currentPlayerPosition = 0;
    
    // 单例模式
    public static BoardManager Instance { get; private set; }
    
    // 事件
    public System.Action<int> OnPlayerMoveComplete; // 玩家移动完成事件
    
    void Awake()
    {
        // 单例模式设置
        if (Instance == null)
        {
            Instance = this;
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
        // 设置玩家初始位置
        if (playerChess != null && boardPoints.Count > 0)
        {
            playerChess.position = boardPoints[0].position;
            currentPlayerPosition = 0;
        }
        
        // 设置按钮点击事件
        if (rollDiceButton != null)
        {
            rollDiceButton.onClick.AddListener(OnRollDiceButtonClicked);
        }
    }
    
    /// <summary>
    /// 掷骰子按钮点击事件
    /// </summary>
    public void OnRollDiceButtonClicked()
    {
        if (isPlayerMoving) return;
        
        // 禁用按钮
        if (rollDiceButton != null)
        {
            rollDiceButton.interactable = false;
        }
        
        // 开始掷骰
        if (Dice.Instance != null)
        {
            Dice.Instance.Roll();
        }
        else
        {
            Debug.LogError("BoardManager: 未找到Dice实例！");
        }
    }
    
    /// <summary>
    /// 处理骰子结果
    /// </summary>
    /// <param name="diceResult">骰子点数</param>
    public void HandleDiceResult(int diceResult)
    {
        if (isPlayerMoving) return;
        
        Debug.Log($"BoardManager: 处理骰子结果: {diceResult}");
        Debug.Log($"BoardManager: 当前玩家位置: {currentPlayerPosition}");
        Debug.Log($"BoardManager: playerChess引用: {(playerChess != null ? "已设置" : "未设置")}");
        Debug.Log($"BoardManager: boardPoints数量: {boardPoints.Count}");
        
        // 更新UI显示
        if (diceResultText != null)
        {
            diceResultText.text = $"骰子点数: {diceResult}";
        }
        
        // 计算新位置
        int newPosition = (currentPlayerPosition + diceResult) % totalBoardPoints;
        Debug.Log($"BoardManager: 目标位置: {newPosition}");
        
        // 开始移动
        StartCoroutine(MovePlayerToPosition(newPosition, diceResult));
    }
    
    /// <summary>
    /// 移动玩家到指定位置
    /// </summary>
    /// <param name="targetPosition">目标位置</param>
    /// <param name="steps">移动步数</param>
    IEnumerator MovePlayerToPosition(int targetPosition, int steps)
    {
        isPlayerMoving = true;
        
        // 逐步移动
        for (int i = 1; i <= steps; i++)
        {
            int nextPosition = (currentPlayerPosition + i) % totalBoardPoints;
            
            if (nextPosition < boardPoints.Count && playerChess != null)
            {
                yield return StartCoroutine(MovePlayerToPoint(boardPoints[nextPosition].position));
            }
        }
        
        // 更新玩家位置
        currentPlayerPosition = targetPosition;
        
        // 移动完成
        isPlayerMoving = false;
        
        // 重新启用按钮
        if (rollDiceButton != null)
        {
            rollDiceButton.interactable = true;
        }
        
        // 触发移动完成事件
        OnPlayerMoveComplete?.Invoke(currentPlayerPosition);
        
        // 处理格子事件
        HandleBoardEvent(currentPlayerPosition);
    }
    
    /// <summary>
    /// 移动玩家到指定点位的协程
    /// </summary>
    /// <param name="targetPosition">目标位置</param>
    IEnumerator MovePlayerToPoint(Vector3 targetPosition)
    {
        if (playerChess == null)
        {
            Debug.LogError("BoardManager: playerChess为空！");
            yield break;
        }
        
        Debug.Log($"BoardManager: 开始移动棋子从 {playerChess.position} 到 {targetPosition}");
        
        float moveDuration = 0.3f; // 每步移动时间
        Vector3 startPosition = playerChess.position;
        float elapsed = 0f;
        
        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);
            playerChess.position = newPosition;
            Debug.Log($"BoardManager: 移动中... 位置: {newPosition}, 进度: {t:F2}");
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        playerChess.position = targetPosition;
        Debug.Log($"BoardManager: 移动完成，最终位置: {playerChess.position}");
    }
    
    /// <summary>
    /// 直接移动玩家到指定位置（无动画）
    /// </summary>
    /// <param name="position">目标位置</param>
    public void MovePlayerToPosition(int position)
    {
        if (position < boardPoints.Count && playerChess != null)
        {
            playerChess.position = boardPoints[position].position;
            currentPlayerPosition = position;
        }
    }
    
    /// <summary>
    /// 处理格子事件
    /// </summary>
    /// <param name="position">玩家位置</param>
    void HandleBoardEvent(int position)
    {
        // 根据位置触发不同事件
        int eventType = position % 5; // 简单的格子类型分配
        
        switch (eventType)
        {
            case 0:
                Debug.Log($"玩家到达位置 {position + 1}: 普通格子");
                break;
            case 1:
                Debug.Log($"玩家到达位置 {position + 1}: 奖励格子");
                break;
            case 2:
                Debug.Log($"玩家到达位置 {position + 1}: 惩罚格子");
                break;
            case 3:
                Debug.Log($"玩家到达位置 {position + 1}: 机会格子");
                break;
            case 4:
                Debug.Log($"玩家到达位置 {position + 1}: 命运格子");
                break;
        }
    }
    
    /// <summary>
    /// 重置游戏
    /// </summary>
    public void ResetGame()
    {
        currentPlayerPosition = 0;
        isPlayerMoving = false;
        
        // 重置玩家位置
        if (playerChess != null && boardPoints.Count > 0)
        {
            playerChess.position = boardPoints[0].position;
        }
        
        // 重置UI
        if (rollDiceButton != null)
        {
            rollDiceButton.interactable = true;
        }
        
        if (diceResultText != null)
        {
            diceResultText.text = "骰子点数: -";
        }
        
        Debug.Log("BoardManager: 游戏已重置");
    }
    
    /// <summary>
    /// 获取当前玩家位置
    /// </summary>
    /// <returns>玩家位置</returns>
    public int GetCurrentPlayerPosition()
    {
        return currentPlayerPosition;
    }
    
    /// <summary>
    /// 检查玩家是否正在移动
    /// </summary>
    /// <returns>是否正在移动</returns>
    public bool IsPlayerMoving()
    {
        return isPlayerMoving;
    }
    
    /// <summary>
    /// 测试移动方法（右键菜单调用）
    /// </summary>
    [ContextMenu("测试移动")]
    public void TestMove()
    {
        if (playerChess != null && boardPoints.Count > 1)
        {
            Debug.Log("BoardManager: 开始测试移动");
            playerChess.position = boardPoints[1].position;
            Debug.Log($"BoardManager: 测试移动完成，位置: {playerChess.position}");
        }
        else
        {
            Debug.LogError("BoardManager: 无法测试移动 - playerChess或boardPoints未正确设置");
        }
    }
    
    /// <summary>
    /// 检查引用方法（右键菜单调用）
    /// </summary>
    [ContextMenu("检查引用")]
    public void CheckReferences()
    {
        Debug.Log($"BoardManager: playerChess引用: {(playerChess != null ? "已设置" : "未设置")}");
        Debug.Log($"BoardManager: boardPoints数量: {boardPoints.Count}");
        if (playerChess != null)
        {
            Debug.Log($"BoardManager: playerChess位置: {playerChess.position}");
        }
        if (boardPoints.Count > 0)
        {
            Debug.Log($"BoardManager: 第一个点位位置: {boardPoints[0].position}");
        }
    }
}
