using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DisplayManager クラス
/// 製作者：実川
/// </summary>
public class DisplayManager : MonoBehaviour
{
	enum DisplayType
	{
		Logo,
		Menu
	}

	private readonly Dictionary<DisplayType, DisplayBase> _DISPLAY_MAP = new Dictionary<DisplayType, DisplayBase>
	{
		{ DisplayType.Logo, new DisplayBase() }
	}
}
