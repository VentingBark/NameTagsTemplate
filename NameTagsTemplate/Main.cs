using BepInEx;
using NameTagsTemplate.Tags;
using HarmonyLib;
using TMPro;
using UnityEngine;

[BepInPlugin("NameTags.Template", "NameTagsTemplate", "1.0.0")]
public class Main : BaseUnityPlugin
{
    // Font style used for every nametag this mod creates.
    public static FontStyles activeTMPFontStyle = FontStyles.Bold;

    // The nametag shown above every player's head.
    private readonly Name nameTags = new Name();

    public void Awake()
    {
        new Harmony(Info.Metadata.GUID).PatchAll();
    }

    // Runs every frame: refresh every player's nametag.
    public void Update()
    {
        nameTags.Update();
    }

    /// <summary>Makes a tag always rotate to face the local player's camera.</summary>
    public class TMPLookAt : MonoBehaviour
    {
        public VRRig who;
        public TextMeshPro text;

        private void Update()
        {
            if (Camera.main == null || text == null)
                return;

            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0f;
            forward.Normalize();

            transform.rotation = Quaternion.LookRotation(forward);
        }
    }
}
