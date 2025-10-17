using NUnit;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Simulation;

public class CouplingTests
{
    private Mouse _mouse;
    private string _currentScene;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        LogAssert.ignoreFailingMessages = true;
        _currentScene = SceneManager.GetActiveScene().name;
        yield return LoadScene("PlacementScene");
        setupInput();
        yield return new WaitUntil(() => ARSession.state == ARSessionState.SessionTracking);
        yield return simulateScanning();
    }

    private IEnumerator LoadScene(string SceneName)
    {
        bool sceneLoaded = false;
        SceneManager.activeSceneChanged += (oldScene, newScene) => { sceneLoaded = true; };
        SceneManager.LoadScene(SceneName);
        yield return new WaitUntil(() => sceneLoaded);
    }

    private void setupInput()
    {
        _mouse = InputSystem.GetDevice<Mouse>();
    }

    private IEnumerator simulateScanning()
    {
        SimulationCameraPoseProvider camera = GameObject.FindFirstObjectByType<SimulationCameraPoseProvider>();
        float elapsed = 0f; float rotationSpeed = 25f; float duration = 5f;

        while (elapsed < duration)
        {
            float delta = rotationSpeed * Time.deltaTime;
            camera.transform.Rotate(camera.transform.up, delta);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    [UnityTest]
    public IEnumerator CanOnlyPlaceOneCoupling()
    {
        LogAssert.ignoreFailingMessages = true;
        ARPlane plane = ARPlaneTestUtils.FindFirstPlaneInFrustum(GameObject.FindFirstObjectByType<ARPlaneManager>());
        yield return simulateLeftClick(plane.center);
        yield return null;

        yield return simulateScanning();
        plane = ARPlaneTestUtils.FindFirstPlaneInFrustum(GameObject.FindFirstObjectByType<ARPlaneManager>());
        yield return simulateLeftClick(plane.center);
        yield return null;
        yield return new WaitForSeconds(1f);

        int objectCount = GameObject.FindObjectsByType<XRGrabInteractable>(FindObjectsSortMode.None).Length;
        Assert.AreEqual(1, objectCount, "More than one coupling was placed.");
    }

    private IEnumerator simulateLeftClick(Vector3 position)
    {
        Camera camera = GameObject.FindFirstObjectByType<SimulationCameraPoseProvider>().GetComponent<Camera>();
        Vector2 screenPos = camera.WorldToScreenPoint(position);
        InputSystem.QueueStateEvent(_mouse, new MouseState
        {
            buttons = 1,
            position = position
        });
        InputSystem.Update();
        yield return null; // Wait a frame

        InputSystem.QueueStateEvent(_mouse, new MouseState
        {
            buttons = 0,
            position = position
        });
        InputSystem.Update();
        Debug.Log($"Simulated click at {position} " + screenPos);
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return LoadScene(_currentScene);
    }
}
