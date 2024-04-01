using System.Collections.Generic;
using UnityEngine;

public interface IRoomAIState
{
    bool IsStateFin { get; }
    bool IsWalk { get; }
    void EnterState(Vector3 targetPos);
    void UpdateState();
    void MonitorStateExit();
}

public enum RoomAIState
{
    STAY_ROOM,
    EXIT_ROOM,
    LEAVE_ROOM,
    GO_TO_ROOM
}

// ���[��NPC�̐���N���X
public class NPCController : MonoBehaviour
{
    [Header("�L�����N�^�[�̕���")]
    [SerializeField]
    private int _baseRoom = 0;
    public int BaseRoom => _baseRoom;
    [Header("�ړ����x")]
    [SerializeField] 
    private float _moveSpeed;
    [Header("��]���x")]
    [SerializeField] 
    private float _rotationSpeed;
    [Header("�������̑��x�ቺ�W��")]
    [SerializeField]
    private float _roomFriction;
    [Header("�ڕW���W�ɑ΂��鋖�e�덷")]
    [SerializeField] 
    private float _stoppingDistance = 0.1f;
    [Header("�؍ݎ��Ԃ̍ŏ��l")]
    [SerializeField] 
    private float _minStayTime;
    [Header("�؍ݎ��Ԃ̍ő�l")]
    [SerializeField] 
    private float _maxStayTime;
    [Header("�������̏�Q���F�m����")]
    [SerializeField]
    private float _stayRoomRayLength;
    [Header("�����ړ����̏�Q���F�m����")]
    [SerializeField]
    private float _goToRoomRayLength;
    [Header("�����ړ����̉���I������")]
    [SerializeField]
    private float _goToAvoidDistance;

    private RoomAIState _currentState;
    private Dictionary<RoomAIState, IRoomAIState> _states = new Dictionary<RoomAIState, IRoomAIState>();

    // �ڕW���[���I��N���X
    private RoomSelecter _roomSelecter;
    private Animator _animator;
    // �؍ݒ��̕����ԍ���ێ�
    private int _currentRoomNum;
    // �^�[�Q�b�g���W��ێ�
    private Vector3 _targetPos;
    private bool _isCurrentWalk;
    // �s������
    public bool IsFreedom = true;

    void Start()
    {
        InitializeNPC();
    }

    void Update()
    {
        if (!IsFreedom) return;
        _states[_currentState].UpdateState();
        ChangeAnimWalk(_states[_currentState].IsWalk);

        if (_states[_currentState].IsStateFin) NextState(_currentState);
    }

    void InitializeNPC()
    {
        _currentRoomNum = _baseRoom;
        _roomSelecter = RoomSelecter.Instance;
        _animator = gameObject.GetComponent<Animator>();

        // �e��Ԃ̃C���X�^���X���쐬���ēo�^
        _states.Add(RoomAIState.STAY_ROOM, new StayRoomState(gameObject, _moveSpeed * _roomFriction, _rotationSpeed * _roomFriction, _stoppingDistance, _stayRoomRayLength, _minStayTime, _maxStayTime, _roomSelecter.ErrorVector));
        _states.Add(RoomAIState.EXIT_ROOM, new ExitRoomState(gameObject, _moveSpeed, _rotationSpeed, _stoppingDistance));
        _states.Add(RoomAIState.LEAVE_ROOM, new LeaveRoomState(gameObject, _moveSpeed, _rotationSpeed, _stoppingDistance));
        _states.Add(RoomAIState.GO_TO_ROOM, new GoToRoomState(gameObject, _moveSpeed, _rotationSpeed, _stoppingDistance, _goToRoomRayLength, _goToAvoidDistance, _baseRoom));
        // STAY_ROOM����J�n
        _currentState = RoomAIState.STAY_ROOM;
        _states[_currentState].EnterState(_roomSelecter.ErrorVector);
    }

    // �A�j���[�V�����̑J��
    void ChangeAnimWalk(bool isWalk)
    {
        if (_isCurrentWalk == isWalk) return;
        _animator.SetBool("IsWalk", isWalk);
        _isCurrentWalk = isWalk;
    }

    void NextState(RoomAIState state)
    {
        RoomAIState newState;
        switch (state)
        {
            case RoomAIState.STAY_ROOM:
                newState = RoomAIState.EXIT_ROOM;
                _targetPos = _roomSelecter.TargetPosSelection(_currentRoomNum, RoomSelecter.PointKind.EXIT_POINT, transform.position.y);
                break;
            case RoomAIState.EXIT_ROOM:
                newState = RoomAIState.LEAVE_ROOM;
                _targetPos = _roomSelecter.TargetPosSelection(_currentRoomNum, RoomSelecter.PointKind.OUT_POINT, transform.position.y);
                break;
            case RoomAIState.LEAVE_ROOM:
                newState = RoomAIState.GO_TO_ROOM;
                // �����̈ړ�
                _currentRoomNum = _roomSelecter.SelectNextRoomNum(_baseRoom, _currentRoomNum);
                _targetPos = _roomSelecter.TargetPosSelection(_currentRoomNum, RoomSelecter.PointKind.OUT_POINT, transform.position.y);
                break;
            case RoomAIState.GO_TO_ROOM:
                newState = RoomAIState.STAY_ROOM;
                _targetPos = _roomSelecter.TargetPosSelection(_currentRoomNum, RoomSelecter.PointKind.IN_POINT, transform.position.y);
                break;
            default:
                newState = state;
                break;
        }
        if (_currentRoomNum == RoomBunker.ERROR_ROOM_NUM && state == RoomAIState.STAY_ROOM)
        {
            _targetPos = _roomSelecter.ErrorVector;
            newState = RoomAIState.STAY_ROOM;
        }
        _states[newState].EnterState(_targetPos);
        _currentState = newState;
    }

    /// <summary>
    /// �O������̎Q��
    /// </summary>
    /// <returns></returns>
    
    public float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, _targetPos);
    }

    public void FinWarpHandler(RoomAIState targetState, int currentRoom)
    {
        _currentRoomNum = currentRoom;
        NextState(targetState);
    }

}
