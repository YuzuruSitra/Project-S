using UnityEngine;

// �����̓���ւ�����
public class RoomChanger
{
//     float _zPosition;
//     Vector3 tmpPos;
//     private RoomDetails _firstTouch;
//     private GameObject _currentHitRoom;
//     private bool _isEditing = false;
//     private int _targetLayer;

//     public RoomChanger(int layer)
//     {
//         _targetLayer = layer;
//     }

//     public void ChangerSwitch()
//     {
//         if (_isEditing)
//             FinishChanging();
//         else
//             LaunchChanging();
//     }

//     public void MoveToTarget()
//     {
//         if (!_isEditing) return;
//         if (_firstTouch == null) return;
//         if (Input.GetMouseButtonDown(0))
//         {
//             _zPosition = _firstTouch.transform.position.z - Camera.main.transform.position.z;
//             tmpPos = _firstTouch.transform.position;
//         }
//         if (Input.GetMouseButton(0))
//         {
//             Vector3 inputVector = Input.mousePosition;
//             inputVector.z = _zPosition;
//             Vector3 targetPosition = Camera.main.ScreenToWorldPoint(inputVector);
//             // �I�u�W�F�N�g���ړ�������
//             _firstTouch.transform.position = targetPosition;

//             // ���̕����̈ʒu�����炷
// /*            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//             RaycastHit hit;
//             if (!Physics.Raycast(ray, out hit, Mathf.Infinity, _targetLayer)) return;
//             GameObject hitObj = hit.collider.gameObject;
//             if (hitObj.tag == "Room" && _currentHitRoom != hitObj)
//             {
//                 Debug.Log(hitObj.transform.position);
//                 _currentHitRoom = hitObj;
//                 RoomDetails hitRoom = hitObj.GetComponent<RoomDetails>();
//                 Vector3 tmp = hitRoom.transform.position;
//                 hitRoom.transform.position = tmpPos;
//                 //tmpPos = tmp;
//                 //Debug.Log("�ŏ��̕��� : " + tmpPos);
//             }*/
//         }
//         if (Input.GetMouseButtonUp(0))
//         {
//             _firstTouch.transform.position = tmpPos;
//             _firstTouch = null;
//         }
//     }

//     public void ChangeRoom(RoomDetails target)
//     {
//         _firstTouch = target;        
//     }

//     // ���������̊J�n
//     private void LaunchChanging()
//     {            
//         _isEditing = true;
//     }

//     // ���������̏I��
//     private void FinishChanging()
//     {
//         _isEditing = false;
//     }

//     // �I������
//     public void FinRoomChange()
//     {
//         _isEditing = false;
//     }

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
