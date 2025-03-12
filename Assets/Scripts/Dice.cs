using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public static Dice Instance { get; private set; }
    bool _isrolling = false;
    int finalResult;
    [SerializeField]
    Image image;
    [SerializeField]
    List<Sprite> diceSprites = new();
    [SerializeField]
    float rollDuration;
    public Events events;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }
    void Start()
    {
        image = GetComponent<Image>();
    }
    public void Roll()
    {
        if (_isrolling)
        {

        }
        else
        {
            StartCoroutine(RollAnimation());
            while (_isrolling)
            {

            }
            events.oneParaEvent.Invoke(finalResult);
        }
    }
    private IEnumerator RollAnimation()
    {
        _isrolling = true;

        // ����������Σ�ģ�����ӹ���Ч��
        int rollCount = Random.Range(10, 20);
        for (int i = 0; i < rollCount; i++)
        {
            int randomIndex = Random.Range(0,5);
            image.sprite = diceSprites[randomIndex];
            yield return new WaitForSeconds(rollDuration / rollCount);
        }

        // ����ͣ�����������
        finalResult = Random.Range(0, 5);
        image.sprite = diceSprites[finalResult];

        // ��������
        _isrolling = false;
    }
}
