using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Checkerboard : MonoBehaviour
{
    public Piece piece;

    public Material material;

    // 回合数
    int round = 0;

    // 当前轮到的玩家
    int playerIndex = 0;

    List<PlayerVO> players = new List<PlayerVO>();

    Ray ray;

    void Start()
    {
        //material = gameObject.GetComponent<Renderer>().material;

        PlayerVO player1 = new PlayerVO();
        player1.ShaderName = "_Plyer1";
        players.Add(player1);

        PlayerVO player2 = new PlayerVO();
        player2.ShaderName = "_Plyer2";
        players.Add(player2);

        GameStart();
    }

    void OnDisable()
    {
        players.ForEach(p =>
        {
            Vector4[] vector = new Vector4[49];

            material.SetVectorArray(p.ShaderName, vector);
        });
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.textureCoord);
                Vector2 vector = new Vector2(Mathf.Floor(hit.textureCoord.x * 7), Mathf.Floor(hit.textureCoord.y * 7));

                players[playerIndex].BuyChecks(vector, vector);

                UpdateShaderChecks();

                this.TurnEnd();
            }
        }


        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

    }

    /// <summary>
    /// 游戏开始
    /// </summary>
    public void GameStart()
    {
        round = 1;
    }

    /// <summary>
    /// 回合开始
    /// </summary>
    private void TurnStart()
    {

    }

    /// <summary>
    /// 回合结束
    /// </summary>
    private void TurnEnd()
    {
        playerIndex++;
        if (playerIndex >= players.Count)
        {
            playerIndex = 0;
            round++;
        }
    }

    /// <summary>
    /// 更新棋盘上的方格块
    /// </summary>
    private void UpdateShaderChecks()
    {
        string[,] shaderChecks = new string[7, 7];

        for (int i = 0; i < round * 2; i++)
        {
            players.ForEach(p =>
            {
                if (p.ChecksRecord.Count > i)
                {
                    Vector2 check = p.ChecksRecord[i];
                    shaderChecks[(int)check.x, (int)check.y] = p.ShaderName;
                }
            });
        }

        Dictionary<string, List<Vector4>> valuePairs = new Dictionary<string, List<Vector4>>();

        players.ForEach(p =>
        {
            valuePairs[p.ShaderName] = new List<Vector4>();
        });

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                string pName = shaderChecks[i, j];

                if (pName != null)
                {
                    valuePairs[pName].Add(new Vector4(i + 1, j + 1));
                }
            }
        }

        players.ForEach(p =>
        {
            Vector4[] vector = new Vector4[49];

            valuePairs[p.ShaderName].ToArray().CopyTo(vector, 0);

            material.SetVectorArray(p.ShaderName, vector);
        });
    }
}
