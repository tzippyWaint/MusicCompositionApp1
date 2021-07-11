using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/composition")]
    public class CompositionController : ApiController
    {
        public static List<InstrumentsModel> listOfInstruments = new List<InstrumentsModel>();
        MusicCompositionBL.classes.CompositionBL compositionBL;
        [AcceptVerbs("GET", "POST")]
        [Route("insertcomp")]
        [HttpPost]
        public int Insert(MusicCompositionBL.classes.addCompTApp a)
        {
            compositionBL = new MusicCompositionBL.classes.CompositionBL(); 
            return compositionBL.InsertComposition(a);
        }
        [Route("deletecomp")]
        [HttpPost]
        public int Delete(CompositionsModel comp)
        {
            compositionBL = new MusicCompositionBL.classes.CompositionBL();
            return compositionBL.DeleteComposition(comp);
        }
        [Route("instrumentsofcomp/{codec}")]
        [HttpGet]
        public List<InstrumentsModel> InstrumentsOfComp(int codec)
        {
            compositionBL = new MusicCompositionBL.classes.CompositionBL();
            return compositionBL.InstrumentsOfComp(codec);
        }
        [Route("show")]
        [HttpGet]
        public List<CompositionsModel> Show()
        {
            compositionBL = new MusicCompositionBL.classes.CompositionBL();
            return compositionBL.listOfCompositions;
        }
    }
}