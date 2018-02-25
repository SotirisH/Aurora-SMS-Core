using System;

namespace Aurora.SMS.Worker
{
    /// <summary>
    /// Schedules a job to read the messages from the SQS and send to the SMS provider
    /// </summary>
    public class JobScheduler
    {
        private System.Timers.Timer timer = new System.Timers.Timer();
        public void Start()
        {
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Stop()
        {
            timer.Stop();
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            //eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
            /*This method:
             * - Reads some messages from the queue
             * - Sends the messages to the provider, updates the status in the database and deletes them from the queue
             */
        }
    }
}
