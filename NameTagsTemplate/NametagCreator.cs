using TMPro;
using UnityEngine;
using static Main;

namespace NameTagsTemplate
{
    /// <summary>Builds the actual floating text object used by both the Name and Platform tags.</summary>
    public class NametagCreator
    {
        /// <param name="rig">The player this tag belongs to.</param>
        /// <param name="color">Default text color (Name/Platform may override it with inline &lt;color&gt; tags).</param>
        /// <param name="offset">How far above the player's body the tag sits.</param>
        /// <param name="initialText">Placeholder text shown until the first real update.</param>
        public static GameObject CreateTag(VRRig rig, Color color, float offset, string initialText = "Test")
        {
            GameObject gameObject = new GameObject("NameTag_TMP");
            Transform parent = rig.transform.Find("Body") ?? rig.transform;

            gameObject.transform.SetParent(parent, false);
            gameObject.transform.localPosition = new Vector3(0f, offset, 0f);
            gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
            textMeshPro.enableAutoSizing = false;
            textMeshPro.fontSize = 4f;
            textMeshPro.alignment = TextAlignmentOptions.Center;
            textMeshPro.fontStyle = FontStyles.Bold;
            textMeshPro.color = color;
            textMeshPro.text = initialText;

            // Keeps the tag rotated to always face whoever is looking at it.
            TMPLookAt lookAt = gameObject.AddComponent<TMPLookAt>();
            lookAt.who = rig;
            lookAt.text = textMeshPro;

            return gameObject;
        }
    }
}
