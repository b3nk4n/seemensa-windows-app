using Windows.ApplicationModel.Background;

namespace SeeMensaWindows.Common.Agents
{
    /// <summary>
    /// Helper class which manages the background task registration.
    /// </summary>
    public static class BackgroundTask
    {
        /// <summary>
        /// Registers a timed background task.
        /// </summary>
        /// <param name="name">The name of the background agent.</param>
        /// <param name="entryPoint">The entry point of the background agent.</param>
        /// <param name="intervall">The task intervall. The value must be 15 or more.</param>
        public static void RegisterTimedBackgroundTask(string name, string entryPoint, uint intervall)
        {
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
            // Friendly string name identifying the background task
            builder.Name = name;
            // Class name
            builder.TaskEntryPoint = entryPoint;

            IBackgroundTrigger trigger = new MaintenanceTrigger(intervall, false);
            builder.SetTrigger(trigger);

            IBackgroundTaskRegistration task = builder.Register();
        }
    }
}
