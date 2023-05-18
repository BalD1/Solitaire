using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_IG : Singleton<UIManager_IG>
{
	[SerializeField] private PausePanel pausePanel;

	protected override void Awake()
	{
		base.Awake();
	}
}