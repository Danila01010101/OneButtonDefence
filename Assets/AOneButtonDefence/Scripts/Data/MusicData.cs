using UnityEngine;

[CreateAssetMenu(fileName = "Music Data", menuName = "ScriptableObjects/new Music Data", order = 1)]
public class MusicData : ScriptableObject
{
	[field:Header("StartValues")]
	[field:SerializeField] public float StartValue { get; private set; }
	[field:Header("Sounds")]
	[field:SerializeField] public AudioClip LoadingSound { get; private set; }
	[field:SerializeField] public AudioClip StartDialogueMusic { get; private set; }
	[field:SerializeField] public AudioClip BattleMusic { get; private set; }
	[field:SerializeField] public AudioClip BattleWinSoundEffect { get; private set; }
	[field:SerializeField] public AudioClip BattleLostSoundEffect { get; private set; }
	[field:SerializeField] public AudioClip UpgradeMusic { get; private set; }
	[field:Header("Upgrade effects")]
	[field:SerializeField] public AudioClip SpellBuffSoundEffect { get; private set; }
	[field:SerializeField] public AudioClip WarriorBuffSoundEffect { get; private set; }
	[field:SerializeField] public AudioClip UpgradeFactorySoundEffect { get; private set; }
	[field:SerializeField] public AudioClip UpgradeChurchSoundEffect { get; private set; }
	[field:SerializeField] public AudioClip UpgradeCampSoundEffect { get; private set; }
	[field:SerializeField] public AudioClip UpgradeFarmSoundEffect { get; private set; }
}