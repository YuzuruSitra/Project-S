using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DebugRoomSelecter : MonoBehaviour
{
    private RoomSelecter _roomSelecter;
    public Button LaunchButton;
    public GameObject DebugPanel;
    public InputField IF1;
    public InputField IF2;

    private void Start()
    {
        _roomSelecter = GameObject.FindWithTag("PathSelecter").GetComponent<RoomSelecter>();
        LaunchButton.onClick.AddListener(LaunchDebug);
    }

    private void LaunchDebug()
    {
        _roomSelecter.SelectNextRoomNum(int.Parse(IF1.text), int.Parse(IF2.text));
    }

    public void OutValueDebug(List<int> contenderRoom, List<int> alternativeRooms)
    {
        // �f�o�b�O
        string out1 = "��╔�� : ";
        foreach (int roomNum in contenderRoom)
        {
            out1 += roomNum�@+ ", ";
        }
        string out2 = "�ŏI��╔�� : ";
        foreach (int roomNum in alternativeRooms)
        {
            out2 += roomNum + ", ";
        }
        Debug.Log(out1);
        Debug.Log(out2);
    }
}
