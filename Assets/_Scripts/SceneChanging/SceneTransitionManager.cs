using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    private EntryInteraction.EntryToSpawnAt _entryToSpawnAt;
    private Vector3 _positionToSpawnAt;
    private static bool _loadFromEntry;
    private static bool _loadFromPosition;

    private GameObject _player;
    private Transform _cameraTargetPoint;
    private Collider2D _playerCollider;
    private Vector3 _playerEntryPoint;
    private Collider2D _entryCollider;
    private CinemachineCamera vcam;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerCollider = _player.GetComponent<Collider2D>();
        _cameraTargetPoint = _player.transform.Find("CameraFollowPoint");
        vcam = GameObject.FindGameObjectWithTag("CinemachineCamera").GetComponent<CinemachineCamera>();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {   
        if (SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            return;
        }
        SceneFadeManager.instance.StartFadeIn();
        if (_loadFromEntry)
        {
            FindEntry(_entryToSpawnAt);
            Vector3 oldPosition = _cameraTargetPoint.position;
            _player.transform.position = _playerEntryPoint;
            vcam.OnTargetObjectWarped(_cameraTargetPoint, _playerEntryPoint - oldPosition);
            _loadFromEntry = false;
        }
        else if (_loadFromPosition)
        {
            Vector3 oldPosition = _cameraTargetPoint.position;
            _player.transform.position = _positionToSpawnAt;
            vcam.OnTargetObjectWarped(_cameraTargetPoint, _positionToSpawnAt - oldPosition);
            _loadFromPosition = false;
        }
    }

    public static void StartSceneChangeFromEntry(string scene, EntryInteraction.EntryToSpawnAt entryToSpawnAt)
    {
        _loadFromEntry = true;
        instance.StartCoroutine(instance.FadeToSceneChange(scene, entryToSpawnAt));
    }

    public static void StartSceneChangeFromPosition(string scene, Vector3 positionToSpawnAt)
    {
        _loadFromPosition = true;
        instance.StartCoroutine(instance.FadeToSceneChangePosition(scene, positionToSpawnAt));
    }

    private IEnumerator FadeToSceneChange(string scene, EntryInteraction.EntryToSpawnAt entryToSpawnAt = EntryInteraction.EntryToSpawnAt.None)
    {
        SceneFadeManager.instance.StartFadeOut();

        while (SceneFadeManager.instance.isFadingOut)
        {
            yield return null;
        }

        _entryToSpawnAt = entryToSpawnAt;
        SceneManager.LoadScene(scene);
    }

    private IEnumerator FadeToSceneChangePosition(string scene, Vector3 positionToSpawnAt)
    {
        SceneFadeManager.instance.StartFadeOut();
        while (SceneFadeManager.instance.isFadingOut)
        {
            yield return null;
        }

        _positionToSpawnAt = positionToSpawnAt;
        SceneManager.LoadScene(scene);

    }

    private void FindEntry(EntryInteraction.EntryToSpawnAt entryToSpawnAt)
    {
        EntryInteraction[] entries = GameObject.FindObjectsByType<EntryInteraction>(FindObjectsSortMode.None);
        {
            foreach (EntryInteraction entry in entries)
            {
                if (entry._thisEntryNumber == entryToSpawnAt)
                {
                    _entryCollider = entry.gameObject.GetComponent<Collider2D>();
                    _playerEntryPoint = _entryCollider.transform.position - new Vector3(0, _playerCollider.bounds.extents.y, 0);
                    return;
                }
            }
        }
    }
}
