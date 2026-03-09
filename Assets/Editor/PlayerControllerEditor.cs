using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EllenPlayerController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EllenPlayerController playerController = target as EllenPlayerController;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Ellen Player", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            switch (playerController.PlayerState)
            {
                case PlayerController.EPlayerState.None:
                    GUI.backgroundColor = new Color(0f, 0f, 0f, 1f);
                    break;
                case PlayerController.EPlayerState.Idle:
                    GUI.backgroundColor = new Color(1f, 0f, 0f, 1f);
                    break;
                case PlayerController.EPlayerState.Move:
                    GUI.backgroundColor = new Color(0f, 0f, 1f, 1f);
                    break;
                case PlayerController.EPlayerState.Jump:
                    GUI.backgroundColor = new Color(0f, 1f, 0f, 1f);
                    break;
            }
        }

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField("Player State", playerController.PlayerState.ToString(), EditorStyles.boldLabel);
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndVertical();
    }

    private void OnEnable()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
    }

    private void OnEditorUpdate()
    {
        if (target != null)
            Repaint();
    }
}
