using UnityEngine;

// ���[�g�̑I��
public class RoomSelecter: MonoBehaviour
{
    [Header("�e�K�w�̕�����")]
    [SerializeField]
    private int _floorRoomCount;
    [SerializeField]
    private RoomDetails[] _roomDetails;

    // �ŏ��ɂ��郋�[�����擾
    public RoomDetails OutRemainRoom(int NPCRoom)
    {
        return _roomDetails[NPCRoom];
    }

    // �^�[�Q�b�g�̕�����I��
    public RoomDetails SelectTargetRoom(int NPCRoom, int currentRoomNum)
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
        if (acceptableRoomCount == 0) return null;

        // �O�̕����ƈႤ������I��
        int targetRoomNum;
        do
        {
            targetRoomNum = Random.Range(0, contenderRoom.Length);
        } while (contenderRoom[targetRoomNum] == currentRoomNum || (contenderRoom[targetRoomNum] != NPCRoom && !_roomDetails[contenderRoom[targetRoomNum]].IsRoomAcceptance));

        return _roomDetails[contenderRoom[targetRoomNum]];
    }
}
