using UnityEngine;

// �h�S�̂̕�������ێ�
public class RoomBunker : MonoBehaviour
{
    [Header("�e�K�w�̕�����")]
    [SerializeField]
    private int _floorRoomCount;
    public int FloorRoomCount => _floorRoomCount;
    [Header("�������i�[")]
    [SerializeField]
    private RoomDetails[] _roomDetails;
    public RoomDetails[] RoomDetails => _roomDetails;
    [Header("�K�i���i�[")]
    [SerializeField]
    private Stair[] _stairs;
    public Stair[] Stairs => _stairs;
    private int _topFloor => _stairs.Length - 1;
    public int TopFloor => _topFloor;

    [Header("npc�̖ڕW���W�G���[�l(�G�}�̕���)")]
    [SerializeField]
    private Transform _errorPos;
    public Vector3 ErrorVector => _errorPos.position;
    public const int ERROR_ROOM_NUM = -1;

}
