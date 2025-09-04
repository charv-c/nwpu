using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [Header("游戏板设置")]
    public int boardSize = 20; // 游戏板大小
    public float boardSizeX = 10f; // 游戏板X轴大小
    public float boardSizeY = 10f; // 游戏板Y轴大小
    public GameObject boardPointPrefab; // 点位预制体
    public GameObject boardLinePrefab; // 连线预制体
    
    [Header("生成设置")]
    public bool generateOnStart = true; // 是否在开始时自动生成
    public bool showNumbers = true; // 是否显示数字
    public Color boardColor = Color.white; // 游戏板颜色
    
    [Header("生成结果")]
    public List<GameObject> generatedPoints = new List<GameObject>(); // 生成的点位
    public List<GameObject> generatedLines = new List<GameObject>(); // 生成的连线
    
    void Start()
    {
        if (generateOnStart)
        {
            GenerateBoard();
        }
    }
    
    /// <summary>
    /// 生成游戏板
    /// </summary>
    public void GenerateBoard()
    {
        ClearBoard();
        GenerateBoardPoints();
        GenerateBoardLines();
        SetupBoardManager();
    }
    
    /// <summary>
    /// 清除现有游戏板
    /// </summary>
    public void ClearBoard()
    {
        // 清除点位
        foreach (GameObject point in generatedPoints)
        {
            if (point != null)
            {
                DestroyImmediate(point);
            }
        }
        generatedPoints.Clear();
        
        // 清除连线
        foreach (GameObject line in generatedLines)
        {
            if (line != null)
            {
                DestroyImmediate(line);
            }
        }
        generatedLines.Clear();
    }
    
    /// <summary>
    /// 生成游戏板点位（方形路线）
    /// </summary>
    void GenerateBoardPoints()
    {
        // 计算每边的点数
        int pointsPerSide = boardSize / 4;
        int remainingPoints = boardSize % 4;
        
        int currentPoint = 0;
        
        // 生成方形路线的点位
        for (int side = 0; side < 4; side++)
        {
            int pointsOnThisSide = pointsPerSide + (side < remainingPoints ? 1 : 0);
            
            for (int i = 0; i < pointsOnThisSide; i++)
            {
                Vector3 position = CalculateSquarePosition(side, i, pointsOnThisSide);
                GameObject point = CreateBoardPoint(position, currentPoint + 1);
                generatedPoints.Add(point);
                currentPoint++;
            }
        }
    }
    
    /// <summary>
    /// 计算方形路线上的位置
    /// </summary>
    /// <param name="side">边数（0-3）</param>
    /// <param name="index">在当前边上的索引</param>
    /// <param name="totalPointsOnSide">当前边的总点数</param>
    /// <returns>计算出的位置</returns>
    Vector3 CalculateSquarePosition(int side, int index, int totalPointsOnSide)
    {
        float halfSizeX = boardSizeX / 2f;
        float halfSizeY = boardSizeY / 2f;
        
        float t = totalPointsOnSide > 1 ? (float)index / (totalPointsOnSide - 1) : 0f;
        
        switch (side)
        {
            case 0: // 底边（从左到右）
                return new Vector3(-halfSizeX + t * boardSizeX, -halfSizeY, 0f);
            case 1: // 右边（从下到上）
                return new Vector3(halfSizeX, -halfSizeY + t * boardSizeY, 0f);
            case 2: // 上边（从右到左）
                return new Vector3(halfSizeX - t * boardSizeX, halfSizeY, 0f);
            case 3: // 左边（从上到下）
                return new Vector3(-halfSizeX, halfSizeY - t * boardSizeY, 0f);
            default:
                return Vector3.zero;
        }
    }
    
    /// <summary>
    /// 创建单个点位
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="number">点位编号</param>
    /// <returns>创建的点位对象</returns>
    GameObject CreateBoardPoint(Vector3 position, int number)
    {
        GameObject point;
        
        if (boardPointPrefab != null)
        {
            point = Instantiate(boardPointPrefab, position, Quaternion.identity, transform);
        }
        else
        {
            // 创建默认点位
            point = new GameObject($"BoardPoint_{number}");
            point.transform.SetParent(transform);
            point.transform.position = position;
            
            // 添加SpriteRenderer
            SpriteRenderer spriteRenderer = point.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = CreateDefaultSprite();
            spriteRenderer.color = boardColor;
            
            // 添加CircleCollider2D
            CircleCollider2D collider = point.AddComponent<CircleCollider2D>();
            collider.radius = 0.5f;
        }
        
        // 设置点位编号
        point.name = $"BoardPoint_{number}";
        
        // 添加数字标签
        if (showNumbers)
        {
            AddNumberLabel(point, number);
        }
        
        return point;
    }
    
    /// <summary>
    /// 添加数字标签
    /// </summary>
    /// <param name="point">点位对象</param>
    /// <param name="number">数字</param>
    void AddNumberLabel(GameObject point, int number)
    {
        // 创建文本对象
        GameObject textObj = new GameObject("NumberLabel");
        textObj.transform.SetParent(point.transform);
        textObj.transform.localPosition = Vector3.zero;
        
        // 添加TextMeshPro组件
        TMPro.TextMeshPro textMesh = textObj.AddComponent<TMPro.TextMeshPro>();
        textMesh.text = number.ToString();
        textMesh.fontSize = 2f;
        textMesh.color = Color.black;
        textMesh.alignment = TMPro.TextAlignmentOptions.Center;
        
        // 调整位置
        textMesh.transform.localPosition = new Vector3(0, 0, -0.1f);
    }
    
    /// <summary>
    /// 生成游戏板连线
    /// </summary>
    void GenerateBoardLines()
    {
        if (boardLinePrefab == null) return;
        
        for (int i = 0; i < generatedPoints.Count; i++)
        {
            int nextIndex = (i + 1) % generatedPoints.Count;
            
            Vector3 startPos = generatedPoints[i].transform.position;
            Vector3 endPos = generatedPoints[nextIndex].transform.position;
            
            GameObject line = CreateBoardLine(startPos, endPos);
            generatedLines.Add(line);
        }
    }
    
    /// <summary>
    /// 创建连线
    /// </summary>
    /// <param name="startPos">起始位置</param>
    /// <param name="endPos">结束位置</param>
    /// <returns>连线对象</returns>
    GameObject CreateBoardLine(Vector3 startPos, Vector3 endPos)
    {
        GameObject line = Instantiate(boardLinePrefab, transform);
        
        // 计算连线位置和旋转
        Vector3 center = (startPos + endPos) / 2f;
        Vector3 direction = endPos - startPos;
        float distance = direction.magnitude;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        line.transform.position = center;
        line.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        // 调整连线大小
        if (line.GetComponent<SpriteRenderer>() != null)
        {
            line.GetComponent<SpriteRenderer>().size = new Vector2(distance, 0.1f);
        }
        
        return line;
    }
    
    /// <summary>
    /// 设置BoardManager
    /// </summary>
    void SetupBoardManager()
    {
        BoardManager boardManager = FindObjectOfType<BoardManager>();
        if (boardManager != null)
        {
            boardManager.boardPoints.Clear();
            foreach (GameObject point in generatedPoints)
            {
                boardManager.boardPoints.Add(point.transform);
            }
            boardManager.totalBoardPoints = boardSize;
        }
    }
    
    /// <summary>
    /// 创建默认精灵
    /// </summary>
    /// <returns>默认精灵</returns>
    Sprite CreateDefaultSprite()
    {
        // 创建一个简单的圆形精灵
        Texture2D texture = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        
        Vector2 center = new Vector2(16, 16);
        float radius = 14f;
        
        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                pixels[y * 32 + x] = distance <= radius ? Color.white : Color.clear;
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
    }
    
    /// <summary>
    /// 获取生成的点位列表
    /// </summary>
    /// <returns>点位列表</returns>
    public List<GameObject> GetGeneratedPoints()
    {
        return generatedPoints;
    }
    
    /// <summary>
    /// 设置游戏板颜色
    /// </summary>
    /// <param name="color">颜色</param>
    public void SetBoardColor(Color color)
    {
        boardColor = color;
        
        foreach (GameObject point in generatedPoints)
        {
            if (point != null && point.GetComponent<SpriteRenderer>() != null)
            {
                point.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
    
    /// <summary>
    /// 设置游戏板大小
    /// </summary>
    /// <param name="size">大小</param>
    public void SetBoardSize(int size)
    {
        boardSize = size;
        GenerateBoard();
    }
    
    /// <summary>
    /// 设置游戏板尺寸
    /// </summary>
    /// <param name="sizeX">X轴大小</param>
    /// <param name="sizeY">Y轴大小</param>
    public void SetBoardDimensions(float sizeX, float sizeY)
    {
        boardSizeX = sizeX;
        boardSizeY = sizeY;
        GenerateBoard();
    }
}
