using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;
using System.Reflection;
using static Define;

public class DataParser : MonoBehaviour
{
    public List<MechanismData> list;
    public TextAsset datas;
    private void Start()
    {
        list = ParseMechanismDataFromTSV(datas);
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
