using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using APILearning.Helpers;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Task = System.Threading.Tasks.Task;

namespace APILearning.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public async Task<ActionResult> GetPatientDetails()
        {
            Patient patient = await GetPatients();

            Demographics demo = new Demographics();
            demo.Surname = patient.Name[0].Family.ToString();
            var current = patient.Name[0].Given.GetEnumerator();
            if (current != null)
                demo.Forename = current.ToString();

            IEnumerable<string> name = patient.Name[0].Given;

            List<Hl7.Fhir.Model.Address> address = patient.Address.ToList();

            //demo.Address1 = patient.Address[0].Children.ToString();
            //demo.Address2 = patient.Address[0].City.ToString();
            //demo.Address3 = patient.Address[0].City.ToString();
            demo.Address4 = patient.Address[0].City.ToString();
            demo.Postcode = patient.Address[0].PostalCode.ToString();

            return View(demo);
        }

        public async Task<ActionResult> GetDiagnosticsDetails()
        {
            DiagnosticReport diagnosticReport = await GetDiagnosticsResults();

            Diagnostics diagnostics = new Diagnostics();
            diagnostics.Id = diagnosticReport.Id;
            diagnostics.PatientName = diagnosticReport.Subject.Display.ToString();
            diagnostics.Code = diagnosticReport.Code.Text.ToString();
            diagnostics.CodedDiagnosis = diagnosticReport.CodedDiagnosis.Count;

            return View(diagnostics);
        }

        static async Task<Patient> GetPatients()
        {
            Hl7.Fhir.Model.Patient patient = null;
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "");
            // client.DefaultRequestHeaders.Add("Authorization", "{access token}");

            // Request parameters
            queryString["family"] = "davies";
            var uri = "https://dhew-nwis.azure-api.net/mpi/fhir-patient/Patient?" + queryString;

            using (HttpResponseMessage response = await client.GetAsync(uri))
            {
                using (HttpContent content = response.Content)
                {
                    string responseString = await content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();

                    FhirJsonParser fjp = new FhirJsonParser(); /* there is a FhirXmlParser as well */
                    /* You may need to Parse as something besides a Bundle depending on the return payload */
                    Hl7.Fhir.Model.Bundle bundle = fjp.Parse<Hl7.Fhir.Model.Bundle>(responseString);
                    Hl7.Fhir.Model.Bundle.EntryComponent entryComponent = bundle?.Entry.FirstOrDefault();
                    if (entryComponent?.Resource != null)
                    {
                        /* again, this may be a different kind of object based on which rest url you hit */
                        patient = entryComponent.Resource as Hl7.Fhir.Model.Patient;
                    }

                }

            }

            return patient;
        }

        static async Task<DiagnosticReport> GetDiagnosticsResults()
        {
            Hl7.Fhir.Model.DiagnosticReport diagnosticReport = null;
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "");
            // client.DefaultRequestHeaders.Add("Authorization", "{access token}");

            // Request parameters
            queryString["patient"] = "43";
            var uri = "https://dhew-nwis.azure-api.net/wrrs/fhir-diagnostics/DiagnosticReport?" + queryString;

            using (HttpResponseMessage response = await client.GetAsync(uri))
            {
                using (HttpContent content = response.Content)
                {
                    string responseString = await content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();

                    FhirJsonParser fjp = new FhirJsonParser(); /* there is a FhirXmlParser as well */
                    /* You may need to Parse as something besides a Bundle depending on the return payload */
                    Hl7.Fhir.Model.Bundle bundle = fjp.Parse<Hl7.Fhir.Model.Bundle>(responseString);
                    Hl7.Fhir.Model.Bundle.EntryComponent entryComponent = bundle?.Entry.FirstOrDefault();
                    if (entryComponent?.Resource != null)
                    {
                        /* again, this may be a different kind of object based on which rest url you hit */
                        diagnosticReport = entryComponent.Resource as Hl7.Fhir.Model.DiagnosticReport;
                    }

                }

            }

            return diagnosticReport;
        }

        public async Task<ActionResult> ResultAPI()
        {
            ViewBag.Title = "Home Page";

            await MakeRequest();
            //            await GetAPI();
            // Test();
            return View();
        }

        static async Task MakeRequest()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "");
            // client.DefaultRequestHeaders.Add("Authorization", "{access token}");

            // Request parameters
            queryString["family"] = "davies";
            var uri = "https://dhew-nwis.azure-api.net/mpi/fhir-patient/Patient?" + queryString;

            using (HttpResponseMessage response = await client.GetAsync(uri))
            {
                using (HttpContent content = response.Content)
                {
                    string responseString = await content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();

                    FhirJsonParser fjp = new FhirJsonParser(); /* there is a FhirXmlParser as well */
                    /* You may need to Parse as something besides a Bundle depending on the return payload */
                    Hl7.Fhir.Model.Bundle bundle = fjp.Parse<Hl7.Fhir.Model.Bundle>(responseString);
                    Hl7.Fhir.Model.Bundle.EntryComponent entryComponent = bundle?.Entry.FirstOrDefault();
                    if (entryComponent?.Resource != null)
                    {
                        /* again, this may be a different kind of object based on which rest url you hit */
                        Hl7.Fhir.Model.Patient patient = entryComponent.Resource as Hl7.Fhir.Model.Patient;

                        patient.Name = new List<HumanName>();
                    }

                }

            }
        }

        private void Test()
        {
            const string endpoint = "https://ontoserver.csiro.au/stu3-latest";
            const string valueSetUrl = "http://snomed.info/sct?fhir_vs=refset/1072351000168102";

            var client = new FhirClient(endpoint);
            var filter = new FhirString("inr");
            var url = new FhirUri(valueSetUrl);
            var result = client.ExpandValueSet(url, filter);
            // Console.WriteLine(result.Expansion.Contains.FirstOrDefault()?.Display);
        }

        private async Task GetAPI()
        {
            string url = "https://dhew-nwis.azure-api.net/mpi/fhir-patient/Patient?family=davies";

            try
            {
                var client2 = new HttpClient();
                client2.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "");

                var httpResponseMessage = client2.GetAsync(url).Result;
                var responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }



            HttpClient client = new HttpClient();
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "");
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        string responseString = await content.ReadAsStringAsync();
                        response.EnsureSuccessStatusCode();

                        /* Hl7.Fhir.DSTU2  \.nuget\packages\hl7.fhir.dstu2\0.96.0 */

                        FhirJsonParser fjp = new FhirJsonParser(); /* there is a FhirXmlParser as well */
                        /* You may need to Parse as something besides a Bundle depending on the return payload */
                        Hl7.Fhir.Model.Bundle bund = fjp.Parse<Hl7.Fhir.Model.Bundle>(responseString);
                        if (null != bund)
                        {
                            Hl7.Fhir.Model.Bundle.EntryComponent ec = bund.Entry.FirstOrDefault();
                            if (null != ec && null != ec.Resource)
                            {
                                /* again, this may be a different kind of object based on which rest url you hit */
                                Hl7.Fhir.Model.Patient pat = ec.Resource as Hl7.Fhir.Model.Patient;
                            }
                        }

                    }

                }
            }
        }
    }
}
