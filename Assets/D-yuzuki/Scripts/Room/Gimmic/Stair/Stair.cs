using System.Collections;
using UnityEngine;

// �K�i
public class Stair : MonoBehaviour
{
    [Header("�K (1F=0)")]
    [SerializeField]
    private int _roomFloor;
    [Header("�ړ����̑ҋ@����")]
    [SerializeField] 
    private float _waitTime;
    private RoomDetails _roomDetails;
    public RoomDetails RoomDetails => _roomDetails;
    private GameObject _targetObj;
    private StairSelecter _stairSelecter;
    
    private Vector3 _entryPos;
    public Vector3 EntryPos => _entryPos;

    private Vector3 _npcOutPos;
    public Vector3 NPCOutPos => _npcOutPos;

    // Start is called before the first frame update
    void Start()
    {
        _roomDetails = GetComponent<RoomDetails>();
        _entryPos = _roomDetails.RoomInPoints.position;
        _npcOutPos = _roomDetails.RoomOutPoints.position;
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
        BraverController braver = _targetObj.GetComponent<BraverController>();
        braver.IsFreedom = false;

        // �G���g���[ 
        braver.InnNPCMover.SetTarGetPos(_entryPos);
        while (!braver.InnNPCMover.IsAchieved)
        {
            braver.InnNPCMover.Moving();
            yield return null;
        }

        // �K�w�̃��[�v
        Stair targetFloor = _stairSelecter.FloorSelecter(_roomFloor, braver.BaseRoom);
        _targetObj.transform.position = targetFloor.EntryPos;
        
        yield return _waitTime;

        // �ޏo    
        braver.InnNPCMover.SetTarGetPos(targetFloor.NPCOutPos);
        while (!braver.InnNPCMover.IsAchieved)
        {
            braver.InnNPCMover.Moving();
            yield return null;
        }

        braver.IsFreedom = true;
        int targetStairNum = targetFloor.RoomDetails.RoomNum;
        braver.FinWarpHandler(targetStairNum);
    }

}
