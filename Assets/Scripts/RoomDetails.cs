using UnityEngine;

// �������̕ێ�
public class RoomDetails : MonoBehaviour
{
    [Header("�����ԍ�")]
    [SerializeField]
    private int _roomNum;
    public int RoomNum => _roomNum;

    [Header("��������")]
    [SerializeField]
    private bool _isRoomAcceptance;
    public bool IsRoomAcceptance => _isRoomAcceptance;

    [Header("�����̒�")]
    [SerializeField]
    private Transform _roomInPoints;
    public Transform RoomInPoints => _roomInPoints;

    [Header("�����̌�")]
    [SerializeField]
    private Transform _roomExitPoints;
    public Transform RoomExitPoints => _roomExitPoints;

    [Header("�����̊O")]
    [SerializeField]
    private Transform _roomOutPoints;
    public Transform RoomOutPoints => _roomOutPoints;
}
