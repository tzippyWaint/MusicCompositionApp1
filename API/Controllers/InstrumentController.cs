using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/instrument")]
    public class InstrumentController:ApiController
    {
        MusicCompositionBL.classes.InstrumentsBL instrumentsBL;
        [AcceptVerbs("GET", "POST")]
        [Route("create/{style}/{sizeplace}")]
        [HttpGet]
        public Dictionary<int, List<InstrumentsModel>> CreateComposition(int style,int sizePlace)
        {
            instrumentsBL = new MusicCompositionBL.classes.InstrumentsBL();
            Dictionary<int, List<InstrumentsModel>> d = new Dictionary<int, List<InstrumentsModel>>();
            d=instrumentsBL.CreateComposition(style,sizePlace);
            if (d.Count() == 0)
                return null;
            return d;
        }
        [Route("show")]
        [HttpGet]
        public List<InstrumentsModel> Show()
        {
            instrumentsBL = new MusicCompositionBL.classes.InstrumentsBL();
            return instrumentsBL.listOfInstruments;
        }
        [Route("insert")]
        [HttpPost]
        public string InsertInst(Instruments inst)
        {
            instrumentsBL = new MusicCompositionBL.classes.InstrumentsBL();
            return instrumentsBL.InsertInstrument(inst.nameInst,inst.style,inst.size,inst.voice,inst.type);
        }
    }
}