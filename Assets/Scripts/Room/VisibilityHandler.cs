using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

// �����̉����ύX
public class VisibilityHandler
{
    // �V���O���g��
    private static VisibilityHandler instance;
    public static VisibilityHandler Instance => instance ?? (instance = new VisibilityHandler());
    private RoomBunker _roomBunker;
    private bool _isVisibleAllRoom = true;
    public bool IsVisibleAllRoom => _isVisibleAllRoom;
    private int _isClearRoomNum = -1;
    public int IsClearRoomNum => _isClearRoomNum;
    const float DURATION = 0.2f;

    private VisibilityHandler()
    {
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
    }

    public void ChangeAllRoom()
    {
        _isVisibleAllRoom = !_isVisibleAllRoom;
        RoomDetails[] rooms = _roomBunker.RoomDetails;
        List<Material> mats = new List<Material>();
        for (int i = 0; i < rooms.Length; i++)
        {
            if (_isClearRoomNum == i) continue;
            for (int u = 0; u < rooms[i].FrontMesh.Length; u++)
                mats.Add(rooms[i].FrontMesh[u].material);
        }
        FadingMaterial(_isVisibleAllRoom, mats);
    }

    public void ChangeTargetRoom(bool state, int roomNum)
    {
        if (!_isVisibleAllRoom) return;
        RoomDetails[] rooms = _roomBunker.RoomDetails;
        List<Material> mats = new List<Material>();
        for (int i = 0; i < rooms[roomNum].FrontMesh.Length; i++)
            mats.Add(rooms[roomNum].FrontMesh[i].material);
        FadingMaterial(state, mats);
        if (!state)
            _isClearRoomNum = roomNum;
        else
            _isClearRoomNum = -1;
    }

    static async void FadingMaterial(bool state, List<Material> targetMats)
    {
        float[] startAlphas = targetMats.Select(mat => mat.color.a).ToArray();
        float[] targetAlphas = Enumerable.Repeat(state ? 1.0f : 0.0f, targetMats.Count).ToArray();

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        while (sw.ElapsedMilliseconds < DURATION * 1000)
        {
            float currentTime = sw.ElapsedMilliseconds / 1000f;
            await Task.Delay((int)(DURATION * 10));
            for (int i = 0; i < targetMats.Count; i++)
            {
                Color color = targetMats[i].color;
                color.a = Mathf.Lerp(startAlphas[i], targetAlphas[i], currentTime / DURATION);
                targetMats[i].color = color;
            }
        }
        sw.Stop();
        // Debug.Log(sw.ElapsedMilliseconds + "ms");

        // �ڕW�̃A���t�@�l�ɐݒ�
        for (int i = 0; i < targetMats.Count; i++)
        {
            Color color = targetMats[i].color;
            color.a = targetAlphas[i];
            targetMats[i].color = color;
        }
    }
}

// �e��Ԃ̃X�e�[�g�����

