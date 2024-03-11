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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �o�Ă������W�̑I��
    public Stair FloorSelecter(int calledFloor)
    {
        if (calledFloor == 0) return _stairs[calledFloor + 1];
        if (calledFloor + 1 == _maxFloor) return _stairs[calledFloor - 1];
        int rnd = Random.Range(0, 2);
        if (rnd == 0) return _stairs[calledFloor - 1];
        else return _stairs[calledFloor + 1];
    }
}
