using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("主界面")]
    public GameObject mainMenuPanel; // 主菜单面板
    public Button startGameButton; // 开始游戏按钮
    public Button settingsButton; // 设置按钮
    public Button quitButton; // 退出按钮
    
    [Header("游戏界面")]
    public GameObject gamePanel; // 游戏面板
    public Button rollDiceButton; // 掷骰子按钮
    public Button backToMenuButton; // 返回主菜单按钮
    
    [Header("游戏信息显示")]
    public TextMeshProUGUI diceResultText; // 骰子结果
    
    [Header("设置界面")]
    public GameObject settingsPanel; // 设置面板
    public Toggle soundToggle; // 音效开关
    public Slider musicVolumeSlider; // 音乐音量
    public Button closeSettingsButton; // 关闭设置按钮
    
    [Header("游戏对象")]
    public GameObject playerChess; // 玩家棋子
    
    // 私有变量
    private int turnCount = 0;
    
    void Start()
    {
        InitializeUI();
        ShowMainMenu();
    }
    
    /// <summary>
    /// 初始化UI
    /// </summary>
    void InitializeUI()
    {
        // 主菜单按钮
        if (startGameButton != null)
            startGameButton.onClick.AddListener(StartGame);
        if (settingsButton != null)
            settingsButton.onClick.AddListener(ShowSettings);
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        
        // 游戏界面按钮
        if (rollDiceButton != null)
            rollDiceButton.onClick.AddListener(OnRollDiceClicked);
        if (backToMenuButton != null)
            backToMenuButton.onClick.AddListener(BackToMainMenu);
        
        // 设置界面
        if (soundToggle != null)
            soundToggle.onValueChanged.AddListener(OnSoundToggled);
        if (closeSettingsButton != null)
            closeSettingsButton.onClick.AddListener(CloseSettings);
    }
    
    /// <summary>
    /// 显示主菜单
    /// </summary>
    public void ShowMainMenu()
    {
        // 隐藏所有界面
        HideAllPanels();
        
        // 显示主菜单
        SetPanelActive(mainMenuPanel, true);
        
        // 隐藏玩家棋子
        SetPlayerChessActive(false);
        
        turnCount = 0;
    }
    
    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        // 隐藏所有界面
        HideAllPanels();
        
        // 显示游戏界面
        SetPanelActive(gamePanel, true);
        
        // 显示玩家棋子
        SetPlayerChessActive(true);
        
        turnCount = 0;
        
        // 重置游戏
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGame();
        }
    }
    
    /// <summary>
    /// 显示设置界面
    /// </summary>
    public void ShowSettings()
    {
        // 显示设置界面（覆盖当前界面）
        SetPanelActive(settingsPanel, true);
    }
    
    /// <summary>
    /// 关闭设置界面
    /// </summary>
    public void CloseSettings()
    {
        SetPanelActive(settingsPanel, false);
    }
    
    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void BackToMainMenu()
    {
        ShowMainMenu();
    }
    
    /// <summary>
    /// 掷骰子按钮点击
    /// </summary>
    public void OnRollDiceClicked()
    {
        if (Dice.Instance != null && !Dice.Instance.IsRolling())
        {
            turnCount++;
        }
    }
    
    /// <summary>
    /// 音效开关切换
    /// </summary>
    /// <param name="enabled">是否启用</param>
    public void OnSoundToggled(bool enabled)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetSoundEnabled(enabled);
        }
    }
    
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    /// <summary>
    /// 更新骰子结果显示
    /// </summary>
    /// <param name="result">骰子结果</param>
    public void UpdateDiceResult(int result)
    {
        if (diceResultText != null)
        {
            diceResultText.text = $"骰子点数: {result}";
        }
    }
    
    /// <summary>
    /// 隐藏所有面板
    /// </summary>
    void HideAllPanels()
    {
        SetPanelActive(mainMenuPanel, false);
        SetPanelActive(gamePanel, false);
        SetPanelActive(settingsPanel, false);
    }
    
    /// <summary>
    /// 设置面板显示状态
    /// </summary>
    /// <param name="panel">面板</param>
    /// <param name="active">是否激活</param>
    void SetPanelActive(GameObject panel, bool active)
    {
        if (panel != null)
        {
            panel.SetActive(active);
        }
    }
    
    /// <summary>
    /// 设置玩家棋子显示状态
    /// </summary>
    /// <param name="active">是否激活</param>
    void SetPlayerChessActive(bool active)
    {
        if (playerChess != null)
        {
            playerChess.SetActive(active);
        }
    }
    
    /// <summary>
    /// 设置掷骰子按钮状态
    /// </summary>
    /// <param name="interactable">是否可交互</param>
    public void SetRollDiceButtonInteractable(bool interactable)
    {
        if (rollDiceButton != null)
        {
            rollDiceButton.interactable = interactable;
        }
    }
    
    /// <summary>
    /// 获取当前回合数
    /// </summary>
    /// <returns>回合数</returns>
    public int GetTurnCount()
    {
        return turnCount;
    }
}
