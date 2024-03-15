using System.Collections.Generic;
using UnityEngine;

// �K�i�̏㉺�T��
public class StairSelecter
{
    // �V���O���g��
    private static StairSelecter instance;
    public static StairSelecter Instance => instance ?? (instance = new StairSelecter());

    private RoomBunker _roomBunker;
    private StairSelecter()
    {
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
    }

    // �o�Ă������W�̑I��
    public Stair FloorSelecter(int calledFloor, int baseRoom)
    {
        List<int> stairList = SearchStairs(calledFloor, baseRoom);
        int rnd = Random.Range(0, stairList.Count);
        return _roomBunker.Stairs[stairList[rnd]];
    }

    // �K�i�̏㉺�T��
    public List<int> SearchStairs(int floor, int npcRoom)
    {
        int floorRoomCount = _roomBunker.FloorRoomCount;
        int npcFloor = npcRoom / floorRoomCount + 1;
        List<int> stairList = new List<int> { };
        int roomNum = (floor * floorRoomCount) + floorRoomCount - 1;
        int upperStairRoom = roomNum + floorRoomCount;
        int floorDifference = Mathf.Abs((upperStairRoom / floorRoomCount) - npcFloor);
        if (floor != _roomBunker.TopFloor && floorDifference <= 1)
        {
            if (_roomBunker.RoomDetails[upperStairRoom - 1].IsRoomAcceptance(npcRoom))
                stairList.Add(upperStairRoom / floorRoomCount);
        }
        if (floor != 0 && floorDifference <= 1)
        {
            int lowerStairRoom = roomNum - floorRoomCount;
            if (_roomBunker.RoomDetails[lowerStairRoom].IsRoomAcceptance(npcRoom))
                stairList.Add(lowerStairRoom / floorRoomCount);
        }
        return stairList;
    }
}
