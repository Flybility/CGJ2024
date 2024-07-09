using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class ParagraphConfig
{
    public static Dictionary<int,ParagraphData> paraDic = new Dictionary<int, ParagraphData>();
    public static void LoadParaCfgData(){
        paraDic = new Dictionary<int, ParagraphData>();
        string txt = Resources.Load<TextAsset>("para_cfg").text;
        JsonData jd = JsonMapper.ToObject<JsonData>(txt);
        for (int i = 0, cnt = jd.Count; i < cnt; ++i)
        {
            var itemJd = jd[i] as JsonData;
            ParagraphData cfgItem = JsonMapper.ToObject<ParagraphData>(itemJd.ToJson());

            if (!paraDic.ContainsKey(cfgItem.paraId))
            {
                paraDic[cfgItem.paraId] = new ParagraphData();
            }
            paraDic[cfgItem.paraId]=cfgItem;
        }
    }
    public static ParagraphData GetParaData(int id){
        if (paraDic.ContainsKey(id) )return paraDic[id];
        return null;
    }
}
public enum GameState
{
    Null,
    Restart,
    Finish
}
public class ParagraphData
{
    public int paraId;
    public string paraContent;
    public string paraAudioClip;
    public string paraBGMChange;
    public string gameState;
    public GameState  GameState{
        get{
            if (string.IsNullOrEmpty(gameState)) return GameState.Null;
            return (GameState)Enum.Parse(typeof(GameState), gameState);
        }
    }

    // public int paraTime;
    // public float ParaSpawnTime=>paraTime/10f;

}

