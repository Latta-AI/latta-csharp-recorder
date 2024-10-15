using Latta_CSharp.models;
using Newtonsoft.Json;
using System.Text;

namespace Latta_CSharp
{
    internal class LattaAPI
    {
        private string apiKey;

        private const string NEW_INSTANCE_URL = "https://recording.latta.ai/v1/instance/backend";

        private const string SNAPSHOT_URL = "https://recording.latta.ai/v1/snapshot/";

        public LattaAPI(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public LattaInstance? putInstance(string os, string osVersion, string lang, string device, string framework, string frameworkVersion)
        {
            Dictionary<string, string> instanceData = new Dictionary<string, string>
            {
                { "os", os },
                { "os_version", osVersion },
                { "lang", lang },
                { "device", device },
                { "framework", framework },
                { "framework_version", frameworkVersion }
            };

            var requestBody = JsonConvert.SerializeObject(instanceData);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = client.PutAsync(NEW_INSTANCE_URL, content).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<LattaInstance>(responseString);
            }
        }

        public LattaSnapshot? putSnapshot(LattaInstance instance, string message, string? relation_id, string? related_to_relation_id)
        {
            Dictionary<string, string> snapshotData = new Dictionary<string, string>
            {
                { "message", message },
                { "relation_id", relation_id },
                { "related_to_relation_id", related_to_relation_id }
            };

            var requestBody = JsonConvert.SerializeObject(snapshotData);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = client.PutAsync(SNAPSHOT_URL + instance.id, content).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<LattaSnapshot>(responseString);
            }
        }

        public void putAttachment(LattaSnapshot snapshot, object attachment)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                var content = new StringContent(JsonConvert.SerializeObject(attachment), Encoding.UTF8, "application/json");
                var response = client.PutAsync(SNAPSHOT_URL + snapshot.id + "/attachment", content).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
