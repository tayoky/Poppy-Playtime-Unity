using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class PPU
{
    [MenuItem("Tools/PPU/Update/OpenPage")]
    public static void OpenUpdatePage()
    {
        Application.OpenURL("https://github.com/tayoky/Poppy-Playtime-Unity/tree/main");
    }
    [MenuItem("Tools/PPU/PuzzleObject/GreenPower")]
    public static void CreateGreenPower()
    {
        Inst("Assets/Prefab/Puzzle/GreenPower.prefab");
    }
    [MenuItem("Tools/PPU/PuzzleObject/GreenRecevier")]
    public static void CreateGreenRecevier()
    {
        PrefabUtility.InstantiatePrefab(Resources.Load("Assets/Prefab/Puzzle/GreenRecevier.prefab"));
    }
    [MenuItem("Tools/PPU/Item")]
    public static void CreateItem()
    {

    }

    [MenuItem("Tools/PPU/Credit")]
    public static void Credit()
    {
        Debug.Log("Tayoky");
    }

    public static void Inst(string path)
    {
        PrefabUtility.InstantiatePrefab(Resources.Load(path));
    }
}
