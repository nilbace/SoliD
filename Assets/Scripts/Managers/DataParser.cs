using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;
using System.Reflection;
using static Define;
using UnityEngine.Networking;
using System.Collections;

public class DataParser : MonoBehaviour
{
    public List<MechanismData> list;
    public TextAsset datas;
    private const string URL_CardData = "https://docs.google.com/spreadsheets/d/1-taJJ7Z8a61PP_4emH93k5ooAO3j0-tKZxo4WkM7wz8/export?format=tsv&gid=0&range=A2:K32";
    private const string URL_CardEffectData = "https://docs.google.com/spreadsheets/d/1-taJJ7Z8a61PP_4emH93k5ooAO3j0-tKZxo4WkM7wz8/export?format=tsv&gid=1198669234&range=B2:D26";


    private void Start()
    {
        list = ParseMechanismDataFromTSV(datas);
        StartCoroutine(RequestAndSetDayDatas(URL_CardData));
        StartCoroutine(RequestAndSetDayDatas(URL_CardEffectData));
    }

    public IEnumerator RequestAndSetDayDatas(string www)
    {
        UnityWebRequest wwww = UnityWebRequest.Get(www);
        yield return wwww.SendWebRequest();

        string data = wwww.downloadHandler.text;
        Debug.Log(data);
        string[] lines = data.Substring(0, data.Length).Split('\n');
        //Queue<string> stringqueue = new Queue<string>();

        //foreach (string line in lines)
        //{
        //    stringqueue.Enqueue(line);
        //}
        //for (int i = 0; i < (int)BroadCastType.MaxCount_Name; i++)
        //{
        //    ProcessStringToList(ContentType.BroadCast, i, stringqueue.Dequeue());
        //}
       
    }

    public List<MechanismData> ParseMechanismDataFromTSV(TextAsset datas)
    {
        List<MechanismData> dataList = new List<MechanismData>();

        // ���Ͽ��� ��� ���� �о�ɴϴ�.
        string[] lines = datas.text.Split('\n');

        // ù ��° ��(���)���� ��� ���� �̸��� �����ɴϴ�.
        string[] headers = lines[0].Split('\t');

        for (int i = 1; i < lines.Length; i++) // ������ ���� ��ȸ�մϴ�.
        {
            string[] columns = lines[i].Split('\t');
            MechanismData data = new MechanismData();
            for (int j = 0; j < headers.Length; j++)
            {
                if (columns[j] == "") continue;

                PropertyInfo propertyInfo = typeof(MechanismData).GetProperty(headers[j]);
                FieldInfo fieldInfo = typeof(MechanismData).GetField(headers[j]);


                if (propertyInfo != null)
                {
                    // �Ӽ��� ���� ó�� (MechanismData���� �Ӽ��� �����Ƿ� �� ���������� ������ �ʽ��ϴ�.)
                }
                else if (fieldInfo != null)
                {
                    // �ʵ� Ÿ�Կ� ���� ������ �Ľ��� �����ϰ� ���� �Ҵ��մϴ�.
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
                        value = columns[j]; // ���ڿ��� ��� ���� �Ҵ�
                    }

                    fieldInfo.SetValue(data, value);
                }
            }

            dataList.Add(data);
        }

        return dataList;
    }
}
