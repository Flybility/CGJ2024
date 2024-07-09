using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordNodeManager : MonoBehaviour
{
    private static KeywordNodeManager instance;
    public static KeywordNodeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeywordNodeManager>();
            }
            return instance;
        }
    }
    public Canvas canvas;
    public Vector2 clampRange;
    public GameObject textNodePrefab;
    public Dictionary<string,KeywordNode> currentKeywordNodes = new Dictionary<string, KeywordNode>();

    public Transform spawnParent;
    public Transform dragParent;
    public void RemoveKeywordNode(KeywordNode keyNode){
        currentKeywordNodes.Remove(keyNode.keywordData.keyId);
    }
    public void SpawnTextNode(KeywordData keywordData,ParagraphEntity paragraphEntity)
    {
        if (!currentKeywordNodes.ContainsKey(keywordData.keyId))
        {
            GameObject newTextNode = Instantiate(textNodePrefab, spawnParent);
            //newTextNode.transform.SetParent(dragParent);
            KeywordNode keywordNode = newTextNode.GetComponent<KeywordNode>();
            keywordNode.Init(keywordData,paragraphEntity);
            currentKeywordNodes[keywordData.keyId] = keywordNode;
        }

    }
    public KeywordNode CheckKeywordNodeOverlap(KeywordNode targetNode){
        foreach(KeywordNode node in currentKeywordNodes.Values){
            if(node==targetNode){
                continue;
            }
            if (Mathf.Abs(targetNode.transform.position.x-node.transform.position.x)<40&& Mathf.Abs(targetNode.transform.position.y - node.transform.position.y) < 24)
            {
                
                //Debug.Log(node.GetChild(i).gameObject);
                return node;
            }
        }
        return null;
    }
}
