using System.Collections.Generic;
using UnityEngine;

// ���[��NPC�̐���N���X
public class MaidController : MonoBehaviour
{
    public enum Mode
    {
        None,
        Cleaning,
        Feeding,
        Barrier
    }
    private MaidRoomSelecter _maidRoomSelecter;
    private Mode _currentMode = Mode.Cleaning;
    [Header("�ړ����x")]
    [SerializeField] 
    private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;
    [Header("�ڕW���W�ɑ΂��鋖�e�덷")]
    [SerializeField] 
    private float _stoppingDistance = 0.1f;
    public RoomAIState CurrentState { get; private set; }
    private Dictionary<RoomAIState, IRoomAIState> _states = new Dictionary<RoomAIState, IRoomAIState>();

    private RoomPosAllocation _roomPosAllocation;
    //private Animator _animator;
    private List<int> _cleanRoomList;
    private int _currentRoomID;
    public int StayRoomNum { get; private set; }
    // ���̕����ԍ���ێ�
    private int _nextRoomNum;
    // �^�[�Q�b�g���W��ێ�
    private Vector3 _targetPos;
    public InnNPCMover InnNPCMover { get; private set; }
    
    void Start()
    {
        InitializeNPC();
    }

    void Update()
    {
        if (_currentMode != Mode.Cleaning) return;
        _states[CurrentState].UpdateState();
        // ChangeAnimWalk(_states[_currentState].IsWalk);
        if (_states[CurrentState].IsStateFin) NextState(CurrentState);
    }

    void InitializeNPC()
    {
        InnNPCMover = new InnNPCMover(gameObject, _moveSpeed, _stoppingDistance);
        _maidRoomSelecter = MaidRoomSelecter.Instance;
        _roomPosAllocation = RoomPosAllocation.Instance;
        //_animator = gameObject.GetComponent<Animator>();

        // �e��Ԃ̃C���X�^���X���쐬���ēo�^
        _states.Add(RoomAIState.STAY_ROOM, new MaidStayState(InnNPCMover, _roomPosAllocation.ErrorVector));
        _states.Add(RoomAIState.EXIT_ROOM, new MaidExitState(InnNPCMover));
        _states.Add(RoomAIState.LEAVE_ROOM, new MaidLeaveState(InnNPCMover));
        _states.Add(RoomAIState.GO_TO_ROOM, new MaidGoToState(InnNPCMover));

        // �z�u�@�\����������܂ł̉��u��
        // _cleanRoomList = _maidRoomSelecter.CreateRoomList(1);
        // _currentRoomID = 0;
        // _nextRoomNum = _cleanRoomList[_currentRoomID];
        // NextState(RoomAIState.LEAVE_ROOM);        
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
                _currentRoomID ++;
                if (_currentRoomID >= _cleanRoomList.Count) 
                {
                    _cleanRoomList.Reverse();
                    _currentRoomID = 1;
                }
                _nextRoomNum = _cleanRoomList[_currentRoomID];
                newState = RoomAIState.EXIT_ROOM;
                _targetPos = _roomPosAllocation.TargetPosSelection(StayRoomNum, RoomPosAllocation.PointKind.EXIT_POINT, transform.position.y);
                break;
            case RoomAIState.EXIT_ROOM:
                newState = RoomAIState.LEAVE_ROOM;
                _targetPos = _roomPosAllocation.TargetPosSelection(StayRoomNum, RoomPosAllocation.PointKind.OUT_POINT, transform.position.y);
                break;
            case RoomAIState.LEAVE_ROOM:
                newState = RoomAIState.GO_TO_ROOM;
                // �����̈ړ�
                StayRoomNum = _nextRoomNum;
                _targetPos = _roomPosAllocation.TargetPosSelection(StayRoomNum, RoomPosAllocation.PointKind.OUT_POINT, transform.position.y);
                break;
            case RoomAIState.GO_TO_ROOM:
                newState = RoomAIState.STAY_ROOM;
                _targetPos = _roomPosAllocation.TargetPosSelection(StayRoomNum, RoomPosAllocation.PointKind.IN_POINT, transform.position.y);
                break;
            default:
                newState = state;
                break;
        }
        if ((_nextRoomNum == RoomBunker.ERROR_ROOM_NUM && state == RoomAIState.STAY_ROOM) || _cleanRoomList.Count <= 1)
        {
            _targetPos = _roomPosAllocation.ErrorVector;
            newState = RoomAIState.STAY_ROOM;
        }
        _states[CurrentState].ExitState();
        _states[newState].EnterState(_targetPos, StayRoomNum);
        CurrentState = newState;
    }

    public void SetMode(Mode mode, Vector3 pos, Vector3 rot, int floor = 0)
    {
        _currentMode = mode;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot);
        if (_currentMode == Mode.Cleaning)
        {
            _cleanRoomList = _maidRoomSelecter.CreateRoomList(floor);
            _currentRoomID = 0;
            _nextRoomNum = _cleanRoomList[_currentRoomID];
            NextState(RoomAIState.LEAVE_ROOM);
        }
        // �Ή������A�j���[�V�������Đ��\��
    }

}
