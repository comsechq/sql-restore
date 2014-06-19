namespace Comsec.SqlRestore.Domain
{
    /// <summary>
    /// A single database file list entry
    /// </summary>
    public class FileListEntry
    {
        /// <summary>
        /// Gets or sets the name of the logical.
        /// </summary>
        /// <value>
        /// The name of the logical.
        /// </value>
        public string LogicalName { get; set; }

        /// <summary>
        /// Gets or sets the name of the physical.
        /// </summary>
        /// <value>
        /// The name of the physical.
        /// </value>
        public string PhysicalName { get; set; }

        /// <summary>
        /// Gets or sets the type ("D" for data (MDF) file, "L" for log (LDF) file).
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }
    }
}
