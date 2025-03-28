
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Diagnostics.CodeAnalysis;

namespace wps_master_pod_1.Modal.POCOS
{
    [Table("mpd01")]
    public class MPD01
    {
        /// <summary>
        /// Clinet_id (who owns this server)
        /// </summary>
        public int D01F01;

        /// <summary>
        /// server id (unique id for this server)
        /// </summary>
        public int D01F02;

        /// <summary>
        /// worker_id
        /// </summary>
        [MaxLength(20)]
        public string? D01F03;
    }
}
