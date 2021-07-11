using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/player")]
    public class PlayerController:ApiController
    {
        MusicCompositionBL.classes.PlayersBL PlayersBL;
        [AcceptVerbs("GET", "POST")]
        [Route("insert")]
        [HttpPost]
        public string InsertPlayer(MusicCompositionBL.classes.insertPlayer insertPlayer)
        {
            PlayersBL = new MusicCompositionBL.classes.PlayersBL();
            return PlayersBL.InsertPlayer(insertPlayer);
        }
        [Route("delete")]
        [HttpPost]
        public int deletePlayer(Players player)
        {
            PlayersBL = new MusicCompositionBL.classes.PlayersBL();
            return PlayersBL.DeletePlayer(player);
        }
        [Route("costapp")]
        [HttpPost]
        public int costApp(Dictionary<int, int> dict)
        {
            PlayersBL = new MusicCompositionBL.classes.PlayersBL();
            Dictionary<int?, int> dict2 = new Dictionary<int?, int>();
            foreach (var item in dict)
            {
                dict2.Add((int?)item.Key, item.Value);
            }
            return (int)PlayersBL.GetCostOfApp(dict2);
        }
        [Route("update")]
        [HttpPost]
        public bool updatePlayer(Players player)
        {
            PlayersBL = new MusicCompositionBL.classes.PlayersBL();
            return PlayersBL.UpDatePlayer(player);
        }
        [Route("playerforapp")]
        [HttpPost]
        public Dictionary<int, int> PlayerForAppearance(MusicCompositionBL.classes.CodeCompAndDateApp codeCompAndDateApp)
        {
            PlayersBL = new MusicCompositionBL.classes.PlayersBL();
            Dictionary<int, int> dict = new Dictionary<int, int>();
            foreach (var item in PlayersBL.PlayerForAppearance(codeCompAndDateApp.codeComp, codeCompAndDateApp.date))
            {
                dict.Add((int)item.Key, item.Value);
            }
            return dict;
        }
        [Route("schedule/{codep}")]
        [HttpGet]
        public List<AppearancesModel> ScheduleForPlayer(int codeP)
        {
            PlayersBL = new MusicCompositionBL.classes.PlayersBL();
            return PlayersBL.ScheduleForPlayer(codeP);
        }
        [Route("playersApp/{codea}")]
        [HttpGet]
        public List<PlayersModel> GetPlayersInAppByCodeApp(int codeA)
        {
            PlayersBL = new MusicCompositionBL.classes.PlayersBL();
            MusicCompositionBL.classes.PlayerInAppearenceBL pia = new MusicCompositionBL.classes.PlayerInAppearenceBL();
            return PlayersBL.ConvertListToModel(pia.getPlayersInAppByCodeApp(codeA));
        }
        [Route("namebycode/{codep}")]
        [HttpGet]
        public string GetNameByCodeP(int codeP)
        {
            PlayersBL = new MusicCompositionBL.classes.PlayersBL();
            return PlayersBL.GetNameByCodeP(codeP);
        }
        [Route("show")]
        [HttpGet]
        public List<PlayersModel> Show()
        {
            PlayersBL = new MusicCompositionBL.classes.PlayersBL();
            return PlayersBL.ConvertListToModel(PlayersBL.GetAllPlayers());
        }
        [Route("showcon")]
        [HttpGet]
        public List<PlayersModel> ShowCon()
        {
            PlayersBL = new MusicCompositionBL.classes.PlayersBL();
            return  PlayersBL.ConvertListToModel(  PlayersBL.GetAllPlayers().Where(p=>p.status=="activeC").ToList());
        }
    }
}