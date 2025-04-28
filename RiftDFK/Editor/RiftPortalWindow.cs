#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace RiftDFK.EditorTools
{
    public class RiftPortalWindow : EditorWindow
    {
        private const string RiftKernelName = "RiftKernel";

        [MenuItem("RiftDFK/Portal/Open Rift Portal")]
        public static void ShowWindow()
        {
            var window = GetWindow<RiftPortalWindow>();

            Texture2D customIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/RiftDFK/Editor/Icons/rift_portal_icon.png");
            if (customIcon != null)
            {
                window.titleContent = new GUIContent("Rift Portal", customIcon);
            }
            else
            {
                window.titleContent = new GUIContent("Rift Portal");
                Debug.LogWarning("⚠️ Rift Portal Icon not found at path!");
            }

            window.minSize = new Vector2(400, 300);
        }

        private void OnGUI()
        {
            DrawHeader();

            GUILayout.Space(10);

            DrawBigButton(
                "Initialize Rift Kernel",
                "d_Rigidbody Icon",
                InitializeKernel,
                64
            );

            DrawBigButton(
                "Create Rift Body",
                "d_Prefab Icon",
                CreateRiftBody,
                64
            );

            DrawBigButton(
                "Create LineJoint",
                "d_JointAngularLimits Icon",
                CreateLineJoint,
                48
            );

            DrawBigButton(
                "Setup Gravity Force",
                "d_PhysicsMaterial Icon",
                CreateGravityForce,
                48
            );

            GUILayout.Space(20);

            DrawBigButton(
                "Portal Update Selected RiftBody",
                "d_Refresh",
                PortalUpdateSelectedRiftBody,
                32
            );

            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("RiftDFK v1.1.8", EditorStyles.centeredGreyMiniLabel);
        }

        private void DrawBigButton(string text, string iconName, System.Action onClick, float iconSize = 32)
        {
            Texture icon = EditorGUIUtility.IconContent(iconName).image;
            GUIContent content = new GUIContent("  " + text, icon);

            GUIStyle style = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft,
                fixedHeight = 50,
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                imagePosition = ImagePosition.ImageLeft,
                padding = new RectOffset(10, 10, 5, 5)
            };

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(content, style, GUILayout.Height(50)))
            {
                onClick.Invoke();
            }
            GUILayout.EndHorizontal();
        }

        private void DrawHeader()
        {
            Rect rect = GUILayoutUtility.GetRect(0f, 50f, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f));

            GUILayout.BeginArea(rect);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);

            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 20,
                alignment = TextAnchor.MiddleLeft
            };

            GUILayout.Label("🌀 Rift Portal", titleStyle);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        // 🔥 New reusable AddComponentOrCreateNew<T> helper
        private void AddComponentOrCreateNew<T>(string objectName = "New Rift Object") where T : Component
        {
            if (Selection.activeGameObject != null)
            {
                if (Selection.activeGameObject.GetComponent<T>() == null)
                {
                    Undo.AddComponent<T>(Selection.activeGameObject);
                    Debug.Log($"✅ Added {typeof(T).Name} to selected object '{Selection.activeGameObject.name}'!");
                }
                else
                {
                    Debug.LogWarning($"⚠️ Selected object already has {typeof(T).Name}!");
                }
            }
            else
            {
                GameObject newObj = new GameObject(objectName);
                Undo.RegisterCreatedObjectUndo(newObj, $"Create {objectName}");
                newObj.AddComponent<T>();
                Selection.activeGameObject = newObj;
                Debug.Log($"✅ Created new object '{objectName}' with {typeof(T).Name} component!");
            }
        }

        // ➡️ Updated functions using the helper:

        private void InitializeKernel()
        {
            AddComponentOrCreateNew<RiftDFK.RiftKernel>(RiftKernelName);
        }

        private void CreateRiftBody()
        {
            AddComponentOrCreateNew<RiftDFK.RiftBody>("RiftBody");
        }

        private void CreateLineJoint()
        {
            AddComponentOrCreateNew<RiftDFK.LineJoint>("RiftLineJoint");
        }

        private void CreateGravityForce()
        {
            AddComponentOrCreateNew<RiftDFK.GravityForce>("GravityForce");
        }

        private void PortalUpdateSelectedRiftBody()
        {
            if (Selection.activeGameObject == null)
            {
                Debug.LogWarning("⚠️ No GameObject selected!");
                return;
            }

            RiftDFK.RiftBody riftBody = Selection.activeGameObject.GetComponent<RiftDFK.RiftBody>();
            if (riftBody == null)
            {
                Debug.LogWarning("⚠️ Selected object does not have a RiftBody component!");
                return;
            }

            Rigidbody rb = Selection.activeGameObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogWarning("⚠️ No Rigidbody found on selected object to import values from!");
                return;
            }

            Undo.RecordObject(riftBody, "Portal Update RiftBody");

            riftBody.Mass = rb.mass;
            riftBody.Drag = new Vector3(rb.drag, rb.drag, rb.drag);
            riftBody.MaxVelocity = new Vector3(rb.maxAngularVelocity, rb.maxAngularVelocity, rb.maxAngularVelocity);

            riftBody.Editor_SetVelocity(rb.velocity);
            riftBody.Editor_SetInitialVelocity(rb.velocity);

            Debug.Log("✅ Portal Update: Imported Rigidbody values into RiftBody!");
        }
    }
}
#endif
