using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Net;
using System.ComponentModel;
using Ionic.Zip;
using System;

namespace Localization
{
    [System.Serializable]
    public class LocalizationConfig
    {
        [SerializeField]
        private string m_fileNameOutput;
        [SerializeField]
        private List<string> m_cellForKey;
        [SerializeField]
        private string m_beginData;
        [SerializeField]
        private List<SystemLanguage> m_langageNameColumns;
        [SerializeField]
        private string m_pathExcelFile;
        private readonly string m_pathConfig = Directory.GetCurrentDirectory() + "\\Localization\\config.ini";
        private readonly string m_pathPahIni = Directory.GetCurrentDirectory() + "\\Localization\\path.ini";

        public LocalizationConfig()
        {
        }

        public string PathExcelFile { get => m_pathExcelFile; set => m_pathExcelFile = value; }

        public void ReadConfig()
        {
            using StreamReader sr = new(m_pathConfig), sr2 = new(m_pathPahIni);
            sr.ReadLine();
            m_fileNameOutput = sr.ReadLine().Split('=')[1];
            m_cellForKey = sr.ReadLine().Split('=')[1].Split(',').ToList();
            m_beginData = sr.ReadLine().Split('=')[1];
            List<string> enumLangageNameString = sr.ReadLine().Split('=')[1].Split(',').ToList();
            m_langageNameColumns = new List<SystemLanguage>();
            foreach (string s in enumLangageNameString)
            {
                m_langageNameColumns.Add((SystemLanguage)Enum.Parse(typeof(SystemLanguage), s));
            }
            // m_langageNameColumns =
             string m_pathGen = sr.ReadLine().Split('=')[1];
            PathExcelFile = sr2.ReadLine();
        }
        public void WriteConfig()
        {

            using (StreamWriter sw = new(m_pathConfig), sw2 = new(m_pathPahIni))
            {
                sw.WriteLine("[OPTIONS]");
                sw.WriteLine("FileNameOutput=" + m_fileNameOutput);
                sw.WriteLine("CellForKey=" + string.Join(",", m_cellForKey.ToArray()));
                sw.WriteLine("BeginData=" + m_beginData);
                List<string> enumLangageNameString = new();
                foreach (SystemLanguage s in m_langageNameColumns)
                {
                    enumLangageNameString.Add(s.ToString());
                }
                sw.WriteLine("LangageColumnName=" + string.Join(",", enumLangageNameString.ToArray()));
                sw.WriteLine("Path=Assets\\Resources\\Traduction\\");
                sw2.Write(PathExcelFile);
            }
            Debug.Log("Save succefull");
        }
    }

    public class LocalizationConfigEditor : EditorWindow
    {
        [SerializeField]
        private LocalizationConfig config;
        Editor editor;
        private bool HasFolder = false;
        float progression = 0;
        [MenuItem("Localization/Configuration")]
        static void Init()
        {

            // Get existing open window or if none, make a new one:
            LocalizationConfigEditor window = (LocalizationConfigEditor)EditorWindow.GetWindow(typeof(LocalizationConfigEditor));
            window.Show();

        }
        private void OnEnable()
        {
            HasFolder = Directory.Exists(Directory.GetCurrentDirectory() + "\\Localization\\");
            if (HasFolder)
            {
                config = new LocalizationConfig();
                config.ReadConfig();
            }


        }
        void OnGUI()
        {
            if (HasFolder)
            {
                if (!editor) { editor = Editor.CreateEditor(this); }
                if (editor) { editor.OnInspectorGUI(); }
                if (GUILayout.Button("Sauvegarder"))
                {
                    editor.serializedObject.ApplyModifiedProperties();
                    config.WriteConfig();
                }
            }
            else
            {
                EditorGUI.ProgressBar(new Rect(3, 45, position.width - 6, 20), progression, (progression*100).ToString("f0")+"%");

                if (GUILayout.Button("DownLoad"))
                {
                    WebClient webClient = new();
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    webClient.DownloadFileAsync(new Uri("http://laverdure.alwaysdata.net/Localization.zip"), Directory.GetCurrentDirectory() +"\\Localization.zip");
                }
            }
        }


        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progression = (float)e.ProgressPercentage / 100;
            this.Repaint();
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            Debug.Log("Download completed!");
            using (Ionic.Zip.ZipFile zf = Ionic.Zip.ZipFile.Read(Directory.GetCurrentDirectory()+"\\Localization.zip"))
            {
                zf.ExtractProgress += ProgressExtract;
                zf.ExtractAll(Directory.GetCurrentDirectory(), ExtractExistingFileAction.InvokeExtractProgressEvent);
            }
            config.PathExcelFile = Directory.GetCurrentDirectory() + "\\Localization";
            config.WriteConfig();
            File.Delete(Directory.GetCurrentDirectory() + "\\Localization.zip");
        }
        private void ProgressExtract(object sender, ExtractProgressEventArgs e)
        {
            progression = (float)e.EntriesExtracted / e.EntriesTotal;
            this.Repaint();
            if (e.EventType == ZipProgressEventType.Extracting_AfterExtractAll)
            {
                this.OnEnable();

            }
        }
    }
}
