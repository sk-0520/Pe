namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    partial class NativeMethods
    {
        #region function

        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GetACP")]
        public static extern uint GetACP();

        #endregion
    }
}
