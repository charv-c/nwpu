using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dice : MonoBehaviour
{
    [Header("骰子设置")]
    public List<Sprite> diceSprites = new List<Sprite>(); // 骰子图片（1-6点）
    public float rollDuration = 1.5f; // 掷骰动画持续时间
    public int minRollCount = 8; // 最小滚动次数
    public int maxRollCount = 15; // 最大滚动次数
    
    [Header("UI组件")]
    public Image diceImage; // 骰子图片组件
    public TextMeshProUGUI diceText; // 骰子数字文本
    
    [Header("音效")]
    public AudioSource diceAudioSource; // 骰子音效
    public AudioClip rollSound; // 滚动音效
    public AudioClip stopSound; // 停止音效
    
    // 单例模式
    public static Dice Instance { get; private set; }
    
    // 私有变量
    private bool isRolling = false;
    private int finalResult = 1;
    
    // 事件
    public System.Action<int> OnDiceRollComplete; // 骰子掷完事件
    
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
        // 获取组件
        if (diceImage == null)
        {
            diceImage = GetComponent<Image>();
            if (diceImage == null)
            {
                Debug.LogError("Dice: 未找到Image组件！请确保Dice对象有Image组件。");
            }
        }
        
        // 检查骰子图片
        if (diceSprites.Count == 0)
        {
            Debug.LogWarning("Dice: 未设置骰子图片！请在Inspector中设置diceSprites。");
        }
        else if (diceSprites.Count < 6)
        {
            Debug.LogWarning($"Dice: 骰子图片数量不足！当前有{diceSprites.Count}张，需要6张。");
        }
        
        // 初始化显示
        ShowDiceResult(1);
    }
    
    /// <summary>
    /// 开始掷骰子
    /// </summary>
    public void Roll()
    {
        if (isRolling) 
        {
            Debug.Log("Dice: 骰子正在滚动中，忽略新的掷骰请求。");
            return;
        }
        
        if (diceSprites.Count == 0)
        {
            Debug.LogError("Dice: 无法掷骰，未设置骰子图片！");
            return;
        }
        
        Debug.Log("Dice: 开始掷骰子");
        StartCoroutine(RollAnimation());
    }
    
    /// <summary>
    /// 掷骰动画协程
    /// </summary>
    private IEnumerator RollAnimation()
    {
        isRolling = true;
        
        // 播放滚动音效
        if (diceAudioSource != null && rollSound != null)
        {
            diceAudioSource.PlayOneShot(rollSound);
        }
        
        // 随机滚动次数
        int rollCount = Random.Range(minRollCount, maxRollCount + 1);
        
        // 滚动动画
        for (int i = 0; i < rollCount; i++)
        {
            // 随机显示骰子面
            int randomFace = Random.Range(1, 7);
            ShowDiceResult(randomFace);
            
            // 等待一段时间
            yield return new WaitForSeconds(rollDuration / rollCount);
        }
        
        // 生成最终结果
        finalResult = Random.Range(1, 7);
        ShowDiceResult(finalResult);
        
        // 播放停止音效
        if (diceAudioSource != null && stopSound != null)
        {
            diceAudioSource.PlayOneShot(stopSound);
        }
        
        // 等待一小段时间显示结果
        yield return new WaitForSeconds(0.5f);
        
        // 完成掷骰
        isRolling = false;
        
        Debug.Log($"Dice: 掷骰完成，结果: {finalResult}");
        
        // 触发完成事件
        OnDiceRollComplete?.Invoke(finalResult);
        
        // 通知BoardManager处理结果
        if (BoardManager.Instance != null)
        {
            BoardManager.Instance.HandleDiceResult(finalResult);
        }
        else
        {
            Debug.LogError("Dice: 未找到BoardManager实例！");
        }
    }
    
    /// <summary>
    /// 显示骰子结果
    /// </summary>
    /// <param name="result">骰子点数</param>
    private void ShowDiceResult(int result)
    {
        // 确保结果在有效范围内
        result = Mathf.Clamp(result, 1, 6);
        
        // 更新图片
        if (diceSprites.Count >= result && diceImage != null)
        {
            diceImage.sprite = diceSprites[result - 1];
            diceImage.enabled = true; // 确保Image组件启用
        }
        else
        {
            if (diceImage == null)
            {
                Debug.LogError("Dice: diceImage为空！");
            }
            else if (diceSprites.Count < result)
            {
                Debug.LogError($"Dice: 骰子图片索引超出范围！需要索引{result-1}，但只有{diceSprites.Count}张图片。");
            }
        }
        
        // 更新文本
        if (diceText != null)
        {
            diceText.text = result.ToString();
        }
    }
    
    /// <summary>
    /// 设置骰子图片
    /// </summary>
    /// <param name="sprites">骰子图片列表</param>
    public void SetDiceSprites(List<Sprite> sprites)
    {
        diceSprites = sprites;
        Debug.Log($"Dice: 设置了{diceSprites.Count}张骰子图片");
    }
    
    /// <summary>
    /// 获取当前骰子结果
    /// </summary>
    /// <returns>骰子点数</returns>
    public int GetCurrentResult()
    {
        return finalResult;
    }
    
    /// <summary>
    /// 检查是否正在掷骰
    /// </summary>
    /// <returns>是否正在掷骰</returns>
    public bool IsRolling()
    {
        return isRolling;
    }
    
    /// <summary>
    /// 强制停止掷骰（用于调试）
    /// </summary>
    public void ForceStopRoll()
    {
        if (isRolling)
        {
            StopAllCoroutines();
            isRolling = false;
            ShowDiceResult(finalResult);
            Debug.Log("Dice: 强制停止掷骰");
        }
    }
    
    /// <summary>
    /// 创建默认骰子图片（用于测试）
    /// </summary>
    public void CreateDefaultDiceSprites()
    {
        diceSprites.Clear();
        
        for (int i = 1; i <= 6; i++)
        {
            // 创建简单的数字纹理
            Texture2D texture = new Texture2D(64, 64);
            Color[] pixels = new Color[64 * 64];
            
            // 填充白色背景
            for (int j = 0; j < pixels.Length; j++)
            {
                pixels[j] = Color.white;
            }
            
            texture.SetPixels(pixels);
            texture.Apply();
            
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
            diceSprites.Add(sprite);
        }
        
        Debug.Log("Dice: 创建了默认骰子图片");
        ShowDiceResult(1);
    }
}
