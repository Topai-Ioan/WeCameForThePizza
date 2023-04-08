using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Chess.Controllers
{
    public class GameController : Controller
    {
        static List<Models.ChessMatch> _MatchList = new();
        static int _MatchCounter = 1;
        public IActionResult Index()
        {
            return View();
        }
        //also used to request ongoing match by giving an id on the request
        public IActionResult StartMatch(int _Id = -1)
        {
            Trace.WriteLine("------ DEBUG ------");
            Trace.WriteLine($"IActionResult StartMatch with id: {_Id}");
            if (_Id < 0)
            {
                Trace.WriteLine("creating new match");
                Trace.WriteLine(_MatchList.Count);
                _MatchList.Add(new(_MatchCounter));
                Trace.WriteLine(_MatchList.Count);
                Trace.WriteLine("new match created");
                _Id = _MatchCounter;
                _MatchCounter++;
            }
            List<Models.ChessMatch> MatchList = _MatchList;
            foreach (Models.ChessMatch match in MatchList)
            {
                if (_Id == match.Id)
                {
                    Trace.WriteLine(JsonConvert.SerializeObject(match));
                    Trace.WriteLine("Json away");
                    Trace.WriteLine("------  END  ------");
                    return Json(JsonConvert.SerializeObject(match));
                }
            }
            return Content("Error");
        }
        [HttpPost]
        public IActionResult Move(string _move)
        {
            Trace.WriteLine("------ DEBUG ------");
            Trace.WriteLine("IActionResult Move");
            Trace.WriteLine($"Matches in MatchList: {_MatchList.Count}");
            Trace.WriteLine($"json received: {_move}");
            Models.Move Movement;
            try
            { 
                Movement = JsonConvert.DeserializeObject<Models.Move>(_move);
            }
            catch
            {
                Trace.WriteLine("json error");
                return Content("Error: json parce error");
            }
            Trace.WriteLine("json - ok, validating");
            Trace.WriteLine("comparing ids");
            Trace.WriteLine(_MatchList.Count);
            List<Models.ChessMatch> MatchList = _MatchList;
            int matchNumber = 0;
            foreach (Models.ChessMatch Match in MatchList)
            {
                Trace.WriteLine($"{Match.Id} | {Movement.Id}");
                if(Match.Id == Movement.Id)
                {
                    if (Match.Move(Movement))
                    {
                        Trace.WriteLine("movement made");
                        Trace.WriteLine("overwriting:");
                        foreach (List<string> row in _MatchList[matchNumber].Board)
                        {
                            string rowReturn = "<";
                            foreach (string col in row)
                            {
                                if (rowReturn != "<") { rowReturn += ";"; };
                                if (col == "") { rowReturn += "__"; }
                                else { rowReturn += col; }
                            }
                            rowReturn += ">";
                            Trace.WriteLine(rowReturn);
                        }
                        Trace.WriteLine("with:");
                        foreach (List<string> row in Match.Board)
                        {
                            string rowReturn = "<";
                            foreach (string col in row)
                            {
                                if (rowReturn != "<") { rowReturn += ";"; };
                                if (col == "") { rowReturn += "__"; }
                                else { rowReturn += col; }
                            }
                            rowReturn += ">";
                            Trace.WriteLine(rowReturn);
                        }
                        _MatchList[matchNumber] = Match;
                        Trace.WriteLine("result:");
                        foreach (List<string> row in _MatchList[matchNumber].Board)
                        {
                            string rowReturn = "<";
                            foreach (string col in row)
                            {
                                if (rowReturn != "<") { rowReturn += ";"; }
                                if (col == "") { rowReturn += "__"; }
                                else { rowReturn += col; }
                            }
                            rowReturn += ">";
                            Trace.WriteLine(rowReturn);
                        }
                    }
                    else { Trace.WriteLine("movement not made"); }
                    return Content("Success");
                }
                matchNumber++;
            }
            return Content("Error: id not found");
            }
    }
}
