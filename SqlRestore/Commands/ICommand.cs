namespace Comsec.SqlRestore.Commands
{
    /// <summary>
    /// Interface representing a command.
    /// </summary>
    /// <typeparam name="T">The options to pass to the command.</typeparam>
    public interface ICommand<in T>
    {
        /// <summary>
        /// Executes a command with the specified <see cref="input"/>.
        /// </summary>
        /// <param name="input">The command input.</param>
        void Execute(T input);
    }
}