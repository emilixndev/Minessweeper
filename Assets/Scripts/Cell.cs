using UnityEngine;

public class Cell
{
    public enum Type
    {
        Empty,
        Mine,
        Number,
        Invalid,
    }

    public Type type;
    public int number;
    public bool revealed;
    public bool isFlagged;
    public Vector3Int position;
    public bool exploded;
    public bool chorded;
}