#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;


/// <summary>
/// AutoGetComponentAttributeがついてる変数をインスペクターに表示する用のクラス
/// PropertyDrawerを継承するとその辺がいじれる
/// </summary>
[CustomPropertyDrawer(typeof(AutoGetComponentAttribute))]
public class AutoGetComponentDrawer : PropertyDrawer
{
    /// <summary>
    /// インスペクターの描画処理
    /// Updateみたいに毎フレーム通るような関数のため注意
    /// ちゃんと対策しないと毎フレームGetComponentみたいな事になる
    /// </summary>
    /// <param name="position">描画位置</param>
    /// <param name="property">対象のオブジェクト</param>
    /// <param name="label">知らん</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // シリアライズにコンポーネントを設定
        GetOrAddComponent(property);

        // 標準のプロパティフィールドを描画
        // インスペクターに表示しない場合はここの３行を消す
        EditorGUI.BeginDisabledGroup(true);// 入力出来ない欄の表示開始
        EditorGUI.PropertyField(position, property, label);　// 対象の変数を表示
        EditorGUI.EndDisabledGroup();// 入力ない欄の表示終了
    }

    /// <summary>
    /// シリアライズに対して、対象のクラスを入れ込む
    /// </summary>
    /// <param name="property">対象のオブジェクト</param>
    private void GetOrAddComponent(SerializedProperty property)
    {
        // AutoGetCompornentにシリアライズフィールドがあるか確認
        if (property.propertyType != SerializedPropertyType.ObjectReference) return;
        // シリアライズが未設定の場合にのみ実行
        if (property.objectReferenceValue != null) return;
        // 対象がGameObjectの場合にのみ実行
        MonoBehaviour targetObject = property.serializedObject.targetObject as MonoBehaviour;
        if (targetObject == null) return;


        // すでについているコンポーネントを取得
        var component = targetObject.GetComponent(fieldInfo.FieldType);
        if (component == null)
        {
            // ついていなかった場合コンポーネントを追加
            component = targetObject.gameObject.AddComponent(fieldInfo.FieldType);
            if (component == null)
            {
                Debug.LogError(targetObject.name + "のAutoGetComponentに失敗しました。\nMonoBehaviour以外のクラスのアトリビュートが設定されています。\n※エディタが重くなるのでこのエラーは必ず解消してください");
                return;
            }
        }

        // シリアライズの情報を更新
        if (component != null)
        {
            property.objectReferenceValue = component;
            property.serializedObject.ApplyModifiedProperties(); // プロパティを更新
        }

    }
}
#endif


/// <summary>
/// アトリビュートを作成する用のクラス
/// PropertyAttributeを継承するとアトリビュートに設定できる
/// AttributeTargets.Fieldは変数に付けれるって意味、MethodとかClassとかあるはず、知らんけど
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class AutoGetComponentAttribute : PropertyAttribute
{
}