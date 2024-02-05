using UnityEngine;
using System;
using System.IO;

// �f�[�^�擾�̃e���v���[�g
public class MasterDataIO : MonoBehaviour
{
    [SerializeField]
    private Entity_item1 _ei;

    private int _count;
    private string[] _stringData;
    private int[] _intData;

    // Start is called before the first frame update
    void Start()
    {
        // �v�f���̎擾
        _count = _ei.sheets[0].list.Count;
        _stringData = new string[_count];
        _intData = new int[_count];

        for (int i = 0; i < _count; i++)
        {
            _stringData[i] = _ei.sheets[0].list[i].string_data;
            _intData[i] = _ei.sheets[0].list[i].int_data;
            Debug.Log("string_data��"+ i +"�s�ځF" + _stringData[i]);
            Debug.Log("int_data��"+ i +"�s�ځF" + _intData[i]);
        }

    }

}
