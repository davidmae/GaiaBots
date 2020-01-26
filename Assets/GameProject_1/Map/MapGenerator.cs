using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameProject_1.Map
{
    public class MapGenerator : MonoBehaviour
    {
        public enum ChunkDirection { None, Right, Bottom, Left, Top }
        
        public GameObject Chunk;

        public void GenerateChunk(ChunkDirection chunkDirection)
        {
            if (chunkDirection == ChunkDirection.None)
                return;

            var position = Chunk.transform.position;

            switch (chunkDirection)
            {
                case ChunkDirection.Right:
                    position.x += Chunk.transform.localScale.x;
                    break;
                case ChunkDirection.Bottom:
                    position.z -= Chunk.transform.localScale.z;
                    break;
                case ChunkDirection.Left:
                    position.x -= Chunk.transform.localScale.x;
                    break;
                case ChunkDirection.Top:
                    position.z += Chunk.transform.localScale.z;
                    break;
                default:
                    break;
            }

            var newChunk = Instantiate(Chunk, position, Quaternion.identity);
            newChunk.transform.parent = gameObject.transform;
        }

    }
}
