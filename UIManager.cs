using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   private static UIManager _instance;

    private Transform _uiRoot;

    private Dictionary<string, string> pathDict;

    private Dictionary<string, GameObject> prefabDict;

    public Dictionary<string, BasePanel> panelDict;
    public int sortingOrder = 0;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }

    public Transform UIRoot
    {
        get
        {
            if (_uiRoot == null)
            {
                if (GameObject.Find("Canvas"))
                {
                    _uiRoot = GameObject.Find("Canvas").transform;
                }
                else
                {
                    _uiRoot = new GameObject("Canvas").transform;
                }
            };
            return _uiRoot;
        }
    }

    private UIManager()
    {
        InitDicts();
    }

    private void InitDicts()
    {
        prefabDict = new Dictionary<string, GameObject>();
        panelDict = new Dictionary<string, BasePanel>();

        pathDict = new Dictionary<string, string>
        {
            { UIConst.MainMenuPanel, "Menu / MainMenuPanel" },
            { UIConst.UserPanel, "Menu / UserPanel" },
            { UIConst.NewUserPanel, "Menu / NewUserPanel" }
        };
    }

    public BasePanel GetPanel(string name)
    {
        BasePanel panel = null;
        if (panelDict.TryGetValue(name, out panel))
        {
            return panel;
        }
        return null;
    }
    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;
        if (panelDict.TryGetValue(name, out panel))
        {
            Debug.Log("�����Ѵ򿪣�" + name);
            return null;
        }

        string path = "";
        if (!pathDict.TryGetValue(name, out path))
        {
            Debug.Log("�������ƴ��� ��δ����·����" + path);
            return null;
        }
        GameObject panelPrefab = null;
        if (! prefabDict.TryGetValue(name, out panelPrefab))
        {
            string realPath = "Prefab/Panel/" + path;
            panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
            prefabDict.Add(name, panelPrefab);
        }
        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel);
        panel.OpenPanel(name);
        Canvas panelCavas = panelObject.GetComponent<Canvas>();
        panelCavas.sortingOrder = sortingOrder;
        sortingOrder += 1;
        return panel;
    }

    public bool ClosePanel(string name)
    {
        BasePanel panel = null;

        // ���Դ��ֵ��л�ȡ���
        if (!panelDict.TryGetValue(name, out panel))
        {
            Debug.Log("����δ��: " + name);
            return false;
        }

        // �ر����
        panel.ClosePanel();
        // ��ѡ���Ƴ���壬����ȡ��������
        // panelDict.Remove(name);
        return true;
    }
    public class UIConst
    {
        public const string MainMenuPanel = "MainMenuPanel";
        public const string UserPanel = "UserPanel";
        public const string NewUserPanel = "NewUserPane";
    }
}
