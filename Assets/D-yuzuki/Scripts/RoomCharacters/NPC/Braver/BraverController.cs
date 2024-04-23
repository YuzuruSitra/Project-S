using System.Collections.Generic;
using UnityEngine;

public interface IRoomAIState
{
    bool IsStateFin { get; }
    bool IsWalk { get; }
    void EnterState(Vector3 targetPos, int targetRoom);
    void UpdateState();
}

public enum RoomAIState
{
    STAY_ROOM,
    EXIT_ROOM,
    LEAVE_ROOM,
    GO_TO_ROOM
}

// ���[��NPC�̐���N���X
public class BraverController : MonoBehaviour
{
    [Header("�L�����N�^�[�̕���")]
    [SerializeField]
    private int _baseRoom = 0;
    public int BaseRoom => _baseRoom;
    [Header("�ړ����x")]
    [SerializeField] 
    private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;
    [Header("�ڕW���W�ɑ΂��鋖�e�덷")]
    [SerializeField] 
    private float _stoppingDistance = 0.1f;
    public float StoppingDistance => _stoppingDistance;
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
    private BraverRoomSelecter _braverRoomSelecter;
    private RoomPosAllocation _roomPosAllocation;
    //private Animator _animator;
    // �؍ݒ��̕����ԍ���ێ�
    private int _targetRoomNum;
    // ���̕����ԍ���ێ�
    private int _nextRoomNum;
    // �^�[�Q�b�g���W��ێ�
    private Vector3 _targetPos;
    private bool _isCurrentWalk;
    // �s������
    public bool IsFreedom = true;
    // �ړ��p�N���X
    private InnNPCMover _innNPCMover;
    public InnNPCMover InnNPCMover => _innNPCMover;

    void Start()
    {
        InitializeNPC();
    }

    void Update()
    {
        if (!IsFreedom) return;
        _states[_currentState].UpdateState();
        // ChangeAnimWalk(_states[_currentState].IsWalk);

        if (_states[_currentState].IsStateFin) NextState(_currentState);
    }

    void InitializeNPC()
    {
        _innNPCMover = new InnNPCMover(gameObject, _moveSpeed, _stoppingDistance);
        _targetRoomNum = _baseRoom;
        _braverRoomSelecter = BraverRoomSelecter.Instance;
        _roomPosAllocation = RoomPosAllocation.Instance;
        //_animator = gameObject.GetComponent<Animator>();

        // �e��Ԃ̃C���X�^���X���쐬���ēo�^
        _states.Add(RoomAIState.STAY_ROOM, new StayRoomState(_innNPCMover, _roomPosAllocation.ErrorVector));
        _states.Add(RoomAIState.EXIT_ROOM, new ExitRoomState(_innNPCMover));
        _states.Add(RoomAIState.LEAVE_ROOM, new LeaveRoomState(_innNPCMover));
        _states.Add(RoomAIState.GO_TO_ROOM, new GoToRoomState(_innNPCMover));
        // STAY_ROOM����J�n
        _currentState = RoomAIState.STAY_ROOM;
        _states[_currentState].EnterState(_roomPosAllocation.ErrorVector, _targetRoomNum);
    }

    // �A�j���[�V�����̑J��
    void ChangeAnimWalk(bool isWalk)
    {
        /*
        if (_isCurrentWalk == isWalk) return;
        _animator.SetBool("IsWalk", isWalk);
        _isCurrentWalk = isWalk;
        */
    }

    void NextState(RoomAIState state)
    {
        RoomAIState newState;
        switch (state)
        {
            case RoomAIState.STAY_ROOM:
                _nextRoomNum = _braverRoomSelecter.SelectNextRoomNum(_baseRoom, _targetRoomNum);
                newState = RoomAIState.EXIT_ROOM;
                _targetPos = _roomPosAllocation.TargetPosSelection(_targetRoomNum, RoomPosAllocation.PointKind.EXIT_POINT, transform.position.y);
                break;
            case RoomAIState.EXIT_ROOM:
                newState = RoomAIState.LEAVE_ROOM;
                _targetPos = _roomPosAllocation.TargetPosSelection(_targetRoomNum, RoomPosAllocation.PointKind.OUT_POINT, transform.position.y);
                break;
            case RoomAIState.LEAVE_ROOM:
                newState = RoomAIState.GO_TO_ROOM;
                // �����̈ړ�
                _targetRoomNum = _nextRoomNum;
                _targetPos = _roomPosAllocation.TargetPosSelection(_targetRoomNum, RoomPosAllocation.PointKind.OUT_POINT, transform.position.y);
                break;
            case RoomAIState.GO_TO_ROOM:
                newState = RoomAIState.STAY_ROOM;
                _targetPos = _roomPosAllocation.TargetPosSelection(_targetRoomNum, RoomPosAllocation.PointKind.IN_POINT, transform.position.y);
                break;
            default:
                newState = state;
                break;
        }
        if (_nextRoomNum == RoomBunker.ERROR_ROOM_NUM && state == RoomAIState.STAY_ROOM)
        {
            _targetPos = _roomPosAllocation.ErrorVector;
            newState = RoomAIState.STAY_ROOM;
        }
        
        _states[newState].EnterState(_targetPos, _targetRoomNum);
        _currentState = newState;
    }

    // �O������̎Q��
    
    public float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, _targetPos);
    }

    public void FinWarpHandler(int currentRoom)
    {
        _nextRoomNum = _braverRoomSelecter.SelectNextRoomNum(_baseRoom, currentRoom);
        NextState(RoomAIState.EXIT_ROOM);
    }

}
