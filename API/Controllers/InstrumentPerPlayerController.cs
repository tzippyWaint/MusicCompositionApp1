using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace API.Controllers
{
    [RoutePrefix("api/instrumentperplayer")]
    public class InstrumentPerPlayerController:ApiController
    {
        MusicCompositionBL.classes.InstrumentPerPlayer instrumentPerPlayerBL;
        [AcceptVerbs("GET", "POST")]
        [Route("insert")]
        [HttpPost]
        public string Insert(InstumentPerPlayer instumentPerPlayer)
        {
            instrumentPerPlayerBL = new MusicCompositionBL.classes.InstrumentPerPlayer();
            return instrumentPerPlayerBL.InsertInstrumPerPlayer(instumentPerPlayer);
        }
        [Route("update")]
        [HttpPost]
        public bool Update(InstumentPerPlayer instumentPerPlayer)
        {
            instrumentPerPlayerBL = new MusicCompositionBL.classes.InstrumentPerPlayer();
            return instrumentPerPlayerBL.UpDateInstrumPerPlayer(instumentPerPlayer);
        }
        [Route("show")]
        [HttpGet]
        public List<InstumentPerPlayer> Show()
        {
            instrumentPerPlayerBL = new MusicCompositionBL.classes.InstrumentPerPlayer();
            return instrumentPerPlayerBL.listOfInstrumentsPerPlayer;
        }
        [Route("showp/{codep}")]
        [HttpGet]
        public List<InstrumentPerPlayerModel> Show(int codep)
        {
            instrumentPerPlayerBL = new MusicCompositionBL.classes.InstrumentPerPlayer();
            return instrumentPerPlayerBL.ConvertListToModel( instrumentPerPlayerBL.listOfInstrumentsPerPlayer.Where(i=>i.codeP==codep).ToList());
        }
    }
}