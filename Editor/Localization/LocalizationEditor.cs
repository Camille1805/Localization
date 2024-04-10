using System.Diagnostics;
using System.IO;

namespace Localization
{
    public static class LocalizationEditor 
    {
    	[UnityEditor.MenuItem("Localization/GenerateJSON")]
    	public static void GenerateJSON()
    	{
            ProcessStartInfo startInfo = new()
            {
                FileName = Directory.GetCurrentDirectory() + "\\Localization\\LocalizationConverter.exe"
            };
            UnityEngine.Debug.Log(Directory.GetCurrentDirectory() + "\\Localization\\LocalizationConverter.exe");
    		startInfo.WorkingDirectory = Directory.GetCurrentDirectory()+"\\Localization"; 
    		Process.Start(startInfo);
    	}
    }
}
