
using System.Linq;
using NameTagsTemplate.Tags;


namespace NameTagsTemplate.Tags
{
    /// <summary>Extension methods for VRRig.</summary>

    public static class VRRigExtensions
    {
        private static RigContainer GetContainer(this VRRig rig) =>
            VRRigCache.ActiveRigContainers.FirstOrDefault(rc => rc.Rig == rig);

        public static int GetPing(this VRRig rig) =>
            rig.GetContainer()?.PlayerStats.Ping ?? 0;

        public static int GetFps(this VRRig rig) =>
            rig.GetContainer()?.PlayerStats.FPS ?? 0;
    }

    /// <summary>The nametag that floats above a player's head, showing their nickname.</summary>
    /// 
    public class Name : TagTracker
    {
        public Name() : base(UnityEngine.Color.white, 0.8f) { }

        protected override string GetText(VRRig rig)
        {
            string platform = NetworkSystem.Instance.GetPlayerPlatform(rig.Creator);
            return $"{rig.Creator.NickName}\n" +
                   $"ID: {rig.Creator.UserId}\n" +
                   $"Ping: {rig.GetPing()}ms\n" +
                   $"FPS: {rig.GetFps()}\n" +
                   $"Platform: {platform}\n" +
                   $"Master: {rig.Creator.IsMasterClient}\n" +
                   $"Local: {rig.Creator.IsLocal}\n" +
                   $"InRoom: {rig.Creator.InRoom}\n" +
                   $"Valid: {rig.Creator.IsValid}";
        }
    }
}
