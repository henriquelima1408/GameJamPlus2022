namespace App.System.Utils
{
    public static class CompilationUtils
    {
        public static bool IsEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }


    }
}
