using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int mapWidth = 20;
    [SerializeField] private int mapHeight = 20;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject corridorPrefab;
    [SerializeField] private int minRoomSize = 3;
    [SerializeField] private int maxRoomSize = 8;
    
    private List<Bounds> rooms = new List<Bounds>();
    
    public void GenerateMap()
    {
        // 清除旧地图
        ClearMap();
        
        // 使用BSP算法生成房间
        GenerateRooms();
        
        // 连接房间
        ConnectRooms();
    }
    
    private void ClearMap()
    {
        // 清除所有子物体
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        rooms.Clear();
    }
    
    private void GenerateRooms()
    {
        // 生成初始大房间
        Bounds initialRoom = new Bounds(
            new Vector3(mapWidth / 2, mapHeight / 2, 0),
            new Vector3(mapWidth, mapHeight, 0)
        );
        
        // 递归分割房间
        SplitRoom(initialRoom);
        
        // 实例化房间
        foreach (var room in rooms)
        {
            InstantiateRoom(room);
        }
    }
    
    private void SplitRoom(Bounds room)
    {
        // 检查房间是否还需要继续分割：
        // 当房间的宽度或高度超过最大房间尺寸的2倍时继续分割
        if (room.size.x > maxRoomSize * 2 || room.size.y > maxRoomSize * 2)
        {
            // 随机决定分割方向：水平或垂直（50%概率）
            bool splitHorizontal = Random.Range(0, 2) == 0;
            
            if (splitHorizontal)
            {
                // 水平分割逻辑：
                // 1. 在x轴上随机选择一个分割点，确保分割后两个子房间都能满足最小尺寸要求
                //    - 分割点最小值为房间左边界 + 最小房间尺寸
                //    - 分割点最大值为房间右边界 - 最小房间尺寸
                float splitPoint = Random.Range(room.min.x + minRoomSize, room.max.x - minRoomSize);
                
                // 2. 创建左子房间：
                //    - 中心点x坐标：(原房间左边界 + 分割点)/2
                //    - 宽度：分割点 - 原房间左边界
                //    - 高度保持不变
                SplitRoom(new Bounds(
                    new Vector3((room.min.x + splitPoint) / 2, room.center.y, 0),
                    new Vector3(splitPoint - room.min.x, room.size.y, 0)
                ));
                
                // 3. 创建右子房间：
                //    - 中心点x坐标：(分割点 + 原房间右边界)/2
                //    - 宽度：原房间右边界 - 分割点
                //    - 高度保持不变
                SplitRoom(new Bounds(
                    new Vector3((splitPoint + room.max.x) / 2, room.center.y, 0),
                    new Vector3(room.max.x - splitPoint, room.size.y, 0)
                ));
            }
            else
            {
                // 垂直分割逻辑（与水平分割类似，只是x和y轴互换）：
                // 1. 在y轴上随机选择一个分割点
                float splitPoint = Random.Range(room.min.y + minRoomSize, room.max.y - minRoomSize);
                
                // 2. 创建下子房间
                SplitRoom(new Bounds(
                    new Vector3(room.center.x, (room.min.y + splitPoint) / 2, 0),
                    new Vector3(room.size.x, splitPoint - room.min.y, 0)
                ));
                
                // 3. 创建上子房间
                SplitRoom(new Bounds(
                    new Vector3(room.center.x, (splitPoint + room.max.y) / 2, 0),
                    new Vector3(room.size.x, room.max.y - splitPoint, 0)
                ));
            }
        }
        else
        {
            // 当房间尺寸足够小时，停止分割并将其添加到房间列表
            rooms.Add(room);
        }
    }
    
    private void InstantiateRoom(Bounds room)
    {
        GameObject roomObj = Instantiate(roomPrefab, room.center, Quaternion.identity, transform);
        roomObj.transform.localScale = new Vector3(room.size.x - 1, room.size.y - 1, 1);
    }
    
    private void ConnectRooms()
    {
        // 按X坐标排序房间
        rooms.Sort((a, b) => a.center.x.CompareTo(b.center.x));
        
        // 连接相邻房间
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            CreateCorridor(rooms[i], rooms[i + 1]);
        }
    }
    
    private void CreateCorridor(Bounds startRoom, Bounds endRoom)
    {
        Vector3 start = startRoom.center;
        Vector3 end = endRoom.center;
        Vector3 direction = end - start;
        
        // 创建水平走廊（X轴方向）
        if (Mathf.Abs(direction.x) > 0.5f)
        {
            // 确定走廊起点和终点的X坐标
            float startX = direction.x > 0 ? start.x + startRoom.size.x/2 : start.x - startRoom.size.x/2;
            float endX = direction.x > 0 ? end.x - endRoom.size.x/2 : end.x + endRoom.size.x/2;
            
            Vector3 horizontalCorridorPos = new Vector3(
                (startX + endX) / 2,
                start.y, // 保持Y坐标不变
                0
            );
            
            GameObject corridor = Instantiate(
                corridorPrefab,
                horizontalCorridorPos,
                Quaternion.identity,
                transform
            );
            
            corridor.transform.localScale = new Vector3(
                Mathf.Abs(startX - endX),
                1, // 走廊高度固定为1
                1
            );
        }
        
        // 创建垂直走廊（Y轴方向）
        if (Mathf.Abs(direction.y) > 0.5f)
        {
            // 确定走廊起点和终点的Y坐标
            float startY = direction.y > 0 ? start.y + startRoom.size.y/2 : start.y - startRoom.size.y/2;
            float endY = direction.y > 0 ? end.y - endRoom.size.y/2 : end.y + endRoom.size.y/2;
            
            Vector3 verticalCorridorPos = new Vector3(
                end.x, // 保持X坐标不变
                (startY + endY) / 2,
                0
            );
            
            GameObject corridor = Instantiate(
                corridorPrefab,
                verticalCorridorPos,
                Quaternion.identity,
                transform
            );
            
            corridor.transform.localScale = new Vector3(
                1, // 走廊宽度固定为1
                Mathf.Abs(startY - endY),
                1
            );
        }
    }
}