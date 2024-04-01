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

        // 파일에서 모든 줄을 읽어옵니다.
        string[] lines = datas.text.Split('\n');

        // 첫 번째 줄(헤더)에서 멤버 변수 이름을 가져옵니다.
        string[] headers = lines[0].Split('\t');

        for (int i = 1; i < lines.Length; i++) // 데이터 행을 순회합니다.
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
