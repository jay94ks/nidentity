namespace NIdentity.Core.Helpers
{
    public static class TaskHelpers
    {
        /// <summary>
        /// Convert the generic task to object task.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="Task"></param>
        /// <returns></returns>
        public static async Task<object> GenericToObject<TReturn>(Task<TReturn> Task)
        {
            return await Task.ConfigureAwait(false);
        }

        /// <summary>
        /// Converter.
        /// </summary>
        /// <param name="ReturnedTask"></param>
        /// <param name="ReturnType"></param>
        /// <returns></returns>
        public static Task<object> Convert(Task ReturnedTask, Type ReturnType)
        {
            return (Task<object>)typeof(TaskHelpers)
                .GetMethod(nameof(GenericToObject))
                .MakeGenericMethod(ReturnType)
                .Invoke(null, new[] { ReturnedTask });
        }
    }
}
