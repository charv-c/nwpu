using UnityEngine.Events;

[System.Serializable]
public class Events
{
    public UnityEvent zeroParaEvent;
    public UnityEvent<int> oneParaEvent;
    public UnityEvent<int, string> twoParaEvent;
    public UnityEvent<int, string, float> threeParaEvent;
}