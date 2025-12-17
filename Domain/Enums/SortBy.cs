namespace Domain.Enums
{
    public enum SortBy
    {
        /// <summary>
        /// Sort by Task ID (newest first)
        /// </summary>
        IdDesc = 0,

        /// <summary>
        /// Sort by Task ID (oldest first)
        /// </summary>
        IdAsc = 1,

        /// <summary>
        /// Sort by Title (A to Z)
        /// </summary>
        TitleAsc = 2,

        /// <summary>
        /// Sort by Title (Z to A)
        /// </summary>
        TitleDesc = 3,

        /// <summary>
        /// Sort by Due Date (earliest first)
        /// </summary>
        DueDateAsc = 4,

        /// <summary>
        /// Sort by Due Date (latest first)
        /// </summary>
        DueDateDesc = 5,

        /// <summary>
        /// Sort by Status ID
        /// </summary>
        StatusAsc = 6,

        /// <summary>
        /// Sort by Creation Date (newest first)
        /// </summary>
        CreatedDesc = 7,

        /// <summary>
        /// Sort by Creation Date (oldest first)
        /// </summary>
        CreatedAsc = 8
    }
}
