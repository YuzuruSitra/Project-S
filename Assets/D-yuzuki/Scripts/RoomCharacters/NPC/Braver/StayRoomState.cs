using UnityEngine;

public class StayRoomState : IRoomAIState
{
    private GameObject _npc;
    private const float RAY_FACTOR = 1.5f;
    private float _rayDistance;
    private InnNPCMover _innNpcMover;
    private RoomBunker _roomBunker;
    private Vector3 _entryTargetPos;
    private Vector3 _inRoomTargetPos;
    private const float TARGET_FACTOR = 3.0f;
    private Vector3 _errorVector;
    private bool _isEntry;
    private float _changeTime;
    private int _remainState;
    private bool _obstacleHit;
    private const float MAX_ANGLE = 180.0f;
    private const int MAX_ATTEMP = 100;
    private float _remainStateTime;
    private bool _isWalk;

    public bool IsWalk => _isWalk;
    public bool IsStateFin => MonitorStateExit();

    public StayRoomState(InnNPCMover mover, Vector3 errorVector)
    {
        _innNpcMover = mover;
        _npc = _innNpcMover.Character;
        _rayDistance = _npc.GetComponent<SpriteRenderer>().bounds.size.x * RAY_FACTOR;
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
        _errorVector = errorVector;
    }

    // �X�e�[�g�ɓ��������̏���
    public void EnterState(Vector3 pos, int targetRoom)
    {
        _entryTargetPos = pos;
        _innNpcMover.SetTarGetPos(_entryTargetPos);
        _remainStateTime = _roomBunker.RoomDetails[targetRoom].RemainTime;
        _isEntry = false;

        if (_entryTargetPos == _errorVector) _isEntry = true;
    }

    // �X�e�[�g�̍X�V

    public void UpdateState()
    {
        // ����
        if (!_isEntry)
        {
            _isWalk = true;
            // �����ɓ���܂ł̈ړ�
            _innNpcMover.Moving();
            if (_innNpcMover.IsAchieved) _isEntry = true;
            
            return;
        }

        // ���R���s
        _changeTime -= Time.deltaTime;

        // �X�e�[�g�̃p�����[�^���Đݒ�
        if (_changeTime <= 0 || _innNpcMover.IsAchieved) SetInStateParam();
        

        switch (_remainState)
        {
            case 0: // ��~
                _isWalk = false;
                break;
            case 1: // ���s
                if (_inRoomTargetPos == Vector3.zero)
                {
                    _remainState = 0;
                    break;
                }
                // �O���ɏ�Q��������Ή��
                AvoidObstacle();
                _innNpcMover.Moving();
                _isWalk = true;
                break;
        }

        // �X�e�[�g�J�E���g�_�E��
        MonitorStateExit();
    }

    private void SetInStateParam()
    {
        _remainState = Random.Range(0, 2);
        _changeTime = Random.Range(1, 3);
        if(_remainState == 1) SetRandomDirection();
    }

    private void SetRandomDirection()
    {
        int attempts = 0;
        while (attempts < MAX_ATTEMP)
        {
            float randomAngle = Random.Range(-MAX_ANGLE, MAX_ANGLE);
            Vector3 randomDirection = Quaternion.AngleAxis(randomAngle, _npc.transform.up) * _npc.transform.forward;

            if (!Physics.Raycast(_npc.transform.position, randomDirection, out RaycastHit hit, _rayDistance))
            {
                // �m���Œ�~
                int selectRnd = Random.Range(0, 3);
                if (selectRnd == 0) 
                    _remainState = 0;
                else 
                    _inRoomTargetPos = randomDirection.normalized * TARGET_FACTOR;
                    _innNpcMover.SetTarGetPos(_inRoomTargetPos);
                return;
            }

            attempts++;
        }

        _inRoomTargetPos = Vector3.zero;
    }

    private void AvoidObstacle()
    {
        Debug.DrawRay(_npc.transform.position, -_npc.transform.forward * _rayDistance, Color.red);
        if (Physics.Raycast(_npc.transform.position, -_npc.transform.forward, out RaycastHit hit, _rayDistance))
        {
            if ((hit.collider.CompareTag("Wall") || hit.collider.CompareTag("RoomNPC")) && !_obstacleHit)
            {
                SetRandomDirection();
                _obstacleHit = true;
                return;
            }
        }
        _obstacleHit = false;
    }

    // �X�e�[�g�̏I�����Ď�
    public bool MonitorStateExit()
    {
        _remainStateTime -= Time.deltaTime;
        return (_remainStateTime <= 0);
    }
}
