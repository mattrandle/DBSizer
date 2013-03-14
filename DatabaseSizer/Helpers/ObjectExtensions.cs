using System;

namespace DatabaseSizer.Helpers
{
    public static class ObjectExtensionMethods
    {
        public static void TryCatchFinally(this object source, Action method, Action<Exception> exceptionHandler,
                                           Action finallyMethod)
        {
            try
            {
                method();
            }
            catch (Exception e)
            {
                exceptionHandler(e);
            }
            finally
            {
                finallyMethod();
            }
        }

        public static void TryFinally(this object source, Action method, Action finallyMethod)
        {
            try
            {
                method();
            }
            finally
            {
                finallyMethod();
            }
        }

        public static void TryCatch(this object source, Action method)
        {
            try
            {
                method();
            }
// ReSharper disable EmptyGeneralCatchClause
            catch
// ReSharper restore EmptyGeneralCatchClause
            {
            }
        }

        public static void TryCatch(this object source, Action method, Action<Exception> exceptionHandler)
        {
            try
            {
                method();
            }
            catch (Exception e)
            {
                exceptionHandler(e);
            }
        }
    }
}