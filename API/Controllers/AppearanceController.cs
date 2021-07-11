using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Models;
using MusicCompositionBL;

namespace API.Controllers
{
    [RoutePrefix("api/appearance")]
    public class AppearanceController: ApiController
    {
        MusicCompositionBL.classes.AppearancesBL appearancesBL;
        [AcceptVerbs("GET", "POST")]
        [Route("insertappearance")]
        [HttpPost]
        public bool Insert(MusicCompositionBL.classes.AppAndListOfPlayers obj)
        {
            appearancesBL = new MusicCompositionBL.classes.AppearancesBL();
            return  appearancesBL.InsertAppearance(obj.app, obj.listP, obj.dateA, obj.startHour, obj.endHour);

        }
        [Route("deleteappearance")]
        [HttpPost]
        public int Delete(Appearances appearance)
        {
            appearancesBL = new MusicCompositionBL.classes.AppearancesBL();
            return appearancesBL.DeleteAppearance(appearance);
        }
        [Route("updateappearance")]
        [HttpPost]
        public int Update(Appearances app)
        {
            appearancesBL = new MusicCompositionBL.classes.AppearancesBL();
            return appearancesBL.UpDateAppearance(app);
        }
        [Route("show")]
        [HttpGet]
        public List<AppearancesModel> Show()
        {
            appearancesBL = new MusicCompositionBL.classes.AppearancesBL();
            return appearancesBL.GetAllApearancesModels();
        }
    }
}

