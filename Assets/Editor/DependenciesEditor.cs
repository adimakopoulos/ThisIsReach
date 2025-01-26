using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace ThisIsReach
{

    [CustomEditor(typeof(PersonDependenciesManager), true)]
    public class DependenciesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PersonDependenciesManager dependenciesManager = (PersonDependenciesManager)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Manual Update"))
            {
                //UpdateDependencies(dependenciesManager);
                EditorUtility.SetDirty(dependenciesManager);
            }
        }

        //private void UpdateDependencies(PersonDependenciesManager dependenciesManager)
        //{
        //    dependenciesManager.dependenciesDictionary.Clear();

        //    MonoBehaviour[] monoBehaviours = dependenciesManager.gameObject.GetComponents<MonoBehaviour>();

        //    foreach (var monoBehaviour in monoBehaviours)
        //    {
        //        Type type = monoBehaviour.GetType();
        //        string typeName = type.ToString();
        //        if (typeName.Contains("DependenciesManager"))
        //        {
        //            continue;
        //        }
        //        dependenciesManager.dependenciesDictionary.Add(new ThisIsReach.Dep(typeName, monoBehaviour));
        //        Debug.Log("additing typename: "+ typeName + ",mono: " +monoBehaviour.ToString());
        //    }
        //}
    }
}
