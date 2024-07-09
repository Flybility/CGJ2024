using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Attorney;
public class ParagraphEntity : MonoBehaviour,IPointerClickHandler
{
    public string textUnparsed;
    public TMP_Text thisTMPText;
    ParagraphData paragraphData;
    public CanvasGroup canvasGroup;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        GameManager.Instance.hideAllUI+=Fade;
    }
    public void InitText(ParagraphData _paragraphData){
        this.paragraphData = _paragraphData;
        if(paragraphData.paraContent.IsUnityNull())return;
        textUnparsed = paragraphData.paraContent;
        string targetText = ParseTextService.Instance.ParseUnParseStr(paragraphData.paraContent,out string normalStr);
        DOTween.To(() => string.Empty, value => {thisTMPText.text = value;ParagraphSpawner.Instance.EnableAutoScroll();}, normalStr, 0.03f*normalStr.Length).SetEase(DG.Tweening.Ease.Linear)
        .OnComplete(()=>{thisTMPText.text=targetText;
            AudioManager.Instance.StopSound();
            switch (paragraphData.GameState)
        {
            case GameState.Restart:
                GameManager.Instance.GameFinish(true);
                break;
            case GameState.Finish:
                GameManager.Instance.GameFinish();
                break;
        }
        });
        //Debug.Log("Text: " + targetText);
        //thisTMPText.DoText(normalStr,0.5f);
        AudioManager.Instance.PlaySound("Typewriter 0");
        AudioManager.Instance.ChangeBGM(paragraphData.paraBGMChange);
        AudioManager.Instance.PlaySFX(paragraphData.paraAudioClip); //

        
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        string clickedText =null;
        ParseTextService.Instance.TryGetClickedWord(thisTMPText,textUnparsed,out clickedText);
        Debug.Log("Keyword Found: " + clickedText);
        if(clickedText.IsUnityNull())return;
        KeywordData keywordData=null;
        keywordData=KeywordCofig.GetKeywordData(clickedText);
        if (keywordData != null)
        {
            Debug.Log("Keyword Found: " + clickedText);
            if (keywordData.isSpawnNode == 1)
            {
                KeywordNodeManager.Instance.SpawnTextNode(keywordData,this);
            }
            ParagraphSpawner.Instance.SpawnParagraph(keywordData.keyParaId);
        }
        Debug.Log("Clicked Text: " + clickedText);

    }
    public void Fade(float duration){
        canvasGroup.DOFade(0, duration).OnComplete(()=> gameObject.SetActive(false));
    }
}
