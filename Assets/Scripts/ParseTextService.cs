using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ParseTextService : MonoBehaviour
{
    private static ParseTextService instance;
    public static ParseTextService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ParseTextService>();
            }
            return instance;
        }
    }
    public TMP_Text curTextComponent;
    //public string curParseStr; 

    public void SetParseText(TMP_Text tMP_Text)
    {
        curTextComponent = tMP_Text;
    }
    public void RemoveParseText(TMP_Text tMP_Text)
    {
        if (curTextComponent == tMP_Text)
        {
            curTextComponent = null;
        }
    }
    public string ParseUnParseStr(string parseStr,out string normalStr){
        string result = parseStr;
        string normalStr1 = result;
        normalStr1 = normalStr1.Replace("（", "");
        normalStr1 = normalStr1.Replace("）", "");
        normalStr1 = normalStr1.Replace("{", "");
        normalStr1 = normalStr1.Replace("}", "");
        normalStr=normalStr1;
        result=result.Replace("（", "<color=#00ff00ff>");
        result=result.Replace("）", "</color>");
        result=result.Replace("{", "");
        result=result.Replace("}", "");
        //<color=#ff0000ff>colorfully</color>
        Debug.Log(result);
        return result;
    }
    public void TryGetClickedWord(TMP_Text tMP_Text,string parseStr,out string targetWord)
    {
        targetWord=null;
        if (TMP_TextUtilities.IsIntersectingRectTransform(tMP_Text.rectTransform, Input.mousePosition, null))
        {
            int characterIndex = TMP_TextUtilities.FindIntersectingCharacter(tMP_Text, Input.mousePosition, null, true);
            string selectedText = tMP_Text.textInfo.characterInfo[characterIndex].character.ToString();
            Debug.Log(selectedText);
            #region 解析关键词
            List<int[]> keyWordRange = new List<int[]>();
            int startIndex = -1;
            for (int i = 0; i < parseStr.Length; i++)
            {
                int endIndex=-1;
                if (parseStr[i] == '（'||parseStr[i] == '{')
                {
                    startIndex = i;
                }
                else if(parseStr[i] == '）'||parseStr[i] == '}'){
                    endIndex = i;
                }
                if(endIndex>=0&&startIndex>=0){
                    int[] range=new int[2];
                    range[0]=startIndex;
                    range[1]=endIndex;
                    keyWordRange.Add(range);
                    startIndex = -1;    
                }
            }
            #endregion
        //Debug.Log(keyWordRange.Count);

            for(int i=0;i<keyWordRange.Count;i++){
                string keyWord = parseStr.Substring(keyWordRange[i][0]+1,keyWordRange[i][1]-keyWordRange[i][0]-1);
                Debug.Log("KeyWord: " + keyWord);
                //Debug.Log("Range: " + (keyWordRange[i][0]-i*2)+":"+keyWordRange[i][1]-(i)*2+":"+characterIndex);
                if (keyWord.Contains(selectedText)&&characterIndex>=keyWordRange[i][0]-i*2&&characterIndex<=keyWordRange[i][1]-(i)*2)
                {
                    targetWord=keyWord;
                    break;
                    //TODO: 解析关键词
                }
            }
            //Debug.Log("Not Found Selected Text");
            return ;
            //Debug.Log("Selected Text: " + selectedText);
        }
    }
    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     if (TMP_TextUtilities.IsIntersectingRectTransform(curTextComponent.rectTransform, Input.mousePosition,null))
        //     {
        //         int characterIndex = TMP_TextUtilities.FindIntersectingCharacter(curTextComponent, Input.mousePosition, null, true);
        //         string selectedText = curTextComponent.textInfo.characterInfo[characterIndex].character.ToString();
        //         Debug.Log("Selected Text: " + selectedText);
        //     }
        // }
    }
}
