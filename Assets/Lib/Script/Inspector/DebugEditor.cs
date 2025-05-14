using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugEditor : EditorWindow
{

    #region UIBuilder

    static private VisualTreeAsset rootVisualTreeAsset;
    static private VisualTreeAsset logVisualTreeAsset;
    static private VisualTreeAsset infoVisualTreeAsset;
    static private VisualTreeAsset buttonVisualTreeAsset;
    static private StyleSheet rootStyleSheet;
    ScrollView view;

    static private VisualTreeAsset _rootVisualTreeAsset
    {
        get
        {
            if (rootVisualTreeAsset == null)
                rootVisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Lib/LibDebug/Debug.uxml");

            return rootVisualTreeAsset;
        }
    }

    static private VisualTreeAsset _logVisualTreeAsset
    {
        get
        {
            if(logVisualTreeAsset == null) 
                logVisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Lib/LibDebug/Log.uxml");

            return logVisualTreeAsset;
        }
    }

    static private VisualTreeAsset _infoVisualTreeAsset
    {
        get
        {
            if (infoVisualTreeAsset == null)
                infoVisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Lib/LibDebug/Info.uxml");

            return infoVisualTreeAsset;
        }
    }

    static private VisualTreeAsset _buttonVisualTreeAsset
    {
        get
        {
            if (buttonVisualTreeAsset == null)
                buttonVisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Lib/LibDebug/Button.uxml");

            return buttonVisualTreeAsset;
        }
    }

    static private StyleSheet _rootStyleSheet
    {
        get
        {
            if (rootStyleSheet == null)
                rootStyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Lib/LibDebug/Debug.uxml");

            return rootStyleSheet;
        }
    }

    #endregion

    private List<LogInfo> logList = new List<LogInfo>();
    private List<string> rayList;
    private List<ButtonInfo> buttonList = new List<ButtonInfo>();

    DebugUser user = DebugUser.All;
    DebugWindowMenu menu = DebugWindowMenu.Log;

    #region 描画更新

    // ヘッダメニュー名/ヘッダ以下のメニュー名
    [MenuItem("カスタムエディター/Debug", false, 5)]
    private static void ShowWindow()
    {
        var window = GetWindow<DebugEditor>("UIElements");
        window.titleContent = new GUIContent("Debug"); // エディタ拡張ウィンドウのタイトル
        window.Show();
    }

    private void CreateGUI()
    {
        //シートの登録
        SheetSetting();

        //ボタンの登録
        ButtonSetting();

        //リストの設定
        ListSetting();
    }
    
    #endregion

    #region 初期設定

    public void SheetSetting()
    {
        if (view != null) return;

        _rootVisualTreeAsset.CloneTree(rootVisualElement);
        rootVisualElement.styleSheets.Add(_rootStyleSheet);

        view = rootVisualElement.Q<ScrollView>("LogArea");
    }

    private void ButtonSetting()
    {
        Button clearButton = rootVisualElement.Q<Button>("Clear");
        clearButton.clicked += ClearButtonOnClick;

        Button logButton = rootVisualElement.Q<Button>("Log");
        logButton.clicked += LogButtonOnClick;

        Button rayButton = rootVisualElement.Q<Button>("Ray");
        rayButton.clicked += RayButtonOnClick;

        Button methodButton = rootVisualElement.Q<Button>("Method");
        methodButton.clicked += MethodButtonOnClick;

        Button allButton = rootVisualElement.Q<Button>("All");
        allButton.clicked += AllButtonClick;

        Button taniButton = rootVisualElement.Q<Button>("Tani");
        taniButton.clicked += TaniButtonOnClick;

        Button mathuButton = rootVisualElement.Q<Button>("Mathu");
        mathuButton.clicked += MathuButtonOnClick;
    }

    private void ListSetting()
    {
        logList = new List<LogInfo>();
    }

    #endregion

    #region ボタンが押された際の処理

    private void ClearButtonOnClick()
    {
        ViewReset();
        Action action = menu switch
        {
            DebugWindowMenu.Log => () => logList.Clear(),
            DebugWindowMenu.Ray => () => rayList.Clear(),
            DebugWindowMenu.Method => () => buttonList.Clear(),
            _ => () => { }
        };
        action.Invoke();
    }


    private void LogButtonOnClick()
    {
        ViewReset();
        menu = DebugWindowMenu.Log;
        ViewUpdate();
    }

    private void RayButtonOnClick()
    {
        ViewReset();
        menu = DebugWindowMenu.Ray;
        ViewUpdate();
    }

    private void MethodButtonOnClick()
    {
        ViewReset();
        menu = DebugWindowMenu.Method;
        ViewUpdate();
    }

    private void AllButtonClick()
    {
        ViewReset();
        user = DebugUser.All;
        ViewUpdate();
    }

    private void TaniButtonOnClick()
    {
        ViewReset();
        user = DebugUser.Taniyama;
        ViewUpdate();
    }

    private void MathuButtonOnClick()
    {
        ViewReset();
        user = DebugUser.Matuoka;
        ViewUpdate();
    }

    #endregion

    #region Libデバックから呼び出す用

    public void Log(object obj,DebugUser user)
    {
#if UNITY_EDITOR
        const int LAPPER_COUNT = 3;//呼び出し関数→LibDebug.Log→ここ

        //呼び出し元情報の調整
        (String filePath, int line) methodData = GetMethodData(LAPPER_COUNT);

        LogListUpdate(obj, methodData.filePath, methodData.line, user);
#endif
    }

    public void Ray()
    {

    }

    public void Button(object obj,Action action,DebugUser user)
    {
#if UNITY_EDITOR
        const int LAPPER_COUNT = 3;//呼び出し関数→LibDebug.ButtonLog→ここ

        (String filePath, int line) methodData = GetMethodData(LAPPER_COUNT);

        ButtonListUpdate(obj,action,user, methodData.filePath, methodData.line);
#endif
    }

    #endregion

    #region Log関係のprivate

    private void LogListUpdate(object obj,string filePath,int line,DebugUser user)
    {
        if (logList == null) return;

        //リストの更新
        bool isSame = CheckIsSameLogInfo(obj, filePath, line, user, out LogInfo info);
        info.Add(obj);
        if (isSame == false) logList.Add(info);

        //見た目の更新
        if(menu == DebugWindowMenu.Log)
        {   
            if(isSame == false && info.CheckOutputUser(user) == true) AddEditor_Log(info);
            if (info.CheckOutputUser(user) == true) info.RefreshEditor(obj);
        }
    }

    private bool CheckIsSameLogInfo(object output,string filePath,int line,DebugUser user ,out LogInfo info)
    {
        foreach (LogInfo log in logList)
        {
            if (log.GetIsSameGroup(filePath, line) == true)
            {
                info = log;
                return true;
            }
        }

        info = new LogInfo(filePath, line, user);
        return false;
    }

    private void AddEditor_Log(LogInfo info)
    {
        VisualElement logElement = DebugEditor._logVisualTreeAsset.CloneTree();
        view.Add(logElement);
        info.AddElement(logElement);
    }

    public class LogInfo
    {
        string filePath;
        int line;
        object lastObj;
        DebugUser user;
        List<OutputData> outputs;
        int count = 0;
        VisualElement element;
        bool isOpen = false;
        ScrollView items;
        bool colorSwitcher = false;
        Color WHITE = new Color(75 / 255f, 75 / 255f, 75 / 255f, 75 / 255f);
        Color BLACK = new Color(50 / 255f, 50 / 255f, 50 / 255f, 255 / 255f);
        private LogInfo() { }

        public LogInfo(string filePath,int line,DebugUser user)
        {
            this.filePath = filePath;
            this.line = line;
            this.user = user;
            outputs = new List<OutputData>();
        }


        public bool GetIsSameGroup(string filePath, int line)
        {
            bool answer = false;
            bool isSameFilePath = this.filePath.Equals(filePath);
            bool isSameLine = this.line == line;
            answer = (isSameFilePath && isSameLine);
            return answer;
        }

        public void Add(object output)
        {
            count++;
            bool isSame = CheckIsSameOutputData(output,out OutputData data);

            data.AddCount();

            if(isSame == false)
            {
                outputs.Add(data);
            }
            
            if(isOpen == true)
            {
                VisualElement infoElement = DebugEditor._infoVisualTreeAsset.CloneTree();
                items.Add(infoElement);

                Label label = infoElement.Q<Label>("Label");
                label.text = data.output.ToString();
                label.style.backgroundColor = data.myColor;
            }
        }

        public void AddElement(VisualElement element)
        {
            this.element = element;
            Button old = element.Q<Button>("old");
            old.clicked += ChangeViewOld;
            Button script = element.Q<Button>("script");
            script.clicked += OpenScript;
            items = element.Q<ScrollView>("newItems");
        }

        private bool CheckIsSameOutputData(object output,out OutputData outputData)
        {
            foreach(OutputData data in outputs)
            {
                if(data.GetIsSmaeOutput(output) == true)
                {
                    outputData = data;
                    return true;
                }
            }

            Color col = (colorSwitcher) ? WHITE : BLACK;
            colorSwitcher = !colorSwitcher;
            outputData = new OutputData(count, output, col);
            return false;
        }

        public void RefreshEditor(object obj)
        {
            lastObj = obj;
            Label output = element.Q<Label>("Message");
            output.text = obj.ToString();
            Label count = element.Q<Label>("Count");
            count.text = this.count.ToString();
        }

        public void RefreshEditor()
        {
            Label output = element.Q<Label>("Message");
            output.text = lastObj.ToString();
            Label count = element.Q<Label>("Count");
            count.text = this.count.ToString();
        }

        private void ChangeViewOld()
        {
            if(isOpen == false)
            {
                //OpenEditor();

                foreach(OutputData data in outputs)
                {
                    VisualElement infoElement = DebugEditor._infoVisualTreeAsset.CloneTree();
                    items.Add(infoElement);

                    Label label = infoElement.Q<Label>("Label");
                    label.text = data.output.ToString();
                    label.style.backgroundColor = data.myColor;
                }
            }
            else
            {
                //CloseEditor();
                items.Clear();
            }

            isOpen =! isOpen;
        }

        private void OpenScript()
        {
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(filePath,line);
        }

        public bool CheckOutputUser(DebugUser user)
        {
            if (user == DebugUser.All) return true;
            return this.user == user;
        }

        public class OutputData
        {
            
            public Color myColor { get; private set; }
            int count = 0;
            int timing;
            
            public object output { get; private set; }

            public OutputData(int timing, object output,Color col)
            {
                this.timing = timing;
                this.output = output;
                myColor = col;
            }

            public bool GetIsSmaeOutput(object output)
            {
                return this.output == output;
            }

            public void AddCount()
            {
                count++;
            }
        }
    }

    #endregion

    #region Button関係のprivate

    private void ButtonListUpdate(object obj,Action action,DebugUser user,string filePath,int line)
    {
        if (buttonList == null) return;

        //リストの更新
        bool isSame = CheckIsSameButtonInfo(obj, action,user,filePath, line, out ButtonInfo info);
        if (isSame == false) buttonList.Add(info);

        //見た目の更新
        if (menu == DebugWindowMenu.Method)
        {
            if (isSame == false && info.CheckOutputUser(user) == true) AddEditor_Button(info);
        }
    }
    
    private bool CheckIsSameButtonInfo(object obj, Action action, DebugUser user, string filePath, int line,out ButtonInfo info)
    {
        foreach (ButtonInfo buttonInfo in buttonList)
        {
            if (buttonInfo.GetIsSameGroup(filePath, line) == true)
            {
                info = buttonInfo;
                return true;
            }
        }

        info = new ButtonInfo(obj,action, user, filePath, line);
        return false;
    }

    private void AddEditor_Button(ButtonInfo info)
    {
        VisualElement logElement = DebugEditor._buttonVisualTreeAsset.CloneTree();
        view.Add(logElement);
        info.AddElement(logElement);
    }

    public class ButtonInfo
    {
        object obj;
        Action action;
        DebugUser user;
        string filePath;
        int line;
        
        private ButtonInfo() { }
        public ButtonInfo(object obj, Action action, DebugUser user, string filePath, int line)
        {
            this.obj = obj;
            this.action = action;
            this.user = user;
            this.filePath = filePath;
            this.line = line;
        }
        
        public bool GetIsSameGroup(string filePath,int line)
        {
            return (this.filePath.Equals(filePath) && this.line == line);
        }

        public bool CheckOutputUser(DebugUser user)
        {
            if (user == DebugUser.All) return true;
            return this.user == user;
        }

        public void AddElement(VisualElement logElement)
        {
            Button actionButton = logElement.Q<Button>("action");
            actionButton.clicked += action;
            actionButton.text = obj.ToString();
            Button script = logElement.Q<Button>("script");
            script.clicked += OpenScript;
        }

        private void OpenScript()
        {
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(filePath, line);
        }
    }

    #endregion

    #region 全体的に使う関数

    private void ViewReset()
    {
        view.Clear();
    }

    private void ViewUpdate()
    {
        Action action = menu switch
        {
            DebugWindowMenu.Log => LogVeiw,
            DebugWindowMenu.Ray => RayVeiw,
            DebugWindowMenu.Method => MethodView,
            _ => () => { }
        };

        action.Invoke();
    }

    private void LogVeiw()
    {
        foreach(LogInfo log in logList)
        {
            if (log.CheckOutputUser(user) == false) continue;
            AddEditor_Log(log);
            log.RefreshEditor();
        }
    }

    private void RayVeiw()
    {

    }

    private void MethodView()
    {
        foreach (ButtonInfo log in buttonList)
        {
            if (log.CheckOutputUser(user) == false) continue;
            AddEditor_Button(log);
            log.AddElement(view);
        }
    }

    private (string filePath,int line) GetMethodData(int before)
    {
        //呼び出し元情報の調整
        StackFrame caller = new StackFrame(before, true);
        string filePath = caller.GetFileName();
        int line = caller.GetFileLineNumber();

        return (filePath, line);
    }
    #endregion
}

public enum DebugWindowMenu
{
    Log,
    Ray,
    Method,
}