#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace LeTai.Asset.TranslucentImage
{
    [ExecuteInEditMode]
    [AddComponentMenu("UI/Translucent Image", 2)]
    public partial class TranslucentImage
    {
        protected override void Reset()
        {
            base.Reset();
            color = new Color(1, 1, 1, .65f);

            material = AssetDatabase.LoadAssetAtPath<Material>(
                "Assets/Le Tai's Asset/TranslucentImage/Material/Default-Translucent.mat");
            vibrancy = material.GetFloat("_Vibrancy");
            source = source ?? FindObjectOfType<TranslucentImageSource>();

            PrepShader();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            Update();
        }

        // [InitializeOnLoadMethod]
        protected void Init()
        {
            Start();
        }
    }
}
#endif