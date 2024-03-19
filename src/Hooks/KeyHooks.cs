using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using UnityEngine;

namespace RebindDevTools;

public static partial class Hooks
{
    private static void RebindKeyIL(ILCursor c, string key, Func<bool> inputAction, bool getKeyDown=false, int? keyCode=null)
    {
        int rebinds = 0;

        try
        {
            c.Index = 0;

            if (keyCode == null)
            {
                while (c.TryGotoNext(MoveType.After,
                    x => x.MatchLdstr(key),
                    x => x.MatchCallOrCallvirt<Input>(getKeyDown ? nameof(Input.GetKeyDown) : nameof(Input.GetKey))))
                {
                    c.Emit(OpCodes.Pop);
                    c.EmitDelegate(inputAction);
                    
                    rebinds++;
                }
            }
            else
            {
                while (c.TryGotoNext(MoveType.After,
                    x => x.MatchLdcI4((int)keyCode),
                    x => x.MatchCallOrCallvirt<Input>(getKeyDown ? nameof(Input.GetKeyDown) : nameof(Input.GetKey))))
                {
                    c.Emit(OpCodes.Pop);
                    c.EmitDelegate(inputAction);

                    rebinds++;
                }
            }
        }
        catch (Exception e)
        {
            Plugin.Logger.LogWarning($"Error IL Rebinding Key! ({key})\n" + e);
        }


        // n is intentionally attempted to be rebound more than it should be, for all creatures and objects, just in case it is used in a camera offset
        if (rebinds == 0 && key != "n")
        {
            Plugin.Logger.LogWarning($"Key was not rebound at all! ({key})");
        }
    }

    private static void RebindKeyDownIL(ILCursor c, string key, Func<bool> inputAction) => RebindKeyIL(c, key, inputAction, true);

    private static void RebindKeyCodeIL(ILCursor c, int keyCode, Func<bool> inputAction) => RebindKeyIL(c, keyCode.ToString(), inputAction, false, keyCode);
}
