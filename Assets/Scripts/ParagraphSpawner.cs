using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ParagraphSpawner : MonoBehaviour
{
    private static ParagraphSpawner instance;
    public static ParagraphSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ParagraphSpawner>();
            }
            return instance;
        }
    }
    public GameObject paraPrefab;
    public Transform spwanParent;
    public float autoScrollSpeed=10;
    public Dictionary<int,ParagraphEntity> paragraphIDEntities=new Dictionary<int, ParagraphEntity>();
    bool canAutoScroll=false;
    public void DisableAutoScroll(){
        canAutoScroll=false;
    }
    public void EnableAutoScroll(){
        canAutoScroll=true;
    }
    public void SpawnParagraph(int paraId)
    {
        ParagraphData paragraphData = ParagraphConfig.GetParaData(paraId);
        if (paragraphData != null)
        {
            if (!paragraphIDEntities.ContainsKey(paragraphData.paraId)||paragraphData.paraId==23)
            {
                canAutoScroll=true;
                Debug.Log("Spawn Paragraph ID: " + paragraphIDEntities.Keys.Count);
                GameObject newParaObj = Instantiate(paraPrefab, spwanParent);
                ParagraphEntity newPara = newParaObj.GetComponent<ParagraphEntity>();
                newPara.InitText(ParagraphConfig.GetParaData(paraId));
                paragraphIDEntities.Add(paragraphData.paraId,newPara);
            }


        }
        Debug.Log("StartTelling");
    }
    public void ShowRichText(){
        foreach(ParagraphEntity para in paragraphIDEntities.Values){
            para.thisTMPText.richText=true;
        }
    }
    public void HideRichText(){
        foreach(ParagraphEntity para in paragraphIDEntities.Values){
            para.thisTMPText.richText=false;
        }
    }
    public void HideAllParagraphs(float fadeTime){
         foreach(ParagraphEntity para in paragraphIDEntities.Values){
            para.canvasGroup.DOFade(0,fadeTime);
        }
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(canAutoScroll){
            spwanParent.position+=Vector3.up*Time.deltaTime*autoScrollSpeed;
        }
        
    }
}
