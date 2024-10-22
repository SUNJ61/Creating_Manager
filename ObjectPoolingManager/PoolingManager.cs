using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    /*
    풀링매니저 데이터를 딕셔너리로 저장해 효율적인 관리를 할 수 있도록 설계.
    PoolingData는 풀링한 오브젝트를 담을 리스트, 풀링할 오브젝트, 풀링한 오브젝트들의 부모 오브젝트 이름 데이터, 풀링된 오브젝트 이름, 풀링할 개수를 저장한다.
    풀링매니저에 선언된 Data 딕셔너리를 통해 풀링된 오브젝트의 리스트, 오브젝트, 풀링된 오브젝트의 부모 이름, 풀링된 오브젝트의 이름, 풀링한 오브젝트 개수를 반환 받을 수 있다.
    */
    public static PoolingManager instance;
    public Dictionary<int, PoolingData> Data = new Dictionary<int, PoolingData>();

    [SerializeField] List<GameObject> EX_Pool;

    GameObject EX_Prefab;

    private readonly int EX_Max = 10;

    private readonly string EX_Group = "EXGroup";
    private readonly string EX_Obj = "EX";
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        EX_Prefab = Resources.Load<GameObject>(EX_Obj);

        Data.Add(0, new PoolingData(EX_Pool, EX_Prefab, EX_Group, EX_Obj, EX_Max));

        for (int i = 0; i < Data.Count; i++)
            Pooling(i, Data);
    }

    private void Pooling(int key, Dictionary<int, PoolingData> data) //딕셔너리에 저장한 데이터로 오브젝트 풀링
    {
        GameObject Group = new GameObject(data[key].GroupName);
        for (int i = 0; i < data[key].MaxPool; i++)
        {
            var obj = Instantiate(data[key].Prefab, Group.transform);
            obj.transform.position = new Vector3(0f, -30f, 0f);
            obj.transform.rotation = Quaternion.identity;
            obj.name = $"{data[key].ObjName}";
            data[key].Pool_List.Add(obj);
            obj.SetActive(false);
        }
    }

    public GameObject GetObject(int key) //딕셔너리 리스트에 저장된 오브젝트중 비활성화된 오브젝트 반환.
    {
        foreach (var obj in Data[key].Pool_List)
        {
            if (!obj.activeSelf)
                return obj;
        }
        return null;
    }
}
