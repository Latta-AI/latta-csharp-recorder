using System.Globalization;

namespace Latta_CSharp
{
    public class LattaRecorder
    {
        public static void RecordApplication(string apiKey, Action mainMethod)
        {
            var api = new LattaAPI(apiKey);

            try
            {
                mainMethod();
            }
            catch (Exception ex)
            {
                var lattaInstance = api.putInstance(
                    Environment.OSVersion.Platform.ToString(),
                    Environment.OSVersion.Version.ToString(),
                    CultureInfo.CurrentCulture.TwoLetterISOLanguageName,
                    "desktop",
                    ".NET",
                    Environment.Version.Build.ToString()
                );

                if (lattaInstance != null)
                    api.putSnapshot(lattaInstance, ex.Message + ex.StackTrace, null, null);
            }
        }
    }
}