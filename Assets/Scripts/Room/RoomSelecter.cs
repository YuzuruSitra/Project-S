using UnityEngine;

// ���[�g�̑I��
public class RoomSelecter : MonoBehaviour
{
    [Header("�e�K�w�̕�����")]
    [SerializeField]
    private int _floorRoomCount;    

    [SerializeField]
    private RoomDetails[] _roomDetails;

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

        if (pointKind == PointKind.IN_POINT) outPos = _roomDetails[roomNum].RoomInPoints.position;
        else if (pointKind == PointKind.EXIT_POINT) outPos = _roomDetails[roomNum].RoomExitPoints.position;
        else if (pointKind == PointKind.OUT_POINT) outPos = _roomDetails[roomNum].RoomOutPoints.position;
        outPos.y = npcPosY;

        return outPos;
    }

}
