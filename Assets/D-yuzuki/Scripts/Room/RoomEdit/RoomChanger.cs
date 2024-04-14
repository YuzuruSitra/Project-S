using UnityEngine;

// �����̓���ւ�����
public class RoomChanger
{
    private Vector3 _offSet;
    private GameObject _selectionObj;
    private RoomDetails _touchObj;
    private RoomDetails _cloneTouchObj;
    private bool _isEditing = false;

    public RoomChanger(Vector3 offSet, GameObject selectionObj)
    {
        _offSet = offSet;
        _selectionObj = selectionObj;
    }

    public void ChangerSwitch()
    {
        if (_isEditing)
            FinishChanging();
        else
            LaunchChanging();
    }

    public void ChangeRoom(RoomDetails target)
    {
        _touchObj = target;
        if (!_isEditing) return;
        if (!IsEditingEnabled(target)) return;
        
        // �ꏊ������
        Vector3 tmpPos = _cloneTouchObj.transform.position;
        _cloneTouchObj.transform.position = target.transform.position;
        target.transform.position = tmpPos;
        // �����̔ԍ�������
        int tmpNum = _cloneTouchObj.RoomNum;
        _cloneTouchObj.SetRoomNum(target.RoomNum);
        target.SetRoomNum(tmpNum);

        // �\���G���A�̍X�V
        _selectionObj.transform.position = target.transform.position - _offSet;
        FinishChanging();
    }

    // ���������̊J�n
    private void LaunchChanging()
    {
        if (_touchObj == null) return;
        _cloneTouchObj = _touchObj;
        _selectionObj.SetActive(true);
        _selectionObj.transform.position = _touchObj.transform.position - _offSet;            
        _isEditing = true;    
    }

    // ���������̏I��
    private void FinishChanging()
    {
        _selectionObj.SetActive(false);
        _isEditing = false;
    }

    // ����̕����̎��͑I��s��
    private bool IsEditingEnabled(RoomDetails target)
    {
        if (target.RoomType == RoomType.Lift) return false;
        return true;
    }

    // �I������
    public void FinRoomChange()
    {
        _touchObj = null;
    }
}
