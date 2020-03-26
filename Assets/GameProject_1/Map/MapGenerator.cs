using Assets.GameFramework.Movement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.GameProject_1.Map
{
    public class MapGenerator : MonoBehaviour
    {
        public enum ChunkDirection { None, Right, Bottom, Left, Top }

        public int ChunksX;
        public int ChunksY;
        public Chunk InitialChunk;

        Chunk Chunk;
        Chunk[,] ChunkMatrix;
        ChunkDirection chunkDirection;
        
        event Action hideChunks;

        private void Awake()
        {
            Chunk = Resources.Load<Chunk>("Chunk");

            ChunkMatrix = new Chunk[ChunksX, ChunksY];

            ChunkMatrix[0, 0] = InitialChunk;

            var position = Chunk.transform.position;
            position.x = InitialChunk.transform.localScale.x;
            position.y = 0f;
           
            for (int posX = 0; posX < ChunksX; ++posX)
            {
                for (int posY = 0; posY < ChunksY; ++posY)
                {
                    if (posX == 0 && posY == 0)
                        continue;

                    var newchunk = Instantiate(Chunk, position, Quaternion.identity);
                    newchunk.transform.parent = gameObject.transform;

                    //newchunk.gameObject.AddComponent<NavMeshSurface>();
                    newchunk.positionMatrix.x = posX;
                    newchunk.positionMatrix.y = posY;
                    newchunk.name = $"Chunk{posX}{posY}";

                    hideChunks += () => newchunk.gameObject.SetActive(false);
                    
                    ChunkMatrix[posX, posY] = newchunk;

                    position.x += Chunk.transform.localScale.x;
                }

                position.z -= Chunk.transform.localScale.z;
                position.x = 0f;
            }

            //Chunk.gameObject.SetActive(false);
            //ChunkMatrix[0, 0].gameObject.SetActive(true);

            ChunkMatrix[0, 0].GetComponent<NavMeshSurface>().BuildNavMesh();
            hideChunks();

            var collider = InitialChunk.GetComponent<BoxCollider>();
            collider.center = new Vector3((ChunksX - 1) * 0.5f, 1f, (ChunksY - 1) * -0.5f);
            collider.size = new Vector3(ChunksX, 0f, ChunksY);

        }

        public Chunk GetChunk(int x, int y) => ChunkMatrix[x, y];

        public void GenerateChunk(ChunkDirection chunkDirection, Chunk currentChunk = null)
        {
            if (chunkDirection == ChunkDirection.None)
                return;

            Chunk = currentChunk ?? Chunk;

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

        public void ActivateChunk(ChunkDirection chunkDirection, Chunk currentChunk)
        {
            if (chunkDirection == ChunkDirection.None)
                return;

            var nextPosition = currentChunk.positionMatrix;

            switch (chunkDirection)
            {
                case ChunkDirection.Right:
                    MovementStaticsValues.XRange[1] += currentChunk.transform.localScale.x;
                    nextPosition.y++;
                    break;
                case ChunkDirection.Bottom:
                    MovementStaticsValues.ZRange[0] -= currentChunk.transform.localScale.z;
                    nextPosition.x++;
                    break;
                case ChunkDirection.Left:
                    MovementStaticsValues.XRange[0] -= currentChunk.transform.localScale.x;
                    nextPosition.y--;
                    break;
                case ChunkDirection.Top:
                    MovementStaticsValues.ZRange[1] += currentChunk.transform.localScale.z;
                    nextPosition.x--;
                    break;
                default:
                    break;
            }

            var nextChunk = ChunkMatrix[(int)nextPosition.x, (int)nextPosition.y];
            var mapBtns = nextChunk.GetComponentsInChildren<MapButton>();

            Chunk adjacentChunkTop = null;
            Chunk adjacentChunkRight = null;
            Chunk adjacentChunkBottom = null;
            Chunk adjacentChunkLeft = null;

            try { adjacentChunkTop = ChunkMatrix[(int)nextPosition.x - 1, (int)nextPosition.y]; }
            catch (Exception ex) { }

            try { adjacentChunkRight = ChunkMatrix[(int)nextPosition.x, (int)nextPosition.y + 1]; }
            catch (Exception ex) { }

            try { adjacentChunkBottom = ChunkMatrix[(int)nextPosition.x + 1, (int)nextPosition.y]; }
            catch (Exception ex) { }

            try { adjacentChunkLeft = ChunkMatrix[(int)nextPosition.x, (int)nextPosition.y - 1]; }
            catch (Exception ex) { }


            if (adjacentChunkTop != null && adjacentChunkTop.gameObject.activeSelf)
            {
                adjacentChunkTop.GetComponentsInChildren<MapButton>().Where(x => x.chunkDirection == ChunkDirection.Bottom).FirstOrDefault().gameObject.SetActive(false);
                mapBtns.Where(x => x.chunkDirection == ChunkDirection.Top).FirstOrDefault().gameObject.SetActive(false);
            }

            if (adjacentChunkRight != null && adjacentChunkRight.gameObject.activeSelf)
            {
                adjacentChunkRight.GetComponentsInChildren<MapButton>().Where(x => x.chunkDirection == ChunkDirection.Left).FirstOrDefault().gameObject.SetActive(false);
                mapBtns.Where(x => x.chunkDirection == ChunkDirection.Right).FirstOrDefault().gameObject.SetActive(false);
            }

            if (adjacentChunkBottom != null && adjacentChunkBottom.gameObject.activeSelf)
            {
                adjacentChunkBottom.GetComponentsInChildren<MapButton>().Where(x => x.chunkDirection == ChunkDirection.Top).FirstOrDefault().gameObject.SetActive(false);
                mapBtns.Where(x => x.chunkDirection == ChunkDirection.Bottom).FirstOrDefault().gameObject.SetActive(false);
            }

            if (adjacentChunkLeft != null && adjacentChunkLeft.gameObject.activeSelf)
            {
                adjacentChunkLeft.GetComponentsInChildren<MapButton>().Where(x => x.chunkDirection == ChunkDirection.Right).FirstOrDefault().gameObject.SetActive(false);
                mapBtns.Where(x => x.chunkDirection == ChunkDirection.Left).FirstOrDefault().gameObject.SetActive(false);
            }

            ChunkMatrix[(int)nextPosition.x, (int)nextPosition.y].gameObject.SetActive(true);
        }
    }
}
