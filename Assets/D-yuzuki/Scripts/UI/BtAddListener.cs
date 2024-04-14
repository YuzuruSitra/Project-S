using UnityEngine;
using UnityEngine.UI;

public class BtAddListener : MonoBehaviour
{
    [SerializeField]
    private Button _changeViewButton;
    [SerializeField]
    private Button _changeVisibilButton;
    [SerializeField]
    private Button _changeEditModeButton;
    [SerializeField]
    private Button _changeDefaultModeButton;
    [SerializeField]
    private Button _addFloorButton;
    [SerializeField]
    private Button _changeRoomButton;
    // �N���X
    [SerializeField]
    private CamController _camController;
    private ChangeInnStateHandler _changeInnStateHandler;
    [SerializeField]
    private RoomBuilder _roomBuilder;
    [SerializeField]
    private RoomChanger _roomChanger;
    void Start()
    {
        _changeInnStateHandler = new ChangeInnStateHandler();
        _changeViewButton.onClick.AddListener(_camController.DefaultCamChanger);
        _changeVisibilButton.onClick.AddListener(VisibilityHandler.Instance.ChangeAllRoom);
        _changeEditModeButton.onClick.AddListener(_changeInnStateHandler.ChangeEditState);
        _changeDefaultModeButton.onClick.AddListener(_changeInnStateHandler.ChangeDefaultState);
        _addFloorButton.onClick.AddListener(_roomBuilder.BuildFloor);
        _changeRoomButton.onClick.AddListener(_roomChanger.ChangerSwitch);
    }

}
