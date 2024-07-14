using System.Collections.Generic;
using UnityEngine;

namespace D_yuzuki.Scripts.RoomCharacters.NPC.Braver
{
    public class BraverGenerator : MonoBehaviour
    {
        [SerializeField] private RoomBunker _roomBunker;
        [SerializeField] private BraverController _braverPrefab;
        public List<BraverController> Braver { get; private set; }
        void Start()
        {
            Braver = new List<BraverController>();
            // ���[�h����������
            
            // ���u��
            for (var i = 0; i < 1; i++)
            {
                NewBraverIns(i, 0);
            }
        }

        // �V�����u���[�o�[�̐�������
        private void NewBraverIns(int braverNum, int baseRoom)
        {
            var pos = _roomBunker.RoomDetails[baseRoom].transform.position;
            pos.y += _braverPrefab.transform.localScale.y / 2.0f; 
            var braver = Instantiate(_braverPrefab, pos, Quaternion.identity);
            braver.SetNumber( braverNum, baseRoom);         
            Braver.Add(braver);
        }
    }
}
