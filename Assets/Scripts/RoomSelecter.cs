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
        int[] contenderRoom = NPCRoom % _floorRoomCount == 0 || NPCRoom % _floorRoomCount == (_floorRoomCount - 1) ? 
            new int[] { NPCRoom, NPCRoom + 1 } : new int[] { NPCRoom, NPCRoom - 1, NPCRoom + 1 };

        // �󂯓���\�ȕ����̐����J�E���g
        int acceptableRoomCount = 0;
        foreach (int roomNum in contenderRoom)
            if (_roomDetails[roomNum].IsRoomAcceptance) 
                acceptableRoomCount++;

        // �󂯓���\�ȕ������Ȃ��ꍇ��null��Ԃ�
        if (acceptableRoomCount == 0)
            return null;

        // �O�̕����ƈႤ������I��
        int targetRoomNum;
        do
        {
            targetRoomNum = Random.Range(0, contenderRoom.Length);
        } while (contenderRoom[targetRoomNum] == currentRoomNum || !_roomDetails[contenderRoom[targetRoomNum]].IsRoomAcceptance);

        return _roomDetails[contenderRoom[targetRoomNum]];
    }
}
