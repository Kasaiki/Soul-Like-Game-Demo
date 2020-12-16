using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponMasterUI : MonoBehaviour 
{
	public Animator previewAnim;
	public GameObject prefabButton;
	public string replaceName;
	public AnimationClip[] line1_anims;
	public AnimationClip[] line2_anims;
	public AnimationClip[] line3_anims;

	void Start ()
	{
		prefabButton.SetActive (false);

		//Line1
		for (int i = 0; i < line1_anims.Length; i++)
		{
			ButtonInstantiate(line1_anims[i], 0, i);
		}
		//Line2
		for (int i = 0; i < line2_anims.Length; i++)
		{
			ButtonInstantiate(line2_anims[i], 1, i);
		}
		//Line2
		for (int i = 0; i < line3_anims.Length; i++)
		{
			ButtonInstantiate(line3_anims[i], 2, i);
		}
	}

	void Update () 
	{
		
	}

	private void ButtonInstantiate(AnimationClip anim,int Xindex , int Yindex)
	{
		GameObject go = Instantiate (prefabButton, prefabButton.transform.position + new Vector3(160 * Xindex, -30 * Yindex, 0), Quaternion.identity);
		go.transform.parent = gameObject.transform;
		go.GetComponentInChildren<Text> ().text = anim.name.Replace (replaceName, "");
		go.GetComponent<Button> ().onClick.AddListener(delegate() 
			{ 
				previewAnim.Rebind();
				previewAnim.Play(anim.name);
			});
		go.SetActive (true);
	}
}
