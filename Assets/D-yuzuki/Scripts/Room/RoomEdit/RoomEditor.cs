using UnityEngine;
using static InnGameController;

public class RoomEditor : MonoBehaviour
{
    private RoomBunker _roomBunker;
    private RoomOutliner _roomOutliner;
    // RoomClicker
    private RoomClicker _roomClicker;
    [SerializeField]
    private LayerMask _targetLayer;
    // RoomChanger
    private RoomChanger _roomChanger;
    public RoomChanger RoomChanger => _roomChanger;
    [SerializeField]
    private Vector3 _offSet;
    [SerializeField]
    private GameObject _selectionObj;
    // RoomBuilder
    private RoomBuilder _roomBuilder;
    [Header("�����������̐e�I�u�W�F�N�g")]
    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private GameObject _roomsFPrefab;

    void Start()
    {
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
        _roomClicker = new RoomClicker(_targetLayer);
        _roomOutliner = new RoomOutliner();
        _roomChanger = new RoomChanger(_offSet, _selectionObj);
        _roomBuilder = new RoomBuilder(_roomBunker);
        _roomClicker.ChangeRetentionRoom += _roomOutliner.ChangeOutLine;
        _roomClicker.ChangeRetentionRoom += _roomChanger.ChangeRoom;
    }

    // ���[���I��
    public void InputRoomSelect()
    {
        if (Input.GetMouseButtonDown(0)) _roomClicker.SelectRoomObj();
    }

    // �K�̑��z
    public void BuildFloor()
    {
        if (_roomBunker == null)  return;
        if (!_roomBuilder.IsBuildValid()) return;

        Vector3 insPos = _roomBuilder.CalculateInstantiatePosition();
        GameObject newFloor = Instantiate(_roomsFPrefab, insPos, Quaternion.identity, _parent);
        _roomBuilder.UpdateRoomBunker(newFloor);
        newFloor.name = "Rooms" + _roomBunker.TopFloor + "F";
    }

    // �ҏW���[�h�̏I��
    public void FinishEditing(InnState newState)
    {
        if (newState == InnState.EDIT) return;
        _roomOutliner.FinOutLine();
        _roomChanger.FinRoomChange();
    }

}
