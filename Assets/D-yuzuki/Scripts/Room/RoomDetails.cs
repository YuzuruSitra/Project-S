using UnityEngine;

public enum RoomType
{
    Private,
    Facility,
    Stair,
    Lift
}

// �������̕ێ�
public class RoomDetails : MonoBehaviour
{
    [Header("�����ԍ�")]
    [SerializeField]
    private int _roomNum;
    public int RoomNum => _roomNum;

    [Header("���[���^�C�v��I��")]
    [SerializeField]
    private RoomType _roomType;
    public RoomType RoomType => _roomType;

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

    [Header("���߂��������b�V��")]
    [SerializeField]
    private MeshRenderer[] _frontMesh;
    public MeshRenderer[] FrontMesh => _frontMesh;    

    // �N���\���̌��{
    public bool IsRoomAcceptance(int npcRoom)
    {
        bool outBool = false;
        switch (_roomType)
        {
            case RoomType.Private:
                outBool = npcRoom == _roomNum;
                break;
            case RoomType.Facility:
            case RoomType.Stair:
            case RoomType.Lift:
                outBool = true;
                break;
        }
        return outBool;
    }


}
