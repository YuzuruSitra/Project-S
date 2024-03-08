using UnityEngine;

// ���[�g�̑I��
public class RoomSelecter: MonoBehaviour
{
    [Header("�e�K�w�̕�����")]
    [SerializeField]
    private int _floorRoomCount;

    [Header("�����̉���")]
    [SerializeField]
    private float _roomWidth;
    [Header("�����̏c��")]
    [SerializeField]
    private float _roomHeight;
    [Header("����0�̒�")]
    [SerializeField]
    private Transform _room0InPoints;
    [Header("����0�̌�")]
    [SerializeField]
    private Transform _room0ExitPoints;
    [Header("����0�̊O")]
    [SerializeField]
    private Transform _room0OutPoints;
    
    [SerializeField]
    private RoomDetails[] _roomDetails;

    // WayPoint�̌v�Z�N���X
    private CalcWayPoint _calcWayPoint;
    [Header("npc�̖ڕW���W�G���[�l(�G�}�̕���)")]
    [SerializeField]
    private Transform _errorPos;
    public readonly Vector3 ErrorVector;
    public const int ERROR_ROOM_NUM = -1;
    public enum PointKind
    {
        IN_POINT,
        EXIT_POINT,
        OUT_POINT
    }

    private void Start()
    {
        Debug.Log(ErrorVector);
        _calcWayPoint = new CalcWayPoint(_floorRoomCount, _roomWidth, _roomHeight);
        _calcWayPoint.SetWayPoints(_roomDetails.Length, _room0InPoints.position, _room0ExitPoints.position, _room0OutPoints.position);
    }

    // �^�[�Q�b�g�̕�����I��
    public int SelectNextRoomNum(int NPCRoom, int currentRoomNum)
    {
        // �[�������l�������I�������쐬
        int[] contenderRoom;
        int calcPos = NPCRoom % _floorRoomCount;
        
        // ���[
        if (calcPos == 0)
        {
            contenderRoom = new int[] { NPCRoom, NPCRoom + 1 };
        }
        // �E�[
        else if (calcPos == _floorRoomCount - 1)
        {
            contenderRoom = new int[] { NPCRoom, NPCRoom - 1 };
        }
        else
        {
            contenderRoom = new int[] { NPCRoom, NPCRoom - 1, NPCRoom + 1 };
        }

        // �󂯓���\�ȕ����̐����J�E���g
        int acceptableRoomCount = 0;
        foreach (int roomNum in contenderRoom)
            if (_roomDetails[roomNum].IsRoomAcceptance && roomNum != NPCRoom) 
                acceptableRoomCount++;

        // �󂯓���\�ȕ������Ȃ��ꍇ��null��Ԃ�
        if (acceptableRoomCount == 0) return ERROR_ROOM_NUM;

        // �O�̕����ƈႤ������I��
        int targetRoomNum;
        do
        {
            targetRoomNum = Random.Range(0, contenderRoom.Length);
        } while (contenderRoom[targetRoomNum] == currentRoomNum || (contenderRoom[targetRoomNum] != NPCRoom && !_roomDetails[contenderRoom[targetRoomNum]].IsRoomAcceptance));

        return contenderRoom[targetRoomNum];
    }

    // �w����W�̕����o��
    public Vector3 TargetPosSelection(int roomNum, PointKind pointKind, float npcPosY)
    {
        Vector3 outPos = Vector3.zero;
        // �s���ȃ��[���ԍ�
        if (roomNum >= _roomDetails.Length || roomNum < 0) return ErrorVector;

        if (pointKind == PointKind.IN_POINT) outPos = _calcWayPoint.RoomInPoints[roomNum];
        else if (pointKind == PointKind.EXIT_POINT) outPos = _calcWayPoint.RoomExitPoints[roomNum];
        else if (pointKind == PointKind.OUT_POINT) outPos = _calcWayPoint.RoomOutPoints[roomNum];
        outPos.y = npcPosY;

        return outPos;
    }
}
