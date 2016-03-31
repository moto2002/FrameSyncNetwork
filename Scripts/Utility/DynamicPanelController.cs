using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicPanelController : MonoBehaviour
{

	public static GameObject Create(string Path, Transform parent)
	{
        var go = GameObject.Instantiate(Resources.Load(Path) as GameObject) as GameObject;
        go.transform.SetParent(parent);
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = new Vector3 (1, 1, 1);
		return go;
	}

}

