using UnityEngine;

public class RoomChanger
{
    private float _zPosition;
    private Vector3 _tmpPos;
    private RoomDetails _firstTouch;
    private GameObject _firstTouchObj;
    private bool _isEditing = false;
    private int _targetLayer;

    public RoomChanger(int layer)
    {
        _targetLayer = layer;
    }

    // �����̌����J�n�ƏI����؂�ւ���
    public void ChangerSwitch()
    {
        if (_isEditing)
            FinishChanging();
        else
            LaunchChanging();
    }

    // �������^�[�Q�b�g�Ɉړ�
    public void MoveToTarget()
    {
        if (!_isEditing || _firstTouchObj == null) return;

        if (Input.GetMouseButton(0))
        {
            MoveRoomWithMouse();
            SwapRoomPositions();
        }

        if (Input.GetMouseButtonUp(0))
        {
            FinishRoomMove();
        }
    }

    // �����ɐG�ꂽ�Ƃ��̏���
    public void TouchRoom(RoomDetails target)
    {
        _firstTouch = target;
        _firstTouchObj = target.gameObject;
        _zPosition = _firstTouchObj.transform.position.z - Camera.main.transform.position.z;
        _tmpPos = _firstTouchObj.transform.position;
    }

    // ���������̊J�n
    private void LaunchChanging()
    {
        _isEditing = true;
    }

    // ���������̏I��
    private void FinishChanging()
    {
        _isEditing = false;
    }

    // �}�E�X�ɂ�镔���̈ړ�
    private void MoveRoomWithMouse()
    {
        var inputVector = Input.mousePosition;
        inputVector.z = _zPosition;
        var targetPosition = Camera.main.ScreenToWorldPoint(inputVector);
        _firstTouchObj.transform.position = targetPosition;
    }

    // �����̈ʒu�𑼂̕����ƌ������鏈��
    private void SwapRoomPositions()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, _targetLayer)) return;

        var hitObj = hit.collider.gameObject;
        if (hitObj.CompareTag("Room") && _firstTouchObj != hitObj)
        {
            // �ʒu����
            var hitPos = hitObj.transform.position;
            hitObj.transform.position = _tmpPos;
            _tmpPos = hitPos;

            // �����ԍ�����
            var hitRoom = hitObj.GetComponent<RoomDetails>();
            var hitRoomNum = hitRoom.RoomNum;
            hitRoom.SetRoomNum(_firstTouch.RoomNum);
            _firstTouch.SetRoomNum(hitRoomNum);
        }
    }

    // �����ړ��̏I��
    private void FinishRoomMove()
    {
        _firstTouchObj.transform.position = _tmpPos;
        _firstTouchObj = null;
    }

    // �I������
    public void FinRoomChange()
    {
        _isEditing = false;
    }
}
