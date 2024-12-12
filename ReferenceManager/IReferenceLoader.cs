namespace ReferenceManager
{
    /// <summary>
    /// Interface for loading references.
    /// </summary>
    public interface IReferenceLoader
    {
        /// <summary>
        /// Loads a list of references.
        /// </summary>
        /// <returns>A list of references loaded from a data source.</returns>
        List<Reference> LoadReferences();
    }
}
