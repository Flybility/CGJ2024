using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Linq;

public class KeywordNode : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerDownHandler,IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
{
    public KeywordData keywordData;
    //public ParagraphEntity paragraphEntity;
    [SerializeField] TMP_Text tMP_Text;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject border;
    RectTransform LimitContainer;
    Canvas canvas;

    RectTransform rt;
    // 位置偏移量
    Vector3 offset = Vector3.zero;
    // 最小、最大X、Y坐标
    float minX, maxX, minY, maxY;
    
    void Start()
    {
         tMP_Text = GetComponent<TMP_Text>();
        rt = GetComponent<RectTransform>();
        canvas=KeywordNodeManager.Instance.canvas;
        canvasGroup=GetComponent<CanvasGroup>();
        LimitContainer = KeywordNodeManager.Instance.dragParent.GetComponent<RectTransform>();
        GameManager.Instance.hideAllUI+=Fade;
    }

    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="eventData"></param>

    /// <summary>
    /// 拖拽中
    /// </summary>
    /// <param name="eventData"></param>

    // 设置最大、最小坐标
    void SetDragRange()
    {
        // 最小x坐标 = 容器当前x坐标 - 容器轴心距离左边界的距离 + UI轴心距离左边界的距离
        minX = LimitContainer.position.x
            - LimitContainer.pivot.x * LimitContainer.rect.width * canvas.scaleFactor
            + rt.rect.width * canvas.scaleFactor * rt.pivot.x;
        // 最大x坐标 = 容器当前x坐标 + 容器轴心距离右边界的距离 - UI轴心距离右边界的距离
        maxX = LimitContainer.position.x
            + (1 - LimitContainer.pivot.x) * LimitContainer.rect.width * canvas.scaleFactor
            - rt.rect.width * canvas.scaleFactor * (1 - rt.pivot.x);

        // 最小y坐标 = 容器当前y坐标 - 容器轴心距离底边的距离 + UI轴心距离底边的距离
        minY = LimitContainer.position.y
            - LimitContainer.pivot.y * LimitContainer.rect.height * canvas.scaleFactor
            + rt.rect.height * canvas.scaleFactor * rt.pivot.y;

        // 最大y坐标 = 容器当前x坐标 + 容器轴心距离顶边的距离 - UI轴心距离顶边的距离
        maxY = LimitContainer.position.y
            + (1 - LimitContainer.pivot.y) * LimitContainer.rect.height * canvas.scaleFactor
            - rt.rect.height * canvas.scaleFactor * (1 - rt.pivot.y);
    }
    // 限制坐标范围
    Vector3 DragRangeLimit(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        return pos;
    }
    public void Fade(float duration){
        canvasGroup.DOFade(0, duration).OnComplete(()=> gameObject.SetActive(false));
    }
    public void Init(KeywordData keywordData,ParagraphEntity paragraphEntity=null)
    {
        this.keywordData = keywordData;
        //this.paragraphEntity = paragraphEntity;
        tMP_Text.text = keywordData.keyId;
        //AudioManager.Instance.PlaySound(keywordData.audioClip);
    }
    public void InteractWithKeyword(string keyword)
    {
        if (keywordData.keyInteractKey == keyword)
        {
            keywordData=KeywordCofig.GetKeywordData(keywordData.keyInteractedKeyId);
            //触发关键字交互事件
            ParagraphSpawner.Instance.SpawnParagraph(int.Parse(keywordData.keyInteractParaId));
        }
    }
    int initialSiblingIndex;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.enterEventCamera, out Vector3 globalMousePos))
        {
            // 计算偏移量
            offset = rt.position - globalMousePos;
            // 设置拖拽范围
            SetDragRange();
        }
        //throw new System.NotImplementedException();
        transform.SetParent(KeywordNodeManager.Instance.dragParent);
        initialSiblingIndex=transform.GetSiblingIndex();
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        if(eventData.button== PointerEventData.InputButton.Left)
        {            
            Vector3 pos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.enterEventCamera, out pos);
            transform.position = pos-(Vector3)dragOffset;
        }
        // 将屏幕空间上的点转换为位于给定RectTransform平面上的世界空间中的位置
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, null, out Vector3 globalMousePos))
        {
            rt.position = DragRangeLimit(globalMousePos + offset);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //bool interactSuccess=false;
        KeywordNode targetNode=KeywordNodeManager.Instance.CheckKeywordNodeOverlap(this);

        if(targetNode != null)
        {
            //Debug.LogError("与"+targetNode.keywordData.keyId+"交互");
            if (targetNode.keywordData.KeyInteractKeys.Contains(keywordData.keyId))
            {
                int index = -1;
                for (int i = 0; i < targetNode.keywordData.KeyInteractKeys.Length; i++)
                {
                    if (targetNode.keywordData.KeyInteractKeys[i] == keywordData.keyId)
                    {
                        //targetNode.InteractWithKeyword(targetNode.keywordData.KeyInteractKeys[i]);
                        index = i;
                        break;
                    }
                }
                // }
                // if(targetNode.keywordData.keyInteractKey==keywordData.keyId){
                // Debug.Log(keywordData.keyId+"与"+targetNode.keywordData.keyId+"交互成功");
                // Debug.Log("触发"+targetNode.keywordData.keyInteractParaId+"段落");
                ParagraphSpawner.Instance.SpawnParagraph(int.Parse(targetNode.keywordData.KeyInteractParaIds[index]));
                if (targetNode.keywordData.isDestroyThen == 1)
                {
                    targetNode.canvasGroup.DOFade(0, 0.5f).OnComplete(() => targetNode.gameObject.SetActive(false));
                    //Destroy(gameObject);
                }
                if (keywordData.isDestroyThen==1){
                    //KeywordNodeManager.Instance.RemoveKeywordNode(this);
                    canvasGroup.DOFade(0,0.5f).OnComplete(()=>gameObject.SetActive(false));
                }
                if(!targetNode.keywordData.keyInteractedKeyId.IsUnityNull()){
                    //targetNode.keywordData=KeywordCofig.GetKeywordData(targetNode.keywordData.keyInteractedKeyId);
                    
                    //targetNode.paragraphEntity.thisTMPText.text=paragraphEntity.thisTMPText.text.Replace(targetNode.keywordData.keyId,targetNode.keywordData.keyText);
                    targetNode.Init(KeywordCofig.GetKeywordData(targetNode.keywordData.keyInteractedKeyId));
                    //ParagraphSpawner.Instance.paragraphIDEntities.
                }
            }
        }
    }
    Vector2 dragOffset;
    public void OnPointerDown(PointerEventData eventData)
    {
        dragOffset=eventData.position-(Vector2)transform.position;
        border.transform.DOScale(Vector2.one*1.1f, 0.1f);
    }
        public void OnPointerUp(PointerEventData eventData)
    {
        border.transform.DOScale(Vector2.one*1f, 0.1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        border.SetActive(true);
        border.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        border.SetActive(false);
    }


}
