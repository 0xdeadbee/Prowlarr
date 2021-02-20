using System.Collections.Generic;
using System.Linq;
using NzbDrone.Core.IndexerStats;
using Prowlarr.Http.REST;

namespace Prowlarr.Api.V1.Indexers
{
    public class IndexerStatsResource : RestResource
    {
        public List<IndexerStatistics> Indexers { get; set; }
        public List<UserAgentStatistics> UserAgents { get; set; }
    }
}
