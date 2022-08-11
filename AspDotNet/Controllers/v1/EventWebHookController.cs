using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.SignalR;
using HermesCenter.Logger;
using HermesCenter.Models.Hubs;

namespace HermesCenter.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EventWebHookController : ControllerBase
    {
        private bool EventTypeSubcriptionValidation
            => HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault() ==
               "SubscriptionValidation";

        private bool EventTypeNotification
            => HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault() ==
               "Notification";

        private readonly ILogManager _logManager;
        private readonly IHubContext<GridEventsHub> _hubContext;

        public EventWebHookController(ILogManager logManager, IHubContext<GridEventsHub> gridEventsHubContext)
        {
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
            _hubContext = gridEventsHubContext;
        }

        [HttpOptions]
        public IActionResult Options()
        {
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var webhookRequestOrigin = HttpContext.Request.Headers["WebHook-Request-Origin"].FirstOrDefault();
                var webhookRequestCallback = HttpContext.Request.Headers["WebHook-Request-Callback"];
                var webhookRequestRate = HttpContext.Request.Headers["WebHook-Request-Rate"];
                HttpContext.Response.Headers.Add("WebHook-Allowed-Rate", "*");
                HttpContext.Response.Headers.Add("WebHook-Allowed-Origin", webhookRequestOrigin);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            _logManager.Information("ML Event - received event", "CraniumCollector");
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var jsonContent = await reader.ReadToEndAsync();

            // FACT: excluding SubcriptionValidation requests
            if (!EventTypeSubcriptionValidation)
            {
                _logManager.Information($"ML Event - event data: {jsonContent}", "CraniumCollector");
            }

            //TODO format message
            //await _redisQueueService.InsertMessageAsync(jsonContent);

            // Check the event type. Return the validation code if it's a subscription validation request. 
            if (EventTypeSubcriptionValidation)
            {
                return await HandleValidation(jsonContent);
            }
            else if (EventTypeNotification)
            {
                // Check to see if this is passed in using the CloudEvents schema
                if (IsCloudEvent(jsonContent))
                {
                    return await HandleCloudEvent(jsonContent);
                }

                return await HandleGridEvents(jsonContent);
            }

            return Ok();
        }

        private async Task<JsonResult> HandleValidation(string jsonContent)
        {
            var gridEvent =
                JsonConvert.DeserializeObject<List<GridEvent<Dictionary<string, string>>>>(jsonContent)
                    .First();

            await _hubContext.Clients.All.SendAsync(
                "gridupdate",
                gridEvent.Id,
                gridEvent.EventType,
                gridEvent.Subject,
                gridEvent.EventTime.ToLongTimeString(),
                jsonContent.ToString());

            // Retrieve the validation code and echo back.
            var validationCode = gridEvent.Data["validationCode"];
            var res = new JsonResult(new
            {
                validationResponse = validationCode
            });
            res.StatusCode = 200;
            return res;
        }

        private async Task<IActionResult> HandleCloudEvent(string jsonContent)
        {
            var details = JsonConvert.DeserializeObject<CloudEvent<dynamic>>(jsonContent);
            var eventData = JObject.Parse(jsonContent);

            await _hubContext.Clients.All.SendAsync(
                "gridupdate",
                details.Id,
                details.Type,
                details.Subject,
                details.Time,
                eventData.ToString()
            );

            return Ok();
        }

        private async Task<IActionResult> HandleGridEvents(string jsonContent)
        {
            var events = JArray.Parse(jsonContent);

            foreach (var e in events)
            {
                // Invoke a method on the clients for an event grid notiification.                        
                var details = JsonConvert.DeserializeObject<GridEvent<dynamic>>(e.ToString());
                await _hubContext.Clients.All.SendAsync(
                    "gridupdate",
                    details.Id,
                    details.EventType,
                    details.Subject,
                    details.EventTime.ToLongTimeString(),
                    e.ToString());
            }

            return Ok();
        }

        private static bool IsCloudEvent(string jsonContent)
        {
            // Cloud events are sent one at a time, while Grid events are sent in an array. As a result, the JObject.Parse willnfail for Grid events. 
            try
            {
                // Attempt to read one JSON object. 
                var eventData = JObject.Parse(jsonContent);

                // Check for the spec version property.
                var version = eventData["specversion"].Value<string>();
                if (!string.IsNullOrEmpty(version)) return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
    }
}
