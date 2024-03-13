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

    [Header("���[�v���ۂ�")]
    [SerializeField]
    private bool _isWarp;
    public bool IsWarp => _isWarp;


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

    [SerializeField]
    private MeshRenderer _frontWall;
    public MeshRenderer FrontWall => _frontWall;
    [SerializeField]
    private MeshRenderer _frontDoor;
    public MeshRenderer FrontDoor => _frontDoor;
    

}
