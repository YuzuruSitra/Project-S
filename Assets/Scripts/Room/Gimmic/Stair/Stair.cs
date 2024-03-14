using System.Collections;
using UnityEngine;

// �K�i
public class Stair : MonoBehaviour
{
    [Header("�K (1F=0)")]
    [SerializeField]
    private int _roomFloor;
    [Header("�������s���x")]
    [SerializeField]
    private float _moveSpeed;
    [Header("������]���x")]
    [SerializeField]
    private float _rotSpeed;
    [Header("�ڕW���W�ɑ΂��鋖�e�덷")]
    [SerializeField] 
    private float _stoppingDistance;
    [Header("�ړ����̑ҋ@����")]
    [SerializeField] 
    private float _waitTime;
    [SerializeField]
    private RoomDetails _roomDetails;
    private GameObject _targetObj;
    private StairSelecter _stairSelecter;
    
    private Vector3 _entryPos;
    public Vector3 EntryPos => _entryPos;

    private Vector3 _npcOutPos;
    public Vector3 NPCOutPos => _npcOutPos;
    
    private Vector3 _playerOutPos;
    public Vector3 PlayerOutPos => _playerOutPos;

    // Start is called before the first frame update
    void Start()
    {
        _entryPos = _roomDetails.RoomInPoints.position;
        _npcOutPos = _roomDetails.RoomOutPoints.position;
        _playerOutPos = _roomDetails.RoomExitPoints.position;
        _stairSelecter = StairSelecter.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoomNPC"))
        {
            // ��O����̐N���̂݋���
            Vector3 direction = (transform.position - other.transform.position).normalized;
            if (direction.z <= 0) return;
            _targetObj = other.gameObject;
            StartCoroutine(AutoMoving_NPC());
        }
    }

    private IEnumerator AutoMoving_NPC()
    {
        NPCController npc = _targetObj.GetComponent<NPCController>();
        npc.IsFreedom = false;

        // �G���g���[
        while (Vector3.Distance(_targetObj.transform.position, _entryPos) >= _stoppingDistance)
        {
            Vector3 direction1 = (_entryPos - _targetObj.transform.position).normalized;
            direction1.y = 0f;
            _targetObj.transform.position += direction1 * _moveSpeed * Time.deltaTime;

            Quaternion targetRotation1 = Quaternion.LookRotation(-direction1);
            _targetObj.transform.rotation = Quaternion.Slerp(_targetObj.transform.rotation, targetRotation1, _rotSpeed * Time.deltaTime);
            yield return null;
        }

        // �K�w�̃��[�v
        Stair targetFloor = _stairSelecter.FloorSelecter(_roomFloor, npc.BaseRoom);
        _targetObj.transform.position = targetFloor.EntryPos;

        yield return _waitTime;

        // �ޏo    
        while (Vector3.Distance(_targetObj.transform.position, targetFloor.NPCOutPos) >= _stoppingDistance)
        {
            Vector3 direction2 = (targetFloor.NPCOutPos - _targetObj.transform.position).normalized;
            direction2.y = 0f;
            _targetObj.transform.position += direction2 * _moveSpeed * Time.deltaTime;
            Quaternion targetRotation2 = Quaternion.LookRotation(-direction2);
            _targetObj.transform.rotation = Quaternion.Slerp(_targetObj.transform.rotation, targetRotation2, _rotSpeed * Time.deltaTime);
            yield return null;
        }
        npc.IsFreedom = true;
        npc.FinWarpHandler(RoomAIState.EXIT_ROOM, _roomDetails.RoomNum);
    }

}
