using Assets.GameProject_1.Map;
using UnityEngine;

namespace Assets.GameProject_1.UI
{

    public class UIMap : MonoBehaviour
    {
        private MapGenerator.ChunkDirection ChunkDirection;
        private MapGenerator mapGenerator;

        public bool SetRightDirection { get; set; } = false;
        public bool SetBottomDirection { get; set; } = false;
        public bool SetLeftDirection { get; set; } = false;
        public bool SetTopDirection { get; set; } = false;


        private void Awake()
        {
            mapGenerator = GameObject
                .FindGameObjectWithTag("WorldManager")
                .GetComponent<MapGenerator>();
        }

        private void Update()
        {
            if (SetRightDirection)          ChunkDirection = MapGenerator.ChunkDirection.Right;
            else if (SetBottomDirection)    ChunkDirection = MapGenerator.ChunkDirection.Bottom;
            else if (SetLeftDirection)      ChunkDirection = MapGenerator.ChunkDirection.Left;
            else if (SetTopDirection)       ChunkDirection = MapGenerator.ChunkDirection.Top;

            mapGenerator.GenerateChunk(ChunkDirection);

            SetRightDirection = SetBottomDirection = SetLeftDirection = SetTopDirection = false;
            ChunkDirection = MapGenerator.ChunkDirection.None;
        }

        
    }

}