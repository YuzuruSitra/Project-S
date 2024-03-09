using UnityEngine;

public class GoToRoomState : IRoomAIState
{
    private enum AvoidPatterns
    {
        Wait,
        Move
    }
    private AvoidPatterns _currentAvoid = AvoidPatterns.Move;
    private GameObject _npc;
    private float _moveSpeed;
    private float _rotSpeed;
    private float _distance;
    private Vector3 _targetPos;
    private bool _isWalk;
    private bool _isStateFin;
    private float _rayDistance;

    private GameObject _currentHitObj;
    private float _avoidDistance;

    public bool IsWalk => _isWalk;
    public bool IsStateFin => _isStateFin;

    public GoToRoomState(GameObject npc, float moveSpeed, float rotSpeed, float distance, float ray, float avoidDistance)
    {
        _npc = npc;
        _moveSpeed = moveSpeed;
        _rotSpeed = rotSpeed;
        _distance = distance;
        _rayDistance = ray;
        _avoidDistance = avoidDistance;
    }

    // �X�e�[�g�ɓ��������̏���
    public void EnterState(Vector3 pos)
    {
        _targetPos = pos;
        _isStateFin = false;
        _isWalk = true;
    }

    // �X�e�[�g�̍X�V
    public void UpdateState()
    {
        if (IsAvoid())
        {
            if (_currentAvoid == AvoidPatterns.Wait) AvoidWaiting();
            else AvoidMoving();
        }
        else
        {
            DefaultMoving();
            if (Vector3.Distance(_npc.transform.position, _targetPos) <= _distance) _isStateFin = true;
        }
    }

    private void DefaultMoving()
    {
        Vector3 direction = (_targetPos - _npc.transform.position).normalized;
        _npc.transform.position += direction * _moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        }
    }

    private void AvoidMoving()
    {
        Vector3 direction = (_targetPos - _npc.transform.position).normalized;
        _npc.transform.position += -direction * _moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        }
    }

    private void AvoidWaiting()
    {
        Vector3 direction = (_targetPos - _npc.transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        }
    }

    // ��Q����������s���邩�ǂ����𔻒肷�郁�\�b�h
    private bool IsAvoid()
    {
        //Debug.DrawRay(_npc.transform.position, -_npc.transform.forward * _rayDistance, Color.red);
        if (Physics.Raycast(_npc.transform.position, -_npc.transform.forward, out RaycastHit hit, _rayDistance) &&
            hit.collider.CompareTag("RoomNPC") && _currentHitObj == null)
        {
            _currentHitObj = hit.collider.gameObject;
            _currentAvoid = DecideAvoidancePattern(_currentHitObj);
            return _currentAvoid != AvoidPatterns.Wait;
        } 

        return CheckObstacleTooFar();
    }

    // ��Q���Ƃ̋������`�F�b�N���郁�\�b�h
    private bool CheckObstacleTooFar()
    {
        if (_currentHitObj == null) return false;
        float distanceX = Mathf.Abs(_currentHitObj.transform.position.x - _npc.transform.position.x);
        if (distanceX > _avoidDistance)
        {
            _currentHitObj = null;
            return false;
        }
        return true;
    }

    // ��Q������̃p�^�[�������肷�郁�\�b�h
    private AvoidPatterns DecideAvoidancePattern(GameObject otherObj)
    {
        NPCController otherNPCController = _currentHitObj.GetComponent<NPCController>();
        float otherDistance = otherNPCController.GetDistanceToTarget();
        float thisDistance = Vector3.Distance(_npc.transform.position, _targetPos);
        return otherDistance < thisDistance ? AvoidPatterns.Wait : AvoidPatterns.Move;
    }

}

// �]���K�v�������ꍇ�͑ҋ@����