using System.Text.RegularExpressions;
using AngleSharp.Io;

namespace txts.Extensions;

public static partial class RequestExtensions
{
    /// <summary>
    /// Check if the user is on a particularly old device. Used to
    /// disable the stylesheet when rendering pages.
    /// </summary>
    /// <param name="request">The HTTP request context</param>
    /// <returns>Boolean value</returns>
    public static bool IsReallyOldDevice
        (this HttpRequest request) => ReallyOldDeviceRegex().IsMatch(request.Headers[HeaderNames.UserAgent].ToString());

    [GeneratedRegex("(?i)BlackBerry|IEMobile|Opera Mini|PlayStation 3|PlayStation Vita|Nintendo 3DS|WebTV")]
    private static partial Regex ReallyOldDeviceRegex();
}