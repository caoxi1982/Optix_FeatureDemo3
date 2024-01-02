#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.Recipe;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.WebUI;
using FTOptix.UI;
using FTOptix.Alarm;
using FTOptix.DataLogger;
using FTOptix.EventLogger;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.Report;
using FTOptix.OPCUAServer;
using FTOptix.RAEtherNetIP;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using FTOptix.NativeUI;
using FeaturesDemo2;
using System.IO;
#endregion

public class SentimentPredict : BaseNetLogic
{
    public override void Start()
    {
        Sentiment.MLNetModelPath = Path.Combine(Project.Current.ProjectDirectory, @"Model/Sentiment.mlnet");
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
    [ExportMethod]
    public void Predict(string comments, out float pos, out float nati)
    {
        //Load sample data
        var sampleData = new Sentiment.ModelInput()
        {
            Col0 = comments,
        };

        //Load model and predict output
        var result = Sentiment.Predict(sampleData);
        Log.Info($"Predicted sentiment for \"{sampleData.Col0}\" is: {result.PredictedLabel}");
        pos = result.Score[0];
        nati = result.Score[1];

    }
}
