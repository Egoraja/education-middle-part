using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class DissolvingController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] skinnedMesh;
    [SerializeField] private VisualEffect VFXGraph;
    [SerializeField] private float dissolveRate = 0.0125f;
    [SerializeField] private float refreshRate = 0.025f;    
    private Material[] skinnedMaterials;      

    private void Start()
    {       
        if (skinnedMesh != null)
        {
            skinnedMaterials = new Material[skinnedMesh.Length];
            for (int i = 0; i < skinnedMesh.Length; i++)
            {
                skinnedMaterials[i] = skinnedMesh[i].material;
            }              
        }        
    }

    public void StartDissolving()
    {       
            StartCoroutine(DissolveCoroutine());       
    }

    private IEnumerator DissolveCoroutine()
    {
        if (VFXGraph != null)
        {
            VFXGraph.Play();
        }
        if (skinnedMaterials.Length>0)
        {
            float counter = 0;

            while (skinnedMaterials[0].GetFloat("_DisolveAmount") < 1 )
            {
                counter += dissolveRate;

                for (int i = 0; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_DisolveAmount", counter);                
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }    
    } 
}
