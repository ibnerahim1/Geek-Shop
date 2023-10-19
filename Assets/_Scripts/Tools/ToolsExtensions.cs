using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

namespace Game.Tools
{

    public static class ToolsExtensions
    {
        // Extension method to find a child object by name
        /// <summary>
        /// Get Component of child by it's name
        /// </summary>
        public static T FindChildByName<T>(this Transform i_Parent, string i_Name) where T : Component
        {
            Transform[] i_Children = i_Parent.GetComponentsInChildren<Transform>(true);

            foreach (Transform i_Child in i_Children)
            {
                if (i_Child.name == i_Name)
                {
                    T component = i_Child.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }

            return null;
        }

        // Extension method to Activate Random Child GameObject
        /// <summary>
        /// Activate Random Child Object
        /// </summary>
        /// <param name="i_Parent">Transform of whose child are to be Activated</param>
        /// <param name="i_SingleActive">Disable all other child object and keep only one child active</param>
        public static void ActivateRandomChild(this Transform i_Parent, bool i_SingleActive = false)
        {
            if(i_Parent.childCount<1)
            {
                Debug.Log("No child found to activate");
                return;
            }    

            if (i_SingleActive)
            {
                for (int i = 0; i < i_Parent.childCount; i++)
                {
                    i_Parent.GetChild(i).gameObject.SetActive(false);
                }
                i_Parent.GetChild(Random.Range(0, i_Parent.childCount)).gameObject.SetActive(true);

                return;
            }

            GameObject[] i_Children = new GameObject[i_Parent.childCount];
            int i_ActiveChildren = 0;
            for (int i = 0; i < i_Children.Length; i++)
            {
                if(i_Parent.GetChild(i).gameObject.activeSelf)
                {
                    i_Children[i_ActiveChildren] = i_Parent.GetChild(i).gameObject;
                    i_ActiveChildren++;
                }
            }
            if(i_ActiveChildren<1)
            {
                Debug.Log("No inactive child found to activate");
                return;
            }
            i_Children[Random.Range(0, i_ActiveChildren)].SetActive(true);
        }

        public static T GetRandomElement<T>(this List<T> i_List)
        {
            return i_List[Random.Range(0, i_List.Count)];
        }
        public static T GetRandomElement<T>(this T[] i_Array)
        {
            return i_Array[Random.Range(0, i_Array.Length)];
        }

        /// <param name="index"> child index that is to affected, "index: -1" affects all the children</param>
        public static void SetChildActive(this Transform transform, int index = -1, bool value = false)
        {
            if(index<0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(value);
                }
                return;
            }
            transform.GetChild(index).gameObject.SetActive(value);
        }
        public static List<T> FindObjectsOfInterface<T>(this Transform rootObject) where T : class
        {
            List<T> objectsOfInterface = new List<T>();

            T[] components = rootObject.GetComponentsInChildren<T>(true);
            foreach (T component in components)
            {
                objectsOfInterface.Add(component);
            }

            return objectsOfInterface;
        }
        public static void DelayedAction(this MonoBehaviour mono, UnityAction action, float delay)
        {
            mono.StartCoroutine(DoAction(delay, action));
        }
        private static IEnumerator DoAction(float delay, UnityAction action)
        {
            yield return new WaitForSecondsRealtime(delay);
            action.Invoke();
        }
        public static void SkipFrame(this MonoBehaviour mono, UnityAction action)
        {
            mono.StartCoroutine(DoAction(action));
        }
        private static IEnumerator DoAction(UnityAction action)
        {
            yield return new WaitForEndOfFrame();
            action.Invoke();
        }
    }
}