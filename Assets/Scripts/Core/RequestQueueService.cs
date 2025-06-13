using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System.Collections;

public class RequestQueueService : MonoBehaviour
{
    private readonly Queue<AppRequest> _requests = new Queue<AppRequest>();
    private AppRequest _currentRequest;

    private void Awake()
    {
        StartCoroutine(ProcessQueue());
    }

    public void Enqueue(AppRequest request)
    {
        Debug.Log($"[RequestQueue] �������� � ������� ������ � �����: {request.Tag}");
        _requests.Enqueue(request);
    }

    public void CancelRequestByTag(string tag)
    {
        Debug.Log($"[RequestQueue] ������� ������ ������� � �����: {tag}");

        if(_currentRequest != null && _currentRequest.Tag == tag)
        {
            Debug.Log($"[RequestQueue] �������� ������� ������: {tag}");
            _currentRequest.WebRequest?.Abort();
            _currentRequest = null;
        }

        var filteredQueue = new Queue<AppRequest>(_requests.Where(req => req.Tag != tag));
        if(filteredQueue.Count < _requests.Count)
        {
            Debug.Log($"[RequestQueue] ������� �� ������� ������(�) � �����: {tag}");

            _requests.Clear();
            foreach(var req in filteredQueue)
            {
                _requests.Enqueue(req);
            }
        }
    }

    private IEnumerator ProcessQueue()
    {
        while (true)
        {
            if(_currentRequest == null && _requests.Count > 0)
            {
                _currentRequest = _requests.Dequeue();
                Debug.Log($"[RequestQueue] �������� ���������� �������: {_currentRequest.Tag}");
                yield return StartCoroutine(ExecuteRequest(_currentRequest));
                _currentRequest = null;
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator ExecuteRequest(AppRequest request)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(request.URL))
        {
            request.WebRequest = webRequest;

            yield return webRequest.SendWebRequest();

            if(_currentRequest == null)
            {
                Debug.Log($"[RequestQueue] ������ {request.Tag} ��� ������� �� ����� ����������.");
                yield break;
            }

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    Debug.Log($"[RequestQueue] �����: {request.Tag}. ��������: {webRequest.downloadHandler.text.Substring(0, 100)}...");
                    request.OnSuccess?.Invoke(webRequest.downloadHandler.text);
                    break;
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log($"[RequestQueue] ������: {request.Tag}. {webRequest.error}");
                    request.OnFailure?.Invoke(webRequest.error);
                    break;
                case UnityWebRequest.Result.InProgress:
                    break;
            }
        }
    }
}
