using System.Collections.Generic;

namespace LabirunModel.Labirun
{
    public partial class JobsResponse
    {
        public JobData data;
        public int status;
    }

    public partial class JobData
    {
        public List<ScheduledJob> scheduledJobs;
    }

    public partial class ScheduledJob
    {
        public string gameId;
        public string description;
        public string jobId;
        public int localTime;
        public int createdAt;
        public int runStartTime;
        public string playerSessionId;
        public int scheduledStartTime;
        public string scriptName;
        public string jobType;
        public object parameters;
        public int updatedAt;
    }
}