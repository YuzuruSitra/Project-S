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
    private BraverStairSelecter _stairSelecter;
    
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
        _stairSelecter = BraverStairSelecter.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoomBraver"))
        {
            // ��O����̐N���̂݋���
            Vector3 direction = (transform.position - other.transform.position).normalized;
            if (direction.z <= 0) return;
            StartCoroutine(AutoMovingBraver(other.gameObject));
        }
    }

    private IEnumerator AutoMovingBraver(GameObject target)
    {
        var braver = target.GetComponent<BraverController>();
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
        target.transform.position = targetFloor.EntryPos;
        
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
