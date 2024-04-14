using UnityEngine;

public class RoomChanger : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offSet;
    [SerializeField]
    private GameObject _selectionObj;
    private RoomEditor _roomEditor;
    private RoomDetails _firstSelectRoom;
    private bool _isEditing = false;

    void Start()
    {
        _roomEditor = RoomEditor.Instance;
        _roomEditor.RoomClicker.ChangeRetentionRoom += ChangeRoom;
    }

    public void ChangerSwitch()
    {
        if (!IsEditingEnabled()) return;
        if (_isEditing)
            FinishChanging();
        else
            LaunchChanging();
    }

    private void ChangeRoom(RoomDetails target)
    {
        if (_firstSelectRoom == null) return;
        if (target.RoomType == RoomType.Lift) return;

        // �ꏊ������
        Vector3 tmpPos = _firstSelectRoom.transform.position;
        _firstSelectRoom.transform.position = target.transform.position;
        target.transform.position = tmpPos;
        // �����̔ԍ�������
        int tmpNum = _firstSelectRoom.RoomNum;
        _firstSelectRoom.SetRoomNum(target.RoomNum);
        target.SetRoomNum(tmpNum);

        // �\���G���A�̍X�V
        _selectionObj.transform.position = target.transform.position - _offSet;
        FinishChanging();
    }

    // �����̕ҏW���L�����ۂ�
    private bool IsEditingEnabled()
    {
        if (_roomEditor.SelectObj == null) return false;
        switch (_roomEditor.SelectObj.RoomType)
        {
            case RoomType.Lift:
                return false;
        }
        return true;
    }

    // ���������̊J�n
    private void LaunchChanging()
    {
        _firstSelectRoom = _roomEditor.SelectObj;
        _selectionObj.SetActive(true);
        _selectionObj.transform.position = _firstSelectRoom.transform.position - _offSet;            
        _isEditing = true;    
    }

    // ���������̏I��
    private void FinishChanging()
    {
        _firstSelectRoom = null;
        _selectionObj.SetActive(false);
        _isEditing = false;
    }
}
