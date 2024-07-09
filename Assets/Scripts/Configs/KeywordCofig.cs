using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class KeywordCofig 
{
   public static Dictionary<string,KeywordData> keywordsDic = new Dictionary<string, KeywordData>();
    public static void LoadKeywordCfgData(){
        keywordsDic = new Dictionary<string, KeywordData>();
        string txt = Resources.Load<TextAsset>("keyword_cfg").text;
        JsonData jd = JsonMapper.ToObject<JsonData>(txt);
        for (int i = 0, cnt = jd.Count; i < cnt; ++i)
        {
            var itemJd = jd[i] as JsonData;
            KeywordData cfgItem = JsonMapper.ToObject<KeywordData>(itemJd.ToJson());

            if (!keywordsDic.ContainsKey(cfgItem.keyId))
            {
                keywordsDic[cfgItem.keyId] = new KeywordData();
            }
            keywordsDic[cfgItem.keyId]=cfgItem;
        }
    }
    public static KeywordData GetKeywordData(string id){
        if (keywordsDic.ContainsKey(id) )return keywordsDic[id];
        return null;
    }
}
public class KeywordData{

    public string keyId;
    public short isSpawnNode;
    public string keyInteractKey;
    public string[] KeyInteractKeys=>keyInteractKey.Split(',');
    public string keyInteractedKeyId;
    public string keyInteractParaId;
    public string[] KeyInteractParaIds=>keyInteractParaId.Split(',');
    public int keyParaId;
    public short isDestroyThen;

}

