namespace DustInTheWind.ActiveTime.Watchman
{
    /// <summary>
    /// Specifies the level at which an instance of the <see cref="Guard"/> class will work.
    /// </summary>
    public enum GuardLevel
    {
        /// <summary>
        /// The instance of the <see cref="Guard"/> class will restrict the duplicates at the application level.
        /// That means that two instances with the same name can exists in two different applications running
        /// on the same machine.
        /// </summary>
        Application,

        /// <summary>
        /// The instance of the <see cref="Guard"/> class will restrict the duplicates at the machine level.
        /// That means that two instances with the same name are not allowed to exist in two different
        /// applications running on the same machine. But in the same application they are allowed.
        /// </summary>
        Machine
    }
}
