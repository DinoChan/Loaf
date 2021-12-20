using System.Linq;
using Windows.ApplicationModel.Resources.Core;

namespace Loaf
{
    internal static class ResourceExtensions
    {
        public static string GetLocalized(this string resourceKey)
            => ResourceManager.Current.MainResourceMap[$"Resources/{resourceKey}"].Candidates.First().ValueAsString;
    }
}
