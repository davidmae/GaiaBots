using UnityEngine;
using static Assets.GameProject_1.Map.MapGenerator;

namespace Assets.GameProject_1.Map
{
    public class MapButton : MonoBehaviour
    {
        public ChunkDirection chunkDirection;
        private MapGenerator mapGenerator;
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            mapGenerator = GameObject
                .FindGameObjectWithTag("WorldManager").GetComponent<MapGenerator>();

            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnMouseOver()
        {
            meshRenderer.enabled = true;
        }

        private void OnMouseExit()
        {
            meshRenderer.enabled = false;
        }

        private void OnMouseDown()
        {
            mapGenerator.ActivateChunk(chunkDirection, transform.parent.gameObject.GetComponent<Chunk>());
            gameObject.SetActive(false);
        }
    }
}
