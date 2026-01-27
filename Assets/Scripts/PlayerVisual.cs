using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMesh;
    private MaterialPropertyBlock mpb;
    void Awake()
    {
        mpb = new MaterialPropertyBlock();
    }

    public void SetPlayerColor(Color color)
    {
        skinnedMesh.GetPropertyBlock(mpb, 0);   // index 1
        mpb.SetColor("_BaseColor", color);
        skinnedMesh.SetPropertyBlock(mpb, 0);
    }
}
