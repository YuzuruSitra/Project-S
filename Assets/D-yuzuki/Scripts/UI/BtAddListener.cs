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
    // �N���X
    [SerializeField]
    private CamController _camController;
    private ChangeInnStateHandler _changeInnStateHandler;
    void Start()
    {
        _changeInnStateHandler = new ChangeInnStateHandler();
        _changeViewButton.onClick.AddListener(_camController.DefaultCamChanger);
        _changeVisibilButton.onClick.AddListener(VisibilityHandler.Instance.ChangeAllRoom);
        _changeEditModeButton.onClick.AddListener(_changeInnStateHandler.ChangeEditState);
        _changeDefaultModeButton.onClick.AddListener(_changeInnStateHandler.ChangeDefaultState);
    }

}
