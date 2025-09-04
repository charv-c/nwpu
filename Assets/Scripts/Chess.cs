using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : MonoBehaviour
{
    [Header("棋子设置")]
    public float moveSpeed = 5f; // 移动速度
    public float jumpHeight = 0.5f; // 跳跃高度
    public float jumpDuration = 0.3f; // 跳跃持续时间
    
    [Header("视觉效果")]
    public GameObject trailEffect; // 拖尾效果
    public ParticleSystem moveParticles; // 移动粒子效果
    
    // 私有变量
    private Vector3 targetPosition;
    private bool isMoving = false;
    
    void Start()
    {
        // 初始化位置
        if (BoardManager.Instance != null && BoardManager.Instance.boardPoints.Count > 0)
        {
            transform.position = BoardManager.Instance.boardPoints[0].position;
        }
        
        // 监听移动完成事件
        if (BoardManager.Instance != null)
        {
            BoardManager.Instance.OnPlayerMoveComplete += OnPlayerMoveComplete;
        }
    }
    
    void OnDestroy()
    {
        // 取消事件监听
        if (BoardManager.Instance != null)
        {
            BoardManager.Instance.OnPlayerMoveComplete -= OnPlayerMoveComplete;
        }
    }
    
    /// <summary>
    /// 玩家移动完成回调
    /// </summary>
    /// <param name="position">到达的位置</param>
    private void OnPlayerMoveComplete(int position)
    {
        // 可以在这里添加移动完成后的效果
        Debug.Log($"玩家棋子到达位置: {position + 1}");
        
        // 播放到达效果
        PlayArrivalEffect();
    }
    
    /// <summary>
    /// 播放到达效果
    /// </summary>
    private void PlayArrivalEffect()
    {
        // 简单的缩放动画
        StartCoroutine(ArrivalAnimation());
        
        // 播放粒子效果
        if (moveParticles != null)
        {
            moveParticles.Play();
        }
    }
    
    /// <summary>
    /// 到达动画
    /// </summary>
    private IEnumerator ArrivalAnimation()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;
        
        // 放大
        float elapsed = 0f;
        while (elapsed < 0.1f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / 0.1f;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }
        
        // 恢复
        elapsed = 0f;
        while (elapsed < 0.1f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / 0.1f;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }
        
        transform.localScale = originalScale;
    }
    
    /// <summary>
    /// 设置拖尾效果
    /// </summary>
    /// <param name="enabled">是否启用</param>
    public void SetTrailEffect(bool enabled)
    {
        if (trailEffect != null)
        {
            trailEffect.SetActive(enabled);
        }
    }
    
    /// <summary>
    /// 重置棋子位置
    /// </summary>
    public void ResetPosition()
    {
        if (BoardManager.Instance != null && BoardManager.Instance.boardPoints.Count > 0)
        {
            transform.position = BoardManager.Instance.boardPoints[0].position;
        }
    }
}
