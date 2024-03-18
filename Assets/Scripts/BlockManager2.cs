using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;
using UnityEngine.SceneManagement;

public class BlockManager2 : MonoBehaviour
{
    public int numBlockRow = 9;
    public int numBlockCol = 16;
    public GameObject blockPrefab;
    public Sprite[] blockSprites;
    public TMP_Text numParejas;
    int n_numParejas2;
    public TMP_Text nFinalParejas2;
    int _nFinalParejas2;
    private int hightScore2;
    public TMP_Text hightScore2Text;
   

    public GameObject panelGameOver;

    List<int> randomTypeMap = new List<int>();  // Asigna la posición lógica a TypeID.
    List<List<GameObject>> blocks = new List<List<GameObject>>();
    Vector3 blocksCenter;
    GameObject clickedBlock;
    LinkAlgorithm linkAlgorithm = new LinkAlgorithm();

    public void Start()
    {
        hightScore2 = PlayerPrefs.GetInt("HightScore2", 0);
        hightScore2Text.text = " " + hightScore2.ToString();
        

        panelGameOver.SetActive(false);
        GenerateTypeMap();

        blocksCenter = new Vector3(
          -(float)(numBlockCol - 1) * 0.5f,
          -(float)(numBlockRow - 1) * 0.5f,
          0);

        // Generate all blocks.
        for (int x = 0; x < numBlockCol; x++)
        {
            blocks.Add(new List<GameObject>());
            for (int y = 0; y < numBlockRow; y++)
            {
                GameObject newBlock = GameObject.Instantiate(blockPrefab);
                blocks[x].Add(newBlock);
                newBlock.transform.parent = this.transform;

                Block block = newBlock.GetComponent<Block>();
                int blockType = GetBlockType(x, y);
                Vector3Int logicalPosition = new Vector3Int(x, y, 0);
                block.SetPhysicalPosition(ToPhysicalPosition(logicalPosition))
                    .SetLogicalPosition(logicalPosition)
                    .SetTypeID(blockType)
                    .SetSprite(blockSprites[blockType]);
            }
        }
        linkAlgorithm.Initialize(blocks, numBlockCol, numBlockRow);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject mousedOverBlock = MousedOverBlock();
            if (mousedOverBlock)
            {
                AudioPlayer.Instance.PlaySFX(AudioPlayer.SFXName.Click);

                List<Vector3Int> turns = LinkableToClickedBlock(mousedOverBlock);
                if (turns != null)
                {
                    StartCoroutine(OnBlocksLinked(turns, clickedBlock, mousedOverBlock));
                    clickedBlock = null;
                    n_numParejas2++;
                    _nFinalParejas2 = n_numParejas2;
                    numParejas.text = n_numParejas2.ToString();
                    if(n_numParejas2 > hightScore2)
                    {
                        Save();
                    }
                   
                }
                else
                {
                    SetClickedBlock(mousedOverBlock);
                }
            }
            else
            {
                ResetClickedBlock();
            }
        }
    }

    void GenerateTypeMap()
    {
        int numBlockTypes = blockSprites.Length;
        Assert.IsTrue(numBlockRow * numBlockCol % numBlockTypes == 0);
        int perTypeBlockCount = numBlockRow * numBlockCol / numBlockTypes;
        Assert.IsTrue(perTypeBlockCount % 2 == 0);

        for (int type = 0; type < numBlockTypes; type++)
        {
            for (int count = 0; count < perTypeBlockCount; count++)
            {
                randomTypeMap.Add(type);
            }
        }

        Shuffle(randomTypeMap);
    }

    static void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(0, list.Count);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }

    int GetBlockType(int x, int y)
    {
        return randomTypeMap[x * numBlockRow + y];
    }

    void SetClickedBlock(GameObject block)
    {
        if (clickedBlock)
        {
            ResetClickedBlock();
        }
        clickedBlock = block;
        clickedBlock.GetComponent<Block>().SetTransparency(0.5f);
    }

    void ResetClickedBlock()
    {
        if (!clickedBlock)
        {
            return;
        }
        clickedBlock.GetComponent<Block>().SetTransparency(1f);
        clickedBlock = null;
    }

    List<Vector3Int> LinkableToClickedBlock(GameObject block)
    {
        if (!clickedBlock || clickedBlock == block || !Block.IsSameType(clickedBlock, block))
        {
            return null;
        }
        return linkAlgorithm.Linkable(clickedBlock, block);
    }

    Vector3 ToPhysicalPosition(Vector3Int logicalPosition)
    {
        return logicalPosition + blocksCenter;
    }

    GameObject MousedOverBlock()
    {
        RaycastHit2D hit = Physics2D.Raycast(
          Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
        if (!hit.collider)
        {
            return null;
        }
        if (hit.collider.gameObject.GetComponent<Block>())
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    IEnumerator OnBlocksLinked(List<Vector3Int> turns, GameObject block1, GameObject block2)
    {
        block1.GetComponent<BoxCollider2D>().enabled=false;
        block2.GetComponent<BoxCollider2D>().enabled=false;

        // Sound effect.
        AudioPlayer.Instance.PlaySFX(AudioPlayer.SFXName.Link);

        // Visual effect.
        block1.GetComponent<Block>().SetTransparency(0.5f);
        block2.GetComponent<Block>().SetTransparency(0.5f);

        List<Vector3> pointsOnLine = new List<Vector3>();
        pointsOnLine.Add(block1.transform.position);
        turns.ForEach((Vector3Int turn) =>
        {
            pointsOnLine.Add(ToPhysicalPosition(turn));
        });
        pointsOnLine.Add(block2.transform.position);
        LineDrawer.DrawLine(pointsOnLine);

        yield return new WaitForSeconds(0.3f);
        LineDrawer.ClearLine();
        Destroy(block1);
        Destroy(block2);
    }

    public void GameOver()
    {
        panelGameOver.SetActive(true);
        n_numParejas2 = _nFinalParejas2;
        nFinalParejas2.text = _nFinalParejas2.ToString();
    }
    void Save()
    {
        PlayerPrefs.SetInt("HightScore2", n_numParejas2);

        hightScore2 = n_numParejas2;
        hightScore2Text.text = "  " + hightScore2.ToString();
    }
    public void Salir()
    {
        Application.Quit();
    }
    public void IrAMenu(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }
}
