using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.Game;
using YoVendoRecargaAPI.WebAPI.Models;


namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class LeaderBoardsInteractor
    {
        GameBL gameBL = new GameBL();
        public IResponse createSuccessResponse(string search)
        {
            GenericApiResponse response = new GenericApiResponse();
            List<LeaderBoardsResponse> list = new List<LeaderBoardsResponse>();
            LeaderBoardsResponse responseLeaderBoards = new LeaderBoardsResponse();
            try
            {
                var appData = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Repository");
                // read JSON directly from a file
                //using (StreamReader file = File.OpenText(appData + "\\LeaderBoards.json"))
                
                var jsonText = File.ReadAllText(appData + "\\LeaderBoards.json");
                var leaderBoards = JsonConvert.DeserializeObject<List<LeaderBoardsResponse>>(jsonText);

                responseLeaderBoards = (from l in leaderBoards
                            where l.filter == search
                            select new LeaderBoardsResponse { 
                            Leaderboards = l.Leaderboards,
                            LastWinner = l.LastWinner
                            }).FirstOrDefault();


                return responseLeaderBoards;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                responseLeaderBoards = null;

                return responseLeaderBoards;
            }

        }

        public IResponse createResponseSaveJson()
        {
            GenericApiResponse response = new GenericApiResponse();
            List<LeaderBoardsResponse> list = new List<LeaderBoardsResponse>();
            
            List<LeaderBoards> listAllLeaderBoards = new List<LeaderBoards>();

            try
            {
                LeaderBoards Leaderboards = new LeaderBoards();
                var pLeaderboards = gameBL.GetLeaderBoards();

                var pLastWinner = gameBL.GetLastWinner();

                listAllLeaderBoards.Add(Leaderboards);

                LeaderBoardsResponse resultToday = new LeaderBoardsResponse();
                resultToday.LastWinner = pLastWinner.LastWinnerOnYesterday;
                resultToday.Leaderboards = pLeaderboards.ListLeaderBoardsToday;
                resultToday.filter = "Today";

                list.Add(resultToday);

                LeaderBoardsResponse resultWeek = new LeaderBoardsResponse();
                resultWeek.LastWinner = pLastWinner.LastWinnerOnWeek;
                resultWeek.Leaderboards = pLeaderboards.ListLeaderBoardsWeek;
                resultWeek.filter = "Week";
                list.Add(resultWeek);

                LeaderBoardsResponse resultMonth = new LeaderBoardsResponse();
                resultMonth.LastWinner = pLastWinner.LastWinnerOnMonth;
                resultMonth.Leaderboards = pLeaderboards.ListLeaderBoardsMonth;
                resultMonth.filter = "Month";
                list.Add(resultMonth);

                LeaderBoardsResponse resultOverAll = new LeaderBoardsResponse();
                resultOverAll.Leaderboards = pLeaderboards.ListLeaderBoardsOverAll;
                resultOverAll.filter = "OverAll";
                list.Add(resultOverAll);

                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                EventViewerLoggerBL.LogError("json data: " + json);

                var appData = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Repository");
                
                foreach (var file in Directory.EnumerateFiles(appData))
                {
                    var dest = Path.Combine(appData, Path.GetFileName(file));
                    if (!File.Exists(dest))
                    {
                        File.Copy(file, dest);
                    }

                    System.IO.File.WriteAllText(dest, json);
                }


                

                response.HttpCode = 200;
                response.InternalCode = "00";
                response.Message = "Success";

            }
            catch (Exception ex)
            {
                EventViewerLoggerBL.LogError(ex.Message);
                response = null;
            }

            return response;
        }
    }
}