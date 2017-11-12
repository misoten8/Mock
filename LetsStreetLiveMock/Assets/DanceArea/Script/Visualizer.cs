using UnityEngine;
using System.Collections;


public class Visualizer : MonoBehaviour
{
	[SerializeField]
	private Reaktion.Reaktor spectrum1;
	[SerializeField]
	private Reaktion.Reaktor spectrum2;
	[SerializeField]
	private Reaktion.Reaktor spectrum3;
	[SerializeField]
	private Reaktion.Reaktor spectrum4;
	public Vector4 spectrum;

	void Update()
	{
		spectrum = new Vector4(spectrum1.Output, spectrum2.Output, spectrum3.Output, spectrum4.Output);
	}

	void OnWillRenderObject()
	{
		if (GetComponent<Renderer>() == null || GetComponent<Renderer>().sharedMaterial == null) { return; }
		Material mat = GetComponent<Renderer>().material;

		if (Vector4.Dot(spectrum, spectrum) <= 1.0f)
		{
			mat.SetVector("_Spectra", spectrum);
		}
	}
}
