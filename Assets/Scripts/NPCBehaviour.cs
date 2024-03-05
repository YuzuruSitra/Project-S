using UnityEngine;

// NPC�̐���
public class NPCBehaviour : MonoBehaviour
{
    [Header("�ړ����x")]
    [SerializeField] private float _moveSpeed;
    [Header("�ڕW���W�ɑ΂��鋖�e�덷")]
    public float _stoppingDistance = 0.1f;
    [Header("�؍ݎ��Ԃ̍ŏ��l")]
    public float _minStayTime = 3f;
    [Header("�؍ݎ��Ԃ̍ő�l")]
    public float _maxStayTime = 5f;
    [Header("�L�����N�^�[�̕���")]
    public int _baseRoom = 0;

    // �L�����N�^�[�̃X�e�[�g
    public enum CharacterState
    {
        STAY_ROOM,
        LEAVE_ROOM,
        GO_TO_ROOM,
        ENTRY_ROOM
    }
    private CharacterState _currentState;

    // �ڕW���[���I��N���X
    private RoomSelecter _roomSelecter;
    // �����؍ݎ��Ԃ�ێ�
    private float _remainStayTime;
    // �؍ݒ��̕�����ێ�
    private RoomDetails _currentRoom;
    // �^�[�Q�b�g���W��ێ�
    private Transform _targetPos;

    void Start()
    {
        InitializeNPC();
    }

    void Update()
    {
        if (_currentState != CharacterState.STAY_ROOM)
            MoveToTarget();
        else
            UpdateStayRoomState();
    }

    void InitializeNPC()
    {
        _roomSelecter = GameObject.FindWithTag("RoomSelecter").GetComponent<RoomSelecter>();
        _currentState = CharacterState.STAY_ROOM;
        _currentRoom = _roomSelecter.OutRemainRoom(_baseRoom);
    }

    void UpdateStayRoomState()
    {
        if (_currentRoom == null) return;

        _remainStayTime -= Time.deltaTime;

        if (_remainStayTime <= 0)
            NextState();
    }

    void NextState()
    {
        switch (_currentState)
        {
            case CharacterState.STAY_ROOM:
                _currentState = CharacterState.LEAVE_ROOM;
                _targetPos = _currentRoom.RoomOutPoints;
                break;
            case CharacterState.LEAVE_ROOM:
                _currentState = CharacterState.GO_TO_ROOM;
                _currentRoom = _roomSelecter.SelectTargetRoom(_baseRoom, _currentRoom.RoomNum);
                if (_currentRoom == null)
                    _currentState = CharacterState.STAY_ROOM;
                else
                    _targetPos = _currentRoom.RoomOutPoints;
                break;
            case CharacterState.GO_TO_ROOM:
                _currentState = CharacterState.ENTRY_ROOM;
                _targetPos = _currentRoom.RoomInPoints;
                break;
            case CharacterState.ENTRY_ROOM:
                _currentState = CharacterState.STAY_ROOM;
                _remainStayTime = Random.Range(_minStayTime, _maxStayTime);
                break;
        }
    }

    void MoveToTarget()
    {
        Vector3 direction = (_targetPos.position - transform.position).normalized;
        transform.position += direction * _moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, _targetPos.position) <= _stoppingDistance)
            NextState();
    }
}
