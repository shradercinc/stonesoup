using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Editor
{
    public class RoomEditor : EditorWindow
    {
        
        public string AuthorName = "AuthorName";
        
        public string RoomName = "RoomName";

        public GameObject RoomPrefab;
        
        private static Vector2Int roomSize = new Vector2Int(10, 8);
        
        private TextAsset roomTextAsset;
        
        private string textAssetPath = "AuthorName/";
        
        private string roomTextAssetName = "RoomName";
        
        private static int[,] tiles;
        
        // private ee

        public Vector2Int RoomSize
        {
            get => roomSize;
            set => roomSize = value;
        }
        
        
        private static List<Texture> allTiles = new List<Texture>();
        
        private static List<Texture> globalTiles = new List<Texture>();
        
        private int selectedTile = 0;

        [MenuItem("Window/RoomEditor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(RoomEditor));
            tiles = new int[roomSize.x, roomSize.y];
            LoadGlobalTiles();
        }

        private static void LoadGlobalTiles()
        {
            var playerTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Sprites/GeneralTiles/player_down_idle.psd");
            var wallTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Sprites/GeneralTiles/wall_tile.psd");
            var stairTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Sprites/GeneralTiles/stairs.psd");
            allTiles = new List<Texture>();
            allTiles.Add(null);
            allTiles.Add(wallTexture);
            allTiles.Add(playerTexture);
            allTiles.Add(stairTexture);
        }

        private void OnGUI()
        {
            AuthorName = EditorGUILayout.TextField("Author Name", AuthorName);
            RoomName = EditorGUILayout.TextField("Room Name", RoomName);
            RoomPrefab = (GameObject) EditorGUILayout.ObjectField("Room Prefab", RoomPrefab, typeof(GameObject), true);
            RoomSize = EditorGUILayout.Vector2IntField("Room Size", RoomSize);
            roomTextAsset = (TextAsset) EditorGUILayout.ObjectField("Room Text Asset", roomTextAsset, typeof(TextAsset), true);
            textAssetPath = (string) EditorGUILayout.TextField("Text Asset Path", textAssetPath);
            roomTextAssetName = (string) EditorGUILayout.TextField("Text Asset Name", roomTextAssetName);
            if (GUILayout.Button("Load Room Prefab"))
            {
                LoadRoomPrefabInfo();
                LoadLocalTilesFromRoomPrefab();
            }

            if (GUILayout.Button("Load Room By Text Asset"))
            {
                ReadRoomTextFile();
            }

            if (GUILayout.Button("Save Room Text Asset"))
            {
                SaveRoomTextFile();
            }

            if (GUILayout.Button("Create Room Text Asset"))
            {
                CreateRoomTextFile();
            }

            var maxNumPerRow = Mathf.RoundToInt((position.width - 20) / 50);
            var rowNum = allTiles.Count / maxNumPerRow + 1;
            var tileSelectionGroupRect = new Rect(0, 250, position.width, 20 + 50 * rowNum);
            GUI.BeginGroup(tileSelectionGroupRect, "Tiles", EditorStyles.helpBox);
            var buttonIndex = 0;
            for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < maxNumPerRow; j++)
                {
                    if (buttonIndex >= allTiles.Count)
                    {
                        break;
                    }
                    if (GUI.Button(new Rect(10 + 50 * j, 20 + 50 * i, 40, 40), allTiles[buttonIndex]))
                    {
                        SelectTile(buttonIndex);
                    }
                    buttonIndex++;
                }
            }

            GUI.EndGroup();
            
            var groupWidth = 50 * RoomSize.x + 40;
            var groupHeight = 50 * RoomSize.y + 20;
            var groupRect = new Rect(position.width / 2 - groupWidth / 2, 290 + 45 * rowNum, groupWidth, groupHeight);
            GUI.BeginGroup(groupRect, "Room Editor", EditorStyles.helpBox);
            for (int i = 0; i < RoomSize.y; i++)
            {
                for (int j = 0; j < RoomSize.x; j++)
                {
                    if (GUI.Button(new Rect(10 + 45 * j, 20 + 45 * i, 40, 40), allTiles[tiles[j, i]]))
                    {
                        tiles[j, i] = selectedTile;
                    }
                }
            }
            GUI.EndGroup();
        }

        private void CreateRoomTextFile()
        {
            if (roomTextAssetName == "RoomName")
            {
                if (RoomPrefab != null)
                {
                    roomTextAssetName = RoomPrefab.name;
                }
                else
                {
                    roomTextAssetName = "Room";
                }
            }
            if (textAssetPath == "AuthorName/")
            {
                textAssetPath = $"{AuthorName}/";
            }
            var fullPath = $"{Application.dataPath}/Resources/{textAssetPath}{roomTextAssetName}.txt";
            if (System.IO.File.Exists(fullPath))
            {
                throw new Exception("File Already Exists, Path: " + fullPath);
            }
            var authorFolder = textAssetPath.TrimEnd('/');
            var authorFolderPathList = authorFolder.Split('/');
            var curPath = $"{Application.dataPath}/Resources/{authorFolderPathList[0]}";
            foreach (var path in authorFolderPathList)
            {
                if (System.IO.Directory.Exists(curPath))
                {
                    curPath += $"/{path}";
                }
                else
                {
                    System.IO.Directory.CreateDirectory(curPath);
                    curPath += $"/{path}";
                }
            }
            FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            byte[] bts = System.Text.Encoding.UTF8.GetBytes(RoomToText());
            fs.Write(bts, 0, bts.Length);
            fs.Close();
            Debug.Log("Create Room Text File Successfully, Path: " + fullPath);
            AssetDatabase.Refresh();
            roomTextAsset = (TextAsset) AssetDatabase.LoadAssetAtPath($"Assets/Resources/{textAssetPath}{roomTextAssetName}.txt", typeof(TextAsset));
        }
        
        private void SaveRoomTextFile()
        {
            var fullPath = $"{Application.dataPath}/Resources/{textAssetPath}{roomTextAssetName}.txt";
            if (!System.IO.File.Exists(fullPath))
            {
                throw new Exception("File Not Exists, Path: " + fullPath);
            }
            FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            byte[] bts = System.Text.Encoding.UTF8.GetBytes(RoomToText());
            fs.Write(bts, 0, bts.Length);
            fs.Close();
            Debug.Log("Save Room Text File Successfully, Path: " + fullPath);
            AssetDatabase.Refresh();
        }

        private string RoomToText()
        {
            var text = "";
            for (int i = 0; i < RoomSize.y; i++)
            {
                for (int j = 0; j < RoomSize.x; j++)
                {
                    text += tiles[j, i] + ",";
                }

                text = text.TrimEnd(',');
                text += "\r";
                text += "\n";
            }

            return text;
        }

      

        private void LoadLocalTilesFromRoomPrefab()
        {
            var room = RoomPrefab.GetComponent<Room>();
            if (room == null)
            {
                throw new Exception("Room Prefab is Invalid");
            }
            allTiles = allTiles.GetRange(0, 4);
            for (int i = 0; i < room.localTilePrefabs.Length; i++)
            {
                var spriteRenderer = room.localTilePrefabs[i].GetComponent<SpriteRenderer>();
                if (spriteRenderer == null)
                {
                    spriteRenderer = room.localTilePrefabs[i].GetComponentInChildren<SpriteRenderer>();
                }

                if (spriteRenderer == null)
                {
                    allTiles.Add(null);
                }
                else
                {
                    allTiles.Add(spriteRenderer.sprite.texture);
                }
            }
        }

        private void SetRoomSize(Vector2Int roomSize)
        {
            roomSize = RoomSize;
        }
        
        
        private void ReadRoomTextFile()
        {
            textAssetPath = AssetDatabase.GetAssetPath(roomTextAsset);
            roomTextAssetName = roomTextAsset.name;
            roomTextAssetName = roomTextAssetName.Replace(".txt", "");
            textAssetPath = textAssetPath.Replace(roomTextAssetName + ".txt", "");
            textAssetPath = textAssetPath.Replace("Assets/Resources/", "");
            var roomText = roomTextAsset.text;
            roomText = roomText.Replace("\r", "");
            roomText = roomText.TrimEnd('\n');
            var lines = roomText.Split('\n');
            roomSize = new Vector2Int(lines[0].Split(',').Length, lines.Length);
            tiles = new int[roomSize.x, roomSize.y];
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var types = line.Split(',');
                for (int j = 0; j < types.Length; j++)
                {
                    tiles[j, i] = int.Parse(types[j]);
                }
            }
        }

        private void LoadRoomPrefabInfo()
        {
            var room = RoomPrefab.GetComponent<Room>();
            if (room == null)
            {
                throw new Exception("Room Prefab is Invalid");
            }
            AuthorName = room.roomAuthor;
            RoomName = room.gameObject.name;
            if (room.designedRoomFile != null)
            {
                roomTextAsset = room.designedRoomFile;
                textAssetPath = AssetDatabase.GetAssetPath(roomTextAsset);
                roomTextAssetName = roomTextAsset.name;
            }
            else
            {
                roomTextAsset = null;
                textAssetPath = AssetDatabase.GetAssetPath(room).Replace(".prefab", ".txt");
                roomTextAssetName = room.name.Replace(".prefab", ".txt");
            }
            roomTextAssetName = roomTextAssetName.Replace(".txt", "");
            textAssetPath = textAssetPath.Replace(roomTextAssetName + ".txt", "");
            textAssetPath = textAssetPath.Replace("Assets/Resources/", "");
            
        }
        
        private void SelectTile(int tileType)
        {
            selectedTile = tileType;
        }
    }
    
    


}