using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PointOfService;

namespace POSMainForm
{
    public class MyCashDrawer : CashDrawer
    {
        public override bool CapStatus => throw new NotImplementedException();

        public override bool CapStatusMultiDrawerDetect => throw new NotImplementedException();

        public override bool DrawerOpened => throw new NotImplementedException();

        public override PowerReporting CapPowerReporting => throw new NotImplementedException();

        public override bool CapStatisticsReporting => throw new NotImplementedException();

        public override bool CapUpdateStatistics => throw new NotImplementedException();

        public override string CheckHealthText => throw new NotImplementedException();

        public override bool Claimed => throw new NotImplementedException();

        public override string DeviceDescription => throw new NotImplementedException();

        public override bool DeviceEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override string DeviceName => throw new NotImplementedException();

        public override bool FreezeEvents { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override PowerNotification PowerNotify { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override PowerState PowerState => throw new NotImplementedException();

        public override string ServiceObjectDescription => throw new NotImplementedException();

        public override ControlState State => throw new NotImplementedException();

        public override event DirectIOEventHandler DirectIOEvent;

        public override event StatusUpdateEventHandler StatusUpdateEvent;

        public override string CheckHealth(HealthCheckLevel level)
        {
            throw new NotImplementedException();
        }

        public override void Claim(int timeout)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override DirectIOData DirectIO(int command, int data, object obj)
        {
            throw new NotImplementedException();
        }

        public override void Open()
        {
            throw new NotImplementedException();
        }

        public override void OpenDrawer()
        {
            //throw new NotImplementedException();
        }

        public override void Release()
        {
            throw new NotImplementedException();
        }

        public override void ResetStatistic(string statistic)
        {
            throw new NotImplementedException();
        }

        public override void ResetStatistics()
        {
            throw new NotImplementedException();
        }

        public override void ResetStatistics(StatisticCategories statistics)
        {
            throw new NotImplementedException();
        }

        public override void ResetStatistics(string[] statistics)
        {
            throw new NotImplementedException();
        }

        public override string RetrieveStatistic(string statistic)
        {
            throw new NotImplementedException();
        }

        public override string RetrieveStatistics()
        {
            throw new NotImplementedException();
        }

        public override string RetrieveStatistics(StatisticCategories statistics)
        {
            throw new NotImplementedException();
        }

        public override string RetrieveStatistics(string[] statistics)
        {
            throw new NotImplementedException();
        }

        public override void UpdateStatistic(string name, object value)
        {
            throw new NotImplementedException();
        }

        public override void UpdateStatistics(Statistic[] statistics)
        {
            throw new NotImplementedException();
        }

        public override void UpdateStatistics(StatisticCategories statistics, object value)
        {
            throw new NotImplementedException();
        }

        public override void WaitForDrawerClose(int beepTimeout, int beepFrequency, int beepDuration, int beepDelay)
        {
            throw new NotImplementedException();
        }
    }
}
