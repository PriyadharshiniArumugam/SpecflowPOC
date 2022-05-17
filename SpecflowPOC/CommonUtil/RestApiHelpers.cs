using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using SpecflowPOC.Model;

namespace SpecflowPOC.CommonUtil
{
    class RestApiHelpers
    {
        public RestClient _restClient;
        public RestRequest _restRequest;
        public IRestResponse _restResponse;
        public string _baseURL = ConfigurationManager.AppSettings["api_BaseURL"];
        public string endpoint = null;
        List<IRestResponse<AutomationDetails>> restResponses = new List<IRestResponse<AutomationDetails>>();

        public void SetURL(string resourceURL)
        {
            endpoint = Path.Combine(_baseURL, resourceURL);
            _restClient = new RestClient(endpoint);
        }

        public void CreateGETRequest()
        {
            _restRequest = new RestRequest(Method.GET);
        }

        public void CreatePostRequest_Payload_FromCSV(string property,string propValues)
        {
            List<string> PropValues_list = propValues.Split(',').ToList<string>();
            List<string> Payload_list = ReadDataCSVFile();
            for (int i=0;i<Payload_list.Count;i++)
            {
                if (!string.IsNullOrEmpty(Payload_list[i]))
                {
                    List<string> cell = Payload_list[i].Split(',').ToList();
                    _restRequest = new RestRequest(Method.POST);
                    _restRequest.RequestFormat = DataFormat.Json;
                    _restRequest.AddBody(new AutomationDetails() { id = int.Parse(cell[0].Trim()), title = cell[1].Trim(), author = cell[2].Trim() });
                    IRestResponse<AutomationDetails> response = _restClient.Execute<AutomationDetails>(_restRequest);
                    if(property == "author")
                    {
                        Assert.AreEqual(PropValues_list[i], response.Data.author);
                    }
                }
            }
        }

        public void CreateAnonymousPostRequest(string profileName)
        {
            _restRequest = new RestRequest(Method.POST);
            _restRequest.RequestFormat = DataFormat.Json;
            _restRequest.AddBody(new { name = profileName});
        }

        public void GetResponce()
        {
            _restResponse = _restClient.Execute(_restRequest);
        }

        public void ValidateGenericResponce(string property, string propValue)
        {
            bool isfound = restResponses.Any(x => x.Data.author.Contains(propValue));
            Assert.AreEqual(true, isfound);

        }

        public void ValidateResponce(string property, string expected_PropValue)
        {
            var deserialize = new JsonDeserializer();
            Dictionary<string, string> result_List = deserialize.Deserialize<Dictionary<string, string>>(_restResponse);
            var actual_PropValue = (from result in result_List where result.Key == property select result.Value).First();
            Assert.AreEqual(expected_PropValue, actual_PropValue);
        }

        public List<string> ReadDataCSVFile()
        {
            string filePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(Hooks)).Location), @"..\..\"));
            string csvData = File.ReadAllText(filePath + @"\DataSource\Payload.csv");
            return csvData.Split('\n').Skip(1).ToList<string>();
        }
    }
}
