using UnityEngine;

public class GoToRoomState : IRoomAIState
{
    // ����p�^�[��
    private enum AvoidPatterns
    {
        Wait,
        Move
    }
    private InnNPCMover _innNpcMover;
    private GameObject _npc;

    // ���݂̉���p�^�[��
    private AvoidPatterns _currentAvoid = AvoidPatterns.Move;
    // �^�[�Q�b�g�̈ʒu
    private Vector3 _targetPos;
    // ���s�t���O
    private bool _isWalk;
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
    private bool _launchState;
    // �X�e�[�g�I���t���O��getter
    public bool IsStateFin => _innNpcMover.IsAchieved && _launchState;
    private bool _currentDirection = true;

    public GoToRoomState(InnNPCMover mover)
    {
        _innNpcMover = mover;
        _npc = _innNpcMover.Character;
    }

    public void EnterState(Vector3 pos, int targetRoom)
    {
        _targetPos = pos;
        _innNpcMover.SetTarGetPos(_targetPos);
        _launchState = true;
    }

    public void UpdateState()
    {
        if (IsAvoidingObstacle())
            if (_currentAvoid == AvoidPatterns.Wait) 
                _isWalk = false;
            else 
                AvoidMoving();
        else
            DefaultMoving();
    }

    public void ExitState()
    {
        _launchState = false;
    }

    // �f�t�H���g�̈ړ�
    private void DefaultMoving()
    {
        _isWalk = true;
        MoveTowardsTarget(_targetPos);
        _innNpcMover.Moving();
    }

    // ������̈ړ�
    private void AvoidMoving()
    {
        _isWalk = true;
        MoveTowardsTarget(_targetPos, reverseDirection: true);
        _innNpcMover.Moving();
    }

    // �^�[�Q�b�g�Ɍ������Ĉړ�
    private void MoveTowardsTarget(Vector3 target, bool reverseDirection = false)
    {
        if (_currentDirection == reverseDirection) return;
        _currentDirection = reverseDirection;
        _innNpcMover.SetTarGetPos((reverseDirection ? -target : target));
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
}
