using System.Collections.Generic;
using D_yuzuki.Scripts.RoomCharacters.NPC.Braver;
using UnityEngine;

public interface IRoomAIState
{
    bool IsStateFin { get; }
    bool IsWalk { get; }
    void EnterState(Vector3 targetPos, int targetRoom);
    void UpdateState();
    void ExitState();
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
    public int BraverNum { get; private set; }

    public int BaseRoom { get; private set; }

    [Header("�ړ����x")]
    [SerializeField] 
    private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;
    [Header("�ڕW���W�ɑ΂��鋖�e�덷")]
    [SerializeField] 
    private float _stoppingDistance = 0.1f;

    public RoomAIState CurrentState { get; private set; }
    private Dictionary<RoomAIState, IRoomAIState> _states = new Dictionary<RoomAIState, IRoomAIState>();

    // �ڕW���[���I��N���X
    private BraverRoomSelecter _braverRoomSelecter;
    private RoomPosAllocation _roomPosAllocation;
    //private Animator _animator;
    // �؍ݒ��̕����ԍ���ێ�
    public int StayRoomNum { get; private set; }
    // ���̕����ԍ���ێ�
    private int _nextRoomNum;
    // �^�[�Q�b�g���W��ێ�
    private Vector3 _targetPos;
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
        _states[CurrentState].UpdateState();
        // ChangeAnimWalk(_states[_currentState].IsWalk);
        if (_states[CurrentState].IsStateFin) NextState(CurrentState);
    }

    void InitializeNPC()
    {
        _innNPCMover = new InnNPCMover(gameObject, _moveSpeed, _stoppingDistance);
        StayRoomNum = BaseRoom;
        _braverRoomSelecter = BraverRoomSelecter.Instance;
        _roomPosAllocation = RoomPosAllocation.Instance;
        //_animator = gameObject.GetComponent<Animator>();

        // �e��Ԃ̃C���X�^���X���쐬���ēo�^
        _states.Add(RoomAIState.STAY_ROOM, new StayRoomState(_innNPCMover, _roomPosAllocation.ErrorVector));
        _states.Add(RoomAIState.EXIT_ROOM, new ExitRoomState(_innNPCMover));
        _states.Add(RoomAIState.LEAVE_ROOM, new LeaveRoomState(_innNPCMover));
        _states.Add(RoomAIState.GO_TO_ROOM, new GoToRoomState(_innNPCMover));
        // STAY_ROOM����J�n
        CurrentState = RoomAIState.STAY_ROOM;
        _states[CurrentState].EnterState(_roomPosAllocation.ErrorVector, StayRoomNum);
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
                _nextRoomNum = _braverRoomSelecter.SelectNextRoomNum(BaseRoom, StayRoomNum);
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
        if (_nextRoomNum == RoomBunker.ERROR_ROOM_NUM && state == RoomAIState.STAY_ROOM)
        {
            _targetPos = _roomPosAllocation.ErrorVector;
            newState = RoomAIState.STAY_ROOM;
        }
        _states[CurrentState].ExitState();
        _states[newState].EnterState(_targetPos, StayRoomNum);
        CurrentState = newState;
    }

    // �O������̎Q��
    
    public float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, _targetPos);
    }

    public void FinWarpHandler(int currentRoom)
    {
        _nextRoomNum = _braverRoomSelecter.SelectNextRoomNum(BaseRoom, currentRoom);
        NextState(RoomAIState.EXIT_ROOM);
    }

    public void SetNumber(int braverNum, int roomNum)
    {
        BraverNum = braverNum;
        BaseRoom = roomNum;
    }

}
