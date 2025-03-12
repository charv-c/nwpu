using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public List<Transform> Points = new List<Transform>();
    public static BoardManager Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMoveEnd(int chesslocation,int index)
    {
        /*��5�ָ��ӣ���Ϊ���ƣ����ۣ�ռ3���¼���ռ���񣩿հ�һ��*/
        int type = chesslocation % 7;
        if(type==0) 
        {
            
        }
    }
}
