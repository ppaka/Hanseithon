﻿using UnityEngine;
using System.IO;
using UnityEditor.AssetImporters;

[ScriptedImporter(1, "osu")]
public class OsuImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var subAsset = new TextAsset(File.ReadAllText(ctx.assetPath));
        ctx.AddObjectToAsset("text", subAsset);
        ctx.SetMainObject(subAsset);
    }
}