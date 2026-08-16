using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace NameTagsTemplate.Tags
{
    /// <summary>
    /// Everything a floating tag needs in order to live above a player's head:
    /// create one the first time we see a player, keep its text up to date,
    /// keep it positioned above their head, and destroy it once they leave.
    ///
    /// Name.cs and Platform.cs both used to duplicate this logic. Now they just
    /// inherit from this class and implement GetText(), which decides what the
    /// tag should actually say for a given player.
    /// </summary>
    public abstract class TagTracker
    {
        // One tag GameObject per player currently in the room.
        private readonly Dictionary<VRRig, GameObject> tags = new Dictionary<VRRig, GameObject>();

        private readonly Color tagColor;
        private readonly float heightOffset;

        protected TagTracker(Color tagColor, float heightOffset)
        {
            this.tagColor = tagColor;
            this.heightOffset = heightOffset;
        }

        /// <summary>Call this once per frame. Refreshes every player's tag and removes tags for anyone who left.</summary>
        public void Update()
        {
            IReadOnlyList<VRRig> activeRigs = VRRigCache.ActiveRigs;

            RemoveTagsForPlayersWhoLeft(activeRigs);

            foreach (VRRig rig in activeRigs)
            {
                // Don't put a tag above your own head.
                if (rig != GorillaTagger.Instance.offlineVRRig)
                    UpdateTag(rig);
            }
        }

        private void RemoveTagsForPlayersWhoLeft(IReadOnlyList<VRRig> activeRigs)
        {
            List<VRRig> playersWhoLeft = new List<VRRig>();

            foreach (KeyValuePair<VRRig, GameObject> entry in tags)
            {
                if (!activeRigs.Contains(entry.Key))
                {
                    GameObject.Destroy(entry.Value);
                    playersWhoLeft.Add(entry.Key);
                }
            }

            foreach (VRRig rig in playersWhoLeft)
                tags.Remove(rig);
        }

        private void UpdateTag(VRRig rig)
        {
            if (!tags.TryGetValue(rig, out GameObject tag))
            {
                tag = NametagCreator.CreateTag(rig, tagColor, heightOffset);
                tags[rig] = tag;
            }

            tag.GetComponent<TextMeshPro>().text = GetText(rig);

            // Keep the tag floating just above the player's head.
            // (Rotating the tag to face the camera is handled separately by
            // the TMPLookAt component that NametagCreator attaches to it.)
            Transform head = rig.transform.Find("Head") ?? rig.transform;
            tag.transform.position = head.position + new Vector3(0f, heightOffset, 0f);
        }

        /// <summary>What should this player's tag say? Implemented by Name and Platform.</summary>
        protected abstract string GetText(VRRig rig);
    }
}
