using SpecflowPOC.CommonUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SpecflowPOC.Steps
{
    [Binding]
    class SampleApiTestCaseSteps
    {
        RestApiHelpers RestApiHelpers = new RestApiHelpers();

        //API Methods
        [Given(@"Navigate to Endpoint with (.*)")]
        public void GivenNavigateToEndpointWith(string resourceURL)
        {
            RestApiHelpers.SetURL(resourceURL);
        }

        [Given(@"I request to view automation details")]
        public void GivenIRequestToViewAutomationDetails()
        {
            RestApiHelpers.CreateGETRequest();
            RestApiHelpers.GetResponce();
        }

        [Given(@"I request to add automation details profile (.*)")]
        public void GivenIRequestToAddAutomationDetailsProfile(string profile)
        {
            RestApiHelpers.CreateAnonymousPostRequest(profile);
            RestApiHelpers.GetResponce();
        }

        [Then(@"I Validate the (.*) and its values as (.*)")]
        public void ThenIValidateTheAndItsValuesAs(string property, string propertyValue)
        {
            RestApiHelpers.ValidateResponce(property, propertyValue);
        }

        [Then(@"I request to add automation details and validate (.*) and its (.*)")]
        public void ThenIRequestToAddAutomationDetailsAndValidateAndIts(string property, string propValues)
        {
            RestApiHelpers.CreatePostRequest_Payload_FromCSV(property,propValues);
        }

    }
}
