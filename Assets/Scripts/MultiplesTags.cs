using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplesTags : MonoBehaviour
{
    [SerializeField]
    private List<string> customTags = new List<string>();   

    public bool HasTag(string tag)
    {
        return customTags.Contains(tag);
    }

    public void AddTag(string tag)
    {
        if (!customTags.Contains(tag))
        {
            customTags.Add(tag);
        }
    }

    public void RemoveTag(string tag)
    {
        customTags.Remove(tag);
    }

    public List<string> GetAllTags()
    {
        return new List<string>(customTags);
    }
}

