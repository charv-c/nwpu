using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Chess : MonoBehaviour
{
    public int ChessIndex;
    public int ChessLocationIndex { get { return chesslocationindex; } }
    int chesslocationindex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Move(int delta)
    {
        int temp = 0;
        while (temp != delta)
        {
            temp++;
            MoveToIndex(temp+chesslocationindex);
        }
        chesslocationindex += temp;
    }
    void MoveToIndex(int index)
    {
        float duration = 0.2f;
        Transform targettransform=BoardManager.Instance.Points[index];
        StartCoroutine(MoveToTarget(targettransform.position, duration));
    }
    IEnumerator MoveToTarget(Vector2 target, float duration)
    {
        Vector2 startPosition = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // 插值计算当前位置 lerp, calculate current location
            float t = elapsed / duration;
            transform.position = Vector2.Lerp(startPosition, target, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 确保最终到达目标位置 make sure get to target
        transform.position = target;
    }
}
