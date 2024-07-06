using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OvO;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System;

namespace UnderCloud
{
    public class MapManager : Singleton<MapManager>
    {
        private static Dictionary<Vector2Int, BaseWallController> tiles;
        public MapManager()
        {
            tiles ??= new Dictionary<Vector2Int, BaseWallController>();
        }
        /// <summary>
        /// ��ȡһ������ؿ����Ϣ
        /// </summary>
        /// <param name="position">�ؿ�����λ��</param>
        /// <param name="playerState">��ҵ�ǰ���۱���״̬</param>
        /// <returns></returns>
        public static BaseWallController GetTile(Vector2Int position)
        {
            if (tiles.TryGetValue(position, out BaseWallController tile))
            {
                return tile;
            }
            else
                return null;
        }
        /// <summary>
        /// һ���ؿ��ܷ�ͨ��
        /// </summary>
        /// <param name="position">�ؿ�����λ��</param>
        /// <param name="playerState">��ҵ�ǰ���۱���״̬</param>
        /// <returns></returns>
        public static bool IsAccessible(Vector2Int position, PlayerState playerState)
        {
            if (tiles.TryGetValue(position, out BaseWallController tile))
            {
                if (playerState == PlayerState.Open)
                    return tile.IsAccessibleOpen;
                else
                    return tile.IsAccessibleClose;
            }
            else
                return true;
        }
        /// <summary>
        /// һ���ؿ��Ƿ��ܶ��������˺�
        /// </summary>
        /// <param name="position">�ؿ�����λ��</param>
        /// <param name="playerState">��ҵ�ǰ���۱���״̬</param>
        /// <returns></returns>
        public static bool IsDamagable(Vector2Int position, PlayerState playerState)
        {
            if (tiles.TryGetValue(position, out BaseWallController tile))
            {
                if (playerState == PlayerState.Open)
                    return tile.IsAccessibleOpen;
                else
                    return tile.IsAccessibleClose;
            }
            else
                return false;
        }
        /// <summary>
        /// �����ͼ���ݣ�����һ���µĹؿ�
        /// </summary>
        /// <param name="levelNum">�ؿ����</param>
        public static void LoadMapOfCurrentLevel()
        {
            //ɨ�貢¼�뵱ǰ��ͼ
            tiles ??= new Dictionary<Vector2Int, BaseWallController>();
            tiles.Clear();
            TileBase tile;
            foreach (Tilemap map in GameObject.FindWithTag(TagName.TileMap).transform.GetComponentsInChildren<Tilemap>())
            {
                for (int i = -32; i < 32; i++)
                {
                    for(int j = -32; j < 32; j++)
                    {
                        tile = map.GetTile(new Vector3Int(i, j, 0));
                        if (tile != null)
                        {
                            if (tile is CustomAnimatedTile customTile)
                            {
                                if (!tiles.ContainsKey(new Vector2Int(i, j)))
                                {
                                    tiles.Add(new Vector2Int(i, j), GenerateTile(customTile.type));
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void UpdateMap(MapUpdate update)
        {
            foreach (var (positionsToUpdate, newTile) in update.values)
            {
                tiles[positionsToUpdate] = newTile;
            }
        }
        private static BaseWallController GenerateTile(TileType type)
        {
            return type switch
            {
                TileType.NormalWall => new NormalWallController(),
                _ => null,
            };
        }
    }
}