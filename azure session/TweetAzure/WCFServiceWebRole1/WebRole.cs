using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Net;
using System.Web;
using TweetAzure;

namespace WCFServiceWebRole1
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // To enable the AzureLocalStorageTraceListner, uncomment relevent section in the web.config  
            DiagnosticMonitorConfiguration diagnosticConfig = DiagnosticMonitor.GetDefaultInitialConfiguration();
            diagnosticConfig.Directories.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);
            diagnosticConfig.Directories.DataSources.Add(AzureLocalStorageTraceListener.GetLogDirectory());

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            StartListening();
            return base.OnStart();
        }
        private void StartListening()
        {
            var roleInstance = RoleEnvironment.CurrentRoleInstance;

            RoleInstanceEndpoint endpoint = roleInstance.InstanceEndpoints["Endpoint1"];

            string baseAddress = "http://" + endpoint.IPEndpoint.ToString();

            string forumsAddress = baseAddress;

            ServiceHost host = new ServiceHost(typeof(Service1), new Uri(forumsAddress));

            host.AddServiceEndpoint(typeof(IService1), new WebHttpBinding(), "").Behaviors.Add(new WebHttpBehavior());

            host.Open();

            LocalResource resource = RoleEnvironment.GetLocalResource("DataFiles");
            Service1.dataFilesDirectory = resource.RootPath;

            Console.WriteLine("Listening on: {0}", forumsAddress);
        }
    }
}
