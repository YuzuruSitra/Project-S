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
    [Header("�K�i���ۂ�")]
    [SerializeField]
    private bool _isStair;
    public bool IsStair => _isStair;

}
