using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections;

public class AsyncHelper : MonoBehaviour
{
    private static AsyncHelper instance;
    public static AsyncHelper Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AsyncHelper>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(AsyncHelper).Name);
                    instance = singletonObject.AddComponent<AsyncHelper>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }
    
    public void RunAsyncWithoutResult(Func<Task> asyncAction)
    {
        StartCoroutine(RunAsyncCoroutineWithoutResult(asyncAction));
    }
    
    public void RunAsyncWithResult<T>(Func<Task<T>> asyncAction, Action<T> onCompleted = null, Action<Exception> onError = null)
    {
        StartCoroutine(RunAsyncCoroutineWithResult(asyncAction, onCompleted, onError));
    }
    
    private IEnumerator RunAsyncCoroutineWithoutResult(Func<Task> asyncAction)
    {
        Task task = asyncAction();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError($"Ошибка в асинхронной задаче: {task.Exception}");
        }
    }
    
    private IEnumerator RunAsyncCoroutineWithResult<T>(Func<Task<T>> asyncAction, Action<T> onCompleted, Action<Exception> onError)
    {
        Task<T> task = asyncAction();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            onError?.Invoke(task.Exception);
        }
        else
        {
            onCompleted?.Invoke(task.Result);
        }
    }
}