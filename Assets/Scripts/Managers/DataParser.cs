using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;
using System.Reflection;
using UnityEngine.Networking;
using System.Collections;
using System;

public class DataParser : MonoBehaviour
{
    public List<CardEffectData> CardEffectList;
    private const string URL_CardData = "https://docs.google.com/spreadsheets/d/1-taJJ7Z8a61PP_4emH93k5ooAO3j0-tKZxo4WkM7wz8/export?format=tsv&gid=0&range=A2:K32";
    private const string URL_CardEffectData = "https://docs.google.com/spreadsheets/d/1-taJJ7Z8a61PP_4emH93k5ooAO3j0-tKZxo4WkM7wz8/export?format=tsv&gid=1198669234&range=B2:D26";

    public DeckManager TempDeck;

    private void Start()
    {
        StartCoroutine(RequestDatas());
    }

    IEnumerator RequestDatas()
    {
        yield return StartCoroutine(RequestAndSetDayDatas(URL_CardEffectData, ProcessCardEffectData_To_List));
        StartCoroutine(RequestAndSetDayDatas(URL_CardData, ProcessCard_To_Deck));
    }

 

    public IEnumerator RequestAndSetDayDatas(string url, Action<string> processData)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            string data = www.downloadHandler.text;
            string[] lines = data.Split('\n');

            foreach (string line in lines)
            {
                processData(line);
            }
        }
    }

    void ProcessCardEffectData_To_List(string data)
    {
        string[] lines = data.Substring(0, data.Length).Split('\t');
        CardEffectData cardEffect = new CardEffectData();
        if (!int.TryParse(lines[0], out cardEffect.EffectID))
        {
            Debug.LogError($"{lines[0]} : Failed to parse the string to EffectID.");
        }
        if (!Enum.TryParse(lines[1], out cardEffect.TargetType))
        {
            Debug.LogError($"{lines[1]} : Failed to parse the string to TargetType enum.");
        }
        if (!Enum.TryParse(lines[2], out cardEffect.CardEffectType))
        {
            Debug.LogError($"{lines[2]} : Failed to parse the string to CardEffectType enum.");
        }
        CardEffectList.Add(cardEffect);
    }

    void ProcessCard_To_Deck(string data)
    {
        string[] lines = data.Substring(0, data.Length).Split('\t');
        CardData cardData = new CardData();
        if (!Enum.TryParse(lines[0], out cardData.CardType))
        {
            Debug.LogError($"{lines[0]} : Failed to parse the string to CardType.");
        }

        if (!Enum.TryParse(lines[1], out cardData.CardOwner))
        {
            Debug.LogError($"{lines[1]} : Failed to parse the string to CardOwner.");
        }

        if (!Enum.TryParse(lines[2], out cardData.CardColor))
        {
            Debug.LogError($"{lines[2]} : Failed to parse the string to CardColor.");
        }

        if (!Enum.TryParse(lines[3], out cardData.CardTier))
        {
            Debug.LogError($"{lines[3]} : Failed to parse the string to CardTier.");
        }

        if (!int.TryParse(lines[4], out cardData.CardCost))
        {
            Debug.LogError($"{lines[4]} : Failed to parse the string to CardCost.");
        }

        cardData.CardName = lines[5];
        cardData.CardInfoText = lines[6];

        if (!bool.TryParse(lines[7], out cardData.NeedTarget))
        {
            Debug.LogError($"{lines[7]} : Failed to parse the string to NeedTarget.");
        }
    

        string[] effectIDs = lines[8].Split('/');
        string[] effectParameters = lines[9].Split('/');
        for (int i = 0; i < effectIDs.Length; i++)
        {
            string index = effectIDs[i];
            if (int.TryParse(index, out int effectIndex))
            {
                // 깊은 복사를 위해 복사 생성자를 사용
                CardEffectData newEffect = new CardEffectData(GetCardEffectFromListByIndex(effectIndex));

                // effectParameters[i] 값을 Amount 속성에 설정
                if (float.TryParse(effectParameters[i], out float amount))
                {
                    newEffect.Amount = amount;
                }
                else
                {
                    Debug.LogError($"{effectParameters[i]} : Failed to parse the string to amount.");
                }

                cardData.CardEffectList.Add(newEffect);
            }
            else
            {
                Debug.LogError($"{index} : Failed to parse the string to effect index.");
            }
        }
        // 처리된 카드를 덱에 추가
        TempDeck.Cards.Add(cardData);
    }

    CardEffectData GetCardEffectFromListByIndex(int index)
    {
        foreach (var effectData in CardEffectList)
        {
            if (effectData.EffectID == index)
            {
                // 깊은 복사를 위해 복사 생성자를 사용하여 반환
                return effectData;
            }
        }
        // 해당하는 EffectID가 없는 경우 null 반환
        return null;
    }


    /// <summary>
    /// 현재 안씀
    /// </summary>
    /// <param name="datas"></param>
    /// <returns></returns>
    public List<CardEffectData> ParseMechanismDataFromTSV(TextAsset datas)
    {
        List<CardEffectData> dataList = new List<CardEffectData>();

        // 파일에서 모든 줄을 읽어옵니다.
        string[] lines = datas.text.Split('\n');

        // 첫 번째 줄(헤더)에서 멤버 변수 이름을 가져옵니다.
        string[] headers = lines[0].Split('\t');

        for (int i = 1; i < lines.Length; i++) // 데이터 행을 순회합니다.
        {
            string[] columns = lines[i].Split('\t');
            CardEffectData data = new CardEffectData();
            for (int j = 0; j < headers.Length; j++)
            {
                if (columns[j] == "") continue;

                PropertyInfo propertyInfo = typeof(CardEffectData).GetProperty(headers[j]);
                FieldInfo fieldInfo = typeof(CardEffectData).GetField(headers[j]);


                if (propertyInfo != null)
                {
                    // 속성에 대한 처리 (MechanismData에는 속성이 없으므로 이 예제에서는 사용되지 않습니다.)
                }
                else if (fieldInfo != null)
                {
                    // 필드 타입에 따라 적절한 파싱을 수행하고 값을 할당합니다.
                    object value;
                    if (fieldInfo.FieldType == typeof(int))
                    {
                        value = int.Parse(columns[j]);
                    }
                    else if (fieldInfo.FieldType == typeof(float))
                    {
                        value = float.Parse(columns[j], CultureInfo.InvariantCulture);
                    }
                    else if (fieldInfo.FieldType.IsEnum)
                    {
                        value = System.Enum.Parse(fieldInfo.FieldType, columns[j]);
                    }
                    else
                    {
                        value = columns[j]; // 문자열의 경우 직접 할당
                    }

                    fieldInfo.SetValue(data, value);
                }
            }

            dataList.Add(data);
        }

        return dataList;
    }
}
