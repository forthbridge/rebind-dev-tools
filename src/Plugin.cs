using BepInEx;
using BepInEx.Logging;
using System.Security.Permissions;
using System.Security;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[module: UnverifiableCode]
#pragma warning restore CS0618 // Type or member is obsolete


namespace RebindDevTools
{
    [BepInPlugin(AUTHOR + "." + MOD_ID, MOD_NAME, VERSION)]
    internal class Plugin : BaseUnityPlugin
    {
        public static new ManualLogSource Logger { get; private set; } = null!;

        public const string VERSION = "1.0.2";
        public const string MOD_NAME = "Rebind Dev Tools";
        public const string MOD_ID = "rebinddevtools";
        public const string AUTHOR = "forthbridge";

        public void OnEnable()
        {
            Logger = base.Logger;
            Hooks.ApplyHooks();

            Execute("Joar");
        }

        private void Execute(string who) => Logger.LogMessage($"Executed {who} for splitting the binds across literally every creature script, wtf man.");
    }
}
