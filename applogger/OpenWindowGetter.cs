/**
 * Via: http://www.tcx.be/blog/2006/list-open-windows/
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>Contains functionality to get all the open windows.</summary>
public static class OpenWindowGetter
{
    /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
    /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
    public static IDictionary<IntPtr, string> GetOpenWindows()
    {
        IntPtr shellWindow = GetShellWindow();
        Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();

        EnumWindows((EnumWindowsProc) delegate(IntPtr IntPtr, int lParam)
        {
            if (IntPtr == shellWindow) return true;
            if (!IsWindowVisible(IntPtr)) return true;

            int length = GetWindowTextLength(IntPtr);
            if (length == 0) return true;

            StringBuilder builder = new StringBuilder(length);
            GetWindowText(IntPtr, builder, length + 1);

            windows[IntPtr] = builder.ToString();
            return true;

        }, 0);

        return windows;
    }

    private delegate bool EnumWindowsProc(IntPtr IntPtr, int lParam);

    [DllImport("USER32.DLL")]
    private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

    [DllImport("USER32.DLL")]
    private static extern int GetWindowText(IntPtr IntPtr, StringBuilder lpString, int nMaxCount);

    [DllImport("USER32.DLL")]
    private static extern int GetWindowTextLength(IntPtr IntPtr);

    [DllImport("USER32.DLL")]
    private static extern bool IsWindowVisible(IntPtr IntPtr);

    [DllImport("USER32.DLL")]
    private static extern IntPtr GetShellWindow();
}