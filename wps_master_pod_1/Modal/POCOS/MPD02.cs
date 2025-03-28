using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using wps_master_pod_1.Modal.Enums;

namespace wps_master_pod_1.Modal.POCOS
{
    [Table("mpd02")]
    public class MPD02
    {
        /// <summary>
        /// worker_id
        /// </summary>
        [MaxLength(20), NotNull]
        public string D02F01;

        /// <summary>
        /// running or push
        /// </summary>
        public WorkerPodStatus D02F03;
    }
}
