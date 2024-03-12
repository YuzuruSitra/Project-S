using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairSelecter : MonoBehaviour
{
    [Header("�K�i���i�[")]
    [SerializeField]
    private Stair[] _stairs;
    [Header("npc�̖ڕW���W�G���[�l(�G�}�̕���)")]
    [SerializeField]
    private Transform _errorPos;
    public Vector3 ErrorVector => _errorPos.position;
    private int _maxFloor => _stairs.Length;
    private RoomSelecter _roomSelecter;

    // Start is called before the first frame update
    void Start()
    {
        _roomSelecter = GameObject.FindWithTag("PathSelecter").GetComponent<RoomSelecter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �o�Ă������W�̑I��
    public Stair FloorSelecter(int calledFloor, int baseRoom)
    {
        List<int> stairList = _roomSelecter.SearchStairs(calledFloor, baseRoom);
        int rnd = Random.Range(0, stairList.Count);
        return _stairs[stairList[rnd]];
    }
}
