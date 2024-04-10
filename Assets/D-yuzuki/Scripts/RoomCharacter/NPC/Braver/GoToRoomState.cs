using Unity.VisualScripting;
using UnityEngine;

public class GoToRoomState : IRoomAIState
{
    // ����p�^�[��
    private enum AvoidPatterns
    {
        Wait,
        Move
    }

    // ���݂̉���p�^�[��
    private AvoidPatterns _currentAvoid = AvoidPatterns.Move;
    // ���g�̃��[���i���o�[
    private int _roomNum;
    // NPC�I�u�W�F�N�g
    private GameObject _npc;
    // �ړ����x
    private float _moveSpeed;
    // ��]���x
    private float _rotSpeed;
    // �ڕW�܂ł̋���
    private float _distance;
    // �^�[�Q�b�g�̈ʒu
    private Vector3 _targetPos;
    // ���s�t���O
    private bool _isWalk;
    // �X�e�[�g�I���t���O
    private bool _isStateFin;
    // ���C�̋���
    private float _rayDistance;
    // ���݂̏Փ˃I�u�W�F�N�g
    private GameObject _currentHitObj;
    // �������
    private float _avoidDistance;
    // ����X�e�[�g����臒l
    private const float AVOID_THRESHOLD = 1.5f;
    // ���s�t���O��getter
    public bool IsWalk => _isWalk;
    // �X�e�[�g�I���t���O��getter
    public bool IsStateFin => _isStateFin;

    public GoToRoomState(GameObject npc, float moveSpeed, float rotSpeed, float distance, float ray, float avoidDistance, int room)
    {
        _npc = npc;
        _moveSpeed = moveSpeed;
        _rotSpeed = rotSpeed;
        _distance = distance;
        _rayDistance = ray;
        _avoidDistance = avoidDistance;
        _roomNum = room;
    }

    public void EnterState(Vector3 pos)
    {
        _targetPos = pos;
        _isStateFin = false;
    }

    public void UpdateState()
    {
        if (IsAvoidingObstacle())
        {
            if (_currentAvoid == AvoidPatterns.Wait) AvoidWaiting();
            else AvoidMoving();
        }
        else
        {
            DefaultMoving();
            MonitorStateExit();
        }
    }

    // �f�t�H���g�̈ړ�
    private void DefaultMoving()
    {
        _isWalk = true;
        MoveTowardsTarget(_targetPos);
    }

    // ������̈ړ�
    private void AvoidMoving()
    {
        _isWalk = true;
        MoveTowardsTarget(_targetPos, reverseDirection: true);
    }

    // �^�[�Q�b�g�Ɍ������Ĉړ�
    private void MoveTowardsTarget(Vector3 target, bool reverseDirection = false)
    {
        Vector3 direction = (target - _npc.transform.position).normalized;
        direction.y = 0f;

        _npc.transform.position += (reverseDirection ? -direction : direction) * _moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(reverseDirection ? direction : -direction);
            _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        }
    }

    // �ҋ@���̈ړ�
    private void AvoidWaiting()
    {
        _isWalk = false;
        Vector3 direction = (_targetPos - _npc.transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        }
    }

    // ��Q����������邩�ǂ����𔻒�
    private bool IsAvoidingObstacle()
    {
        Debug.DrawRay(_npc.transform.position, -_npc.transform.forward * _rayDistance, Color.red);
        if (Physics.Raycast(_npc.transform.position, -_npc.transform.forward, out RaycastHit hit, _rayDistance) &&
            hit.collider.CompareTag("RoomNPC") && _currentHitObj == null)
        {
            _currentHitObj = hit.collider.gameObject;
            
            if (ShouldCancelAvoidance(_currentHitObj)) return false;
            DecideAvoidancePattern(_currentHitObj);
        }

        return CheckObstacleTooFar();
    }

    // ��Q�������ꂷ���Ă��Ȃ������m�F
    private bool CheckObstacleTooFar()
    {
        if (_currentHitObj == null) return false;
        float distanceX = Vector3.Distance(_npc.transform.position, _currentHitObj.transform.position);
        if (distanceX > _avoidDistance)
        {
            _currentHitObj = null;
            return false;
        }

        return true;
    }

    // ������L�����Z�����ׂ����𔻒f
    private bool ShouldCancelAvoidance(GameObject otherObj)
    {
        bool isCancel = false;
        BraverController otherNPCController = _currentHitObj.GetComponent<BraverController>();
        float otherDistance = otherNPCController.GetDistanceToTarget();
        float thisDistance = Vector3.Distance(_npc.transform.position, _targetPos);
        if (thisDistance < otherDistance)
        {
            _currentHitObj = null;
            isCancel = true;
        }
        else if (thisDistance == otherDistance)
        {
            if (otherNPCController.BaseRoom < otherNPCController.BaseRoom) isCancel = true;
        }
        
        return isCancel;
    }

    // ����p�^�[��������
    private void DecideAvoidancePattern(GameObject otherObj)
    {
        BraverController otherNPCController = _currentHitObj.GetComponent<BraverController>();
        float otherDistance = otherNPCController.GetDistanceToTarget();
        float targetDistance = Vector3.Distance(_npc.transform.position, otherObj.transform.position);
        _currentAvoid = targetDistance - AVOID_THRESHOLD >= otherDistance ? AvoidPatterns.Wait : AvoidPatterns.Move;
    }

    // �X�e�[�g�̏I�����Ď�
    public void MonitorStateExit()
    {
        Vector3 tmp1 = _npc.transform.position;
        tmp1.y = 0;
        Vector3 tmp2 = _targetPos;
        tmp2.y = 0;
        if (Vector3.Distance(tmp1, tmp2) <= _distance) _isStateFin = true;
    }
}
