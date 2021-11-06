using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class FighterDisplay : MonoBehaviour
{
	public Fighter fighter;

	public Text nameText;
	public Text descriptionText;

	public Image artworkImage;

	public Text manaText;
	public Text attackText;
	public Text healthText;

	// Use this for initialization
	void Start () {
		nameText.text = fighter.name;
		descriptionText.text = fighter.description;

		artworkImage.sprite = fighter.artwork;

		manaText.text = fighter.manaCost.ToString();
		attackText.text = fighter.attack.ToString();
		healthText.text = fighter.health.ToString();
	}


	[PunRPC]
	public void UpdateCharacter()
    {
		nameText.text = fighter.name;
		descriptionText.text = fighter.description;

		artworkImage.sprite = fighter.artwork;

		manaText.text = fighter.manaCost.ToString();
		attackText.text = fighter.attack.ToString();
		healthText.text = fighter.health.ToString();
	}
	
}
