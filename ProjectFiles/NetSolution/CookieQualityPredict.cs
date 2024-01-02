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
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Core;
using FeaturesDemo2;
using System.IO;
using FTOptix.RAEtherNetIP;
using FTOptix.CommunicationDriver;
using FTOptix.NativeUI;
#endregion

public class CookieQualityPredict : BaseNetLogic
{
    public override void Start()
    {
        CookieQuality.MLNetModelPath = MLQualityModelPath;
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
    [ExportMethod]
    public void predict(float diameter, float exittemperature, float height,out float Quality)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        //Load sample data
        var sampleData = new CookieQuality.ModelInput()
        {
            Diameter = diameter,
            Exit_Temperature = exittemperature,
            Height = height,
        };

        //Load model and predict output
        var result = CookieQuality.Predict(sampleData);
        Quality = result.Score;
        //Another way to feedback to PLC
        var prediction = Project.Current.GetVariable("Model/MachineLearning/PredictResult");
        prediction.Value = Quality;
        watch.Stop();
        Log.Verbose1("Predict Time span ", $"Predict timespan ={watch.Elapsed}");

    }
    private static string MLQualityModelPath = Path.Combine(Project.Current.ProjectDirectory, @"Model/CookieQuality.mlnet");
}
