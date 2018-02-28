using Aurora.SMS.AWS.Interfaces;
using Aurora.SMS.Worker.Interfaces;
using System;

namespace Aurora.SMS.Worker
{
    /// <summary>
    /// Schedules a job to read the messages from the SQS and send to the SMS provider
    /// </summary>
    public class JobScheduler
    {
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();
        private readonly ISQSsmsServices _iSQSsmsServices;
        private readonly IConsumeSMS _consumeSMS;

        public JobScheduler(ISQSsmsServices iSQSsmsServices, IConsumeSMS consumeSMS)
        {
            _iSQSsmsServices = iSQSsmsServices ?? throw new ArgumentNullException(nameof(iSQSsmsServices));
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
            _consumeSMS = consumeSMS ?? throw new ArgumentNullException(nameof(consumeSMS));
        }

        public void Start()
        {
            // The timer runs every VisibilityTimeOut + 1min
            _timer.Interval = _iSQSsmsServices.GetVisibilityTimeOut().Result + 60000;
            _timer.Start();
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }

        public async void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            await _consumeSMS.Execute();
            // TODO: Insert monitoring activities here.
            //eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
            /*This method:
             * - Reads some messages from the queue
             * - Sends the messages to the provider, updates the status in the database and deletes them from the queue
             */
        }
    }
}