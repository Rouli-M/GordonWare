// using iCal.PCL.DataModel;
// using iCal.PCL.Serialization;
// using RestSharp;
// using System;
// using System.Collections.Generic;
// using System.Linq;

// namespace GordonWare {

//     public enum GroupId {
//         Promotion,
//         Group1,
//         Group2,
//         Group3
//     }

//     public static class Calendar {

//         private static bool IsCurrentEvent(DateTime now, iCalVEvent ev) {
//             return ev.DTStart.Date == now.Date && ev.DTEnd.Date == now.Date &&
//                    ev.DTStart.TimeOfDay <= now.TimeOfDay && ev.DTEnd.TimeOfDay >= now.TimeOfDay;
//         }

//         private static bool ForGroup(iCalVEvent ev, GroupId groupId) {
//             return ev.Description.Contains($"INFO3 {Groups[(int) groupId]}");
//         }

//         private const string URL =
//             "http://edt-v2.univ-nantes.fr/calendar/ics?timetables[0]=20739&timetables[1]=20740&timetables[2]=20741&timetables[3]=20742";

//         private static readonly string[] Groups = {"PROMOTION", "G1", "G2", "G3"};

//         /// <summary>
//         /// Gets current class name of the given <see cref="groupId"/> from the public calendar.
//         /// </summary>
//         /// <returns>Returns current class name if there is only one. Otherwise, returns null.</returns>
//         public static string GetCurrentClass(GroupId groupId) {
//             //make and execute a HTTP GET request to get the calendar
//             var client = new RestClient(URL);
//             var request = new RestRequest(Method.GET);
//             var response = client.Execute(request);

//             if (!response.IsSuccessful) {
//                 return null;
//             }

//             //cut the raw iCal content into lines
//             var stringSeparators = new[] {"\r\n"};
//             var lines = response.Content.Split(stringSeparators, StringSplitOptions.None);

//             IEnumerable<iCalVEvent> vEvents;

//             try {
//                 //deserialize the iCal file
//                 var iCal = iCalSerializer.Deserialize(lines);
//                 vEvents = iCal.Cast<iCalVEvent>();
//             } catch (InvalidOperationException e) {
//                 Console.WriteLine(e);
//                 return null;
//             }

//             var now = DateTime.Now;

//             //add 2 hours to get french time and find current class
//             var currentClass = vEvents.Select(ev => {
//                 ev.DTStart = ev.DTStart.AddHours(2);
//                 ev.DTEnd = ev.DTEnd.AddHours(2);
//                 return ev;
//             }).Where(ev =>
//                 IsCurrentEvent(now, ev) && ForGroup(ev, groupId)
//             ).ToList();

//             //return class name only if there is exactly one current class
//             return currentClass.Count == 1 ? currentClass[0].Summary : null;
//         }
//     }
// }