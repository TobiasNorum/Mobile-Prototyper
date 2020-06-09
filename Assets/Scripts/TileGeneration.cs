using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGeneration : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private float heightMultiplier;
    [SerializeField]
    private AnimationCurve heightCurve;
    [SerializeField]
    private TerainType[] terainTypes;
    [SerializeField]
    NoiseMapGeneration noiseMapGeneration;
    [SerializeField]
    private MeshRenderer tileRenderer;
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshCollider meshCollider;
    [SerializeField]
    private float mapScale;
    [System.Serializable]
    public class TerainType
    {
        public string name;
        public float height;
        public Color color;
    }
private void UpdateMeshVertices(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);
        Vector3[] meshVertices = this.meshFilter.mesh.vertices;
                            //iterate through all the height map coordinates, updating the vertex index
        int vertexIndex = 0;
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
        for (int xIndex = 0; xIndex < tileWidth; xIndex++)
        {
        float height = heightMap[zIndex, xIndex];
        Vector3 vertex = meshVertices[vertexIndex];
                            //change the vertex Y coordinate proportional to the height value
        meshVertices[vertexIndex] = new Vector3(vertex.x, this.heightCurve.Evaluate(height) * height * this.heightMultiplier, vertex.z);
        vertexIndex++;
        }
        }
                            //update the vertices in the mesh and update its properties
        this.meshFilter.mesh.vertices = meshVertices;
        this.meshFilter.mesh.RecalculateBounds();
        this.meshFilter.mesh.RecalculateNormals();
                            //update the mesh collider
        this.meshCollider.sharedMesh = this.meshFilter.mesh;
    }
TerainType ChooseTerainType(float height)
{
                            //for each terain type, check if the height is lower than the one for the terain type
foreach (TerainType terainType in terainTypes)
    {
                            //return the first terain type whose height is higher than the generated one
if (height < terainType.height)
        {
            return terainType;
        }
    }
    return terainTypes[terainTypes.Length - 1];
}
    
    void Start()
    {
        GenerateTile();
    }
    void GenerateTile()
    {
                                //calculate tile depth and width based on the mesh vertices
        Vector3[] meshVertices = this.meshFilter.mesh.vertices;
        int tileDepth = (int)Mathf.Sqrt (meshVertices.Length);
        int tileWidth = tileDepth;
                                //calculate the offsets based on the tile position
        float offsetX = -this.gameObject.transform.position.x;
        float offsetZ = -this.gameObject.transform.position.z;
                                //generate a heightMap using noise
        float[,] heightMap = this.noiseMapGeneration.GenerateNoiseMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, waves);
                                //build a texture2D from the height map
        Texture2D tileTexture = BuildTexture(heightMap);
        this.tileRenderer.material.mainTexture = tileTexture;
                                //update the tile mesh vertices acording to the height map
        UpdateMeshVertices(heightMap);
    }
    private Texture2D BuildTexture(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);
        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {                   //transform the 2D map index is an Array index
                int colorIndex = zIndex * tileWidth + xIndex;
                float height = heightMap[zIndex, xIndex];
                                //choose a terrain type acording to the height value                
                TerainType terrainType = ChooseTerainType(height);
                                //assign as color a shade of grey proportional to the height value
                colorMap[colorIndex] = terrainType.color;
            }
        }
                                //create a new texture and set its pixel colors
        Texture2D tileTexture = new Texture2D(tileWidth, tileDepth);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colorMap);
        tileTexture.Apply();
        return tileTexture;
    }
}
