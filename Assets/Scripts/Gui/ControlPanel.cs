using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
	[SerializeField] private Button resetButton;
	[SerializeField] private TMP_Text collectableText;
	private int CollectableSize;

	private void Start()
    {
		resetButton.onClick.AddListener(OnPressResetButton);
		Events.collectionEvent += WriteCollectionSize;
	}

    private void OnPressResetButton()
    { 
		Events.resetEvent?.Invoke();
	}
	public void WriteCollectionSize() 
	{ 
		CollectableSize ++; 
		collectableText.text = CollectableSize.ToString(); 
	}
    private void OnDestroy()
    {
        Events.collectionEvent?.Invoke();
    }
}
