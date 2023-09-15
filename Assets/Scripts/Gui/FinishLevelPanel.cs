using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishLevelPanel : MonoBehaviour, IEventsDependence
{
	[SerializeField] private Button RestartButton;

	private void Start()
	{
		RestartButton.onClick.AddListener(OnPressRestartButton);
		SubscribeToGlobalEvents();
	}

	private void OnPressRestartButton()
	{
		HidePanel();
        Events.resetEvent?.Invoke();
    }

	private void ShowPanel()
	{
        RestartButton.gameObject.SetActive(true);
	}

	private void HidePanel()
	{
        RestartButton.gameObject.SetActive(false);
	}


	public void SubscribeToGlobalEvents()
	{
		Events.finishLevelEvent += ShowPanel;
	}

	public void UnsubscribeFromGlobalEvents()
	{
		Events.finishLevelEvent -= ShowPanel;
	}
}



