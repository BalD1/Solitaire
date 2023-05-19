using System;
using System.Collections.Generic;
using UnityEngine;

public class DataKeeper
{
	[field: SerializeField] public static LocalizationManager.Languages CurrentLanguage = LocalizationManager.Languages.EN;
}